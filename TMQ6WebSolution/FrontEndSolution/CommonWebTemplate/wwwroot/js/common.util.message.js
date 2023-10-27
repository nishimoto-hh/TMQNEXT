/* ========================================================================
 *  機能名　    ：   【共通・メッセージ、エラー制御】
 * ======================================================================== */

/*Public変数：ﾒｯｾｰｼﾞ文字列*/
var P_MessageStr = '';

var P_MessageStrClear = '';

var P_MessageStrBack = '';

/*定義：共通ﾒｯｾｰｼﾞKey*/
var MessageKeys = [
    //==ｲﾝﾌｫﾒｰｼｮﾝ==    
    '941120002',  //【CJ00000.I01】処理が完了しました。    
    '941040002',  //Excelファイルをダウンロードしました。
    '941120003',  //処理中...　しばらくお待ちください。
    '911200005',  //問い合わせ番号

    //==ﾜｰﾆﾝｸﾞ==    
    '941220007',  //【CJ00000.W01】入力エラーがあります。    
    '941120004',  //【CJ00000.W02】処理権限がありません。

    //==ｴﾗｰ==    
    '941120005',  //【CJ00000.E01】処理に失敗しました。    
    '941120006',  //【CJ00000.E02】処理不正です。システム担当者に連絡してください。    
    '941280001',  //【CJ00000.E03】部署別権限の権限部署が設定されていません。システム担当者に連絡してください。

    /* ﾎﾟｯﾌﾟｱｯﾌﾟﾒｯｾｰｼﾞ用 */
    //==ﾜｰﾆﾝｸﾞ==
    '941280002',  //ファイルを指定してください。    
    '941280003',  //ファイルサイズが大きすぎます。    
    '941280004',  //ファイル形式が有効ではありません。    
    '941280005',  //ファイル名が重複しています。

    //==ｴﾗｰ==    
    '941120007',  //処理に失敗しました。

    //==確認ﾒｯｾｰｼﾞ==    
    '941430001',  //ログアウトします。よろしいですか？    
    '941430002',  //ログアウトしてＩＤ切替画面に遷移します。　　　よろしいですか？    
    '941260011',  //パスワードを変更します。よろしいですか？    
    '941060005',  //画面の編集内容は破棄されます。よろしいですか？    
    '941120008',  //します。よろしいですか？
    '941040003',  //Excel出力を実行します。よろしいですか？
    '941280006',  //ファイルを取り込みます。よろしいですか？

    '911130001',  //すべて
];


/*定義：ﾒｯｾｰｼﾞ種類(ﾎﾟｯﾌﾟｱｯﾌﾟ用)*/
var messageType = {
    //0(お知らせ)
    Info: 1,
    //1(警告)
    Warning: 1,
    //2(エラー)
    Error: 2,
    //3(確認)
    Confirm: 3,
};
////【共通 - 共通機能】メッセージ表示用ボタン（※非表示）CtrlId
//var ctrlIdComMessage = "ComMessage";


/**
 *  メッセージエリアにメッセージを設定します。
 *  @param {Element} target :メッセージ表示対象要素
 */
function clearMessage(target) {

    var messageDiv = $("#message_divid");
    if (target) {
        messageDiv = $(target).find(".message_div");
    }
    else if ($(P_Article).closest(".modal_form").length > 0) {
        //ﾎﾟｯﾌﾟｱｯﾌﾟ用ﾒｯｾｰｼﾞｴﾘｱ
        messageDiv = $(P_Article).closest(".modal_form").find(".message_div");
    }
    else if ($("#passwordChangeModal").hasClass("in")) {
        messageDiv = $("#passwordChange_message_div");
    }
    else if ($("#setDispItemModal").hasClass("in")) {
        messageDiv = $("#setDispItem_message_div");
    }

    // メッセージをクリア
    $(messageDiv).children().remove();
    messageDiv = null;
}

