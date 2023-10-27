/* ========================================================================
 *  機能名　    ：   【共通・カスタムコントロール共通のJavaScript】
 * ======================================================================== */
/** Public定数：カスタムコントロールID接頭語(チャート)*/
const P_Prefix_Custom_Chart = "Custom_Chart";

/**
 *  各種カスタムコントロールの初期化処理
 *  @param {string} appPath     : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} conductId   : 機能ID
 *  @param {string} pgmId       : ﾌﾟﾛｸﾞﾗﾑID
 *  @param {number} formNo      : 画面番号
 */
function initCustomControls(appPath, target) {
    if (!target) {
        target = '';
    } else {
        target += ' ';
    }

    // カスタムコントロールの生成処理

//    // ガントチャート(呼び出し例)
//    var customDivs = $(target + 'div[id^="' + P_Prefix_Custom_Chart + '"]');
//    if (customDivs != null && customDivs.length > 0) {
//        $.each(customDivs, function (idx, div) {
//            var conductId = getConductId();
//            var formNo = getFormNo();
//            // ガントチャートの生成処理
//            createChartControl(appPath, conductId, formNo, div);
//        });
//    }
}

