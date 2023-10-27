/* ========================================================================
 *  機能名　    ：   【入庫入力画面】
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0005\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
// tmqcommon.jsが二重に呼び出されると定数の宣言などでエラーとなるので、既に呼び出されている場合は読み込まないように対応
var isExistsTmqCommon = false;
$.each(document.scripts, function (idx, value) {
    if (value.src.indexOf('tmqcommon') > 0) {
        isExistsTmqCommon = true;
    }
});
if (!isExistsTmqCommon) {
    document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
}

// 一覧画面 コントロール項目番号
const PT0005_FormList = {
    ConductId: "PT0005",                        // 機能ID
    No: 0,                                      // 画面番号
    PartsInfo: "CBODY_000_00_LST_5",            // 予備品情報
    InputArea1: {
        Id: "CBODY_010_00_LST_5",               // 入庫情報(入庫日～予備品倉庫)
        InoutDatetime: 1,                       // 入庫日
        StorageLocation: 2,                     // 予備品倉庫 
    },
    InputArea2: {
        Id: "CBODY_030_00_LST_5",               // 入庫情報(棚番)
        PartsLocationNo: 3,                     // 棚番
        PartsLocationDetailNo: 4,               // 棚枝番
        PartsStorageLocation: 5,                // 予備品倉庫ID
        PartsLocationId: 6,                     // 棚ID
    },
    InputArea3: {
        Id: "CBODY_040_00_LST_5",               // 入庫情報(新旧区分～仕入先ID)
        OldNewDivision: 5,                      // 新旧区分
        Department: 6,                          // 部門
        Account: 7,                             // 勘定科目
        ManagementDivision: 8,                  // 管理区分
        ManagementNo: 9,                        // 管理No.
        Vender: 10,                             // 仕入先
        StorageQuantity: 11,                    // 入庫数
        UnitPrice: 12,                          // 入庫単価
        StorageAmount: 13,                      // 入庫金額
        UnitName: 14,                           // 数量管理単位
        CurrencyName: 15,                       // 金額管理単位
        PartsId: 16,                            // 予備品IDID
        FormType: 17,                           // 画面タイプ
        WorkNo: 18,                             // 作業No
        UnitDigit: 19,                          // 小数点以下桁数(数量)
        CurrencyDigit: 20,                      // 小数点以下桁数(金額)
        RoundDivision: 21,                      // 丸め処理区分
        FactoryId: 22,                          // 工場ID
        DepartmentStructureId: 26,              // 部門ID
        AccountStructureId: 27,                 // 勘定科目ID
        AccountOldNewDivision: 28,              // 勘定科目の新旧区分
        VenderStructureId: 29,                  // 仕入先ID
    },
    InputArea4: {
        Id: "CBODY_050_00_LST_5",               // 入庫情報(棚枝番)
        PartsLocationDetailNo: 1,               // 棚枝番
    },
    InputArea5: {
        Id: "CBODY_060_00_LST_5",               // 入庫情報(結合文字列)
        JoinStr: 1,                             // 結合文字列
    }
}

// ボタン名
const PT0005_BtnName = {
    Cancel: "Cancel",             // 取消
    Regist: "Regist",             // 登録
    Back: "Back"                  // 戻る
};

// 新旧区分
const PT0005_OldNewDivisionCd =
{
    New: 0,  // 新品
    Old: 1   // 中古品
};

// 勘定科目
const PT0005_AccountCd =
{
    Equipment: "B4140",  // 設備貯蔵品
    Old: "B4161"         // 中古貯蔵品
};

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
 *  @data {List<COM_TMPTBL_DATA>}    ：初期表示ﾃﾞｰﾀ
 */
