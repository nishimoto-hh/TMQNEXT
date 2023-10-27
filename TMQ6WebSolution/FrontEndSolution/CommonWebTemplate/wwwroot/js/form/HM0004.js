/* ========================================================================
 *  機能名　    ：   【HM0004】変更管理帳票出力画面
 * ======================================================================== */

const HM0004_Form = {
    No: 0 // 画面番号
    , GroupNo: {
        History: 1000
        , Machine: 1003
        , Plan: 1004
        , Hide: 1005
    }
    , HideInfo: {
        //非表示項目
        Id: "CCOND_080_00_LST_0"
        , ConductCode: 1 //対象機能（1：機器台帳、2：長期計画）
    }
    , Condition: {
        //変更管理
        Id: "CCOND_000_00_LST_0"
        , ApplicationStatus: 1 //申請状況
    }
    , Button: {
        Output: "Output" // 出力
        , Back: "Back" // 戻る
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
function HM0004_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    if (conductId != ConductId_HM0004) {
        // 帳票出力画面でない場合は終了
        return;
    }

    // 対象機能取得
    var conductCode = getValue(HM0004_Form.HideInfo.Id, HM0004_Form.HideInfo.ConductCode, 1, CtrlFlag.Label);
    if (conductCode == HistoryOutputDivision.HM0001) {
        //機器台帳から起動した場合、長期計画用の検索条件を非表示
        toggleHideGroup(HM0004_Form.GroupNo.Machine, false);
        toggleHideGroup(HM0004_Form.GroupNo.Plan, true);
        toggleHideGroupTitle(HM0004_Form.GroupNo.Machine, true);
    } else {
        //長期計画から起動した場合、機器台帳用の検索条件を非表示
        toggleHideGroup(HM0004_Form.GroupNo.Machine, true);
        toggleHideGroup(HM0004_Form.GroupNo.Plan, false);
        toggleHideGroupTitle(HM0004_Form.GroupNo.Plan, true);
    }
    //折りたたみアイコンを消す
    toggleHideGroupTitle(HM0004_Form.GroupNo.History, true);
    toggleHideGroupTitle(HM0004_Form.GroupNo.Hide, true);

    //フォーカス設定
    setFocusDelay(HM0004_Form.Condition.Id, HM0004_Form.Condition.ApplicationStatus, 1, CtrlFlag.Label)
}
