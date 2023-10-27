/* ========================================================================
 *  機能名　    ：   【 PT0003 棚卸 】
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0003\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/PT0005.js\"></script>"); // 入庫入力
document.write("<script src=\"" + getPath() + "/PT0006.js\"></script>"); // 出庫入力
document.write("<script src=\"" + getPath() + "/PT0007.js\"></script>"); // 移庫入力

//機能ID
const ConductId_PT0003 = "PT0003";
// 移庫画面
const ConductId_PT0007 = "PT0007";

// 一覧画面
const FormList = {
    // 機能ID
    ConductId: "PT0003",
    // 画面No
    No: 0,
    //検索条件
    Condition: {
        Id: "COND_010_00_LST_0",
        //対象年月
        TargetYearMonth: 1,
        //予備品倉庫
        StorageLocation: 2,
        //部門
        Department: 3,
        //工場(非表示)
        Factory: 5,
    },
    //フィルタ
    Filter: {
        Id: "BODY_030_00_LST_0",
        Input: 1,
    },
    //棚卸データ一覧
    InventoryList: {
        Id: "BODY_040_00_LST_0",
        TabNo: 1,
        //非表示
        Link: 1,
        //棚卸数
        InventoryQuantity: 13,
        //棚卸確定日時
        FixedDatetime: 16,
    },
    // 入庫一覧
    EnterHistory: {
        Id: "BODY_080_00_LST_0",
        Filter: "BODY_060_00_LST_0",
        FilterInput: 1,
        TabNo: 2,
        InventoryDifferenceId: 13,
        ControlFlag: 14
    },
    // 出庫一覧
    IssueHistory: {
        ParentId: "BODY_090_00_LST_0",
        Filter: "BODY_070_00_LST_0",
        ChildId: "BODY_100_00_LST_0",
        FilterInput: 1,
        KeyValueParent: 10,
        KeyValueChild: 11
    },
    ButtonId: {
        //棚卸準備リスト
        Output: "Output",
        //棚卸準備リスト(CSV)
        OutputCsv: "OutputCsv",
        //棚卸準備取消
        DeletePreparation: "DeletePreparation",
        //新規取込
        IngestNew: "IngestNew",
        //一時登録
        TemporaryRegist: "TemporaryRegist",
        //棚差調整
        AdjustDifference: "AdjustDifference",
        //棚卸確定
        ConfirmInventory: "ConfirmInventory",
        //棚卸確定解除
        CancelConfirmInventory: "CancelConfirmInventory",
        //検索
        Search: "Search",
        //入庫一覧
        WarehousingList: "WarehousingList",
        //出庫一覧
        IssueList: "IssueList",
    },
};

//入庫単価入力画面
const EnterInput = {
    Id: "BODY_000_00_LST_2",
    //画面No
    No: 2,
    //1行目
    FirstRowNo: 1,
    //入庫数
    InoutQuantity: 3,
    //入庫単価
    UnitPrice: 4,
    //金額管理単位
    CurrencyUnit: 5,
    Button: {
        Regist: "Regist"
    }
}

//新規取込画面
const FormUpload = {
    //画面No
    No: 3,
    //ファイルアップロード
    Info: {
        Id: "BODY_000_00_LST_3",
        //エラー出力
        ErrorMessage: 2,
    },
    //非表示一覧
    HiddenInfo: {
        Id: "BODY_010_00_LST_3",
        //棚卸ID
        InventoryId: 1,
        //取込フラグ
        UploadFlg: 2,
        //部門ID（パイプ区切り）
        DepartmentId: 3,
    },
    Button: {
        Upload: "Upload",
        Back: "BackUpload",
    }
};

// 棚卸(受払履歴)
const InoutList = {
    // 画面No
    No: 1,
    // 予備品情報
    InventoryList: {
        Id: "BODY_010_00_LST_1",
        // 予備品ID
        PartsId: 9
    },
    // 入出庫履歴
    EnterIssueHistory: {
        Id: "BODY_020_00_LST_1",
        // 入出庫No
        InoutNo: 7,
        // 入庫数
        InQuantity: 8,
        // 入庫金額
        InventoryAmount: 10,
        // 出庫数
        IssueQuantity: 11,
        // 出庫金額
        IssueAmount: 12,
        // 在庫数
        StockQuantity: 13,
        // 在庫金額
        StockAmount: 14,
        // 遷移区分
        Division: 15,
        // 予備品ID
        PartsId: 16,
        //制御用フラグ
        CotrlFlg: 18,
        //作業No
        WorkNo: 19
    }
}

//新規登録画面
const FormRegist = {
    //画面No
    No: 4,
    //グループ番号
    GroupNo: 2,
    PartsLocation: {
        Id: "BODY_000_00_LST_4",
        //棚枝番
        DetailNo: 2,
        //棚と棚枝番の結合文字列
        JoinString: 4,

    },
    List: {
        Id: "BODY_010_00_LST_4",
        //予備品
        Parts: 1,
        //メーカー
        Manufacturer: 2,
        //材質
        Materials: 3,
        //型式
        ModelType: 4,
        //新旧区分
        OldNewDivision: 5,
        //部門
        DepartmentCd: 6,
        //勘定科目
        AccountCd: 7,
        //棚卸数
        InventoryQuantity: 11,
        //棚差
        InventoryDiff: 12,
        //部門ID(非表示)
        DepartmentId: 14,
        //勘定科目ID(非表示)
        AccountId: 15,
        //勘定科目の新旧区分(非表示)
        AccountOldNewDivision: 16,
        //数量管理単位(非表示)
        Unit: 17,
        //丸め処理区分(非表示)
        RoundDivision: 18,

    },
    Button: {
        Regist: "Regist",
        Back: "Back",
    }
};

// 遷移区分
const TranslationDivision = {
    // 繰越
    Forward: 0,
    // 入庫
    Enter: 1,
    // 出庫
    Issue: 2,
    // 棚番移庫
    Shed: 3,
    // 部門移庫
    Category: 4,
    // 棚卸入庫
    InventoryEnter: 5,
    // 棚卸出庫
    InventoryIssue: 6,
    // 入庫単価入力画面遷移
    TransEnterInput: 0
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
 *  @data {List<COM_TMPTBL_DATA>}    ：初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    // 共通-入庫入力画面の初期化処理
    PT0005_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    // 共通-出庫入力画面の初期化処理
    PT0006_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    // 共通-移庫入力画面の初期化処理
    PT0007_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    if (formNo == EnterInput.No) {
        // 決定ボタンにフォーカスをセット
        setFocusButton(EnterInput.Button.Regist);
    }
    else if (formNo == FormUpload.No) {
        //エラー出力を非活性にする
        var errorMsg = getCtrl(FormUpload.Info.Id, FormUpload.Info.ErrorMessage, 1, CtrlFlag.Textarea);
        changeInputControl(errorMsg, false);
    } else if (formNo == FormRegist.No) {
        //グループ折りたたみアイコンを非表示にする
        toggleHideGroupTitle(FormRegist.GroupNo, true);

        // 結合文字列を枝番入力コントロールのヘッダーにセット
        var joinStr = getValue(FormRegist.PartsLocation.Id, FormRegist.PartsLocation.JoinString, 1, CtrlFlag.Label);
        setValue(FormRegist.PartsLocation.Id, FormRegist.PartsLocation.DetailNo, 1, CtrlFlag.Label, joinStr, false, true);

        //棚枝番のテーブルタグの幅を調整（ヘッダが小さい為余白が目立つ為）
        var detailNo = getCtrl(FormRegist.PartsLocation.Id, FormRegist.PartsLocation.DetailNo, 1, CtrlFlag.Label);
        var table = $(detailNo).closest("table.vertical_tbl");
        $(table).addClass("DetailNoTableWidth");

        //棚卸数にイベントを付与（棚差に値を設定する）
        setInventoryQuantityChangeEvent();
    }

    // 一覧の表示状態を切り替え(初期表示時は子要素非表示)
    changeListDisplay(FormList.IssueHistory.ChildId, false, false);
}

/**
 * 棚卸数にイベントを付与（棚差に値を設定する）
 */
