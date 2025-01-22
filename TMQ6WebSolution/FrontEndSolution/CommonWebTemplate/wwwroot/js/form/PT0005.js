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
    DispYearFrom: 8,                            // 表示年度(From) ※予備品詳細画面に戻った際に使用するための値を保持する場所
    DispYearTo: 9,                              // 表示年度(To) ※予備品詳細画面に戻った際に使用するための値を保持する場所
    InputArea1: {
        Id: "CBODY_010_00_LST_5",               // 入庫情報(入庫日～予備品倉庫)
        InoutDatetime: 1,                       // 入庫日
        StorageLocation: 2,                     // 予備品倉庫 
    },
    InputArea2: {
        Id: "CBODY_030_00_LST_5",               // 入庫情報(棚番)
        PartsLocationNo: 3,                     // 棚番
        PartsLocationDetailNo: 4,               // 棚枝番
        PartsStorageLocation: 5,                // 予備品倉庫ID ※1
        PartsLocationId: 6,                     // 棚ID
        PartsStorageLocationNotLink: 7          // 予備品倉庫ID(値は※1と同じで、連動はしていない)
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
        UnitPriceByNewestLot: 31                // 新品購入時の最新のロットの入庫単価
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

// グローバルリストのキー名称
const PT0005_GlobalKeyName =
{
    IsNewAndFirst: "IsNewAndFirst" // 新規で初回表示の場合はこのキー名称のデータがグローバルリストに格納されている(予備品倉庫コンボボックスをブランクにするためのもの)
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
        // 部門の変更時イベントを発生させ、翻訳を表示
        var department = getCtrl(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.Department, 1, CtrlFlag.TextBox, false, false);
        changeNoEdit(department);

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

    // 棚枝番からフォーカスアウトした場合
    var partsLocationDetailNo = getCtrl(PT0005_FormList.InputArea4.Id, PT0005_FormList.InputArea4.PartsLocationDetailNo, 1, CtrlFlag.TextBox, false, false);
    $(partsLocationDetailNo).blur(function () {
        // 入力された棚枝番の値を取得
        var partsLocationDetailNoValue = getValue(PT0005_FormList.InputArea4.Id, PT0005_FormList.InputArea4.PartsLocationDetailNo, 1, CtrlFlag.TextBox, false, false);
        // 非表示の「棚枝番」に設定する
        setValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsLocationDetailNo, 1, CtrlFlag.TextBox, partsLocationDetailNoValue, false, false);
    });


    // 新品購入時の最新のロットの入庫単価(非表示項目)を取得
    var unitPriceByNewestLot = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPriceByNewestLot, 1, CtrlFlag.TextBox, false, false);

    //入庫単価の右隣に新品購入時の単価を表示するための要素を作成
    var newElement = document.createElement('td');     // td要素
    var newContent = document.createTextNode(unitPriceByNewestLot); // 表示値(非表示項目の値を表示)
    newElement.setAttribute('class', 'newest_price');  // スタイルを適用させるためにクラスを付与
    newElement.appendChild(newContent);                // td要素にテキストノードを追加

    // 作成した要素を追加する親要素を取得
    var parentEle = $(P_Article).find("#" + PT0005_FormList.InputArea3.Id + getAddFormNo() + 'VAL' + PT0005_FormList.InputArea3.UnitPrice).parent();

    // 作成した要素を親要素に追加
    parentEle.closest('tr')[0].appendChild(newElement);

    // 要素を追加するとtbodyの右端に枠線ができてしまうため線をなくすスタイルを適用する
    parentEle.closest('tbody').addClass('border-none');

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
    // 新品購入時の最新のロットの入庫単価取得
    var unitPriceByNewestLot = getValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPriceByNewestLot, 1, CtrlFlag.TextBox, false, false).replace(/[^-0-9.]/g, '');

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

    // 新品購入時の最新のロットの入庫単価が入力されている場合
    if (!isNaN(unitPriceByNewestLot) && unitPriceByNewestLot != "") {
        // 入庫単価(非表示)の小数点以下桁数 丸め処理
        var unitPriceByNewestLot = roundDigit(unitPriceByNewestLot, unitDigit, roundDivision);
        // 入庫単価(非表示)に設定
        setValue(PT0005_FormList.InputArea3.Id, PT0005_FormList.InputArea3.UnitPriceByNewestLot, 1, CtrlFlag.TextBox, combineNumberAndUnit(unitPriceByNewestLot, currencyName, true), false, false);
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

    if (P_dicIndividual[GlobalKeys.TransParent] == FormList.No) {
        // 一覧画面からの遷移の場合、登録更新データを一覧画面に反映する（再検索を行わず、一覧データに反映）
        // 詳細画面からの遷移の場合は詳細画面の再検索後に反映する
        PT0005_setUpdateDataForList(conductId, false);
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
        var partsStorageLocation = getCtrl(PT0005_FormList.InputArea1.Id, PT0005_FormList.InputArea1.StorageLocation, 1, CtrlFlag.Combo, false, false).value;

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
        setValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsStorageLocationNotLink, 1, CtrlFlag.Label, data[0].EXPARAM7, false, false);

        // 新規表示の初回の場合は予備品倉庫コンボボックスと非表示で連動している予備品倉庫IDには何も設定しない
        if (!P_dicIndividual[PT0005_GlobalKeyName.IsNewAndFirst]) {
            setValue(PT0005_FormList.InputArea1.Id, PT0005_FormList.InputArea1.StorageLocation, 1, CtrlFlag.Combo, data[0].EXPARAM7, false, false);
            setValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsStorageLocation, 1, CtrlFlag.Label, data[0].EXPARAM7, false, false);
        }

       
        // 棚番を再設定（付属情報を削除）
        setValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsLocationNo, 1, CtrlFlag.Input, data[0].EXPARAM2, false, false);

        // 新規表示の初回かどうかを判定(バックエンド側でグローバルリストにフラグが設定されているかどうか)
        // ※予備品情報の標準棚番が棚まで設定されている場合はキーが格納されている
        if (P_dicIndividual[PT0005_GlobalKeyName.IsNewAndFirst]) {

            // 値を設定する処理が2回実行されるが、1回目は何もしない
            // ※1回目で下記を行うと正常に動作しない
            if (P_dicIndividual[PT0005_GlobalKeyName.IsNewAndFirst] == 2) {
                P_dicIndividual[PT0005_GlobalKeyName.IsNewAndFirst] = 1;
                return;
            }

            // グローバルリストからフラグを削除する
            delete P_dicIndividual[PT0005_GlobalKeyName.IsNewAndFirst];
        }

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



