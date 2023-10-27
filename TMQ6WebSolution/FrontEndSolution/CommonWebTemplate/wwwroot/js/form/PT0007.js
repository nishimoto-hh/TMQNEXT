/* ========================================================================
 *  機能名　    ：   【PT0007】移庫入力画面
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0007\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

// 移庫画面 コントロール項目番号
const PT0007_FormList = {
    FormNo: 0,
    TabNo: {                             // タブ番号
        Location: 3,                     // 棚番移庫
        Department: 4                    // 部門移庫
    },
    PartsInfo:
    {
        Id: "CBODY_000_00_LST_7",        // 予備品情報
        TransFlg: 7,                     // 画面遷移フラグ
        TransType: 8,                    // 画面遷移タイプ
        BtnVisible: 9,                   // 登録・取消ボタンの活性/非活性
        TabSelected: 11,                 // タブ選択フラグ
        LocationDataCnt: 12,             // 棚別在庫一覧の件数
        DepartmentDataCnt: 13            // 部門別在庫一覧の件数
    },
    LocationList: {
        Id: "CBODY_010_00_LST_7",        // 棚別在庫一覧
        WarehouseId: 11,                 // 予備品倉庫ID
        PartsLocationCode: 12,           // 棚番コード(オートコンプリート用)
        PartsLocationId: 13,             // 棚番ID
        JoinStr: 14,                     // 結合文字列
        UnitName: 15,                    // 数量単位名称
        CurrencyName: 16,                // 金額単位名称
        UnitDigit: 17,                   // 小数点以下桁数(数量)
        CurrencyDigit: 18,               // 小数点以下桁数(金額)
        UnitRoundDivision: 19,           // 丸め処理区分(数量)
        CurrencyRoundDivision: 20,       // 丸め処理区分(金額)
        UnitPrice: 21,                   // 入庫単価
        StockQuantity: 22,               // 在庫数
        DetailNo: 23,                    // 枝番
        DepartmentId: 24,                // 部門ID
        LotControlId: 25,                // ロット管理ID
        InventoryControlId: 26,          // 在庫管理ID
        OldNewStructureId: 27,           // 新旧区分
        AccountStructureId: 28           // 勘定科目
    },
    LocationInfoTo:                      // 移庫先情報
    {
        ToWarehouse: {                   // 先頭から予備品倉庫まで
            Id: "CBODY_020_00_LST_7",
            TransDate: 2,                // 移庫日
            Warehouse: 3                 // 予備品倉庫
        },
        ToWarehouseId:                   // 予備品倉庫IDまで
        {
            Id: "CBODY_070_00_LST_7",
            PartsLocationCode: 1,        // 棚番コード
            PartsLocationId: 2,          // 棚番ID
            WarehouseId: 3               // 予備品倉庫ID
        },
        ToJoinStr:                       // 結合文字列まで
        {
            Id: "CBODY_080_00_LST_7",
            LocationDetailNo: 1,         // 枝番
            JoinStr: 2                   // 結合文字列
        },
        ToMoney:                         // 丸め処理区分まで
        {
            Id: "CBODY_090_00_LST_7",
            InoutQuantity: 1,            // 移庫数
            InoutAmount: 2,              // 移庫金額
            UnitName: 3,                 // 数量単位名称
            CurrencyName: 4,             // 金額単位名称
            UnitDigit: 5,                // 小数点以下桁数(数量)
            CurrencyDigit: 6,            // 小数点以下桁数(金額)
            UnitRoundDivision: 7,        // 丸め処理区分(数量)
            CurrencyRoundDivision: 8,    // 丸め処理区分(金額)
            UnitPrice: 9,                // 入庫単価
            StockQuantity: 10,           // 在庫数
            DepartmentId: 12,            // 部門ID
            PartsLocationId: 13,         // 棚番(変更前)
            LocationDetailNo: 14,        // 枝番(変更前)
            LotControlId: 15,            // ロット管理ID
            InventoryControlId: 16,      // 在庫管理ID
            OldNewStructureId: 18,       // 新旧区分
            AccountStructureId: 19       // 勘定科目
        }
    },
    DepartmentList:
    {
        Id: "CBODY_040_00_LST_7",
        InoutQuantity: 5,                // 在庫数
        ManagementNo: 8,                 // 管理No
        ManagementDiv: 9,                // 管理区分
        UnitName: 10,                    // 数量単位名称
        CurrencyName: 11,                // 金額単位名称
        UnitDigit: 12,                   // 小数点以下桁数(数量)
        CurrencyDigit: 13,               // 小数点以下桁数(金額)
        UnitRoundDivision: 14,           // 丸め処理区分(数量)
        CurrencyRoundDivision: 15,       // 丸め処理区分(金額)
        UnitPrice: 16,                   // 入庫単価
        StockQuantity: 17,               // 在庫数
        DepartmentId: 18,                // 部門ID
        SubjectId: 19,                   // 勘定科目ID
        DepFlg: 20,                      // 部門フラグ
        SubjectCode: 22,                 // 勘定科目コード
        LotControlId: 23,                // ロット管理ID
        SubjectTranslation: 24,          // 勘定科目翻訳
        PartsFactoryId: 25,              // 工場ID
        OldNew: 26,                      // 新旧区分

    },
    DepartmentInfoTo:
    {
        Id: "CBODY_050_00_LST_7",
        InoutDate: 2,                    // 移庫日
        InoutQuantity: 3,                // 入庫単価
        TransferCount: 4,                // 移庫数
        TransferAmount: 5,               // 移庫金額
        Department: 6,                   // 部門
        Subject: 7,                      // 勘定科目
        ManegementDiv: 8,                // 管理区分
        ManagementNo: 9,                 // 管理No
        UnitName: 10,                    // 数量単位名称
        CurrencyName: 11,                // 金額単位名称
        UnitDigit: 12,                   // 小数点以下桁数(数量)
        CurrencyDigit: 13,               // 小数点以下桁数(金額)
        UnitRoundDivision: 14,           // 丸め処理区分(数量)
        CurrencyRoundDivision: 15,       // 丸め処理区分(金額)
        DepartmentId: 16,                // 部門ID
        SubjectId: 17,                   // 勘定科目ID
        DepFlg: 18,                      // 部門フラグ
        TransferCountExeptUnit: 19,      // 移庫数(計算用)
        WorkNo: 20,                      // 作業No
        LotControlId: 21,                // ロット管理ID
        InoutQuantityBefore: 22,         // 入庫単価(変更前)
        OldNew: 23,                      // 新旧区分
        PartsFactoryId: 24,              // 工場ID
        DepartmentIdBefore: 26,          // 部門ID(移庫元)
        SubjectIdBefore: 27,             // 勘定科目ID(移庫元)
    },
    Button:
    {
        RegistLocation: "RegistLocation",     // 棚番移庫 登録ボタン
        RegistDepartment: "RegistDepartment", // 部門移庫 登録ボタン
        CancelLocation: "CancelLocation",     // 棚番移庫 取消ボタン
        CancelDepartment: "CancelDepartment"  // 部門移庫 取消ボタン
    }
};

// 画面遷移フラグ
const PT0007_TransFlg =
{
    FlgNew: 0, // 新規
    FlgEdit: 1 // 修正
};

// 画面遷移タイプ
const PT0007_TransType =
{
    Subject: 2,   // 棚番移庫
    Department: 3 // 部門移庫
};

// 登録・取消ボタンの活性/非活性
const PT0007_BtnVisible =
{
    Visible: 0,  // 活性
    UnVisible: 1 // 非活性
}

// 部門フラグ
const PT0007_DepartmentFlg =
{
    Fix: 1     // 修理部門
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
function PT0007_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    // 機能IDが「移庫入力」ではない場合は何もしない
    if (getConductId() != PT0007_ConsuctId) {
        return;
    }

    // 棚別在庫一覧の件数が0件の場合は登録・取消ボタンを非活性にする
    var locationDataCnt = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.LocationDataCnt, 0, CtrlFlag.Label, false, false);
    if (locationDataCnt == 0) {
        // 登録ボタンを非活性にする
        setDispMode(getButtonCtrl(PT0007_FormList.Button.RegistLocation), true);
        // 取消ボタンを非活性にする
        setDispMode(getButtonCtrl(PT0007_FormList.Button.CancelLocation), true);
        // 結合文字列をセット
        setValue(PT0007_FormList.LocationInfoTo.ToJoinStr.Id, PT0007_FormList.LocationInfoTo.ToJoinStr.LocationDetailNo, 1, CtrlFlag.Label, '-', false, true);
    }

    // 部門別在庫一覧の件数が0件の場合は登録・取消ボタンを非活性にする
    var departmentDataCnt = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.DepartmentDataCnt, 0, CtrlFlag.Label, false, false);
    if (departmentDataCnt == 0) {
        // 登録ボタンを非活性にする
        setDispMode(getButtonCtrl(PT0007_FormList.Button.RegistDepartment), true);
        // 取消ボタンを非活性にする
        setDispMode(getButtonCtrl(PT0007_FormList.Button.CancelDepartment), true);
    }

    // 棚番移庫タブの移庫数からフォーカスアウトした場合
    var inoutQuantityLocation = getCtrl(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.TextBox, false, false);
    $(inoutQuantityLocation).blur(function () {

        // 入力された移庫数取得
        var inoutQuantity = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.TextBox, false, false).replace(",", "");
        var currencyName = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.CurrencyName, 1, CtrlFlag.TextBox, false, false).trim();//金額単位名称

        // 数値が入力されている場合
        if (!isNaN(inoutQuantity) && inoutQuantity != "") {

            // 入力された移庫数の丸め処理
            var unitDigit = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitDigit, 1, CtrlFlag.Label, false, false);// 小数点以下桁数
            var unitRoundDivision = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitRoundDivision, 1, CtrlFlag.Label, false, false); // 丸め処理区分
            var roundInoutQuantityDisp = roundDigit(inoutQuantity, unitDigit, unitRoundDivision);

            // 移庫数に設定
            setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.TextBox, roundInoutQuantityDisp, false, false);

            // 計算に必要な値を取得
            var unitPrice = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitPrice, 1, CtrlFlag.TextBox, false, false).replace(',', '');      // 入庫単価
            var currencyDigit = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.CurrencyDigit, 1, CtrlFlag.Label, false, false);                 // 小数点以下桁数
            var currencyRoundDivision = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.CurrencyRoundDivision, 1, CtrlFlag.Label, false, false); // 丸め処理区分

            // 移庫数×入庫単価
            var roundInoutQuantity = roundInoutQuantityDisp.replace(/[^-0-9.]/g, '');
            var amount = roundDigit(roundInoutQuantity * unitPrice, currencyDigit, currencyRoundDivision);

            // 移庫金額に設定
            setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutAmount, 1, CtrlFlag.Label, combineNumberAndUnit(amount, currencyName, true), false, false);
        }
        else {
            // 移庫数に設定
            setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.TextBox, '', false, false);
            // 移庫金額に設定
            setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutAmount, 1, CtrlFlag.Label, combineNumberAndUnit("0", currencyName, true), false, false);
        }
    });

    //部門移庫タブの入庫単価からフォーカスアウトした場合
    var unitPriceDepartment = getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, false, false);
    $(unitPriceDepartment).blur(function () {

        // 入力された入庫単価取得
        var unitPrice = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, false, false).replace(",", "");
        var currencyName = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.CurrencyName, 1, CtrlFlag.TextBox, false, false).trim();//金額単位名称

        // 数値が入力されている場合
        if (!isNaN(unitPrice) && unitPrice != "") {

            // 入力された入庫単価の丸め処理
            var currencyDigit = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.CurrencyDigit, 1, CtrlFlag.Label, false, false);                           // 小数点以下桁数
            var currencyRoundDivision = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.CurrencyRoundDivision, 1, CtrlFlag.Label, false, false);           // 丸め処理区分
            var unitPriceDisp = roundDigit(unitPrice, currencyDigit, currencyRoundDivision);

            // 移庫金額に設定
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, unitPriceDisp, false, false);

            // 計算に必要な値を取得
            var inoutQuantity = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.TransferCountExeptUnit, 1, CtrlFlag.Label, false, false).replace(',', ''); // 移庫数

            // 移庫数×入庫単価
            var roundUnitPrice = unitPriceDisp.replace(/[^-0-9.]/g, '');
            var amount = roundDigit(parseFloat(inoutQuantity) * parseFloat(roundUnitPrice), currencyDigit, currencyRoundDivision);

            // 移庫金額に設定
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.TransferAmount, 1, CtrlFlag.Label, combineNumberAndUnit(amount, currencyName, true), false, false);
        }
        else {
            // 移庫金額に設定
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, '', false, false);
            // 移庫金額に設定
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.TransferAmount, 1, CtrlFlag.Label, combineNumberAndUnit("0", currencyName, true), false, false);
        }
    });
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
function PT0007_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {

    // 機能IDが「移庫入力」ではない場合は何もしない
    if (getConductId() != PT0007_ConsuctId) {
        return;
    }

    // 移庫先情報の「予備品倉庫」コンボボックスが変更された場合
    if (ctrlId == PT0007_FormList.LocationInfoTo.ToWarehouse.Id && valNo == PT0007_FormList.LocationInfoTo.ToWarehouse.Warehouse) {

        // 「予備品倉庫」コンボボックスの値(構成ID)を取得
        var warehouseId = getValue(PT0007_FormList.LocationInfoTo.ToWarehouse.Id, PT0007_FormList.LocationInfoTo.ToWarehouse.Warehouse, 1, CtrlFlag.Combo, false, false);
        // 取得した構成IDを退避
        setValue(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.WarehouseId, 1, CtrlFlag.Label, warehouseId, false, false);
    }
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function PT0007_postBuiltTabulator(tbl, id) {
    // 機能IDが「移庫入力」ではない場合は何もしない
    if (getConductId() != PT0007_ConsuctId) {
        return;
    }

    if (id == "#" + PT0007_FormList.LocationList.Id + getAddFormNo()) {
        var promise = new Promise((resolve) => {
            // 画面遷移フラグ(新規か修正か)を取得
            var transFlg = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TransFlg, 0, CtrlFlag.Label, false, false);

            // 棚番移庫タブをクリックして表示する
            clickTab(PT0007_FormList.TabNo.Location);

            // 棚別在庫一覧
            var table = P_listData['#' + PT0007_FormList.LocationList.Id + getAddFormNo()];
            var row = $($(table.element.children[1]).find(".tabulator-row")[0]).find("div[tabulator-field='ROWNO']").find("a");
            if (row.length > 0) {
                // 先頭行のNo.リンクをクリックする
                $(row).click();
            }
            if (transFlg == PT0007_TransFlg.FlgNew) {
                // 新規の場合

                // 取消ボタンを非活性にする
                setDispMode(getButtonCtrl(PT0007_FormList.Button.CancelLocation), true);

                // 棚番移庫タブ フォーカス
                setFocusLocation();
            }
            else if (transFlg == PT0007_TransFlg.FlgEdit) {
                // 画面遷移タイプ(棚番移庫か部門移庫か)を取得
                var transType = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TransType, 0, CtrlFlag.Label, false, false);
                if (transType == PT0007_TransType.Subject) {
                    // 棚番移庫

                    // 登録ボタンにフォーカスを設定
                    var btnVisible = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.BtnVisible, 0, CtrlFlag.Label, false, false);
                    if (btnVisible == PT0007_BtnVisible.Visible) {
                        setFocusButton(PT0007_FormList.Button.RegistLocation);
                    }

                    // 結合文字列を枝番入力コントロールのヘッダーにセット
                    var joinStr = getValue(PT0007_FormList.LocationInfoTo.ToJoinStr.Id, PT0007_FormList.LocationInfoTo.ToJoinStr.JoinStr, 1, CtrlFlag.Label, false, false);
                    setValue(PT0007_FormList.LocationInfoTo.ToJoinStr.Id, PT0007_FormList.LocationInfoTo.ToJoinStr.LocationDetailNo, 1, CtrlFlag.Label, joinStr, false, true);

                    // 数量管理単位を移庫数にセット
                    var unitName = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitName, 0, CtrlFlag.TextBox, false, false);
                    setValueToLocationNumberUnit(unitName);

                    // 移庫数を3桁ごとにカンマ区切りにする
                    var inoutQuantity = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.TextBox, false, false).replace(",", ""); // 移庫数
                    var unitDigit = getValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitDigit, 1, CtrlFlag.Label, false, false);                            // 小数点以下桁数
                    var inoutQuantityDisp = parseFloat(inoutQuantity).toLocaleString(undefined, { maximumFractionDigits: unitDigit });
                    setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.TextBox, inoutQuantityDisp, false, false);


                    // 登録・取消ボタンの活性/非活性
                    var BtnVisible = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.BtnVisible, 0, CtrlFlag.Label, false, false);
                    if (BtnVisible == PT0007_BtnVisible.UnVisible) {
                        // 登録ボタンを非活性にする
                        setDispMode(getButtonCtrl(PT0007_FormList.Button.RegistLocation), true);
                        // 取消ボタンを非活性にする
                        setDispMode(getButtonCtrl(PT0007_FormList.Button.CancelLocation), true);
                    }

                    // 棚の翻訳は遅らせて表示(オートコンプリート)
                    setTimeout(function () {
                        var ctrl = getCtrl(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.PartsLocationCode, 1, CtrlFlag.TextBox, false, false);
                        changeNoEdit(ctrl);
                    }, 300); //300ミリ秒

                    // 部門移庫タブを非表示
                    hideTab(PT0007_FormList.TabNo.Department);
                }
                else {
                    // 部門移庫タブをクリックして表示する
                    clickTab(PT0007_FormList.TabNo.Department);
                }
            }
            //処理が完了したことを通知（thenの処理を実行）
            resolve();
        }).then(() => {
            //実行ボタンが全て非活性の場合、戻るor閉じるボタンにフォーカス設定
            setFocusBackOrClose();
        });
    }
    else if (id == "#" + PT0007_FormList.DepartmentList.Id + getAddFormNo()) {
        var promise = new Promise((resolve) => {
            // 画面遷移フラグ(新規か修正か)を取得
            var transFlg = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TransFlg, 0, CtrlFlag.Label, false, false);

            // 部門移庫タブをクリックして表示する
            clickTab(PT0007_FormList.TabNo.Department);

            // 部門別在庫一覧
            var table = P_listData['#' + PT0007_FormList.DepartmentList.Id + getAddFormNo()];
            var row = $($(table.element.children[1]).find(".tabulator-row")[0]).find("div[tabulator-field='ROWNO']").find("a");
            if (row.length > 0) {
                // 先頭行のNo.リンクをクリックする
                $(row).click();
            }
            // 部門の翻訳表示は少し遅らせる
            setTimeout(function () {
                var department = getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.Department, 1, CtrlFlag.TextBox, false, false);
                changeNoEdit(department);
            }, 300); //300ミリ秒
            // 勘定科目の翻訳表示は少し遅らせる
            setTimeout(function () {
                var subject = getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.Subject, 1, CtrlFlag.TextBox, false, false);
                changeNoEdit(subject);
            }, 300); //300ミリ秒

            if (transFlg == PT0007_TransFlg.FlgNew) {

                // 取消ボタンを非活性にする
                setDispMode(getButtonCtrl(PT0007_FormList.Button.CancelDepartment), true);

                // 棚番移庫タブをクリックして表示する
                clickTab(PT0007_FormList.TabNo.Location);

                // 棚番移庫タブ フォーカス
                setFocusLocation();
            }
            else if (transFlg == PT0007_TransFlg.FlgEdit) {
                // 画面遷移タイプ(棚番移庫か部門移庫か)を取得
                var transType = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TransType, 0, CtrlFlag.Label, false, false);
                if (transType == PT0007_TransType.Department) {
                    // 部門移庫

                    // 登録ボタンにフォーカスを設定
                    var btnVisible = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.BtnVisible, 0, CtrlFlag.Label, false, false);
                    if (btnVisible == PT0007_BtnVisible.Visible) {
                        setFocusButton(PT0007_FormList.Button.RegistDepartment);
                    }

                    // 入庫単価を3桁ごとにカンマ区切りにする
                    var unitPrice = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, false, false).replace(",", "");
                    var currencyDigit = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.CurrencyDigit, 1, CtrlFlag.Label, false, false); // 小数点以下桁数
                    var unitPriceDisp = parseFloat(unitPrice).toLocaleString(undefined, { maximumFractionDigits: currencyDigit });
                    setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, unitPriceDisp, false, false);

                    // 金額単位名称を移庫数にセット
                    var unitName = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.UnitName, 0, CtrlFlag.TextBox, false, false);
                    setValueToDepartmentNumberUnit(unitName);

                    //部門のフラグ取得
                    var depFlg = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.DepFlg, 1, CtrlFlag.Label, false, false);
                    // 入庫単価の活性/非活性
                    judgeAndDisableDepartment(depFlg);

                    // 登録・取消ボタンの活性/非活性
                    var BtnVisible = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.BtnVisible, 0, CtrlFlag.Label, false, false);
                    if (BtnVisible == PT0007_BtnVisible.UnVisible) {
                        // 登録ボタンを非活性にする
                        setDispMode(getButtonCtrl(PT0007_FormList.Button.RegistDepartment), true);
                        // 取消ボタンを非活性にする
                        setDispMode(getButtonCtrl(PT0007_FormList.Button.CancelDepartment), true);
                    }

                    // 棚番移庫タブを非表示
                    hideTab(PT0007_FormList.TabNo.Location);
                }
                else {
                    // 棚番移庫タブをクリックして表示する
                    clickTab(PT0007_FormList.TabNo.Location);

                    // 棚番移庫タブ フォーカス
                    setFocusLocation(true);
                }

            }

            //処理が完了したことを通知（thenの処理を実行）
            resolve();
        }).then(() => {
            //実行ボタンが全て非活性の場合、戻るor閉じるボタンにフォーカス設定
            setFocusBackOrClose();
        });
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
function PT0007_setCodeTransOtherNames(appPath, formNo, ctrl, data) {

    // 機能IDが「移庫入力」ではない場合は何もしない
    if (getConductId() != PT0007_ConsuctId) {
        return;
    }

    // オートコンプリートでセットされたコントロールIDを判定
    if (ctrl[0].id == PT0007_FormList.LocationInfoTo.ToWarehouseId.Id + getAddFormNo() + "VAL" + PT0007_FormList.LocationInfoTo.ToWarehouseId.PartsLocationCode && data && data.length > 0) {
        // 棚番(オートコンプリート)選択時、構成IDを非表示の項目に設定する
        setValue(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.PartsLocationId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
    }
    else if (ctrl[0].id == PT0007_FormList.DepartmentInfoTo.Id + getAddFormNo() + "VAL" + PT0007_FormList.DepartmentInfoTo.Department && data && data.length > 0) {

        // 部門
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.DepartmentId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.DepFlg, 1, CtrlFlag.Label, data[0].EXPARAM2, false, false);

    }
    else if (ctrl[0].id == PT0007_FormList.DepartmentInfoTo.Id + getAddFormNo() + "VAL" + PT0007_FormList.DepartmentInfoTo.Subject && data && data.length > 0) {
        // 勘定科目
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.SubjectId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
    }
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
function PT0007_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];

    // 機能IDが「移庫入力」ではない場合は何もしない
    if (getConductId() != PT0007_ConsuctId) {
        return [true, conditionDataList];
    }

    // 選択された行を取得
    var selectedRow = $(element).parent().parent();

    var transFlg = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TransFlg, 0, CtrlFlag.Label, false, false);
    // 一覧のコントロールIDを判定
    if (ctrlId == PT0007_FormList.LocationList.Id && transFlg == PT0007_TransFlg.FlgNew) {

        // 選択された行の色を変更
        changeRowsColor(selectedRow);

        var warehouseId = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.WarehouseId);                     // 予備品倉庫ID
        var partsLocationCode = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.PartsLocationCode);         // 棚番コード(オートコンプリート用)
        var partsLocationId = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.PartsLocationId);             // 棚番ID
        var joinStr = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.JoinStr);                             // 結合文字列
        var unitName = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.UnitName).trim();                    // 数量単位名称
        var currencyName = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.CurrencyName).trim();            // 金額単位名称
        var unitDigit = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.UnitDigit);                         // 小数点以下桁数(数量)
        var currencyDigit = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.CurrencyDigit);                 // 小数点以下桁数(金額)
        var unitRoundDivision = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.UnitRoundDivision);         // 丸め処理区分(数量)
        var currencyRoundDivision = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.CurrencyRoundDivision); // 丸め処理区分(金額)
        var unitPrice = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.UnitPrice);                         // 入庫単価
        var stockQuantity = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.StockQuantity);                 // 在庫数
        var detailNo = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.DetailNo);                           // 枝番
        var departmentId = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.DepartmentId);                   // 部門ID
        var lotControlId = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.LotControlId);                   // ロット管理ID
        var inventoryControlId = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.InventoryControlId);       // 在庫管理ID
        var oldNewStructureId = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.OldNewStructureId);         // 新旧区分ID
        var accountStructureId = getValBySelectedRow(selectedRow, PT0007_FormList.LocationList.AccountStructureId);       // 勘定科目ID

        // 取得した値を移庫先情報に設定する
        setValue(PT0007_FormList.LocationInfoTo.ToWarehouse.Id, PT0007_FormList.LocationInfoTo.ToWarehouse.TransDate, 1, CtrlFlag.TextBox, formatDateToYMD(new Date()), false, false);           // 移庫日
        setValue(PT0007_FormList.LocationInfoTo.ToWarehouse.Id, PT0007_FormList.LocationInfoTo.ToWarehouse.Warehouse, 1, CtrlFlag.Combo, warehouseId, false, false);                             // 予備品倉庫
        setValue(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.PartsLocationCode, 1, CtrlFlag.TextBox, partsLocationCode, false, false);         // 棚番コード
        setValue(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.PartsLocationId, 1, CtrlFlag.Label, partsLocationId, false, false);               // 棚番ID

        // 棚番IDをコントロールのvalueにも設定する
        var autoCompCtrl = getCtrl(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.PartsLocationCode, 1, CtrlFlag.TextBox, false, false);
        $(autoCompCtrl).parent().find("input[type=hidden]")[0].value = partsLocationId;

        // 棚の翻訳は遅らせて表示(オートコンプリート)
        setTimeout(function () {
            var ctrl = getCtrl(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.PartsLocationCode, 1, CtrlFlag.TextBox, false, false);
            changeNoEdit(ctrl);
        }, 300); //300ミリ秒

        setValue(PT0007_FormList.LocationInfoTo.ToWarehouseId.Id, PT0007_FormList.LocationInfoTo.ToWarehouseId.WarehouseId, 1, CtrlFlag.Label, warehouseId, false, false);                       // 予備品倉庫
        setValue(PT0007_FormList.LocationInfoTo.ToJoinStr.Id, PT0007_FormList.LocationInfoTo.ToJoinStr.LocationDetailNo, 1, CtrlFlag.Label, joinStr, false, true);                               // 結合文字列
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.TextBox, "", false, false);                                        // 移庫数
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutAmount, 1, CtrlFlag.Label, combineNumberAndUnit("0", currencyName, true), false, false); // 移庫金額
        setValueToLocationNumberUnit(unitName);                                                                                                                                                  // 数量単位名称(移庫数のテキストボックス横)
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitName, 1, CtrlFlag.TextBox, unitName, false, false);                                       // 数量単位名称
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.CurrencyName, 1, CtrlFlag.TextBox, currencyName, false, false);                               // 金額単位名称
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitDigit, 1, CtrlFlag.Label, unitDigit, false, false);                                       // 小数点以下桁数(数量)
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.CurrencyDigit, 1, CtrlFlag.Label, currencyDigit, false, false);                               // 小数点以下桁数(金額)
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitRoundDivision, 1, CtrlFlag.Label, unitRoundDivision, false, false);                       // 丸め処理区分(数量)
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.CurrencyRoundDivision, 1, CtrlFlag.Label, currencyRoundDivision, false, false);               // 丸め処理区分(金額)
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.UnitPrice, 1, CtrlFlag.TextBox, unitPrice, false, false);                                     // 入庫単価
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.StockQuantity, 1, CtrlFlag.TextBox, stockQuantity, false, false);                             // 在庫数
        setValue(PT0007_FormList.LocationInfoTo.ToJoinStr.Id, PT0007_FormList.LocationInfoTo.ToJoinStr.LocationDetailNo, 1, CtrlFlag.TextBox, detailNo, false, false);                           // 枝番
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.DepartmentId, 1, CtrlFlag.Label, departmentId, false, false);                                 // 部門ID
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.PartsLocationId, 1, CtrlFlag.Label, partsLocationId, false, false);                           // 棚番(変更前)
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.LocationDetailNo, 1, CtrlFlag.Label, detailNo, false, false);                                 // 枝番(変更前)
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.LotControlId, 1, CtrlFlag.Label, lotControlId, false, false);                                 // ロット管理ID
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InventoryControlId, 1, CtrlFlag.Label, inventoryControlId, false, false);                     // 在庫管理ID
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.OldNewStructureId, 1, CtrlFlag.Label, oldNewStructureId, false, false);                       // 新旧区分ID
        setValue(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.AccountStructureId, 1, CtrlFlag.Label, accountStructureId, false, false);                     // 勘定科目ID
    }
    else if (ctrlId == PT0007_FormList.DepartmentList.Id && transFlg == PT0007_TransFlg.FlgNew) {

        // 選択された行の色を変更
        changeRowsColor(selectedRow);

        var depFlg = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.DepFlg);                               // 部門フラグ
        var unitPrie = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.UnitPrice).trim();                   // 入庫単価
        var inoutQuantity = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.InoutQuantity).trim();          // 移庫数
        var unitName = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.UnitName).trim();                    // 数量単位名称
        var currencyName = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.CurrencyName).trim();            // 金額単位名称
        var stockQuantity = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.StockQuantity).trim();          // 在庫数
        var managementNo = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.ManagementNo);                   // 管理No
        var managementDiv = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.ManagementDiv);                 // 管理区分
        var unitDigit = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.UnitDigit);                         // 小数点以下桁数(数量)
        var currencyDigit = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.CurrencyDigit);                 // 小数点以下桁数(金額)
        var unitRoundDivision = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.UnitRoundDivision);         // 丸め処理区分(数量)    
        var currencyRoundDivision = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.CurrencyRoundDivision); // 丸め処理区分(金額)
        var subjectCode = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.SubjectCode);                     // 勘定科目コード
        var departmentId = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.DepartmentId);                   // 部門ID
        var subjectId = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.SubjectId);                         // 勘定科目ID
        var lotControlId = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.LotControlId);                   // ロット管理ID
        var subjectTranslation = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.SubjectTranslation);       // 勘定科目翻訳
        var oldNewId = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.OldNew);                             // 新旧区分
        var partsFactoryId = getValBySelectedRow(selectedRow, PT0007_FormList.DepartmentList.PartsFactoryId);               // 工場ID
        var unitPriceDisp = parseFloat(unitPrie).toLocaleString(undefined, { maximumFractionDigits: unitDigit });


        // 取得した値を移庫先情報に設定する
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutDate, 1, CtrlFlag.TextBox, formatDateToYMD(new Date()), false, false);     // 移庫日
        setValueToDepartmentNumberUnit(currencyName);                                                                                                                  // 金額単位名称(移庫数のテキストボックス横)
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.TransferCount, 1, CtrlFlag.Label, inoutQuantity, false, false);                 // 移庫数
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.Department, 1, CtrlFlag.TextBox, "", false, false);                             // 部門
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.ManagementNo, 1, CtrlFlag.TextBox, managementNo, false, false);                 // 管理No
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.ManegementDiv, 1, CtrlFlag.TextBox, managementDiv, false, false);               // 管理区分
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.UnitName, 1, CtrlFlag.TextBox, unitName, false, false);                         // 数量単位名称
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.CurrencyName, 1, CtrlFlag.TextBox, currencyName, false, false);                 // 金額単位名称
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.UnitDigit, 1, CtrlFlag.Label, unitDigit, false, false);                         // 小数点以下桁数(数量)
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.CurrencyDigit, 1, CtrlFlag.Label, currencyDigit, false, false);                 // 小数点以下桁数(金額)
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.UnitRoundDivision, 1, CtrlFlag.Label, unitRoundDivision, false, false);         // 丸め処理区分(数量)
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.CurrencyRoundDivision, 1, CtrlFlag.Label, currencyRoundDivision, false, false); // 丸め処理区分(金額)
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.DepartmentId, 1, CtrlFlag.Label, departmentId, false, false);                   // 部門ID
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.SubjectId, 1, CtrlFlag.Label, subjectId, false, false);                         // 勘定科目ID
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.DepFlg, 1, CtrlFlag.Label, depFlg, false, false);                               // 部門フラグ
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.TransferCountExeptUnit, 1, CtrlFlag.Label, stockQuantity, false, false);        // 移庫数
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.LotControlId, 1, CtrlFlag.Label, lotControlId, false, false);                   // ロット管理ID
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantityBefore, 1, CtrlFlag.Label, unitPriceDisp, false, false);           // 入庫単価(変更前)
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.OldNew, 1, CtrlFlag.Label, oldNewId, false, false);                             // 新旧区分
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.Subject, 1, CtrlFlag.TextBox, subjectCode, false, false);                       // 勘定科目コード
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.PartsFactoryId, 1, CtrlFlag.Label, partsFactoryId, false, false);               // 工場ID
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.DepartmentIdBefore, 1, CtrlFlag.Label, departmentId, false, false);             // 部門ID(移庫元)
        setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.SubjectIdBefore, 1, CtrlFlag.Label, subjectId, false, false);                   // 勘定科目ID(移庫元)



        // 勘定科目の翻訳表示は少し遅らせる
        setTimeout(function () {
            var subject = getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.Subject, 1, CtrlFlag.TextBox, false, false);
            changeNoEdit(subject);
        }, 300); //300ミリ秒

        var depTrans = getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.Department, 1, CtrlFlag.TextBox, false, false);                   // 部門の翻訳
        $(depTrans).parent().find(".honyaku")[0].innerText = "";

        if (depFlg == PT0007_DepartmentFlg.Fix) {
            // 選択された情報が修理部門の場合
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, "", false, false);                                           // 入庫単価    
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.TransferAmount, 1, CtrlFlag.Label, combineNumberAndUnit("0", currencyName, true), false, false); // 移庫金額
        }
        else {
            // 選択された情報が修理部門以外の場合
            var amount = roundDigit(unitPrie * stockQuantity, currencyDigit, currencyRoundDivision);
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, unitPriceDisp, false, false);                                   // 入庫単価
            setValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.TransferAmount, 1, CtrlFlag.Label, combineNumberAndUnit(amount, currencyName, true), false, false); // 移庫金額
        }
        judgeAndDisableDepartment(depFlg); // 入庫単価の活性/非活性

    }
    else if (ctrlId == PT0007_FormList.LocationList.Id && transFlg == PT0007_TransFlg.FlgEdit) {
        // 選択された行の色を変更
        changeRowsColor(selectedRow);
    }
    else if (ctrlId == PT0007_FormList.DepartmentList.Id && transFlg == PT0007_TransFlg.FlgEdit) {
        // 選択された行の色を変更
        changeRowsColor(selectedRow);
    }

    return [false, conditionDataList];
}

/**
 * 移庫入力画面の遷移前処理を行うかどうかの判定
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
function IsExecPT0007_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var result = ctrlId == PT0007_FormList.LocationList.Id || ctrlId == PT0007_FormList.DepartmentList.Id;
    return result;
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function PT0007_prevCommonValidCheck(appPath, conductId, formNo, btn) {

    var isContinue = true; // 個別処理後も共通の入力チェックを行う場合はtrue
    var isError = false;   // エラーがある場合はtrue


    // 機能IDが「移庫入力」ではない場合は何もしない
    if (getConductId() != PT0007_ConsuctId) {
        return [isContinue, isError];
    }

    // 棚番移庫タブ・部門移庫タブの登録ボタンの場合は入力チェックを行う
    if (btn.name == PT0007_FormList.Button.RegistLocation || btn.name == PT0007_FormList.Button.RegistDepartment) {

        isError = PT0007_validListData();
        return [false, !isError];
    }

    return [false, isError];
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
function PT0007_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {
    // 機能IDが「移庫入力」ではない場合は何もしない
    if (conductId != PT0007_ConsuctId) {
        return;
    }

    // 登録・取消ボタン実行正常終了後画面を閉じて遷移元に移動
    var modal = $(btn).closest('section.modal_form');
    $(modal).modal('hide');
}

/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function PT0007_initTabOriginal(tabNo, tableId) {

    // 機能IDが「移庫入力」ではない場合は何もしない
    if (getConductId() != PT0007_ConsuctId) {
        return;
    }

    // 部門移庫タブの場合
    if (tabNo == PT0007_FormList.TabNo.Department) {
        var transFlg = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TransFlg, 0, CtrlFlag.Label, false, false);
        var tabSelected = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TabSelected, 0, CtrlFlag.Label, false, false);
        var date = getValue(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutDate, 1, false, false);
        // 新規で初回のタブ遷移か判定
        if (transFlg == PT0007_TransFlg.FlgNew && tabSelected == 0 && date) {

            setValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TabSelected, 0, CtrlFlag.Label, 1, false, false);
        }
        else if (transFlg == PT0007_TransFlg.FlgNew && tabSelected == 1 && date) {


            // 部門別在庫一覧取得
            var table = P_listData['#' + PT0007_FormList.DepartmentList.Id + getAddFormNo()];
            var rowsLength = $(table.element.children[1]).find(".tabulator-row").length;
            if (rowsLength == 1) {
                // 移庫日にフォーカスを設定
                setFocusDelay(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutDate, 1, CtrlFlag.TextBox, false);
            }
            else if (rowsLength > 1) {
                // 先頭行のNo.リンクにフォーカス
                setTimeout(function () {
                    $($(table.element.children[1]).find(".tabulator-row")[0]).find("a").focus();
                }, 300); //300ミリ秒       
            }

            setValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.TabSelected, 0, CtrlFlag.Label, parseInt(tabSelected) + 1, false, false);
        }
    }
}

/**
 *  明細ﾃﾞｰﾀの入力検証を行う
 *  @return {bool} : ture(OK) false(NG)
 */