/**
 *  メッセージエリアにメッセージを追加します。
 *  @param {string} messageStr：メッセージ
 *  @param {number} messagetype：0:(通常),9(エラー)
 *  @param {Element} target :メッセージ表示対象要素
 */
function addMessage(messageStr, messagetype, target) {

    var messageDiv = $("#message_divid");
    if (target) {
        messageDiv = $(target).find(".message_div");
    }
    else if ($(P_Article).closest(".modal_form").length > 0) {
        //ﾎﾟｯﾌﾟｱｯﾌﾟ用ﾒｯｾｰｼﾞｴﾘｱ
        messageDiv = $(P_Article).closest(".modal_form").find(".message_div");
    }
    else if ($("#passwordChangeModal").hasClass("in")) {
        messageDiv = $("#passwordChange_message_div");
    }
    else if ($("#setDispItemModal").hasClass("in")) {
        messageDiv = $("#setDispItem_message_div");
    }

    if (messagetype == 0) {
        if (messageStr.length > 0) {
            $('<div>').text(messageStr).addClass("success").appendTo($(messageDiv));
        }
    }
    else {
        if (messageStr != null) {
            $('<div>').text(messageStr).addClass("error").appendTo($(messageDiv));
        }
    }
    messageDiv = null;
}

/**
 *  メッセージエリアのメッセージをクリア後、再設定します。
 *  @param {string} ：メッセージ
 *  @messagetype {number} ：0:(通常),9(エラー)
 */
function setMessage(messageStr, messagetype) {

    // メッセージをクリア
    clearMessage();
    //メッセージを追加
    addMessage(messageStr, messagetype);
}

/**
 *  指定要素の後ろにｴﾗｰ要素を表示する。
 *  @param {messageStr} ：メッセージ
 *  @param {elements}   ：ｴﾗｰ要素
 */
function addErrorPlacement(messageStr, elements) {
    //エラー情報をツールチップに配置
    $.each($(elements), function (idx, element) {
        var error = $('<label class="errorcom">' + messageStr + '</label>');
        var td = $(element).closest('td');
        $(error).insertAfter(td);
        $(td).addClass('errorcom');

        $(error).hide();
        $(td).hover(
            function () {
                $(error).fadeIn('fast');
            },
            function () {
                $(error).fadeOut('fast');
            });
        td = null;
    });
}

/**
 *  処理中メッセージを制御します。
 *  @param {bool} ：true（表示）、false（非表示）
 *  @param {int}  ：0（ﾎﾞﾀﾝ系処理）、1（連動ｺﾝﾎﾞ処理）
 */
function processMessage(onFlg, mode) {

    if (mode == null) mode = 0; //初期化

    if (onFlg == true) {
        //※処理開始時

        //メッセージを表示
        //setMessage("処理中...　しばらくお待ちください。", 0);
        setMessage(P_ComMsgTranslated[941120003], 0);

    }
    else {
        //※処理終了時

        switch (mode) {
            case 1:
                //ﾒｯｾｰｼﾞを初期化
                clearMessage();
                break;

            default:
                //※ﾎﾞﾀﾝ系処理時は各処理内でﾒｯｾｰｼﾞを初期化
                // 画面を覆っているタグを削除する
                //$("#" + "lockId").remove();
                break;
        }
        removeLoading();
    }
}

/**
 *  指定エリアのエラークラス(.error/.errorcom)を削除
 *  @param {string} element ：要素指定文字列
 */
function clearErrorClasses(element) {

    //エラー情報をクリア
    clearErrorClass(element);

    //Validationエラー情報をクリア
    clearErrorcomClass(element);
}

/**
 *  指定エリアのエラー状態を初期化。(実行ボタン確認後初期化処理)
 *  @element {string} ：要素指定文字列
 */