function PT0005_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    // 機能IDが「出庫入力」ではない場合は何もしない
    if (getConductId() != PT0005_FormList.ConductId) {
        return;
    }

    // 画面タイプ取得
    var formType = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.FormType, 1, CtrlFlag.Label, false, false);

    // 新規の場合
    if (formType == PartsTransFlg.New) {
        // 取消ボタン非活性
        setDispMode(getButtonCtrl(PT0005_BtnName.Cancel), true);
    }
    // 参照の場合
    else if (formType == PartsTransFlg.Reference) {
        // 入力項目、登録、取消ボタンの非活性
        PT0005_changeDisabled();
    }

    // 結合文字列を棚枝番入力コントロールのヘッダーにセット
    var joinStr = getValue(PT0005_FormList.InputArea5.Id, PT0005_FormList.InputArea5.JoinStr, 1, CtrlFlag.Label, false, false);
    setValue(PT0005_FormList.InputArea4.Id, PT0005_FormList.InputArea4.PartsLocationDetailNo, 1, CtrlFlag.Label, joinStr, false, true);

    // 入庫金額計算
    PT0005_setStorageCalculation();

    // 入庫数からフォーカスアウトした場合
    var storageQuantity = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageQuantity, 1, CtrlFlag.TextBox, false, false);
    $(storageQuantity).blur(function () {
        // 入庫金額計算
        PT0005_setStorageCalculation();
    });

    // 入庫単価からフォーカスアウトした場合
    var unitPrice = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPrice, 1, CtrlFlag.TextBox, false, false);
    $(unitPrice).blur(function () {
        // 入庫金額計算
        PT0005_setStorageCalculation();
    });

    // ブラウザに処理が戻った際に実行
    setTimeout(function () {
        // 棚IDの変更時イベントを発生させる
        var partsLocation = getCtrl(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsLocationNo, 1, CtrlFlag.TextBox, false, false);
        changeNoEdit(partsLocation);
        // 勘定科目の変更時イベントを発生させ、翻訳を表示
        var account = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Account, 1, CtrlFlag.TextBox, false, false);
        changeNoEdit(account);
    }, 300); //300ミリ秒


    // 標準仕入先の値が「0」の場合は空にする(存在しない構成IDが入ってしまっているため)
    var venderId = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Vender, 1, CtrlFlag.TextBox, false, false);
    if (venderId == 0) {
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Vender, 1, CtrlFlag.TextBox, '', false, false);
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.VenderStructureId, 1, CtrlFlag.Label, '', false, false);
    }

    // 標準仕入先からフォーカスアウトした場合
    var vender = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Vender, 1, CtrlFlag.TextBox, false, false);
    $(vender).blur(function () {
        var venderCode = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Vender, 1, CtrlFlag.TextBox, false, false);
        // 入力されていない場合は非表示項目を空に設定する
        if (!venderCode || venderCode == '') {
            setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.VenderStructureId, 1, CtrlFlag.Label, '', false, false);
        }
    });

    // 登録ボタンにフォーカスをセット
    setFocusButton(PT0005_BtnName.Regist);
}

/**
 * 入力項目の非活性
 */
function PT0005_changeDisabled() {

    // 入庫日
    var inoutDatetime = getCtrl(PT0005_FormList.InputArea1.Id, PT0005_FormList.InputArea1.InoutDatetime, 1, CtrlFlag.Input);
    changeInputControl(inoutDatetime, false);
    // 予備品倉庫
    var storageLocation = getCtrl(PT0005_FormList.InputArea1.Id, PT0005_FormList.InputArea1.StorageLocation, 1, CtrlFlag.Combo);
    changeInputControl(storageLocation, false);
    // 棚番
    var partsLocationNo = getCtrl(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsLocationNo, 1, CtrlFlag.TextBox);
    changeInputControl(partsLocationNo, false);
    // 棚枝番
    var partsLocationDetailNo = getCtrl(PT0005_FormList.InputArea4.Id, PT0005_FormList.InputArea4.PartsLocationDetailNo, 1, CtrlFlag.TextBox);
    changeInputControl(partsLocationDetailNo, false);
    // 新旧区分
    var oldNewDivision = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.OldNewDivision, 1, CtrlFlag.Combo);
    changeInputControl(oldNewDivision, false);
    // 部門
    var department = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Department, 1, CtrlFlag.TextBox);
    changeInputControl(department, false);
    // 勘定科目
    var account = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Account, 1, CtrlFlag.TextBox);
    changeInputControl(account, false);
    // 管理区分
    var managementDivision = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.ManagementDivision, 1, CtrlFlag.TextBox);
    changeInputControl(managementDivision, false);
    // 管理No.
    var managementNo = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.ManagementNo, 1, CtrlFlag.TextBox);
    changeInputControl(managementNo, false);
    // 仕入先
    var vender = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Vender, 1, CtrlFlag.TextBox);
    changeInputControl(vender, false);
    // 入庫数
    var storageQuantity = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageQuantity, 1, CtrlFlag.TextBox);
    changeInputControl(storageQuantity, false);
    // 入庫単価
    var unitPrice = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPrice, 1, CtrlFlag.TextBox);
    changeInputControl(unitPrice, false);
    // 入庫金額
    var storageAmount = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageAmount, 1, CtrlFlag.Label);
    changeInputControl(storageAmount, false);

    // 登録ボタン
    setDispMode(getButtonCtrl(BtnName.Regist), true);
    // 取消ボタン
    setDispMode(getButtonCtrl(BtnName.Cancel), true);
}

/**
 * 入庫金額を計算して表示する
 */
