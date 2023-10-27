/* ========================================================================
 *  機能名　    ：   【SU0001】担当者検索
 * ======================================================================== */

// 一覧画面の定義
const SU0001_FormList = {
    No: 0,                           // 画面番号
    Id: "CBODY_020_00_LST_0_SU0001", // 検索結果一覧
    UserId: 3,                       // 担当者ID(ユーザID)
    UserName: 2                      // 担当者名(ユーザ表示名)
};

// グローバルリストのキー名称
const SU0001_GlobalListKey = {
    UserId: "SU0001_UserId",             // 担当者ID(ユーザID)
    UserName: "SU0001_UserName",         // 担当者名(ユーザ表示名)
    TargetCtrlId: "SU0001_TargetCtrlId", // 呼び元画面の担当者のコントロールID
    TargetCtrlNo: "SU0001_TargetCtrlNo"  // 呼び元画面の担当者のコントロール番号
}

/**
 *  共通機能用選択ボタン押下時処理
 *  @param {string}     appPath        :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}     cmConductId    :共通機能ID
 *  @param {article}    cmArticle      :共通機能articleタグ
 *  @param {button}     btn            :選択ボタン要素
 *  @param {string}     fromCtrlId     :共通機能遷移時に押下したﾎﾞﾀﾝ/一覧のｺﾝﾄﾛｰﾙID
 *  @param {string}     fromConductId  :共通機能遷移時に押下したﾎﾞﾀﾝの機能ID
 */
function SU0001_clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, fromConductId) {

    // 機能IDが「担当者検索」ではない場合は何もしない
    if (cmConductId != SU0001_ConsuctId) {
        return;
    }    

    // 選択された行の値を取得
    var userId = $(cmArticle.context).parents(".tabulator-row").find("div[tabulator-field=VAL" + SU0001_FormList.UserId + "] span")[0].innerText;     // 担当者ID(ユーザID)
    var userName = $(cmArticle.context).parents(".tabulator-row").find("div[tabulator-field=VAL" + SU0001_FormList.UserName + "] span")[0].innerText; // 担当者名(ユーザ表示名)

    // 取得した値をグローバルリストに格納
    operatePdicIndividual(SU0001_GlobalListKey.UserId, false, userId);           // 担当者ID(ユーザID)
    operatePdicIndividual(SU0001_GlobalListKey.UserName, false, userName);       // 担当者名(ユーザ表示名)
    operatePdicIndividual(SU0001_GlobalListKey.TargetCtrlId, false, fromCtrlId); // 呼び元画面の担当者のコントロールID
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
function SU0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom) {

    // 機能IDが「担当者検索」ではない場合は何もしない
    if (backFrom != SU0001_ConsuctId) {
        return;
    }

    // 担当者のコントロールを取得
    var targetCtrlId = P_dicIndividual[SU0001_GlobalListKey.TargetCtrlId];
    var targetCtrlNo = P_dicIndividual[SU0001_GlobalListKey.TargetCtrlNo];
    if (targetCtrlId) {
        // 呼び元のコントロールIDに値を設定（チェンジイベント内で担当者名(ユーザ表示名)を設定する）
        setValueAndTrigger(targetCtrlId, targetCtrlNo, 1, CtrlFlag.TextBox, P_dicIndividual[SU0001_GlobalListKey.UserId]);// 担当者ID(ユーザID)
    }

    // グローバルリストから値を削除
    operatePdicIndividual(SU0001_GlobalListKey.UserId, true);       // 担当者ID(ユーザID)
    operatePdicIndividual(SU0001_GlobalListKey.UserName, true);     // 担当者名(ユーザ表示名)
    operatePdicIndividual(SU0001_GlobalListKey.TargetCtrlId, true); // 呼び元画面の担当者のコントロールID
    operatePdicIndividual(SU0001_GlobalListKey.TargetCtrlNo, true); // 呼び元画面の担当者のコントロール番号
}

/**
 *【オーバーライド用関数】
 * 共通機能へデータを渡す
 * @param {string}                      appPath         :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {number}                      conductId       :共通機能ID
 * @param {number}                      parentNo        :親画面NO
 * @param {Array.<Dictionary<string, string>>}  conditionDataList   :条件ﾃﾞｰﾀ
 * @param {string}                      ctrlId          :遷移元の一覧ctrlid
 * @param {string}                      btn_ctrlId      :ボタンのbtn_ctrlid
 * @param {number}                      rowNo           :遷移元の一覧の選択行番号（一覧行でない場合は-1）
 * @param {Element}                     element         :ｲﾍﾞﾝﾄ発生要素
 * @param {string}                      parentConductId :遷移元の個別機能ID
 */
function SU0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId) {

    // 機能IDが「担当者検索」ではない場合は何もしない
    if (conductId != SU0001_ConsuctId) {
        return;
    }

    // 担当者検索画面を開いたコントロールの項目番号を取得
    var ctrlNo = $(element)[0].closest("td").dataset.name.replace("VAL", "");

    // 取得した値をグローバルリストに格納
    operatePdicIndividual(SU0001_GlobalListKey.TargetCtrlNo, false, ctrlNo); // 呼び元画面の担当者のコントロール番号
}