/**
 *【オーバーライド用関数】
 *  閉じる処理の後(ポップアップ画面用)
 */
function PT0005_postBackBtnProcessForPopup(conductId) {

    // 入庫入力画面の機能IDでない場合は何もせずに終了
    if (conductId != PT0005_FormList.ConductId) {
        return;
    }

    var val = null;

    // 表示年度(From)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearFrom]) {
        delete P_dicIndividual[DispYearKeyName.YearFrom];
    }

    // 表示年度(From)の値を取得
    val = getValue(PT0005_FormList.PartsInfo, PT0005_FormList.DispYearFrom, 0, CtrlFlag.Label, false, false).trim();

    if (!val) {
        // 入力されていない場合はSQLで扱うことのできる年の最小値を設定
        val = SqlYear.MinYear;
    }

    // グローバル変数に格納
    P_dicIndividual[DispYearKeyName.YearFrom] = val;

    val = null;

    // 表示年度(To)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearTo]) {
        delete P_dicIndividual[DispYearKeyName.YearTo];
    }

    // 表示年度(To)の値を取得
    val = getValue(PT0005_FormList.PartsInfo, PT0005_FormList.DispYearTo, 0, CtrlFlag.Label, false, false).trim();

    if (!val) {
        // 入力されていない場合はSQLで扱うことのできる年の最大値を設定
        val = SqlYear.MaxYear;
    }

    // グローバル変数に格納
    P_dicIndividual[DispYearKeyName.YearTo] = val;
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function PT0005_prevCommonValidCheck(appPath, conductId, formNo, btn) {

    var isContinue = true; // 個別処理後も共通の入力チェックを行う場合はtrue
    var isError = false;   // エラーがある場合はtrue

    // 機能IDが「入庫入力」ではない場合は何もしない
    if (getConductId() != PT0005_ConsuctId) {
        return [isContinue, isError];
    }

    // 棚番の要素を取得
    var partsLocationNoEle = getCtrl(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsLocationNo, 1, CtrlFlag.TextBox, false, false);

    // 棚番が入力されている場合
    if ($(partsLocationNoEle).val()) {

        // 非表示の予備品倉庫ID(連動ではない)を取得
        var storageLocationId = getValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsStorageLocationNotLink, 1, CtrlFlag.Label, false, false);

        // 取得できた場合
        if (storageLocationId) {

            // 予備品倉庫コンボボックスと連動している非表示のIDを取得
            var storageLocationComboId = getValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsStorageLocation, 1, CtrlFlag.Label, false, false);

            // 取得できなかった場合
            if (!storageLocationComboId) {

                // 選択されている予備品倉庫と入力されている棚番が紐付いていない場合はここには入らない(バックエンド側で入力チェックするため問題なし)
                // 予備品倉庫コンボボックスと連動している非表示のIDに、非表示の予備品倉庫ID(連動ではない)を設定する
                setValue(PT0005_FormList.InputArea1.Id, PT0005_FormList.InputArea1.StorageLocation, 1, CtrlFlag.Combo, storageLocationId, false, false);
                setValue(PT0005_FormList.InputArea2.Id, PT0005_FormList.InputArea2.PartsStorageLocation, 1, CtrlFlag.Label, storageLocationId, false, false);
            }
        }
    }
    return [isContinue, isError];
}