function setInventoryQuantityChangeEvent() {
    //棚卸数
    var inventoryQuantity = getCtrl(FormRegist.List.Id, FormRegist.List.InventoryQuantity, 1, CtrlFlag.TextBox);
    $(inventoryQuantity).blur(function () {
        //入力値
        var val = Number($(this).val()) ? Number($(this).val()) : 0;
        //数量管理単位
        var unit = getCtrl(FormRegist.List.Id, FormRegist.List.Unit, 1, CtrlFlag.Combo);
        var selectEle = $(unit).find("option:selected");
        //小数点以下桁数を取得
        var digitNum = $(selectEle).attr("exparam1") ? selectEle.attr("exparam1") : 0;
        //丸め処理区分取得
        var roundDivision = getValue(FormRegist.List.Id, FormRegist.List.RoundDivision, 1, CtrlFlag.Label);
        var formatVal = roundDigit(val, digitNum, roundDivision);
        //フォーマットした値を棚卸数に設定
        setValue(FormRegist.List.Id, FormRegist.List.InventoryQuantity, 1, CtrlFlag.TextBox, formatVal);
        //棚卸数に-1をかけた値を棚差に設定
        setValue(FormRegist.List.Id, FormRegist.List.InventoryDiff, 1, CtrlFlag.Label, "-" + formatVal);
    });

}

/**
 *【オーバーライド用関数】
 *  画面状態設定後の個別実装
 *
 * @status {number}       ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
 * @pageRowCount {number} ：ﾍﾟｰｼﾞ全体のﾃﾞｰﾀ行数
 * @conductPtn {byte}     ：com_conduct_mst.ptn
 * @formNo {number}       ：画面番号
 */
function setPageStatusEx(status, pageRowCount, conductPtn, formNo) {
    if (formNo == FormList.No) {
        // 棚卸調整データタブ(初期表示は棚卸データタブ)
        // ボタン色を変更
        setGrayButton(FormList.ButtonId.WarehousingList, false);
        setGrayButton(FormList.ButtonId.IssueList, true);

        // フィルタの表示状態を切り替え(初期表示時は入庫一覧を表示)
        changeListDisplay(FormList.EnterHistory.Filter, true);  // 入庫一覧用 フィルタ表示
        changeListDisplay(FormList.IssueHistory.Filter, false); // 出庫一覧用 フィルタ非表示

        // 一覧の表示状態を切り替え(初期表示時は入庫一覧を表示)
        changeListDisplay(FormList.EnterHistory.Id, true);        // 入庫一覧 表示
        changeListDisplay(FormList.IssueHistory.ParentId, false); // 出庫一覧 非表示
        changeListDisplay(FormList.IssueHistory.ChildId, false);  // 出庫一覧(子要素) 非表示

        // 棚卸準備リストボタンの非活性制御（ゲストユーザの場合は非活性とする。EXCEL出力ボタンは共通側での非活性制御の対象外）
        //ボタン権限から棚卸準備取消ボタンの権限を取得
        var buttonDefines = P_buttonDefine[ConductId_PT0003];
        if (buttonDefines) {
            //棚卸準備取消のボタン表示区分（活性/非活性）を取得
            var define = $.grep(buttonDefines, function (item, idx) {
                return item.FORMNO == FormList.No && item.CTRLID && item.CTRLID == FormList.ButtonId.DeletePreparation;
            });
            var dispKbn = define ? define[0].DISPKBN : btnDispKbnDef.Active;
            //棚卸準備リストのボタン表示区分を更新
            $.each(buttonDefines, function (i, buttonDefine) {
                if (buttonDefine.CTRLID && buttonDefine.CTRLID == FormList.ButtonId.Output) {
                    P_buttonDefine[ConductId_PT0003][i].DISPKBN = dispKbn;
                    return false;
                }

            });

            $.each(buttonDefines, function (i, buttonDefine) {
                if (buttonDefine.CTRLID && buttonDefine.CTRLID == FormList.ButtonId.OutputCsv) {
                    P_buttonDefine[ConductId_PT0003][i].DISPKBN = dispKbn;
                    return false;
                }

            });
        }
    }
    else if (formNo == EnterInput.No) {
        // 入庫単価入力画面
        // 入庫単価エリア取得
        var price = getCtrl(EnterInput.Id, EnterInput.UnitPrice, EnterInput.FirstRowNo, CtrlFlag.Input, false, false);
        var CurrencyUnitArea = $(price).closest("div").find(".unit");

        // 単位なので直接値セット
        $(CurrencyUnitArea)[0].innerText = getValue(EnterInput.Id, EnterInput.CurrencyUnit, EnterInput.FirstRowNo, CtrlFlag.TextBox, false, false);
    }

    // 共通-出庫入力画面
    PT0006_setPageStatusEx(status, pageRowCount, conductPtn, formNo);

}