function clearErrorcomClass(element) {

    // + ﾃｰﾌﾞﾙ
    $(element).find("tr.errorcom,th.errorcom,td.errorcom").removeClass("errorcom");
    // + 複数選択ﾘｽﾄ
    $(element).find("ul.multiSelect.errorcom").removeClass("errorcom");
    // + 入力ｺﾝﾄﾛｰﾙ
    $(element).find("input.errorcom,select.errorcom,textarea.errorcom").removeClass("errorcom");
    // + ﾂｰﾙﾁｯﾌﾟ
    //$(element).find("label.errorcom").remove();
    // + ﾂｰﾙﾁｯﾌﾟ
    $("div.errtooltip").find("label.errorcom").remove();

}

/**
 *  指定エリアのエラークラス(.error)を削除。(個別クラスのみ)
 *  @element {string} ：要素指定文字列
 */
function clearErrorClass(element) {

    //エラー情報をクリア
    // + ﾃｰﾌﾞﾙ
    $(element).find("tr.error,th.error,td.error").removeClass("error");
    // + 複数選択ﾘｽﾄ
    $(element).find("ul.multiSelect.error").removeClass("error");
    // + 入力ｺﾝﾄﾛｰﾙ
    $(element).find("input.error,select.error,textarea.error").removeClass("error");
}

/**
 *  指定エリアのエラー状態を初期化。(実行ボタン確認後初期化処理)
 *  @element {string} ：要素指定文字列
 */
function clearErrorClassForPassChange2(element) {

    //エラー情報をクリア
    setAttrByNativeJs(element, "title", "");
    $(element).find("input.errorcom").removeClass("errorcom");
}

/**
 *  指定エリアのエラー状態を初期化。(個別クラスのみ)
 *  @element {string} ：要素指定文字列
 */
function clearErrorClassForPassChange3(element) {

    //エラー情報をクリア
    $(element).find("tr.error").removeClass("error");
}


/**
 *  指定エリアのエラー状態を初期化(.error/.errorcom)
 *  @element {string} ：要素指定文字列
 */
function clearErrorStatus(element) {

    // メッセージをクリア
    clearMessage();
    //指定エリアのエラー状態を初期化
    clearErrorClasses(element);

}

/**
 *  指定エリアのエラー状態を初期化。(.errorcom)
 *  @element {string} ：要素指定文字列
 */
function clearErrorcomStatus(element, targetArea) {

    // メッセージをクリア
    clearMessage(targetArea);

    //指定エリアのエラー状態を初期化
    clearErrorcomClass(element);
}

/**
 *  指定エリアのエラー状態を初期化。(.error)
 *  @element {string} ：要素指定文字列
 */
function clearErrorStatus3(element) {

    // メッセージをクリア
    clearMessage();

    //指定エリアのエラー状態を初期化
    clearErrorClass(element);

}

/**
 * エラー詳細を削除
 * @element {string} ：要素指定文字列
 */
function clearErrorDetail(element) {

    $(element).find("label.errorcom").remove();
}

/**
 *  メッセージエリアにLOGNOメッセージを追加します。
 *  @logNo {string} ：LOGNO文字列
 *  @messagetype {number} ：0:(通常),9(エラー)
 */
function addMessageLogNo(logNo, messagetype) {
    if (logNo == null || logNo.length <= 0) {
        return;
    }

    //出力ﾒｯｾｰｼﾞを生成
    //var msgStr = "=== 問い合わせ番号[" + logNo + "] ===";
    var msgStr = "=== " + P_ComMsgTranslated[911200005] + "[" + logNo + "] ===";
    addMessage(msgStr, messagetype);
}

/**
 *  メッセージポップアップ表示
 *  @messageStr    {array} ：メッセージ文字列
 *  @type          {number} ：0(お知らせ),1(警告),2(エラー),3(確認)
 *  @eventFunc     {eventHandler} ：処理継続用ｺｰﾙﾊﾞｯｸ関数ｲﾍﾞﾝﾄﾊﾝﾄﾞﾗｰ
 *  @btn           {html} btn : ﾎﾞﾀﾝ要素
 */
