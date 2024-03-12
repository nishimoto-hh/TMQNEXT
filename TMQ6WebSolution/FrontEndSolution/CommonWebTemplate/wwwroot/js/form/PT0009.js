/* ========================================================================
 *  機能名　    ：   【PT0009】会計帳票出力
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0009\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
// 帳票出力　抽出条件 コントロール項目番号
const PT0009_FormCondition = {
    Id: "BODY_000_00_LST_0",                // 出力条件一覧
    TargetMonth: 1,                         // 対象年月
    OutputReport: 20,                       // 出力帳票
    OutputReportName: 21,                   // 出力帳票名
    ReportIdRP0270: "RP0270"                // 帳票ID（会計提出表）
}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function reportCheckPre(appPath, conductId, formNo, btn) {

    //エラー状態を初期化
    clearErrorComStatusForAreas(true);

    //明細ｴﾘｱの入力ﾁｪｯｸ
    var listValid = validListData();
    if (!listValid) {
        //入力ｴﾗｰ
        return false;
    }

    //関連チェック
    //帳票名が会計提出表で、対象年月が未入力の場合、エラーとする

    //2024/03/07 一時的に会計提出表の場合の対象年月の必須を解除する 後で元に戻すこと
    //var tdOutputReport = $(P_Article).find("#" + PT0009_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + PT0009_FormCondition.OutputReport + "']");
    //var selectOutputReport = $(tdOutputReport).find("select");
    //if (selectOutputReport != null && selectOutputReport.length > 0) {
    //    var selVal = getCellVal(tdOutputReport);
    //    if (selVal == PT0009_FormCondition.ReportIdRP0270)
    //    {
    //        var tdTargetMonth = $(P_Article).find("#" + PT0009_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + PT0009_FormCondition.TargetMonth + "']");
    //        if (tdTargetMonth != null)
    //        {
    //            selVal = getCellVal(tdTargetMonth);
    //            if (selVal == "")
    //            {
    //                // 帳票コンボの選択名称を取得
    //                var selectValueName = getCellVal(tdOutputReport, 1);
    //                // 対象年月の見出しを取得
    //                var thTargetMonth = $(P_Article).find("#" + PT0009_FormCondition.Id + getAddFormNo()).find("th[data-name='VAL" + PT0009_FormCondition.TargetMonth + "']");
    //                var titleName = getCellVal(thTargetMonth);

    //                //入力ｴﾗｰ
    //                // {0}の場合、{1}を入力してください。
    //                var arrayParam = [2];
    //                arrayParam[0] = selectValueName;
    //                arrayParam[1] = titleName;
    //                strMessage = getMessageParam(P_ComMsgTranslated[941250003], arrayParam);
    //                setMessage(strMessage, procStatus.Error);
    //                // 入力してください。
    //                var element = $(tdTargetMonth).find("input");
    //                dispErrorDetailDetail(element, P_ComMsgTranslated[941220009], null, false)
    //                return false;
    //            }
    //        }
    //    }
    //}


    return true;
}

/*==94:初期化処理==*/
/**
 * 【オーバーライド用関数】
 * 　初期化処理(表示中画面用)
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId {string} 　　　　     ：機能ID
 *  @formNo {byte}　　　　　　　     ：画面番号
 *  @articleForm {article要素}       ：表示画面ｴﾘｱ要素
 *  @curPageStatus {定数：pageStatus}：画面表示ｽﾃｰﾀｽ
 *  @actionCtrlId {string} 　　　　　：Action(ﾎﾞﾀﾝなど)CTRLID
 *  @data {List<Dictionary<string, object>>}    ：初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    // 帳票コンボボックスに対してイベントの設定
    var tdOutputReport = $(P_Article).find("#" + PT0009_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + PT0009_FormCondition.OutputReport + "']");
    var selectOutputReport = $(tdOutputReport).find("select");
    if (selectOutputReport != null && selectOutputReport.length > 0) {
        $(selectOutputReport).off('change.1');
        $(selectOutputReport).on('change.1', function () {
            // 帳票コンボの名称を取得して隠し項目に設定
            var selectValueName = changeVal = getCellVal(tdOutputReport, 1);
            var tdOutputReportName = $(P_Article).find("#" + PT0009_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + PT0009_FormCondition.OutputReportName + "']");
            $(tdOutputReportName).text(selectValueName);
        });
    }
}