function PT0007_validListData() {

    // 共通入力チェック - Validatorの実行
    var formDetail = $(P_Article).find("form[id^='formDetail']");

    // validationのため、畳んだ一覧は展開する
    var switchids = $(formDetail).find("a[data-switchid]");
    var isHiddens = {};
    $.each(switchids, function (idx, switchid) {
        var id = $(switchid).data("switchid");
        var isHidden = isHideId(id);
        isHiddens[id] = isHidden;
        if (isHidden) {
            //表示/非表示切替
            setHideId(id, !isHidden);
        }
    });
    var valid = $(formDetail).valid();
    if (!valid) {
        // 個別ｴﾗｰは最初は非表示
        //var errorHtml = $(formDetail).find("label.errorcom");
        var errorHtml = $("div.errtooltip").find("label.errorcom");
        $(errorHtml).hide();

        //複数選択ﾘｽﾄのｴﾗｰを<ul>ﾀｸﾞで拾う
        setMultiSelectError(formDetail);

        //直接入力ﾊﾟﾀｰﾝの一覧について、行単位に検証
        //直接入力ﾊﾟﾀｰﾝの一覧要素
        var tables = $(formDetail).find("[data-editkbn='" + editPtnDef.Input + "']");
        if (tables != null && tables.length > 0) {

            //直接入力ﾊﾟﾀｰﾝで削除対象行の場合
            //　すべての項目の必須ﾁｪｯｸを行わない。
            //直接入力ﾊﾟﾀｰﾝで新規行の場合
            //　入力項目がすべて未入力の場合は必須入力ｴﾗｰﾁｪｯｸを行わない。

            //ｴﾗｰ有無を再検証
            var valid_w = true;     //ｴﾗｰなし

            $.each($(tables), function (ii, tbl) {

                if ($(tbl).hasClass("vertical_tbl")) {
                    //※縦方向一覧の場合

                    //一覧内にｴﾗｰコントロールを保持していれば、ｴﾗｰ
                    var errors = $(tbl).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                    //var errors = $(tbl).find('.errorcom');    //ｴﾗｰ入力要素
                    if (errors != null && errors.length > 0) {
                        valid_w = false;    //ｴﾗｰあり
                    }
                }
                else {
                    //※横方向一覧の場合

                    //一覧の入力行を取得
                    var trs = getTrsData(tbl);
                    $.each(trs, function (jj, tr) {
                        //※行単位に検証

                        var errors = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                        //var errors = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                        if (errors != null && errors.length > 0) {
                            //※ｴﾗｰありの場合

                            //ｴﾗｰ状態をﾁｪｯｸ

                            //削除行か？
                            var checkedDels = $(tr).find(":checkbox[data-type='ckdel']:checked");     //削除ﾁｪｯｸﾎﾞｯｸｽ:ON
                            if (level == 1 ||
                                (checkedDels != null && checkedDels.length > 0)) {
                                //※行追加時等の必須ﾁｪｯｸを行わないｱｸｼｮﾝの場合
                                //※削除行の場合

                                //すべての入力項目についての必須ﾁｪｯｸを行わない。
                                $.each(errors, function (kk, error) {
                                    //必須ﾁｪｯｸを行わない
                                    var value = $(error).val();
                                    if (value == null || value.length <= 0) {
                                        //255文字以下の場合、ｴﾗｰを解除
                                        relieveItemError(error);
                                    }
                                });

                                //ｴﾗｰ状態を再ﾁｪｯｸ
                                var errorsAf = $(tr).find('select.errorcom,input[type="text"].errorcom,input[type="date"].errorcom,input[type="time"].errorcom,input[type="datetime-local"].errorcom,ul.multiSelect.errorcom,textarea.errorcom');    //ｴﾗｰ入力要素
                                //var errorsAf = $(tr).find('.errorcom');    //ｴﾗｰ入力要素
                                if (errorsAf != null && errorsAf.length > 0) {
                                    valid_w = false;    //ｴﾗｰあり
                                }

                            }
                            else {
                                valid_w = false; // エラーあり
                            }
                        }

                    });     //一覧の入力行数分ループ
                }
            });     //一覧(table)数分ループ

            //ｴﾗｰ再検証結果で上書き
            valid = valid_w;
        }
    }
    // ｴﾗｰがない一覧の折り畳み状態を元に戻す
    $.each(switchids, function (idx, switchid) {
        var id = $(switchid).data("switchid");
        if (!($("#" + id).find(".errorcom").length)) {
            if (isHiddens[id]) {
                //表示/非表示切替
                setHideId(id, isHiddens[id]);
            }
        }
    });


    if (!valid) {
        //個別エラー状態を初期化
        clearErrorStatus3("form[id^='formDetail']");
        //【CJ00000.W01】入力エラーがあります。
        addMessage(P_ComMsgTranslated[941220007], 1)
        $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
        $(P_Article).find(".detail_div").focus();
        return false;
    }

    return true;    //入力ﾁｪｯｸOK
}