function PT0005_setStorageCalculation() {

    // 入庫数取得
    var storageQuantity = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageQuantity, 1, CtrlFlag.TextBox, false, false).replace(/[^-0-9.]/g, '');
    // 入庫単価取得
    var unitPrice = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPrice, 1, CtrlFlag.TextBox, false, false).replace(/[^-0-9.]/g, '');
    // 金額管理単位取得
    var currencyName = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.CurrencyName, 1, CtrlFlag.Label, false, false).trim();
    // 小数点以下桁数(数量)
    var unitDigit = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitDigit, 1, CtrlFlag.Label, false, false);
    // 小数点以下桁数(金額)
    var currencyDigit = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.CurrencyDigit, 1, CtrlFlag.Label, false, false);
    // 丸め処理区分
    var roundDivision = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.RoundDivision, 1, CtrlFlag.Label, false, false);

    // 入庫数が入力されている場合
    if (!isNaN(storageQuantity) && storageQuantity != "") {
        // 入庫数の小数点以下桁数 丸め処理
        var storageQuantityDisp = roundDigit(storageQuantity, unitDigit, roundDivision);
        // 入庫数に設定
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageQuantity, 1, CtrlFlag.TextBox, storageQuantityDisp, false, false);
    }

    // 入庫単価が入力されている場合
    if (!isNaN(unitPrice) && unitPrice != "") {
        // 入庫単価の小数点以下桁数 丸め処理
        var unitPriceDisp = roundDigit(unitPrice, currencyDigit, roundDivision);
        // 入庫単価に設定
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPrice, 1, CtrlFlag.TextBox, unitPriceDisp, false, false);
    }

    // 入庫数、入庫単価が入力されている場合
    if ((!isNaN(storageQuantity) && storageQuantity != "") && (!isNaN(unitPrice) && unitPrice != "")) {
        // 入庫数 * 入庫単価
        var amount = roundDigit(storageQuantity * unitPrice, currencyDigit, roundDivision);
        // 入庫金額に金額と単位を設定
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageAmount, 1, CtrlFlag.Label, combineNumberAndUnit(amount, currencyName, true), false, false);
    } else {
        // 入庫金額に金額と単位を設定
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageAmount, 1, CtrlFlag.Label, combineNumberAndUnit("0", currencyName, true), false, false);
    }
}

/**
 *【オーバーライド用関数】
 *  画面状態設定後の個別実装
 *
 * @status {number}       ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
 * @pageRowCount {number} ：ﾍﾟｰｼﾞ全体のﾃﾞｰﾀ行数
 * @conductPtn {byte}     ：com_conduct_mst.ptn
 * @authShori {IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}>} ：ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
 */
function PT0005_setPageStatusEx(status, pageRowCount, conductPtn, authShori) {

    // 機能IDが「入庫入力」ではない場合は何もしない
    if (getConductId() != PT0005_FormList.ConductId) {
        return;
    }

    // 入庫数単位設定
    var storageQuantity = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.StorageQuantity, 1, CtrlFlag.TextBox, false, false);
    var storageQuantityUnit = $(storageQuantity).closest("div").find(".unit");
    $(storageQuantityUnit)[0].innerText = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitName, 1, CtrlFlag.Label, false, false).trim();

    // 入庫単価単位設定
    var unitPrice = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPrice, 1, CtrlFlag.Input, false, false);
    var unitPriceUnit = $(unitPrice).closest("div").find(".unit");
    $(unitPriceUnit)[0].innerText = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.CurrencyName, 1, CtrlFlag.Label, false, false).trim();
}

/**
 * 入庫入力画面の遷移前処理を行うかどうかの判定
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param formNo      ：遷移元formNo
 *  @param pctrlId      ：遷移元の一覧ctrlid
 *  @param btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param element     ：ｲﾍﾞﾝﾄ発生要素
 *  @returns {bool} Trueの場合はDM0002_prevTransFormの処理を行う、Falseの場合は他の画面に遷移する際の前処理
 */
function IsExecPT0005_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var result = ctrlId == PT0005_FormList.InputArea1.Id;
    return result;
}

/**
 *【オーバーライド用関数】
 *  遷移処理の前
 *
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param {number}     transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {number}     rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {Element}    element     ：ｲﾍﾞﾝﾄ発生要素
 *  @return {bool} 遷移を行わない場合はfalse、行う場合はtrue
 *  @return {Array.<Dictionary<string, object>>} 個別取得の遷移条件データ
 */
function PT0005_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    // 機能IDが「入庫入力」ではない場合は何もしない
    if (getConductId() != PT0005_FormList.ConductId) {
        return;
    }
    var conditionDataList = [];
    return [true, conditionDataList];
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
function PT0005_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

    // 機能IDが「入庫入力」ではない場合は何もしない
    if (conductId != PT0005_FormList.ConductId) {
        return;
    }

    // 登録・取消ボタン実行正常終了後画面を閉じて遷移元に移動
    var modal = $(btn).closest('section.modal_form');
    $(modal).modal('hide');
}