/**
 *【オーバーライド用関数】
 *  個別実装ボタン
 *
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number} formNo      ：画面NO
 *  @param {string} btnCtrlId   ：ボタンCTRLID
 */
function clickIndividualImplBtn(appPath, formNo, btnCtrlId) {
    if (btnCtrlId == FormList.ButtonId.WarehousingList) { // 入庫一覧表示ボタン

        // ボタン色を変更
        setGrayButton(FormList.ButtonId.WarehousingList, false);
        setGrayButton(FormList.ButtonId.IssueList, true);

        // フィルタの表示状態を切り替え
        changeListDisplay(FormList.EnterHistory.Filter, true);  // 入庫一覧用 フィルタ表示
        changeListDisplay(FormList.IssueHistory.Filter, false); // 出庫一覧用 フィルタ非表示

        // 一覧の表示状態を切り替え
        changeListDisplay(FormList.EnterHistory.Id, true);        // 入庫一覧 表示
        changeListDisplay(FormList.IssueHistory.ParentId, false); // 出庫一覧 非表示
        changeListDisplay(FormList.IssueHistory.ChildId, false);  // 出庫一覧(子要素) 非表示

        // 遷移区分が0の場合はリンク非活性
        changeNoneLink();

    } else if (btnCtrlId == FormList.ButtonId.IssueList) { // 出庫一覧表示ボタン

        // ボタン色を変更
        setGrayButton(FormList.ButtonId.WarehousingList, true);
        setGrayButton(FormList.ButtonId.IssueList, false);

        // フィルタの表示状態を切り替え
        changeListDisplay(FormList.EnterHistory.Filter, false);  // 入庫一覧用 フィルタ非表示
        changeListDisplay(FormList.IssueHistory.Filter, true);   // 出庫一覧用 フィルタ表示

        // 一覧の表示状態を切り替え
        changeListDisplay(FormList.EnterHistory.Id, false);      // 入庫一覧 非表示
        changeListDisplay(FormList.IssueHistory.ParentId, true); // 出庫一覧 表示
    }
}

/**
 * 【オーバーライド用関数】
 *   コンボボックス変更時イベント
 * @param  appPath    : アプリケーションルートパス
 * @param  combo      : 変更イベントが発生したコンボボックスの要素
 * @param  datas      : 変更イベントが発生したコンボボックスのデータリスト
 * @param  selected   : 変更されたコンボボックスの選択されたデータ
 * @param  formNo     : イベントの発生した画面番号
 * @param  ctrlId     : イベントの発生した画面のコントロールID
 * @param  valNo      : イベントの発生したコンボボックスのコントロール番号
 */
function setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {

    // 共通-入庫入力画面のコンボボックス変更処理
    PT0005_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    // 共通-移庫入力画面のコンボボックス変更処理
    PT0007_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // 新旧区分コンボボックスが変更された場合
    if (ctrlId == FormRegist.List.Id && valNo == FormRegist.List.OldNewDivision) {
        // 選択された要素の拡張項目の値を取得
        if (selected.EXPARAM1 == PT0005_OldNewDivisionCd.New) {
            // 「新品」の場合、勘定項目に「設備貯蔵品」をセット
            setValueAndTrigger(FormRegist.List.Id, FormRegist.List.AccountCd, 1, CtrlFlag.TextBox, PT0005_AccountCd.Equipment, false, false);
        } else if (selected.EXPARAM1 == PT0005_OldNewDivisionCd.Old) {
            // 「中古品」の場合、勘定項目に「中古貯蔵品」をセット
            setValueAndTrigger(FormRegist.List.Id, FormRegist.List.AccountCd, 1, CtrlFlag.TextBox, PT0005_AccountCd.Old, false, false);
        }
        // 新旧区分にフォーカスをセット
        $(combo).focus();
    }

    // 予備品倉庫コンボボックスが変更された場合
    if (ctrlId == FormList.Condition.Id && valNo == FormList.Condition.StorageLocation) {
        //非表示の工場コンボボックスの値を取得
        var factory = getValue(FormList.Condition.Id, FormList.Condition.Factory, 1, CtrlFlag.Combo);
        if (selected && selected.EXPARAM1 && selected.EXPARAM1 != factory) {
            // 予備品倉庫で選択されたデータに紐づく工場を非表示の工場コンボにセット→部門が工場で絞り込まれる
            setValueAndTrigger(FormList.Condition.Id, FormList.Condition.Factory, 1, CtrlFlag.Combo, selected.EXPARAM1, false, false);
        }
    }
}

/**
 * ボタンコントロールIDを指定して通常カラー/グレーを切り替える
 * @param {any} name ボタンのコントロール
 * @param {any} flg true(非表示)、false(表示)
 */
function setGrayButton(name, flg) {
    //ボタン要素取得
    var button = getButtonCtrl(name);
    setClickedColor(button, flg);
}

/**
 *  指定要素を非表示とする。
 *  @element   {string} ：<div>要素指定文字列
 *  @flg       {bool}   ：true(非表示)、false(表示)
 */
function setClickedColor(element, flg) {
    //true(グレー)、false(通常)
    if (flg) {
        $(element).addClass('clickedButton');
    }
    else {
        $(element).removeClass('clickedButton');
    }
}