function popupMessage(messageStr, type, eventFunc, btn) {

    // ボタン要素が存在する場合、非活性に
    if (btn != null) {
        $(btn).prop("disabled", true);
    }

    if (messageStr == null || messageStr.length <= 0 || messageStr[0].length <= 0) {
        //※メッセージ未設定、または業務ロジックでメッセージ未設定とした場合

        //メッセージを表示せずに処理を続行
        if (eventFunc != null) {
            eventFunc();
        }
        return false;
    }

    var messageDiv = $("#ComMessage_div");

    //表示ﾒｯｾｰｼﾞをｸﾘｱ⇒設定
    $(messageDiv).children().remove();
    $.each(messageStr, function () {
        $('<div>').html(this.replace(/\r?\n/g, '<br />')).appendTo($(messageDiv));
        //$('<div>').text(this).appendTo($(messageDiv));
    });
    messageDiv = null;

    //「OK」ﾎﾞﾀﾝ制御
    var btnOK = $("#ComMessageOK");
    $(btnOK).off("click");
    if (eventFunc != null) {
        //※bootstrapのﾎﾞﾀﾝｸﾘｯｸｲﾍﾞﾝﾄが発動されなくなるため、onclickは使用しない
        //$(btnOK).on("click", eventFunc);

        //※上記でうまくいかない場合は…
        //  少し待ってみる！？
        $(btnOK).on("click", function () {
            setTimeout(eventFunc, 1000);
        });

    }
    btnOK = null;

    //「ｷｬﾝｾﾙ」ﾎﾞﾀﾝ制御
    var btnCancel = $("#ComMessageCancel");
    switch (type) {
        case messageType.Confirm:   //確認
            $(btnCancel).removeClass("hide");     //ﾎﾞﾀﾝ表示
            break;
        default:
            $(btnCancel).addClass("hide");        //ﾎﾞﾀﾝ非表示
            break;
    }
    $(btnCancel).off("click");
    $(btnCancel).on("click", function () {
        var cancelFunc = function () {
            //【オーバーライド用関数】キャンセル押下後の個別実装
            clickPopupCancelBtn();
        }
        setTimeout(cancelFunc, 1000);
        // ボタン要素が存在する場合、活性に
        if (btn != null) {
            $(btn).prop("disabled", false);
        }
    });
    btnCancel = null;

    //ﾒｯｾｰｼﾞﾎﾟｯﾌﾟｱｯﾌﾟ表示
    $('#messageModal').modal();
    // 二重ﾎﾟｯﾌﾟｱｯﾌﾟの場合、z-index制御用クラスに変換
    var backdrop = $('.modal-backdrop');
    if ($(backdrop).length > 1) {
        var backdrop2 = $('.modal-backdrop:last');
        $(backdrop2).addClass("modal-backdrop2");
        $(backdrop2).removeClass("modal-backdrop");
        // モーダルがたくさんある場合があるのでzIndexを動的に取得
        var zIndex = getMaxZindex();
        $('#messageModal').css({ 'cssText': "z-index:" + zIndex + "!important;" });
    }
    backdrop = null;

    var uploadModal = $("#fileUploadModal");
    if (uploadModal.length <= 0) {
        //※確認ﾒｯｾｰｼﾞを複数回表示する場合のおまじない
        $('#messageModal').off('hidden.bs.modal');
        $('#messageModal').on('hidden.bs.modal', function (e) {
            //$('.modal-backdrop').remove();
        });
    }
    uploadModal = null;

    return false;
}

/**
 *  ﾓｰﾀﾞﾙ表示状態をﾁｪｯｸする
 *  
 *  @return     {bool} ：true(表示中)
 */
function checkMessageModalOpen() {
    //ﾓｰﾀﾞﾙ表示中か？
    if ($(document.body).hasClass('modal-open')) {
        return true;
    }

    if ($('#messageModal').hasClass('in')) {
        return true;
    }

    var backdrop = $('.modal-backdrop');
    if (backdrop.length > 0) {
        return true;
    }
    backdrop = null;

    return false;
}