/**
 * 【オーバーライド用関数】
 *   コンボボックス変更時イベント
 * @param {string} appPath  : アプリケーションルートパス
 * @param {要素} combo      : 変更イベントが発生したコンボボックスの要素
 * @param {List} datas      : 変更イベントが発生したコンボボックスのデータリスト
 * @param {data} selected   : 変更されたコンボボックスの選択されたデータ
 * @param {byte} formNo     : イベントの発生した画面番号
 * @param {string} ctrlId   : イベントの発生した画面のコントロールID
 * @param {byte} valNo      : イベントの発生したコンボボックスのコントロール番号
 */
function PT0005_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {

    // 機能IDが「入庫入力」ではない場合は何もしない
    if (getConductId() != PT0005_FormList.ConductId) {
        return;
    }

    // 入庫情報の「予備品倉庫」コンボボックスが変更された場合
    if (ctrlId == PT0005_FormList.InputArea1.Id && valNo == PT0005_FormList.InputArea1.StorageLocation) {
        // 「予備品倉庫」コンボボックスの値(構成ID)を取得
        var partsStorageLocation = getValue(PT0005_FormList.InputArea1.Id, PT0005_FormList.InputArea1.StorageLocation, 1, CtrlFlag.Combo, false, false);
        // 取得した構成IDを棚情報の「予備品倉庫」にセット
        setValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsStorageLocation, 1, CtrlFlag.Label, partsStorageLocation, false, false);
    }

    // 入庫情報の「新旧区分」コンボボックスが変更された場合
    if (ctrlId == PT0005_FormList.InputArea3.Id && valNo == PT0005_FormList.InputArea3.OldNewDivision) {
        // 選択された要素の拡張項目の値を取得
        if (selected.EXPARAM1 == PT0005_OldNewDivisionCd.New) {
            // 「新品」の場合、勘定項目に「設備貯蔵品」をセット
            setValueAndTrigger(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Account, 1, CtrlFlag.TextBox, PT0005_AccountCd.Equipment, false, false);
        } else if (selected.EXPARAM1 == PT0005_OldNewDivisionCd.Old) {
            // 「中古品」の場合、勘定項目に「中古貯蔵品」をセット
            setValueAndTrigger(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Account, 1, CtrlFlag.TextBox, PT0005_AccountCd.Old, false, false);
        }
        // 新旧区分にフォーカスをセット
        $(combo).focus();
    }
}

/**
 * 【オーバーライド用関数】
 * 　コード＋翻訳 他の項目に値セット処理
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @formNo {byte}                   ：画面番号
 *  @ctrl {要素}   　　              ：コード＋翻訳要素
 *  @data {List}                     ：取得データ
 */
function PT0005_setCodeTransOtherNames(appPath, formNo, ctrl, data) {

    // 機能IDが「入庫入力」ではない場合は何もしない
    if (getConductId() != PT0005_FormList.ConductId) {
        return;
    }

    // オートコンプリートでセットされたコントロールIDを判定
    if (ctrl[0].id == PT0005_FormList.InputArea2.Id + getAddFormNo() + "VAL" + PT0005_FormList.InputArea2.PartsLocationNo && data && data.length > 0) {
        // 棚番選択時、構成IDを非表示の項目に設定する
        setValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsLocationId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
    } else if (ctrl[0].id == PT0005_FormList.InputArea3.Id + getAddFormNo() + "VAL" + PT0005_FormList.InputArea3.Department && data && data.length > 0) {
        // 部門選択時、構成IDを非表示の項目に設定する
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.DepartmentStructureId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
    } else if (ctrl[0].id == PT0005_FormList.InputArea3.Id + getAddFormNo() + "VAL" + PT0005_FormList.InputArea3.Account && data && data.length > 0) {
        // 勘定科目選択時、構成IDを非表示の項目に設定する
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.AccountStructureId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
        // 勘定科目選択時、新旧区分を非表示の項目に設定する
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.AccountOldNewDivision, 1, CtrlFlag.Label, data[0].EXPARAM2, false, false);
    } else if (ctrl[0].id == PT0005_FormList.InputArea3.Id + getAddFormNo() + "VAL" + PT0005_FormList.InputArea3.Vender && data && data.length > 0) {
        // 仕入先選択時、構成IDを非表示の項目に設定する
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.VenderStructureId, 1, CtrlFlag.Label, data[0].VALUE1, false, false);
    }
}
