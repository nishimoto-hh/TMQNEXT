/* ========================================================================
 *  機能名　    ：   【SP0001】予備品検索
 * ======================================================================== */

// 一覧画面の定義
const SP0001_FormList = {
    No: 0,                           // 画面番号
    Id: "CBODY_020_00_LST_0_SP0001", // 検索結果一覧
    PartsName: 2,                    // 予備品名
    StandardSize: 3,                 // 規格・寸法
    ManufacturerID: 4,               // メーカID
    ManufacturerName: 5,             // メーカ名
    PartsId: 6                       // 予備品ID
};

// グローバルリストのキー名称
const SP0001_GlobalListKey = {
    PartsId: "SP0001_PartsId",             // 予備品ID(ユーザID)
    PartsName: "SP0001_PartsName",         // 予備品名(ユーザ表示名)
    StandardSize: "SP0001_StandardSize", // 規格・寸法
    ManufacturerId: "SP0001_ManufacturerId", // メーカID
    ManufacturerName: "SP0001_ManufacturerName", // メーカ名
    ConditionMachineId: "SP0001_ConditionMachineId", // 機番ID(検索条件)
    TargetCtrlId: "SP0001_TargetCtrlId", // 呼び元画面の予備品のコントロールID
    TargetCtrlNo: "SP0001_TargetCtrlNo", // 呼び元画面の予備品のコントロール番号
    TargetCtrlRow: "SP0001_TargetCtrlRow"  // 呼び元画面の予備品の行
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
function SP0001_clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, fromConductId) {

    // 機能IDが「予備品検索」ではない場合は何もしない
    if (cmConductId != SP0001_ConductId) {
        return;
    }    

    // 選択された行の値を取得
    var PartsId = $(cmArticle.context).parents(".tabulator-row").find("div[tabulator-field=VAL" + SP0001_FormList.PartsId + "] span")[0].innerText;     // 予備品ID
    var PartsName = $(cmArticle.context).parents(".tabulator-row").find("div[tabulator-field=VAL" + SP0001_FormList.PartsName + "] span")[0].innerText; // 予備品名
    var StandardSize = $(cmArticle.context).parents(".tabulator-row").find("div[tabulator-field=VAL" + SP0001_FormList.StandardSize + "] span")[0].innerText; // 規格・寸法
    var ManufacturerID = $(cmArticle.context).parents(".tabulator-row").find("div[tabulator-field=VAL" + SP0001_FormList.ManufacturerID + "] span")[0].innerText; // メーカID
    var ManufacturerName = $(cmArticle.context).parents(".tabulator-row").find("div[tabulator-field=VAL" + SP0001_FormList.ManufacturerName + "] span")[0].innerHTML;; // メーカ名

    // 取得した値をグローバルリストに格納
    operatePdicIndividual(SP0001_GlobalListKey.PartsId, false, PartsId);           // 予備品ID
    operatePdicIndividual(SP0001_GlobalListKey.PartsName, false, PartsName);       // 予備品名
    operatePdicIndividual(SP0001_GlobalListKey.StandardSize, false, StandardSize);       // 規格・寸法
    operatePdicIndividual(SP0001_GlobalListKey.ManufacturerID, false, ManufacturerID);       // メーカID
    operatePdicIndividual(SP0001_GlobalListKey.ManufacturerName, false, ManufacturerName);       // メーカ名
    operatePdicIndividual(SP0001_GlobalListKey.TargetCtrlId, false, fromCtrlId); // 呼び元画面の予備品のコントロールID
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
function SP0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom) {

    // 機能IDが「予備品検索」ではない場合は何もしない
    if (backFrom != SP0001_ConductId) {
        return;
    }

    // 予備品のコントロールを取得
    var targetCtrlId = P_dicIndividual[SP0001_GlobalListKey.TargetCtrlId];
    var targetCtrlNo = P_dicIndividual[SP0001_GlobalListKey.TargetCtrlNo];
    var targetCtrlRow = P_dicIndividual[SP0001_GlobalListKey.TargetCtrlRow];
    if (targetCtrlId) {
        // 呼び元のコントロールIDに値を設定（チェンジイベント内で予備品名(ユーザ表示名)を設定する）

        // 予備品ID
        //document.getElementById(targetCtrlId + getAddFormNo() + "VAL1E").value = P_dicIndividual[SP0001_GlobalListKey.PartsId];
        //$(document.getElementById(targetCtrlId + getAddFormNo() + "VAL1E")).trigger('change');
        setValueAndTrigger(targetCtrlId, targetCtrlNo, 1, CtrlFlag.TextBox, P_dicIndividual[SP0001_GlobalListKey.PartsId], true, false);// 予備品ID
        setValue(targetCtrlId, Number(targetCtrlNo) + 1, 1, CtrlFlag.Label, P_dicIndividual[SP0001_GlobalListKey.StandardSize], true, false);// 規格・寸法
        setValue(targetCtrlId, Number(targetCtrlNo) + 2, 1, CtrlFlag.Label, P_dicIndividual[SP0001_GlobalListKey.ManufacturerName], true, false);// メーカ
    }

    // グローバルリストから値を削除
    operatePdicIndividual(SP0001_GlobalListKey.PartsId, true);       // 予備品ID(ユーザID)
    operatePdicIndividual(SP0001_GlobalListKey.PartsName, true);     // 予備品名(ユーザ表示名)
    operatePdicIndividual(SP0001_GlobalListKey.StandardSize, true);     // 予備品名(ユーザ表示名)
    operatePdicIndividual(SP0001_GlobalListKey.ManufacturerId, true);     // 予備品名(ユーザ表示名)
    operatePdicIndividual(SP0001_GlobalListKey.ManufacturerName, true);     // 予備品名(ユーザ表示名)
    operatePdicIndividual(SP0001_GlobalListKey.TargetCtrlId, true); // 呼び元画面の予備品のコントロールID
    operatePdicIndividual(SP0001_GlobalListKey.TargetCtrlNo, true); // 呼び元画面の予備品のコントロール番号
    operatePdicIndividual(SP0001_GlobalListKey.TargetCtrlRow, true); // 呼び元画面の予備品のコントロール番号
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
function SP0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId, machineId = null) {

    // 機能IDが「予備品検索」ではない場合は何もしない
    if (conductId != SP0001_ConductId) {
        return;
    }

    // 予備品検索画面を開いたコントロールの項目番号を取得
    var ctrlNo = $(element)[0].closest("td").dataset.name.replace("VAL", "");
    // 取得した値をグローバルリストに格納
    operatePdicIndividual(SP0001_GlobalListKey.TargetCtrlNo, false, ctrlNo); // 呼び元画面の予備品のコントロール番号

    // 取得した値をグローバルリストに格納
    operatePdicIndividual(SP0001_GlobalListKey.TargetCtrlRow, false, rowNo); // 呼び元画面の予備品の行番号

    if (machineId) {
        // 取得した値をグローバルリストに格納
        operatePdicIndividual(SP0001_GlobalListKey.ConditionMachineId, false, machineId); // 呼び元画面の予備品のコントロール番号
    }
}