/**
 * 移庫先情報(棚番)の移庫数のテキストボックス横に単位を設定
 * @param {any} unitName 数量単位名称
 */
function setValueToLocationNumberUnit(unitName) {
    // 数量管理単位を移庫数にセット  
    var unit = getCtrl(PT0007_FormList.LocationInfoTo.ToMoney.Id, PT0007_FormList.LocationInfoTo.ToMoney.InoutQuantity, 1, CtrlFlag.Label, false, false);
    $(unit).find(".unit")[0].innerText = unitName;
}

/**
 * 移庫先情報(部門)の入庫単価のテキストボックス横に単位を設定
 * @param {any} unitName 数量単位名称
 */
function setValueToDepartmentNumberUnit(currencyName) {
    // 数量管理単位を移庫数にセット  
    var currency = getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.Label, false, false);
    $(currency).find(".unit")[0].innerText = currencyName;
}

/**
 * 選択された行から指定された列の値を取得
 * @param {any} selectedRow 選択された行
 * @param {any} val 項目番号
 */
function getValBySelectedRow(selectedRow, val) {
    return $(selectedRow).find("div[tabulator-field='VAL" + val + "']")[0].innerText;
}

/**
 * 指定されたタブをクリックする
 * @param {any} tabNo タブ番号
 */