/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {

    // 入出庫一覧 出庫入力画面
    PT0006_postBuiltTabulator(tbl, id);
    // 移庫入力画面
    PT0007_postBuiltTabulator(tbl, id);

    if (id == "#" + FormList.InventoryList.Id + getAddFormNo()) {
        //棚卸データ一覧
        //棚卸確定日時に値が入っている場合、棚卸数は編集不可
        setDisableInventoryQuantity(tbl);

    }
    else if (id == "#" + FormList.EnterHistory.Id + getAddFormNo()) {
        //入庫履歴一覧
        //入庫一覧行のNoリンク非活性制御
        setDisabledLink(tbl);
    }
    else if (id == "#" + InoutList.EnterIssueHistory.Id + getAddFormNo()) {
        //詳細画面入庫履歴一覧
        //在庫数、在庫金額を計算して表示
        setStockCalculation(tbl);
    }
    else if (id == "#" + FormList.IssueHistory.ParentId + getAddFormNo()) {
        // 出庫一覧の場合Tabulatorの描画が完了後（renderComplete完了後）の処理
        convertTabulatorListToNestedTable(id, FormList.IssueHistory.ParentId, FormList.IssueHistory.ChildId, FormList.IssueHistory.KeyValueParent, FormList.IssueHistory.KeyValueChild)
    }
}

/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginal(tabNo, tableId) {

    // 入庫履歴一覧の場合
    if (tabNo == FormList.EnterHistory.TabNo && tableId == FormList.EnterHistory.Id + getAddFormNo()) {
        // 遷移区分が0の場合はリンク非活性
        changeNoneLink();
    }
    if (tabNo == FormList.InventoryList.TabNo && tableId == FormList.InventoryList.Id + getAddFormNo()) {
        //棚卸データ一覧
        //棚卸確定日時に値が入っている場合、棚卸数は編集不可
        setDisableInventoryQuantity(P_listData["#" + FormList.InventoryList.Id + getAddFormNo()]);

    }

    // 移庫入力画面 タブ切替時
    PT0007_initTabOriginal(tabNo, tableId);
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevCommonValidCheck(appPath, conductId, formNo, btn) {

    return PT0007_prevCommonValidCheck(appPath, conductId, formNo, btn);

}

/**
 * tblがパラメータに渡ってこない場合のリンク非活性制御
 * */
function changeNoneLink() {
    //入庫一覧行のNoリンク非活性制御
    setDisabledLink(P_listData['#' + FormList.EnterHistory.Id + getAddFormNo()]);
}

/**
 * 【オーバーライド用関数】実行正常終了後処理
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId   ：機能ID
 *  @param pgmId       ：プログラムID
 *  @param formNo      ：画面NO
 *  @param btn         ：ボタン要素
 *  @param conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param isEdit      ：単票表示フラグ
 *  @param data        ：結果ﾃﾞｰﾀ
 */
function postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

    // 共通-出庫入力詳細画面の実行正常終了後処理
    PT0006_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
    // 共通-移庫入力詳細画面の実行正常終了後処理
    PT0007_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    // ボタン名取得
    var btnName = $(btn).attr('name');
    if (formNo == EnterInput.No && btnName == EnterInput.Button.Regist) {
        // 入庫単価入力画面
        // 登録実行正常終了後画面を閉じて遷移元に移動
        var modal = $(btn).closest('section.modal_form');
        $(modal).modal('hide');
    }
    else if (formNo == FormUpload.No && btnName == FormUpload.Button.Upload) {
        //新規取込画面 取込押下時

        //取込フラグ
        var uploadFlgStr = getValue(FormUpload.HiddenInfo.Id, FormUpload.HiddenInfo.UploadFlg, 1, CtrlFlag.Label);
        var uploadFlg = convertStrToBool(uploadFlgStr);
        if (uploadFlg) {
            //非表示の棚卸IDに値が入っている場合は、ファイルを取り込んだので一覧画面へ反映

            //棚卸ID
            var inventoryId = getValue(FormUpload.HiddenInfo.Id, FormUpload.HiddenInfo.InventoryId, 1, CtrlFlag.Label);
            if (inventoryId) {
                //取込ボタン押下フラグを設定
                P_dicIndividual["UploadFlg"] = true;

                //部門ID
                var departmentId = getValue(FormUpload.HiddenInfo.Id, FormUpload.HiddenInfo.DepartmentId, 1, CtrlFlag.Label);
                //一覧画面の検索条件の部門に値を反映する
                var article = $('article[name="main_area"][data-formno="' + FormList.No + '"]');
                var trs = $(article).find("#" + FormList.Condition.Id + "_" + FormList.No);
                var td = $(trs).find("td[data-name='VAL" + FormList.Condition.Department + "']")[0];
                var mult = $(td).find('ul.multiSelect');
                var all = $(mult).find('input.ctrloption');
                //一度全解除し、初期化
                $(all).prop('checked', false);
                $(all).trigger('change');
                //部門IDの値を設定
                if (departmentId) {
                    setDataForMultiSelect(td, mult, departmentId);
                } else {
                    //全選択
                    $(all).prop('checked', true);
                    $(all).trigger('change');
                }
            }

            // 新規取込画面を閉じて、一覧画面へ戻る
            var back = getButtonCtrl(FormUpload.Button.Back);
            var modal = $(back).closest('section.modal_form');
            $(modal).modal('hide');
        }
    }
}

/**
 * 【オーバーライド用関数】Tabulatorのページ変更後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} pageNo 表示するページNo
 * @param {any} pagesize 表示中のページの件数
 */
function postTabulatorChangePage(tbl, id, pageNo, pagesize) {
    if (getFormNo() == FormList.No && id == "#" + FormList.InventoryList.Id + getAddFormNo()) {
        //棚卸データ一覧
        //棚卸確定日時に値が入っている場合、棚卸数は編集不可
        setDisableInventoryQuantity(tbl);
    }
    else if (getFormNo() == FormList.No && id == "#" + FormList.EnterHistory.Id + getAddFormNo()) {
        //入庫履歴一覧
        //入庫一覧行のNoリンク非活性制御
        setDisabledLink(tbl);
    }
    else if (getFormNo() == InoutList.No && id == "#" + InoutList.EnterIssueHistory.Id + getAddFormNo()) {
        //詳細画面入庫履歴一覧
        //在庫数、在庫金額を計算して表示
        setStockCalculation(tbl);
    }
}

/**
 * 【オーバーライド用関数】Tabulatorの列フィルター後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} filters フィルターの条件
 * @param {any} rows フィルター後の行rows
 */
