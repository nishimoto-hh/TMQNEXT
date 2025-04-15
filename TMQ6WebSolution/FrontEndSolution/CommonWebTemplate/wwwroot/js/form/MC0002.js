/* ========================================================================
 *  機能名　    ：   【MC0002】機器別管理基準標準
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)MC0002\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");

// 機能ID
const ConductId_MC0002 = "MC0002";
// グローバルリスト 詳細画面へ遷移時の遷移元画面No
const MC0002_RegistFormNo = "MC0002_RegistFormNo";
// グローバル変数のキー、一覧画面の表示データを更新する用
const MC0002_UpdateListData = "MC0002_UpdateListData";
// グローバル変数のキー、一覧画面用の総件数
const MC0002_AllListCount = "MC0002_AllListCount";
// グローバル変数のキー、一覧画面から詳細画面に遷移した際、保全項目一覧のレコードを全選択状態にするためのもの
const MC0002_transFromFormList = "MC0002_transFromFormList";

// 一覧画面の定義
const FormList = {
    No: 0                        // 画面番号
    , List: {                    // 検索結果一覧の情報
        Id: "BODY_020_00_LST_0", // 一覧のコントロールID
        ManagementStandardsName: 1, //標準名称
        Memo: 2, //メモ
        District: 3, //地区
        Factory: 4, //工場
        Job: 5, //職種
        LargeClassfication: 6, //機種大分類
        MiddleClassfication: 7, //機種中分類
        SmallClassfication: 8, //機種小分類
        ManagementStandardsId: 9,// 機器別管理基準標準ID
    }
    , Filter: {                  // フィルターの情報
        Id: "BODY_010_00_LST_0", // 一覧のコントロールID
        Input: 1                 // 入力エリアの項目番号
    }
    , Button: {                  // ボタンコントロールID
        New: "New"               // 新規
    }
}

// 詳細画面の定義
const FormDetail = {
    No: 1                        // 画面番号
    , LocationInfo: { // 場所階層の情報
        Id: "BODY_010_00_LST_1", // 地区～工場 のコントロールID
        District: 1, //地区
        Factory: 2, //工場
    }
    , JobInfo: { // 職種機種の情報
        Id: "BODY_020_00_LST_1", // 職種～機種小分類 のコントロールID
        Job: 1, //職種
        LargeClassfication: 2, //機種大分類
        MiddleClassfication: 3, //機種中分類
        SmallClassfication: 4, //機種小分類
    }
    , ManagementStandardsInfo: { // 標準件名情報の情報
        Id: "BODY_030_00_LST_1", // 標準名称～メモ のコントロールID
        ManagementStandardsName: 1, //標準名称
        Memo: 2, //メモ
    }
    , ManagementStandardsDetailList: { // 保全項目一覧の情報
        Id: "BODY_040_00_LST_1"        // 保全項目一覧の気温トロールID
    }
    , Button: {                  // ボタンコントロールID
        Edit: "Edit"             // 修正
        , Copy: "Copy"           // 複写
        , Quota: "Quota"         // 割当
        , DeleteManagementStandardsDetail: "DeleteManagementStandardsDetail" // 保全項目一覧のレコードを削除するための非表示の削除ボタン
    }
}

// 詳細編集画面の定義
const FormEdit = {
    No: 2                        // 画面番号
    , ManagementStandardsInfo: { // 標準件名情報の情報
        Id: "BODY_020_00_LST_2"  // 標準名称～メモ のコントロールID
    }
    , Button: {                  // ボタンコントロールID
        Regist: "Regist"         // 登録
        , Back: "Back"           // 戻る
    }
}

// 保全項目編集画面の定義
const FormManagementStandarts = {
    No: 3 // 画面番号
    , ManagementStandardsList: {
        Id: "BODY_000_00_LST_3"
        , InspectionSiteImportance: 2 //部位重要度
        , InspectionSiteConservation: 3 //保全方式
        , MaintainanceDivision: 4 //保全区分
        , MaintainanceKind: 6 //点検種別
        , InspectionSiteImportanceSelectVal: 19 //部位重要度(選択値)
        , InspectionSiteConservationSelectVal: 20 //保全方式(選択値)
        , MaintainanceDivisionSelectVal: 21 //保全区分(選択値)
        , MaintainanceKindSelectVal: 22 //点検種別(選択値)
    }
    , Button: {                  // ボタンコントロールID
        Regist: "Regist"         // 登録
    }
}

// 標準割当画面の定義
const FormQuota = {
    No: 4                       // 画面番号
    , SearchCondition: {                               // 検索条件
        ToFacility: "COND_000_00_LST_4"                // 地区～設備
        , ToMachineName: "COND_010_00_LST_4"           // 職種～機器名称
        , ToIsManagementStandards: "COND_020_00_LST_4" // 循環対象～機器別管理基準
        , UseSegment: 11                               // 使用区分
    }
    , ManagementStandardsDetailIdList: { // 非表示の機器別管理基準標準詳細IDリスト
        Id: "COND_100_00_LST_4"          // 一覧のID
    }
    , MachineList: {            // 検索結果の機器一覧
        Id: "BODY_050_00_LST_4" // 機器一覧のコントロールID
        , StartDate: 14         // 開始日
        , CssClass: {                        // 機器一覧に表示している列のCSSクラス
            QuotaStartDate: "QuotaStartDate" // 開始日 列に設定しているクラス
        }
    }
    , StartDate: {              // 開始日(反映元)
        Id: "BODY_080_00_LST_4" // 一覧のコントロールID
        , BaseStartDate: 1      // 開始日
    }
    , ErrorInfo: {              // 検索結果のエラー出力
        Id: "BODY_060_00_LST_4" // エラーメッセージを表示する一覧のID
        , ErrorMessage: 1       // エラーメッセージを表示するテキストエリア
    }
    , Button: {                              // ボタンコントロールID
        ReflectStartDate: "ReflectStartDate" // 反映
    }
}

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

    // 画面番号を判定
    if (formNo == FormList.No) {

        // 標準一覧画面

        //フォーカスを設定
        setFocusButton(FormList.Button.New);

    } else if (formNo == FormDetail.No) {

        // 標準詳細画面

        //登録更新データを一覧画面に反映する（再検索を行わず、一覧データに反映）
        setUpdateDataForList(conductId, false);
        //グローバルリストの値を削除
        delete P_dicIndividual[MC0002_RegistFormNo];

        //フォーカスを設定
        setFocusButton(FormDetail.Button.Copy);

    } else if (formNo == FormEdit.No) {

        // 標準詳細編集画面

        //フォーカスを設定
        setFocusButton(FormEdit.Button.Regist);

    } else if (formNo == FormManagementStandarts.No) {

        // 保全項目編集画面

        //登録された標準未使用アイテム、工場アイテムがコンボボックスのアイテムに存在しない場合、アイテムを追加する
        //部位重要度
        setSelectItemForCombo(FormManagementStandarts.ManagementStandardsList.Id, FormManagementStandarts.ManagementStandardsList.InspectionSiteImportance, FormManagementStandarts.ManagementStandardsList.InspectionSiteImportanceSelectVal);
        //保全方式
        setSelectItemForCombo(FormManagementStandarts.ManagementStandardsList.Id, FormManagementStandarts.ManagementStandardsList.InspectionSiteConservation, FormManagementStandarts.ManagementStandardsList.InspectionSiteConservationSelectVal);
        //保全区分
        setSelectItemForCombo(FormManagementStandarts.ManagementStandardsList.Id, FormManagementStandarts.ManagementStandardsList.MaintainanceDivision, FormManagementStandarts.ManagementStandardsList.MaintainanceDivisionSelectVal);
        //点検種別
        setSelectItemForCombo(FormManagementStandarts.ManagementStandardsList.Id, FormManagementStandarts.ManagementStandardsList.MaintainanceKind, FormManagementStandarts.ManagementStandardsList.MaintainanceKindSelectVal);

        //フォーカスを設定
        setFocusButton(FormManagementStandarts.Button.Regist);

    } else if (formNo == FormQuota.No) {

        // 使用区分コンボボックスにアイテム追加
        addItemToUseSegComb();

        // 標準割当画面
        // 明細エリアを非表示
        changeDisplayDetailArea();

        //エラー出力を非活性にする
        var errorMsg = getCtrl(FormQuota.ErrorInfo.Id, FormQuota.ErrorInfo.ErrorMessage, 1, CtrlFlag.Textarea);
        changeInputControl(errorMsg, false);
    }
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
function setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {

    if (formNo == FormQuota.No &&
        ctrlId == FormQuota.SearchCondition.ToIsManagementStandards &&
        valNo == FormQuota.SearchCondition.UseSegment) {

        // 使用区分コンボボックスにアイテム追加
        addItemToUseSegComb();
    }
}

/**
 * 【オーバーライド用関数】Tabuator一覧の描画前処理
 * @param {any} id 一覧のID(#,_FormNo付き)
 * @param {any} options 一覧のオプション情報
 * @param {any} header ヘッダー情報
 * @param {any} dispData データ
 */