function clickTab(tabNo) {
    var tab = $(P_Article).find("a[data-tabno='" + tabNo + "']");
    $(tab).click();
}

/**
 * 指定されたタブを非表示にする
 * @param {any} tabNo タブ番号
 */
function hideTab(tabNo) {
    var tab = $(P_Article).find("a[data-tabno='" + tabNo + "']");
    $(tab).hide();
}

/**
 * 部門に応じて入庫単価の活性/非活性を切り替える
 * @param {any} depFlg
 */
function judgeAndDisableDepartment(depFlg) {

    if (depFlg == PT0007_DepartmentFlg.Fix) {
        // 修理部門の場合は移庫単価の入力を可能にする
        changeInputControl(getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, false, false), true);
    }
    else {
        // 修理部門以外の場合は移庫単価の入力不可
        changeInputControl(getCtrl(PT0007_FormList.DepartmentInfoTo.Id, PT0007_FormList.DepartmentInfoTo.InoutQuantity, 1, CtrlFlag.TextBox, false, false), false);
    }
}

/**
 * 選択された行の色を変更する
 * @param {any} selectedRow 選択されたレコード
 */
function changeRowsColor(selectedRow) {

    // テーブル要素
    var table = $(selectedRow).parent();

    // レコード分色を戻す
    $.each($(table).children(), function (idx, row) {

        setHikiate(row, false);
    });

    // 選択されたレコードの色変更
    setHikiate(selectedRow[0], true);
}