function postTabulatorDataFiltered(tbl, id, filters, rows) {
    if (getFormNo() == FormList.No && id == "#" + FormList.InventoryList.Id + getAddFormNo()) {
        //棚卸データ一覧
        //棚卸確定日時に値が入っている場合、棚卸数は編集不可
        setDisableInventoryQuantity(tbl);
    }
    else if (getFormNo() == FormList.No && id == "#" + FormList.EnterHistory.Id + getAddFormNo()) {
        //入庫履歴一覧
        //入庫一覧行のNoリンク非活性制御
        setDisabledLink(tbl, rows);
    }
}

/**
 * 棚卸確定日時に値が入っている場合、棚卸数を非活性化
 * @param {any} tbl 一覧
 */
function setDisableInventoryQuantity(tbl) {
    var rows = tbl.getRows("active");
    if (rows.length > 0) {
        $.each(rows, function (idx, row) {
            var data = row.getData();
            //棚卸確定日時
            var fixedDatetime = data["VAL" + FormList.InventoryList.FixedDatetime];
            if (fixedDatetime) {
                //ブラウザに処理が戻った際に実行
                setTimeout(function () {
                    //棚卸確定日時に値が入っている場合、棚卸数は編集不可
                    var ele = row.getCell("VAL" + FormList.InventoryList.InventoryQuantity).getElement();
                    //非活性化
                    setDisableElements(ele, true);
                }, 0);
            }
        });
    }
}

/**
 * 入庫一覧行のNoリンク非活性制御
 * @param {any} tbl 一覧
 * @param {any} rows 行
 */
function setDisabledLink(tbl, rows) {
    // 対象コントール内の全行取得
    var trs = rows ?? tbl.getRows("display");

    $(trs).each(function (i, tr) {

        // 遷移区分取得
        var division = tr.getData()['VAL' + FormList.EnterHistory.ControlFlag];

        // 受払履歴テーブルより表示されている行
        if (division == TranslationDivision.TransEnterInput) {
            //ブラウザに処理が戻った際に実行
            setTimeout(function () {
                // データのリンクを無効にする
                noneLinkCarryData(true, tr.getData()["ROWNO"]);
            }, 0);
        }
    });
}

/**
 * 在庫数、在庫金額を計算して表示
 * @param {any} tbl 一覧
 */
function setStockCalculation(tbl) {
    // 対象コントール内の全行取得
    var trs = tbl.getRows();
    $(trs).each(function (i, tr) {

        // 制御用フラグ取得
        var division = tr.getData()['VAL' + InoutList.EnterIssueHistory.CotrlFlg];

        // 繰越行
        if (division == TranslationDivision.Forward) {
            // データのリンクを無効にする
            noneLinkCarryData(false, 0);
            return true;
        }

        // 出庫以外
        if (division != TranslationDivision.Issue) {
            // 入庫数取得
            var inQuantity = tr.getData()['VAL' + InoutList.EnterIssueHistory.InQuantity].replace(/[^-0-9.]/g, '');
            // 入庫数単位取得
            var unit = tr.getData()['VAL' + InoutList.EnterIssueHistory.InQuantity].replace(/[0-9.,]/g, '');

            // 入庫金額取得
            var inventoryAmount = tr.getData()['VAL' + InoutList.EnterIssueHistory.InventoryAmount].replace(/[^-0-9.]/g, '');
            // 入庫金額単位取得
            var currency = tr.getData()['VAL' + InoutList.EnterIssueHistory.InventoryAmount].replace(/[0-9.,]/g, '')

            // 繰越なしの場合
            if (i == 0) {
                // そのまま画面表示
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockQuantity, i, CtrlFlag.Label, parseFloat(inQuantity).toLocaleString().toString() + unit);
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockAmount, i, CtrlFlag.Label, parseFloat(inventoryAmount).toLocaleString().toString() + currency);
            }
            else {
                // 前行の在庫数、在庫金額取得
                var stockQuantity = getValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockQuantity, i - 1, CtrlFlag.Label, false, false);
                stockQuantity = stockQuantity.replace(/[^-0-9.]/g, '');
                var stockAmount = getValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockAmount, i - 1, CtrlFlag.Label, false, false);
                stockAmount = stockAmount.replace(/[^-0-9.]/g, '');

                // 在庫数 + 入庫数
                var totalStock = parseFloat(stockQuantity) + parseFloat(inQuantity);
                // 在庫金額 + 入庫金額
                var total = parseFloat(stockAmount) + parseFloat(inventoryAmount);

                // 画面表示
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockQuantity, i, CtrlFlag.Label, totalStock.toLocaleString().toString() + unit);
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockAmount, i, CtrlFlag.Label, total.toLocaleString().toString() + currency);
            }
        }
        else {
            // 出庫数取得
            var issueQuantity = tr.getData()['VAL' + InoutList.EnterIssueHistory.IssueQuantity].replace(/[^-0-9.]/g, '');
            // 出庫数単位取得
            var unit = tr.getData()['VAL' + InoutList.EnterIssueHistory.IssueQuantity].replace(/[0-9.,]/g, '');

            // 出庫金額取得
            var issueAmount = tr.getData()['VAL' + InoutList.EnterIssueHistory.IssueAmount].replace(/[^-0-9.]/g, '');
            // 出庫金額単位取得
            var currency = tr.getData()['VAL' + InoutList.EnterIssueHistory.IssueAmount].replace(/[0-9.,]/g, '');

            // 繰越なしの場合
            if (i == 0) {
                // そのまま画面表示
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockQuantity, i, CtrlFlag.Label, issueQuantity.toLocaleString().toString() + unit);
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockAmount, i, CtrlFlag.Label, issueAmount.toLocaleString().toString() + currency);
            }
            else {
                // 前行の在庫数、在庫金額取得
                var stockQuantity = getValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockQuantity, i - 1, CtrlFlag.Label, false, false);
                stockQuantity = stockQuantity.replace(/[^-0-9.]/g, '');
                var stockAmount = getValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockAmount, i - 1, CtrlFlag.Label, false, false);
                stockAmount = stockAmount.replace(/[^-0-9.]/g, '');

                // 在庫数 - 入庫数
                var totalStock = parseFloat(stockQuantity) - parseFloat(issueQuantity);
                // 在庫金額 - 入庫金額
                var total = parseFloat(stockAmount) - parseFloat(issueAmount);

                // 画面表示
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockQuantity, i, CtrlFlag.Label, totalStock.toLocaleString().toString() + unit);
                setValue(InoutList.EnterIssueHistory.Id, InoutList.EnterIssueHistory.StockAmount, i, CtrlFlag.Label, total.toLocaleString().toString() + currency);
            }
        }

    });
}