/**
* 更新データを一覧画面に反映する
*  @param conductId   ：機能ID
*  @param isDelete    ：削除の場合true
*/
function PT0005_setUpdateDataForList(conductId, isDelete) {
    if (!P_dicIndividual[GlobalKeys.UpdateListData] || conductId != PT0005_FormList.ConductId) {
        //更新データが存在しない場合(添付情報の反映は別のタイミングで行う)
        return;
    }

    //反映するデータ
    var updateData = P_dicIndividual[GlobalKeys.UpdateListData];
    if (!updateData || updateData.length < 1) {
        //処理終了
        return;
    }
    //1行目：ステータス（新規、更新、削除）
    var status = updateData[0].STATUS;
    //2行目：一覧画面用の反映データ
    var data = updateData[1];

    if (isDelete && status != rowStatusDef.Delete) {
        //postRegistProcessから呼ばれた場合は削除処理だけ行う
        //postBuiltTabulatorから呼ばれた場合は登録・更新処理を行う
        return;
    }

    //一覧画面のデータ
    var table = P_listData["#" + FormList.Id + "_" + FormList.No];
    if (!table) {
        // 一覧画面を生成していない場合(別タブ遷移で一覧画面を経由していない場合)、処理終了
        return;
    }

    switch (status) {
        case rowStatusDef.Edit: //更新
            //データの予備品IDを取得
            var partsId = data["VAL" + FormList.PartsId];
            //更新前のROWNOを取得（ROWNOがキー）
            var oldData = table.searchData("VAL" + FormList.PartsId, "=", partsId);
            if (oldData && oldData.length > 0) {
                data.ROWNO = oldData[0].ROWNO;
                //詳細画面から値取得
                PT0005_setLabelValueToListData(data, status, oldData[0]);
                table.updateRow(data.ROWNO, data);
            }
            break;

        default:
            break;
    }

    // 予備品一覧の背景色変更
    postSearchList(table);

    delete P_dicIndividual[GlobalKeys.UpdateListData];
}

/**
* 詳細画面の値から一覧用データに値を反映する
* @param data 一覧用データ
* @param status ステータス(新規or更新)
*/
function PT0005_setLabelValueToListData(data, status, oldData) {
    var unitName;
    var stockQuantity;
    var stockQuantityVal;
    var inoutQuantity = data["VAL" + FormList.StockQuantity];
    var leadTime;
    var leadTimeVal;
    if (oldData) {
        // 一覧の選択行データが渡ってきた場合は一覧画面からの入庫登録
        // 最新在庫数には入庫数が入っていて単位文字列が付加されていないため、一覧の選択行から取得して付加する
        unitName = oldData["VAL" + FormList.UnitName];
        var oldStockQuantity = oldData["VAL" + FormList.StockQuantityExceptUnit];
        stockQuantityVal = parseInt(oldStockQuantity) + parseInt(inoutQuantity);
        stockQuantity = stockQuantityVal + unitName;
        leadTimeVal = parseInt(oldData["VAL" + FormList.LeadTimeExceptUnit]);
    } else {
        // 数量管理単位を取得
        unitName = getValue(FormDetail.PurchaseInfo.Id, FormDetail.PurchaseInfo.UnitName, 1, CtrlFlag.Label);
        // 最新在庫数の値を取得
        stockQuantity = getValue(FormDetail.PurchaseInfo.Id, FormDetail.PurchaseInfo.StockQuantity, 1, CtrlFlag.Label);
        stockQuantityVal = parseInt(stockQuantity.replace(unitName, ""));
        // 発注点
        leadTime = getValue(FormDetail.PurchaseInfo.Id, FormDetail.PurchaseInfo.LeadTime, 1, CtrlFlag.Label);
        leadTimeVal = parseInt(leadTime.replace(unitName, ""));
    }

    //一覧に表示している列に詳細画面の対応する項目値を設定
    $.each(Object.keys(data), function (index, key) {
        if (!key.startsWith("VAL")) {
            return true; // continue
        }

        switch (key) {
            case "VAL" + FormList.StockQuantity: // 最新在庫数(単位有り)
                data[key] = stockQuantity;
                break;
            case "VAL" + FormList.StockQuantityExceptUnit: // 最新在庫数(単位無し)
                data[key] = stockQuantityVal;
                break;
            default:
                // 最新在庫数以外は更新しない
                delete data[key];
                break;
        }
    });
    // 発注アラームを設定
    data["VAL" + FormList.OrderAlert.CtrlNo] = isOrderAlertOn(stockQuantityVal, leadTimeVal);

}