/**
 * ﾎﾞﾀﾝﾒｯｾｰｼﾞ取得
 * @param {html} btn : ﾎﾞﾀﾝ要素
 */
function getBtnMessage(btn) {
    var btnMsg = null;
    var btnMsgId = $(btn).data("message");
    if (btnMsgId) {
        if (btnMsgId in P_ComMsgTranslated && P_ComMsgTranslated[btnMsgId].length > 0) {
            btnMsg = P_ComMsgTranslated[btnMsgId];
        }
        else {
            btnMsg = btnMsgId;
        }
    }
    return btnMsg;
}

/**
 * ﾊﾟﾌﾞﾘｯｸ変数にﾒｯｾｰｼﾞをｾｯﾄ
 * @param {button} btn          :ﾎﾞﾀﾝ要素
 * @param {number} confirmKbn   :確認ﾒｯｾｰｼﾞ表示区分（confirmKbnDef）
 */
function setMessageStrForBtn(btn, confirmKbn) {

    if (confirmKbn == null || confirmKbn.length <= 0) {
        confirmKbn = confirmKbnDef.Disp;    //確認ﾒｯｾｰｼﾞ表示する
    }
    P_MessageStr = "";
    if (confirmKbn == confirmKbnDef.Disp) {
        //※確認ﾒｯｾｰｼﾞ表示する

        //ﾃﾞﾌｫﾙﾄ確認ﾒｯｾｰｼﾞを設定
        //『～します。よろしいですか？』
        P_MessageStr = $(btn).val() + P_ComMsgTranslated[941120008];
        //ﾎﾞﾀﾝﾒｯｾｰｼﾞがあれば置き換え
        var btnMsg = getBtnMessage(btn);
        if (btnMsg != null) {
            P_MessageStr = btnMsg;
        }
    }
}

/**
 * Loading イメージ表示
 *  @param {string} msg：画面に表示する文言
 */
function dispLoading(msg) {
    // 未設定の場合
    if (msg == null) {
        // 処理中...　しばらくお待ちください。
        msg = P_ComMsgTranslated[941120003];
    }

    // 画面表示メッセージ
    //===== GIF版の場合 ↓を有効に =====//
    //var dispMsg = "<div class='loadingMsg'>" + msg + "</div>";
    //===== GIF版の場合 ↑を有効に =====//

    //===== CSS版の場合 ↓を有効に =====//
    var msg_icon = "<div class='loadingMsgStr'>" + msg + "</div>";
    msg_icon += "<div class='loadingIcon'><i class='fa fa-spinner fa-pulse fa-2x fa-fw'/></div>";
    var dispMsg = "<div class='loadingMsg'>" + msg_icon + "</div>";
    //===== CSS版の場合 ↑を有効に =====//

    // ローディング画像が表示されていない場合のみ出力
    if ($("#loading").length == 0) {
        $($("#container")).append("<div id='loading'>" + dispMsg + "</div>");
    }
}

/**
 * Loading イメージ削除
 */
function removeLoading() {
    var removeFunc = function () {
        $("#loading").remove();
        //console.timeEnd('javascript');
        //console.timeEnd('outputList');
    }
    // 実行タイミングを描画完了後になるようずらす
    setTimeout(removeFunc, 0);
}

/*
* メッセージに引数を設定する処理
* @message {string} :メッセージ(P_ComMsgTranslated.ID(例： P_ComMsgTranslated.MS00073))
* @arrParam {array[string]}:引数の配列、順番に0から設定
*/
function getMessageParam(message, arrParam) {
    var returnMessage = message;
    for (var i = 0; i < arrParam.length; i++) {
        returnMessage = returnMessage.replace('{' + i.toString() + '}', arrParam[i]);
    }
    return returnMessage;
}