/**
 * データのリンクを無効にする
 * @param {boolean} flg 棚差調整入庫一覧かどうか
 * */
function noneLinkCarryData(flg, rowNo) {

    // コントロールID
    var ctrlId;

    // 入庫履歴一覧の場合
    if (flg) {

        // 棚差調整入庫一覧の要素取得
        ctrlId = FormList.EnterHistory.Id;

        //  一覧情報取得
        var rows = P_listData["#" + ctrlId + getAddFormNo()].searchRows("ROWNO", "=", rowNo);
        var rowEle = rows[0].getElement();
        rows = null;
        if (rowEle && $(rowEle).length > 0) {
            // レコードのNo.リンクを無効化する
            var link = $(rowEle).find("div[tabulator-field='ROWNO']");
            $(link).addClass("linkDisable");
            // 文字色を黒に変更
            var value = $(link).find("a");
            if (value && value.length > 0) {
                $(value)[0].style.color = "black";
            }

        }
    }
    else {
        // 入出庫履歴一覧の要素取得
        ctrlId = InoutList.EnterIssueHistory.Id;

        // 入出庫一覧の要素取得
        var tbl = $("#" + ctrlId + getAddFormNo() + "_div").find("div .tabulator-row");
        if (tbl.length > 0) {

            // 繰越のデータのレコードのNo.リンクを無効化する
            var link = $("#" + ctrlId + getAddFormNo() + "_div").find("div .tabulator-row").find("div[tabulator-field='ROWNO']")[0];
            $(link).addClass("linkDisable");
            // 文字色を黒に変更
            var value = $(link).find("a");
            $(value)[0].style.color = "black";

        }
    }
}

/**
 *【オーバーライド用関数】
 *  遷移処理の前
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param formNo      ：遷移元formNo
 *  @param pctrlId     ：遷移元の一覧ctrlid
 *  @param btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param element     ：ｲﾍﾞﾝﾄ発生要素
 */
function prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];

    // 一覧フィルタ処理実施
    if (ctrlId == FormList.Filter.Id) {
        //棚卸データ
        if (executeListFilter(transTarget, FormList.InventoryList.Id, FormList.Filter.Id, FormList.Filter.Input)) {
            return [false, conditionDataList];
        }
    }
    if (ctrlId == FormList.EnterHistory.Filter) {
        //棚卸調整データ　入庫一覧
        if (executeListFilter(transTarget, FormList.EnterHistory.Id, FormList.EnterHistory.Filter, FormList.EnterHistory.FilterInput)) {
            return [false, conditionDataList];
        }
    }
    if (ctrlId == FormList.IssueHistory.Filter) {
        //棚卸調整データ　出庫一覧
        if (executeListFilter(transTarget, FormList.IssueHistory.ParentId, FormList.IssueHistory.Filter, FormList.IssueHistory.FilterInput)) {
            return [false, conditionDataList];
        }
    }

    if (btn_ctrlId == FormList.ButtonId.IngestNew) {
        //新規取込押下時

        // メッセージをクリア
        clearMessage();

        //対象年月
        var targetYearMonth = getCtrl(FormList.Condition.Id, FormList.Condition.TargetYearMonth, 1, CtrlFlag.TextBox);
        //予備品倉庫
        var storageLocation = getCtrl(FormList.Condition.Id, FormList.Condition.StorageLocation, 1, CtrlFlag.Combo);

        //対象年月、予備品倉庫の必須チェック
        if (!$(targetYearMonth).valid() || !$(storageLocation).valid()) {

            // 個別ｴﾗｰは最初は非表示
            var errorHtml = $("div.errtooltip").find("label.errorcom");
            $(errorHtml).hide();
            //入力エラーがあります。
            addMessage(P_ComMsgTranslated[941220007], 1);

            return [false, conditionDataList];
        }
    }

    if (ctrlId == FormList.InventoryList.Id && rowNo != -1 && transTarget == FormRegist.No) {
        //棚卸データ一覧のNoリンクがクリックされた場合、非表示列のリンクをクリックし受払履歴画面へ遷移
        //一覧の＋アイコンで表示する新規登録画面とリンクで遷移する画面は別画面の為、非表示のリンク列が必要
        var link = $(element).closest(".tabulator-row").find("div[tabulator-field=VAL" + FormList.InventoryList.Link + "]").find("a");
        // 非表示のリンククリック
        $(link).click();
        return [false, conditionDataList];
    }

    // 遷移元が入庫一覧の場合
    if (ctrlId == FormList.EnterHistory.Id) {
        // 遷移区分取得
        var rowData = P_listData["#" + FormList.EnterHistory.Id + getAddFormNo()].searchData("ROWNO", "=", rowNo)[0];
        var division = rowData["VAL" + FormList.EnterHistory.ControlFlag];

        // 遷移区分が0の場合はリンク非活性
        if (division == TranslationDivision.TransEnterInput) {
            noneLinkCarryData(true, rowNo - 1);
            return [false, conditionDataList];
        }
    }

    // 棚卸(受払履歴)への遷移の場合
    if (transTarget == InoutList.No) {
        // 対象年月取得
        var targetDate = getValue(FormList.Condition.Id, FormList.Condition.TargetYearMonth, 0, CtrlFlag.TextBox, false, false);
        // 検索条件をセット
        conditionDataList.push(getParamToPT0003_1(targetDate));
    }

    // 遷移元が入出庫履歴の場合
    if (ctrlId == InoutList.EnterIssueHistory.Id && transTarget == FormList.ConductId) {
        // 遷移リンクVAL値
        var linkVal = 0;

        // 遷移区分取得
        var division = $(element).parent().parent().find("div[tabulator-field='VAL" + InoutList.EnterIssueHistory.Division + "']")[0].innerText;

        // 入庫、棚卸入庫の場合
        if (division == TranslationDivision.Enter || division == TranslationDivision.InventoryEnter) {
            linkVal = TranslationDivision.Enter;
        }
        // 出庫、棚卸出庫の場合
        else if (division == TranslationDivision.Issue || division == TranslationDivision.InventoryIssue) {
            linkVal = TranslationDivision.Issue;
        }
        // 棚番移庫の場合
        else if (division == TranslationDivision.Shed) {
            linkVal = TranslationDivision.Shed;
        }
        // 部門移庫の場合
        else {
            linkVal = TranslationDivision.Category;
        }

        //入出庫履歴のNoリンクがクリックされた場合、非表示列のリンクをクリックし各画面へ遷移
        var transLink = $(element).closest(".tabulator-row").find("div[tabulator-field=VAL" + linkVal + "]").find("a");
        // 非表示のリンククリック
        $(transLink).click();

        return [false, conditionDataList];
    }
    // 移庫入力に遷移の場合は条件を受け渡す
    else if (ctrlId == InoutList.EnterIssueHistory.Id && transTarget == ConductId_PT0007) {
        // 遷移区分取得
        var division = $(element).parent().parent().find("div[tabulator-field='VAL" + InoutList.EnterIssueHistory.Division + "']")[0].innerText;

        // 予備品ID
        var PartsId = $(element).parent().parent().find("div[tabulator-field='VAL" + InoutList.EnterIssueHistory.PartsId + "']")[0].innerText;

        // 作業No
        var WorkNo = $(element).parent().parent().find("div[tabulator-field='VAL" + InoutList.EnterIssueHistory.WorkNo + "']")[0].innerText;

        if (division == TranslationDivision.Shed) {
            // 移庫入力画面の検索条件をセット(棚番移庫)
            conditionDataList.push(getParamToPT0007(PT0007_SubjectList_CtrlId, PartsId, WorkNo));
        }
        // 部門移庫の場合
        else {
            // 移庫入力画面の検索条件をセット(部門移庫)
            conditionDataList.push(getParamToPT0007(PT0007_DepartmentList_CtrlId, PartsId, WorkNo));
        }
    }

    //共通-出庫入力画面部門在庫一覧の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecPT0006_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return PT0006_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }
    //共通-移庫入力画面部門在庫一覧の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecPT0007_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return PT0007_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }

    // 一覧画面から新規登録画面を表示時（棚卸一覧の＋アイコン押下時）
    if (ctrlId == FormList.InventoryList.Id && rowNo == -1 && transTarget == FormRegist.No) {
        // 検索条件のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [FormList.Condition.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
    }


    return [true, conditionDataList];
}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function reportCheckPre(appPath, conductId, formNo, btn) {

    // 一覧画面で棚卸準備リストボタン・棚卸準備リスト(CSV)ボタン押下時
    if (formNo == FormList.No && ($(btn).attr("name") == FormList.ButtonId.Output || $(btn).attr("name") == FormList.ButtonId.OutputCsv)) {
        // 一覧にチェックされた行が存在しない場合、処理をキャンセル
        if (!isCheckedList(FormList.InventoryList.Id)) {
            return false;
        }
    }

    return true;
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function registCheckPre(appPath, conductId, formNo, btn) {
    var btnName = $(btn).attr("name");

    // 一覧画面で棚卸準備取消ボタン、一時登録ボタン、棚差調整ボタン、棚卸確定ボタン、棚卸確定解除ボタン押下時
    if (formNo == FormList.No && (btnName == FormList.ButtonId.DeletePreparation || btnName == FormList.ButtonId.TemporaryRegist
        || btnName == FormList.ButtonId.AdjustDifference || btnName == FormList.ButtonId.ConfirmInventory || btnName == FormList.ButtonId.CancelConfirmInventory)) {
        // 一覧にチェックされた行が存在しない場合、処理をキャンセル
        if (!isCheckedList(FormList.InventoryList.Id)) {
            return false;
        }
    }

    return true;
}

/**
 * 【オーバーライド用関数】Tabuator一覧の描画前処理
 * @param {any} id 一覧のID(#,_FormNo付き)
 * @param {any} options 一覧のオプション情報
 * @param {any} header ヘッダー情報
 * @param {any} dispData データ
 */
function prevCreateTabulator(appPath, id, options, header, dispData) {
    if (getFormNo() == FormList.No && id == '#' + FormList.InventoryList.Id + getAddFormNo()) {
        //棚卸一覧

        $.each(header, function (idx, head) {
            if (head.field == "VAL" + FormList.InventoryList.InventoryQuantity) {
                //棚卸数は入力可とする
                if (head.cssClass) {
                    head.cssClass = head.cssClass + " not_readonly";
                } else {
                    head.cssClass = "not_readonly";
                }
            }
        });
    }
}

/**
 * 【オーバーライド用関数】Tabuator一覧のヘッダー設定前処理
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {string} id               ：一覧のID(#,_FormNo付き)
 * @param {object} header           ：ヘッダー情報
 * @param {Element} headerElement   ：ヘッダー要素
 * @param {List<string>} subTableIdList ：入れ子一覧の場合、子一覧のID
 */
function prevSetTabulatorHeader(appPath, id, header, headerElement, subTableIdList) {
    if (id == "#" + FormList.IssueHistory.ParentId + getAddFormNo()) {
        setNestedTable(id, header, FormList.IssueHistory.ParentId, FormList.IssueHistory.ChildId, subTableIdList);
    }

}

/**
 *【オーバーライド用関数】
 *  戻る処理の前(単票、子画面共用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @codeTransFlg   {int}       ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 */
function prevBackBtnProcess(appPath, btnCtrlId, status, codeTransFlg) {
    if (getFormNo() == FormUpload.No && btnCtrlId == FormUpload.Button.Back) {
        if (P_dicIndividual["UploadFlg"]) {
            //取込ボタン押下による戻る処理の場合は、一覧画面に渡すキー情報をセットし一覧を再検索
            const conditionDataList = getListDataByCtrlIdList([FormUpload.HiddenInfo.Id], FormUpload.No, 0);
            setSearchCondition(ConductId_PT0003, FormList.No, conditionDataList);

            delete P_dicIndividual["UploadFlg"];
            return true;
        }
        //戻るボタンが押下されたときは画面を閉じるだけ
        return false;
    }
    if (getFormNo() == FormRegist.No && btnCtrlId == FormRegist.Button.Back) {
        //戻るボタンが押下されたときは画面を閉じるだけ
        return false;
    }
    return true;
}


/**
 *【オーバーライド用関数】取込処理個別入力チェック
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  
 *  @return {bool}:個別入力チェック後も既存の入力チェックを行う場合はtrue
 *  @return {bool}:個別の入力チェックでエラーの場合はtrue
 */
function preInputCheckUpload(appPath, conductId, formNo) {
    var isContinue = true
    var isError = false;
    var isAutoBackFlg = false;

    return [isContinue, isError, isAutoBackFlg];
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {
    if (formNo == FormUpload.No || formNo == FormRegist.No) {
        //新規取込画面 チェック時、新規登録画面 登録時

        //一覧画面の検索条件を渡す
        return getListDataByCtrlIdList([FormList.Condition.Id], FormList.No, 0);
    }

    // それ以外の場合
    var conditionDataList = [];
    return conditionDataList;
}

/**
 * 【オーバーライド用関数】実行ボタン前処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transDiv：画面遷移アクション区分
 *  @param {string} conductId：機能ID
 *  @param {string} pgmId ：プログラムID
 *  @param {number} formNo ：画面NO
 *  @param {html} btn  ：ボタン要素
 *  @param {number} conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param {boolean} autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param {boolean} isEdit ：単票表示フラグ
 *  @param {number} confirmNo ：確認番号
 *  @return {bool} 処理続行フラグ Trueなら続行、Falseなら処理終了
 */
function preRegistProcess(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo) {
    //ボタン名
    var btnName = $(btn).attr("name");
    if (formNo == FormList.No && btnName == FormList.ButtonId.AdjustDifference) {
        //棚差調整ボタン

        // 表示する確認メッセージ(デフォルト)を取得(P_MessageStr)に格納
        setMessageStrForBtn(btn, confirmKbnDef.Disp);
        var errorMsg = P_MessageStr;
        if (isChangeList(FormList.InventoryList.Id, true)) {
            //明細の行変更フラグがtrueの場合、棚卸数が変更されているとみなし表示メッセージ変更
            errorMsg = P_ComMsgTranslated[141020004];//一時保存後に棚卸数が変更されています。変更内容は破棄され、保存された値で棚差調整します。よろしいですか？
        }

        var key = "PT0003_isSkipConfirm_" + FormList.ButtonId.AdjustDifference;
        // 確認ウインドウの表示判定に用いる値
        var isSkipConfirm = P_dicIndividual[key];
        delete P_dicIndividual[key];
        // isSkipConfirmがこの値ならスキップする
        const isSkipConfirmValue = 1;
        if (isSkipConfirm != isSkipConfirmValue) {
            // 確認ウインドウでOKを押した場合の処理
            var eventFunc = function () {
                // 確認ウインドウの表示判定に用いる値で、次回は確認表示をスキップするよう設定
                P_dicIndividual[key] = isSkipConfirmValue;
                // この処理はキャンセルされるので、もう一度処理を呼び出す。今度は確認メッセージは出ない。
                clickRegistBtnConfirmOK(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo);
                // 処理終了
                return;
            }
            // 確認ﾒｯｾｰｼﾞを表示
            if (!popupMessage([errorMsg], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                // ボタンを活性化
                $(btn).prop("disabled", false);
                return false;
            }
        }
    }

    return true;
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
function setCodeTransOtherNames(appPath, formNo, ctrl, data) {
    // 新規登録画面
    if (formNo == FormRegist.No) {
        //処理対象のコントロールのID属性を取得
        var id = $(ctrl).attr("id");
        //予備品コントロールのID属性を取得
        var parts = getTextBoxId(FormRegist.List.Id, FormRegist.List.Parts);
        if (id == parts) {
            if (data != null && data.length > 0) {
                // 予備品(オートコンプリート)選択時、予備品名を翻訳に設定、メーカー、材質、型式、数量管理単位に値設定する
                setNameToCodeTrans(ctrl, data[0].EXPARAM1);
                setValue(FormRegist.List.Id, FormRegist.List.Manufacturer, 1, CtrlFlag.Label, data[0].EXPARAM2);
                setValue(FormRegist.List.Id, FormRegist.List.Materials, 1, CtrlFlag.Label, data[0].EXPARAM3);
                setValue(FormRegist.List.Id, FormRegist.List.ModelType, 1, CtrlFlag.Label, data[0].EXPARAM4);
                setValue(FormRegist.List.Id, FormRegist.List.Unit, 1, CtrlFlag.Combo, data[0].EXPARAM5);
            } else {
                //メーカー、材質、型式の値をクリアする
                setValue(FormRegist.List.Id, FormRegist.List.Manufacturer, 1, CtrlFlag.Label, "");
                setValue(FormRegist.List.Id, FormRegist.List.Materials, 1, CtrlFlag.Label, "");
                setValue(FormRegist.List.Id, FormRegist.List.ModelType, 1, CtrlFlag.Label, "");
            }
        }
        //部門コントロールのID属性を取得
        var departmentCd = getTextBoxId(FormRegist.List.Id, FormRegist.List.DepartmentCd);
        if (id == departmentCd && data != null && data.length > 0) {
            // 部門選択時、構成IDを非表示の項目に設定する
            setValue(FormRegist.List.Id, FormRegist.List.DepartmentId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
        }
        //勘定科目コントロールのID属性を取得
        var departmentCd = getTextBoxId(FormRegist.List.Id, FormRegist.List.AccountCd);
        if (id == departmentCd && data != null && data.length > 0) {
            // 勘定科目選択時、構成IDを非表示の項目に設定する
            setValue(FormRegist.List.Id, FormRegist.List.AccountId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
            // 勘定科目選択時、新旧区分を非表示の項目に設定する
            setValue(FormRegist.List.Id, FormRegist.List.AccountOldNewDivision, 1, CtrlFlag.Label, data[0].EXPARAM2, false, false);
        }
    }
}