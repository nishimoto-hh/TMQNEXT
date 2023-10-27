/* ========================================================================
 *  機能名　    ：   【HM0003】申請状況変更画面
 * ======================================================================== */

const HM0003_Form = {
    No: 0 // 画面番号
    , Id: "CBODY_000_00_LST_0" // 理由入力一覧
    , ApplicationReason: 1 //申請理由
    , RejectionReason: 2 //否認理由
    , Flg: 3 // 表示制御用フラグ（承認依頼の場合true、否認の場合false）
    , Button: {
        Request: "Request" // 承認依頼
        , Denial: "Denial" // 否認
    }
};

/*
 *  初期化処理(表示中画面用)
 *  @param appPath   　　 ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId 　　 ：機能ID
 *  @param formNo 　　　　：画面番号
 *  @param articleForm    ：表示画面ｴﾘｱ要素
 *  @param curPageStatus  ：画面表示ｽﾃｰﾀｽ
 *  @param actionCtrlId   :Action(ﾎﾞﾀﾝなど)CTRLID
 *  @param data           :初期表示ﾃﾞｰﾀ
 */
function HM0003_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    if (conductId != ConductId_HM0003) {
        // 申請状況変更画面でない場合は終了
        return;
    }

    // 表示制御用フラグ取得（承認依頼の場合true、否認の場合false）
    var flg = convertStrToBool(getValue(HM0003_Form.Id, HM0003_Form.Flg, 1, CtrlFlag.Label));
    // 項目、ボタンの表示制御
    setHideColumnAndButton(flg);

    //フォーカス設定
    setFocusDelay(HM0003_Form.Id, flg ? HM0003_Form.ApplicationReason : HM0003_Form.RejectionReason, 1, CtrlFlag.Label)
}

/**
 * 項目、ボタンの表示制御
 * @param {any} flg 承認依頼の場合true、否認の場合false
 */
function setHideColumnAndButton(flg) {
    //申請理由 表示
    changeColumnDisplay(HM0003_Form.Id, HM0003_Form.ApplicationReason, flg);
    //否認理由 非表示
    changeColumnDisplay(HM0003_Form.Id, HM0003_Form.RejectionReason, !flg);
    //承認依頼ボタン 表示
    setHideButton(HM0003_Form.Button.Request, !flg);
    //否認ボタン 非表示
    setHideButton(HM0003_Form.Button.Denial, flg);

    //画面タイトル
    var title = flg ? P_ComMsgTranslated[111120227] : P_ComMsgTranslated[111270036];
    $(P_Article).parents(".modal-content").find(".modal-header-my .title_div span").text(title);
}

/**
 * 【オーバーライド用関数】実行正常終了後処理
 *  @param {string}                     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}                     conductId   ：機能ID
 *  @param {string}                     pgmId       ：プログラムID
 *  @param {number}                     formNo      ：画面NO
 *  @param {html}                       btn         ：ボタン要素
 *  @param {number}                     conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param {boolean}                    autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param {boolean}                    isEdit      ：単票表示フラグ
 *  @param {List<Dictionary<string>>}   data        ：結果ﾃﾞｰﾀ
 */
function HM0003_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {
    if (conductId != ConductId_HM0003) {
        // 申請状況変更画面でない場合は終了
        return;
    }

    // 登録ボタン実行正常終了後画面を閉じて遷移元に移動
    var modal = $(btn).closest('section.modal_form');
    $(modal).modal('hide');
}

/**
 *【オーバーライド用関数】
 *  画面再表示ﾃﾞｰﾀ取得関数呼出前
 *
 *  @appPath {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId {string} ：機能ID
 *  @pgmId {string} ：ﾌﾟﾛｸﾞﾗﾑID
 *  @formNo {number} ：画面番号
 *  @originNo {number} ：遷移元の親画面番号
 *  @btnCtrlId {number} ：ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
 *  @conductPtn {number} ：機能処理ﾊﾟﾀｰﾝ(10:入力、11：バッチ、20：帳票、30：マスタ)
 *  @selectData {number} ：NOﾘﾝｸ選択行のﾃﾞｰﾀ {List<Dictionary<string, object>>}
 *  @targetCtrlId {number} ：単票入力画面から戻る時、該当一覧のCTRLID
 *  @listData {} ：
 *  @codeTransFlg {int}    ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 *  @status  {CommonProcReturn} : 実行処理結果ｽﾃｰﾀｽ
 *  @param	{number}	selFlg : 共通機能から選択ボタン押下で戻った場合のみ、「1:selFlgDef.Selected」が渡る
 *  @param	{string}	backFrom : 共通機能からの戻る処理時、戻る前の共通機能ID ※他機能同タブ遷移でも使える？
 */
function HM0003_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom) {

    // 共通画面を閉じた場合、指定した画面ならば再検索を行う
    InitFormDataByCommonModal(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
}