/**
 * 棚番移庫タブ フォーカス設定
 * */
function setFocusLocation(isEdit) {

    if (isEdit) {
        // 登録ボタンにフォーカスを設定
        var btnVisible = getValue(PT0007_FormList.PartsInfo.Id, PT0007_FormList.PartsInfo.BtnVisible, 0, CtrlFlag.Label, false, false);
        if (btnVisible == PT0007_BtnVisible.Visible) {
            setFocusButton(PT0007_FormList.Button.RegistLocation);
            return;
        }
    }

    // 棚別在庫一覧取得
    var table = P_listData['#' + PT0007_FormList.LocationList.Id + getAddFormNo()];
    var rowsLength = $(table.element.children[1]).find(".tabulator-row").length;
    if (rowsLength == 1) {
        // 移庫日にフォーカスを設定
        setFocusDelay(PT0007_FormList.LocationInfoTo.ToWarehouse.Id, PT0007_FormList.LocationInfoTo.ToWarehouse.TransDate, 1, CtrlFlag.TextBox, false);
    }
    else if (rowsLength > 1) {
        // 先頭行のNo.リンクにフォーカス
        setTimeout(function () {
            $($(table.element.children[1]).find(".tabulator-row")[0]).find("a").focus();
        }, 300); //300ミリ秒   
    }
}