function prevCreateTabulator(appPath, id, options, header, dispData) {
    if (getFormNo() == FormQuota.No && id == '#' + FormQuota.MachineList.Id + getAddFormNo()) {
        //機器一覧

        $.each(header, function (idx, head) {
            if (head.field == "VAL" + FormQuota.MachineList.StartDate) {
                //開始日は入力可とする
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
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {

    // 一覧画面
    if (id == '#' + FormList.List.Id + getAddFormNo()) {
        // 一覧フィルタ処理実施
        callExecuteListFilter(FormList.List.Id, FormList.Filter.Id, FormList.Filter.Input);
    }
    else if (id == '#' + FormQuota.MachineList.Id + getAddFormNo()) {

        // 機器一覧に検索結果が存在する場合
        if (tbl.getRows().length > 0) {

            // 標準割当画面
            setInitHide($(P_Article).find("#" + FormQuota.StartDate.Id + getAddFormNo() + "_div"), false); // 開始日の反映元を表示
            setInitHide($(P_Article).find(getButtonCtrl(FormQuota.Button.ReflectStartDate)), false);       // 反映ボタンを表示

            // 標準割当画面
            // 開始日入力時のイベント付与
            setBlurEventForStartDate();
        }
    }
    else if (id == '#' + FormDetail.ManagementStandardsDetailList.Id + getAddFormNo()) {
        // 詳細画面の保全項目一覧

        // 保全項目一覧を全選択するためのキーがグローバルリストに格納されているか判定(一覧画面の詳細リンククリック時に格納される)
        if (P_dicIndividual[MC0002_transFromFormList]) {

            // 保全項目一覧の全選択アイコンをクリックする
            $(P_Article).find('#' + FormDetail.ManagementStandardsDetailList.Id + getAddFormNo() + '_div [data-actionkbn="' + actionkbn.SelectAll + '"]').click();

            // 画面編集フラグを変更(変更しないと「割当」ボタンクリック時に「画面の編集内容は...」の確認メッセージが表示されてしまう)
            dataEditedFlg = false;

            // グローバルリストからキーを削除
            operatePdicIndividual(MC0002_transFromFormList, true);
        }
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
function prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var conditionDataList = [];

    // 一覧フィルタ処理実施
    if (executeListFilter(transTarget, FormList.List.Id, FormList.Filter.Id, FormList.Filter.Input)) {
        return [false, conditionDataList];
    }

    // 画面番号を判定
    if (formNo == FormList.No && ctrlId == FormList.List.Id && transTarget == FormDetail.No) {
        // 一覧画面
        // 詳細リンクをクリックして詳細画面に遷移する場合、保全項目一覧を全選択された状態で表示するためのキーをグローバルリストに格納
        // ※グローバルリストにキーが格納されているかだけ判定するため値は「0」とする
        operatePdicIndividual(MC0002_transFromFormList, false, '0');
    }
    else if (formNo == FormDetail.No) {
        // 詳細画面
        // 保全項目一覧の行追加ボタンがクリックされたかどうか
        var isManagementStandardsDetailNew = transTarget == FormManagementStandarts.No && ctrlId == FormDetail.ManagementStandardsDetailList.Id && rowNo == -1;

        // ボタンコントロールIDを判定
        if (btn_ctrlId == FormDetail.Button.Edit ||
            btn_ctrlId == FormDetail.Button.Copy ||
            isManagementStandardsDetailNew) {
            // 複写・修正・保全項目一覧の行追加ボタン

            // 遷移先画面の検索条件をセット
            const ctrlIdList = [FormDetail.ManagementStandardsInfo.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);

        }
        else if (btn_ctrlId == FormDetail.Button.Quota) {
            // 割当ボタン
            // 保全項目一覧が選択されているか判定
            var result = isCheckedList(FormDetail.ManagementStandardsDetailList.Id);
            if (!result) {
                // 未選択ならエラーなので終了
                return [false, conditionDataList];
            }

            // 遷移先画面の検索条件、保全項目一覧のレコードを条件に設定
            const ctrlIdList = [FormDetail.ManagementStandardsInfo.Id, FormDetail.ManagementStandardsDetailList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0, false, true);
        }
    }

    return [true, conditionDataList];
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

    // 画面番号を取得
    var formNo = getFormNo();

    // 画面番号を判定
    if (formNo == FormEdit.No) {
        if (btnCtrlId == FormEdit.Button.Regist) {
            // 詳細編集画面から登録(新規・複写)後、詳細画面に渡すキー情報をセット
            var conditionDataList = getListDataByCtrlIdList([FormEdit.ManagementStandardsInfo.Id], FormEdit.No, 0);

            // 一覧から参照へ遷移する場合と同様に、参照画面の検索条件を追加
            setSearchCondition(ConductId_MC0002, FormDetail.No, conditionDataList);
            P_dicIndividual[MC0002_RegistFormNo] = FormEdit.No;
        } else if (btnCtrlId == FormEdit.Button.Back) {
            //登録画面から戻るボタンで戻る際、再検索は行わない
            return false;
        }
    } else if (formNo == FormEdit.No || formNo == FormManagementStandarts.No || formNo == FormQuota.No) {

        // 保全項目編集画面で登録後または他画面から詳細画面へ戻る場合、詳細画面に渡すキー情報をセット
        var conditionDataList = getListDataByCtrlIdList([FormDetail.ManagementStandardsInfo.Id], FormDetail.No, 0);

        // 一覧から参照へ遷移する場合と同様に、参照画面の検索条件を追加
        setSearchCondition(ConductId_MC0002, FormDetail.No, conditionDataList);
    } else if (formNo == FormDetail.No) {
        //一覧画面へ戻る際、再検索は行わない
        return false;
    }
    return true;
}

/**
 *【オーバーライド用関数】
 *  行削除の前
 *  @param appPath    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param btn        ：行削除ﾘﾝｸのa要素
 *  @param id         ：行削除ﾘﾝｸの対象一覧のCTRLID
 *  @param checkes    ：削除対象の要素リスト
 */
function preDeleteRow(appPath, btn, id, checkes) {

    // 詳細画面の保全項目一覧の行削除ボタンクリック時のチェック
    return preDeleteRowCommonDesignateBtn(id, [FormDetail.ManagementStandardsDetailList.Id], FormDetail.Button.DeleteManagementStandardsDetail);
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

    // 画面番号を判定
    if (formNo == FormQuota.No) {

        // 標準割当画面
        //クリックされたボタンのコントロールIDを判定
        if (btnCtrlId == FormQuota.Button.ReflectStartDate) {

            // 反映 ボタン
            // 反映元の開始日の値を取得
            var baseStartDate = getValue(FormQuota.StartDate.Id, FormQuota.StartDate.BaseStartDate, 2, CtrlFlag.TextBox, false, false);

            // 反映元の開始日に何も入力されていない場合はここで終了
            if (!baseStartDate) {
                return;
            }

            // 検索結果の機器一覧の要素を取得
            var machinList = P_listData["#" + FormQuota.MachineList.Id + "_" + formNo].getData();

            // 取得した要素の開始日に対して繰り返し処理
            $.each(machinList, function (i, machine) {

                // 開始日が空か判定
                if (machine["VAL" + FormQuota.MachineList.StartDate] == "") {

                    // 空の場合は反映元の開始日の値を設定する
                    machine["VAL" + FormQuota.MachineList.StartDate] = baseStartDate;
                }
            });

            // 開始日を設定した機器一覧のデータを画面に設定して再描画する
            P_listData["#" + FormQuota.MachineList.Id + "_" + formNo].updateData(machinList);
        }
    }
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevCommonValidCheck(appPath, conductId, formNo, btn) {

    var isContinue = true; // 個別処理後も共通の入力チェックを行う場合はtrue
    var isError = false;   // エラーがある場合はtrue


    // 画面番号を判定
    if (formNo == FormQuota.No) {
        // エラー出力を空にする
        setValue(FormQuota.ErrorInfo.Id, FormQuota.ErrorInfo.ErrorMessage, 1, CtrlFlag.Textarea, "\n", false, false);


        // 標準割当画面
        // 機器一覧のデータ取得
        var table = P_listData['#' + FormQuota.MachineList.Id + '_' + formNo];

        // 選択チェックボックスがチェックされている行を取得
        var checkedList = $.grep(table.getData(),
            function (obj, idx) {
                return obj.SELTAG == 1;
            });

        // 機器一覧にて行が選択されているかどうか判定
        if (checkedList == null || checkedList.length == 0) {
            // 行が1件も選択されていない場合はエラーメッセージをセットして終了
            // 「対象行が選択されていません。」
            setMessage([P_ComMsgTranslated[941160003]], messageType.Warning);

            setValue(FormQuota.ErrorInfo.Id, FormQuota.ErrorInfo.ErrorMessage, 1, CtrlFlag.Textarea, [P_ComMsgTranslated[941160003]], false, false);

            return [true, true];
        }

        // 機器一覧にて行は選択されているが、開始日が未入力または開始日が日付に変換できない場合エラー出力をする
        var errMsg = "";
        $.each(checkedList, function (i, checked) {


            // 開始日の値を取得
            var strStartDate = checked["VAL" + FormQuota.MachineList.StartDate];

            // 開始日が未入力の場合
            // 開始日が日付に変換できない場合
            // 開始日がyyyy/MM/ddに変換できない場合
            if (strStartDate == "" ||
                isNaN(new Date(strStartDate).getDate()) ||
                strStartDate.match(/\d{4}\/\d{2}\/\d{2}/) == null) {

                // ○行目、日付で入力してください。
                errMsg = errMsg + checked.ROWNO + P_ComMsgTranslated[141070007] + "\n";
                return;
            }
        });

        // エラーメッセージが設定されているかどうか
        if (errMsg) {
            // 個別ｴﾗｰは最初は非表示
            var errorHtml = $("div.errtooltip").find("label.errorcom");
            $(errorHtml).hide();
            //入力エラーがあります。
            addMessage(P_ComMsgTranslated[941220007], 1);

            // エラーがある場合はメッセージを画面下部の エラー出力 に表示する
            setValue(FormQuota.ErrorInfo.Id, FormQuota.ErrorInfo.ErrorMessage, 1, CtrlFlag.Textarea, errMsg, false, false);
            return [isContinue, true];
        }
    }

    return [isContinue, isError];
}


/**
 *【オーバーライド用関数】登録処理前の「listData」個別取得処理
 * @param {any} appPath   : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conductId : 機能ID
 * @param {any} pgmId     : プログラムID
 * @param {any} formNo    : 画面番号
 * @param {any} btn       : クリックされたボタン要素
 * @param {any} listData  : バックエンド側に渡すデータ(何もしない場合はそのまま返す)
 */
function getListDataForRegist(appPath, conductId, pgmId, formNo, btn, listData) {

    // 画面番号を判定
    if (formNo == FormQuota.No) {

        // 標準割当画面
        // 検索条件と機器一覧の選択されているレコードを取得してバックエンド側に渡すためのデータを作成
        return conditionDataList = getListDataByCtrlIdList([FormQuota.MachineList.Id, FormQuota.ManagementStandardsDetailIdList.Id], formNo, 1, true, true);
    }

    // 何もしていないのでそのまま返す
    return listData;
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

    //削除データを一覧画面から削除（登録・更新は詳細画面表示後のタイミングで反映）
    setUpdateDataForList(conductId, true);
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
 *  @param {number} transPtn ：画面遷移のパターン、transPtnDef
 */
function beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn) {

    if (conductId == ConductId_MC0002 && formNo == FormList.No) {
        //ページング再設定
        setListPagination(appPath, conductId, pgmId, status, FormList.List.Id, FormList.No, MC0002_AllListCount);
    }
}


/**
 *【オーバーライド用関数】子画面新規遷移前ﾁｪｯｸ処置
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevTransChildForm(appPath, conductId, formNo, btn) {

    // 詳細画面の「割当」ボタンがクリックされた場合
    if (formNo == formNo == FormDetail.No && $(btn)[0].name == FormDetail.Button.Quota) {
        // 画面編集フラグを変更(「画面の編集内容は...」の確認メッセージを表示しないようにする)
        dataEditedFlg = false;
    }

    return true;
}


/*
 * 標準割当画面 初期表示時に明細ｴﾘｱを非表示にする
 */
function changeDisplayDetailArea() {
    // 明細ｴﾘｱの非表示
    var detailArea = $(P_Article).find("#" + P_formDetailId);
    var detailLists = $(detailArea).find(".detail_div").children().not("[id$='edit_div']"); //明細ｴﾘｱ一覧
    var detailCtrlGroup = $(detailArea).find(".action_detail_div"); //明細ｴﾘｱｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ
    // 明細ｴﾘｱを表示
    setInitHide(detailLists, true);        //一覧表示
    setInitHide(detailCtrlGroup, true);    //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示
    setInitHide($(P_Article).find("#" + FormQuota.StartDate.Id + getAddFormNo() + "_div"), true);  // 開始日の反映元
    setInitHide($(P_Article).find(getButtonCtrl(FormQuota.Button.ReflectStartDate)), true);        // 反映ボタン
}

/*
 * 標準割当画面の機器一覧で開始日が入力された際のイベント
 */
function setBlurEventForStartDate() {
    // 機器一覧の開始日入力後イベント
    $("input.QuotaStartDate").blur(function () {
        // テキストボックスの内容が変更されたときの処理を記述
        // 入力された日付を取得
        var inputVal = $(this).val();

        //ROWNOを取得
        var rowNo = $(this).closest(".tabulator-row").find('.tabulator-cell[tabulator-field="SELTAG"] input').data("rowno");

        // 該当行のデータを取得
        var targetData = P_listData["#" + FormQuota.MachineList.Id + getAddFormNo()].searchData("ROWNO", "=", rowNo);
        if (!targetData || targetData.length == 0) {
            return;
        }

        // 入力された日付を設定
        targetData[0]["VAL" + FormQuota.MachineList.StartDate] = inputVal;
        P_listData["#" + FormQuota.MachineList.Id + getAddFormNo()].updateData(targetData);
    });
}

/**
 * コンボボックスのアイテムに対象のアイテムが存在しない場合、追加
 * @param {any} ctrlId コントロールID
 * @param {any} comboVal コンボボックスのVAL値
 * @param {any} labelVal 設定値（非表示）のVAL値
 */
function setSelectItemForCombo(ctrlId, comboVal, labelVal) {
    //コンボボックス要素
    var combo = getCtrl(ctrlId, comboVal, 1, CtrlFlag.Combo);
    //設定値
    var selectVal = getValue(ctrlId, labelVal, 1, CtrlFlag.Label);
    if (!selectVal) {
        //設定値がない場合は処理なし
        return;
    }

    // 構成ID|翻訳
    var list = selectVal.split('|');
    var structureId = list[0];
    var text = list[1];
    //コンボボックスのアイテムに登録値が存在するか
    var isExist = $(combo).find("option[value='" + structureId + "']").length > 0;
    if (!isExist) {
        //存在しない場合、アイテムを追加して選択
        var ele = $(combo).find("option.ctrloptionblank");
        if (ele.length > 0) {
            //ブランク行の後に追加
            $(ele).after($("<option>").val(structureId).text(text));
        } else {
            //ブランク行が無い場合、先頭に追加
            $(combo).prepend($("<option>").val(structureId).text(text));
        }
        //追加したアイテムを選択
        $(combo).val(structureId);
    }
}

/**
 * 更新データを一覧画面に反映する
 *  @param conductId   ：機能ID
 *  @param isDelete    ：削除の場合true
 */
function setUpdateDataForList(conductId, isDelete) {
    if (!P_dicIndividual[MC0002_UpdateListData] || conductId != ConductId_MC0002) {
        //更新データが存在しない場合
        return;
    }

    //反映するデータ
    var updateData = P_dicIndividual[MC0002_UpdateListData];
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
        //initFormOriginalから呼ばれた場合は登録・更新処理を行う
        return;
    }

    //一覧画面のデータ
    var table = P_listData["#" + FormList.List.Id + "_" + FormList.No];

    switch (status) {
        case rowStatusDef.New: //新規
            //一覧画面のデータのROWNO最大値を取得
            var maxRowNo = 0;
            var list = table.getData();
            if (list && list.length > 0) {
                var rowNoList = list.map(x => x.ROWNO);
                //maxRowNo = Math.max.apply(null, rowNoList);
                maxRowNo = rowNoList.reduce((a, b) => Math.max(a, b));
            }
            //ROWNOに一覧データの最大値以降の値を設定
            data.ROWNO = maxRowNo + 1;
            //詳細画面から値取得
            setLabelValueToListData(data, status);
            //先頭行に追加（ソートが指定されている場合はソートに従った行に表示される）
            table.addRow(data, true, 1);
            break;

        case rowStatusDef.Edit: //更新
            //データの機器別管理基準標準IDを取得
            var managementStandardsId = data["VAL" + FormList.List.ManagementStandardsId];
            //更新前のROWNOを取得（ROWNOがキー）
            var oldData = table.searchData("VAL" + FormList.List.ManagementStandardsId, "=", managementStandardsId);
            if (oldData && oldData.length > 0) {
                data.ROWNO = oldData[0].ROWNO;
                //詳細画面から値取得
                setLabelValueToListData(data, status);
                table.updateRow(data.ROWNO, data);
            }
            break;

        case rowStatusDef.Delete: //削除
            //データの機器別管理基準標準IDを取得
            var managementStandardsId = data["VAL" + FormList.List.ManagementStandardsId];
            //更新前のROWNOを取得（ROWNOがキー）
            var oldData = table.searchData("VAL" + FormList.List.ManagementStandardsId, "=", managementStandardsId);
            if (oldData && oldData.length > 0) {
                table.deleteRow(oldData[0].ROWNO);
            }
            break;

        default:
            break;
    }

    delete P_dicIndividual[MC0002_UpdateListData];
}

/**
 * 詳細画面の値から一覧用データに値を反映する
 * @param data 一覧用データ
 * @param status ステータス(新規or更新)
 */
function setLabelValueToListData(data, status) {

    //一覧に表示している列に詳細画面の対応する項目値を設定
    $.each(Object.keys(data), function (index, key) {
        if (!key.startsWith("VAL")) {
            return true; // continue
        }
        switch (key) {
            case "VAL" + FormList.List.ManagementStandardsName: //標準名称
                data[key] = getValue(FormDetail.ManagementStandardsInfo.Id, FormDetail.ManagementStandardsInfo.ManagementStandardsName, 1, CtrlFlag.Label);
                break;
            case "VAL" + FormList.List.Memo: //メモ
                data[key] = getItemLabelValue(FormDetail.ManagementStandardsInfo.Id, FormDetail.ManagementStandardsInfo.Memo, 1, CtrlFlag.Textarea);
                break;
            case "VAL" + FormList.List.District: //地区
                data[key] = getValue(FormDetail.LocationInfo.Id, FormDetail.LocationInfo.District, 1, CtrlFlag.Label);
                break;
            case "VAL" + FormList.List.Factory: //工場
                data[key] = getValue(FormDetail.LocationInfo.Id, FormDetail.LocationInfo.Factory, 1, CtrlFlag.Label);
                break;
            case "VAL" + FormList.List.Job: //職種
                data[key] = getValue(FormDetail.JobInfo.Id, FormDetail.JobInfo.Job, 1, CtrlFlag.Label);
                break;
            case "VAL" + FormList.List.LargeClassfication: //機種大分類
                data[key] = getValue(FormDetail.JobInfo.Id, FormDetail.JobInfo.LargeClassfication, 1, CtrlFlag.Label);
                break;
            case "VAL" + FormList.List.MiddleClassfication: //機種中分類
                data[key] = getValue(FormDetail.JobInfo.Id, FormDetail.JobInfo.MiddleClassfication, 1, CtrlFlag.Label);
                break;
            case "VAL" + FormList.List.SmallClassfication: //機種小分類
                data[key] = getValue(FormDetail.JobInfo.Id, FormDetail.JobInfo.SmallClassfication, 1, CtrlFlag.Label);
                break;
            default:
                break;
        }
    });
}

/* 
 * 標準割当画面の検索条件にある 使用区分 コンボボックスにアイテムを追加する 
 */
function addItemToUseSegComb() {

    // 使用区分 コンボボックスの要素を取得
    var combo = getCtrl(FormQuota.SearchCondition.ToIsManagementStandards, FormQuota.SearchCondition.UseSegment, 1, CtrlFlag.Combo);

    // コンボボックスのアイテムの「すべて」を取得(画面定義で設定しているので取得できるはず)
    var ele = $(combo).find("option.ctrloption");
    if (ele.length > 0) {

        // 既に追加されている場合は何もしない
        if ($(combo).find("option[value='0']").length == 0) {
            // 「すべて」の次に「使用中」を追加する(値は「0」)
            $(ele).after($("<option>").val(0).text(P_ComMsgTranslated[111120274]));
        }
    }
}