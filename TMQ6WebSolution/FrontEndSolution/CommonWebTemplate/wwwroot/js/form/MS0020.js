/* ========================================================================
 *  機能名　    ：   【MS0020】機種別仕様マスタ
 * ======================================================================== */

// 他のマスタ画面の処理は参考にしているが、仕様の差異によりマスタ共通処理を完全には使用できないため、個別の画面として処理を実装

// 機能ID
const ConductId_MS0020 = "MS0020";

// この画面固有の定数
const Const_MS0020 = {
    // 画面の定義情報
    FormInfo: {
        // 機種別仕様一覧画面
        SpecList: {
            No: 1,
            // 検索条件
            Condition: {
                Id: 'BODY_000_00_LST_0',
                Val: {
                    // 工場
                    FactoryId: 1
                }
            }
            // 非表示項目
            , Keys: {
                Id: 'BODY_050_00_LST_0',
                Val: {
                    // 工場ID退避用
                    FactoryId: 1,
                    // 本務工場
                    MainFactoryId: 2,
                    // システム管理者フラグ
                    SystemAdminFlag: 3
                }
            }
            // 一覧
            , List: {
                Id: "BODY_020_00_LST_0",
                Val: {
                    // 機種別仕様関連付ID
                    RelationId: 51
                }
            }
        }
        // 機種別仕様登録画面
        , SpecRegist: {
            No: 2,
            Header: {
                Id: 'BODY_000_00_LST_1'
            }
            , Keys: {
                Id: 'BODY_020_00_LST_1',
                Val: {
                    // 入力形式
                    InputType: 1,
                    // 数値書式
                    NumberFormat: 2,
                    // 単位種別
                    SpecUnitType: 3,
                    // 単位
                    SpecUnit: 4,
                    // 工場ID退避用
                    FactoryId: 51,
                    // 機種別仕様関連付ID
                    RelationId: 52,
                    // 仕様項目ID
                    SpecId: 53,
                    // 入力形式の拡張項目の値退避用
                    InputTypeExtension: 55
                },
                // 入力形式切り替え時の処理定義
                DisplaySwitch: {
                    // 取得した拡張項目の値(テキストと選択肢の場合、非表示にする)
                    ExParams: ['1', '4'],
                    // 非表示にする項目のVAL値
                    HideVals: [2, 3, 4]
                }
            }
            , Button: {
                Id: "BODY_030_00_BTN_1",
                Name: {
                    // 登録
                    Regist: "Regist",
                    // 仕様項目選択肢
                    SpecImtes: "MoveSpecItems"
                },
                DisplaySwitch: {
                    // 取得した拡張項目の値(選択肢以外の場合、非表示にする)
                    ExParams: ['1', '2', '3'],
                    // 非表示にする項目のVAL値
                    HideVals: [2]
                }
            }
        }
        // 仕様項目選択値一覧画面
        , SpecItemList: {
            No: 3,
            Keys: {
                Id: "BODY_000_00_LST_2",
                Val: {
                    // 仕様項目ID
                    SpecId: 51,
                    // 工場ID
                    FactoryId: 52,
                    // 機種別仕様関連付ID
                    RelationId: 53
                }
            }
        }
        // 選択肢登録画面
        , SpecItemRegist: {
            No: 4,
            Keys: {
                Id: "BODY_010_00_LST_3",
                Val: {
                    // 構成ID
                    StructureId: 51,
                    // 工場ID
                    FactoryId: 52,
                    // 仕様項目ID
                    SpecId: 53,
                    // 機種別仕様関連付ID
                    RelationId: 54
                }
            }
            // ボタン
            , Button: {
                Name: { Regist: "Regist" }
            }
        }

    }
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
 *  @data {List<Dictionary<string, object>>}    ：初期表示ﾃﾞｰﾀ
 */
function initFormOriginalForMS0020(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    if (formNo == Const_MS0020.FormInfo.SpecRegist.No) {
        // 機種別仕様登録画面


        setTimeout(function () {
            // 単位種別コンボに初期値がセットされているので、単位に連動させるために変更時イベントを発生させる
            var specUnit = getValue(Const_MS0020.FormInfo.SpecRegist.Keys.Id, Const_MS0020.FormInfo.SpecRegist.Keys.Val.SpecUnit, 0, CtrlFlag.Combo);
            var specUnitType = getCtrl(Const_MS0020.FormInfo.SpecRegist.Keys.Id, Const_MS0020.FormInfo.SpecRegist.Keys.Val.SpecUnitType, 0, CtrlFlag.Combo);
            changeNoEdit(specUnitType);
            setValue(Const_MS0020.FormInfo.SpecRegist.Keys.Id, Const_MS0020.FormInfo.SpecRegist.Keys.Val.SpecUnit, 0, CtrlFlag.Combo, specUnit);

            // 入力形式の選択値に応じて入力項目の表示状態を切り替えるため、イベントを発生させる
            var specUnit = getCtrl(Const_MS0020.FormInfo.SpecRegist.Keys.Id, Const_MS0020.FormInfo.SpecRegist.Keys.Val.InputType, 0, CtrlFlag.Combo);
            changeNoEdit(specUnit);

            // 初期表示時のみ仕様項目選択肢ボタンは活性、一度でも入力形式を切り替えたら非活性になる
            setDisableSpecItemsBtn(false);
        }, 300); //300ミリ秒

    } else if (formNo == Const_MS0020.FormInfo.SpecList.No) {
        // システム管理者フラグ取得
        var systemAdminFlg = getValue(Const_MS0020.FormInfo.SpecList.Keys.Id, Const_MS0020.FormInfo.SpecList.Keys.Val.SystemAdminFlag, 1, CtrlFlag.ChkBox);
        if (!systemAdminFlg) {
            // 工場管理者の場合

            // 検索条件の工場の要素取得
            var factory = getCtrl(Const_MS0020.FormInfo.SpecList.Condition.Id, Const_MS0020.FormInfo.SpecList.Condition.Val.FactoryId, 1, CtrlFlag.Combo);
            // 検索条件の工場を必須設定
            $(factory).rules("add", "required");
        }

        // フォーカス設定
        setFocusDelay(Const_MS0020.FormInfo.SpecList.Condition.Id, Const_MS0020.FormInfo.SpecList.Condition.Val.FactoryId, 0, CtrlFlag.Combo);

        // 初期検索時のみ初期値セット
        if (actionCtrlId == "") {
            // 本務工場を初期値にセット
            var factoryId = getValue(Const_MS0020.FormInfo.SpecList.Keys.Id, Const_MS0020.FormInfo.SpecList.Keys.Val.MainFactoryId, 1, CtrlFlag.Label);
            setValue(Const_MS0020.FormInfo.SpecList.Condition.Id, Const_MS0020.FormInfo.SpecList.Condition.Val.FactoryId, 1, CtrlFlag.Combo, factoryId);
        }
    }
}

/**
 * 検索条件に指定した値を追加
 * @param {any} conditionDataList 検索条件リスト
 * @param {any} ctrlId 指定する項目のID
 * @param {any} valNos 指定する項目のVAL値のリスト
 * @param {any} values 指定する項目の値のリスト、VAL値のリストと順番を揃える
 */
var setConditionDataList = function (conditionDataList, ctrlId, valNos, values) {
    // 新規の場合はconditionDataListがNullのため初期化
    if (conditionDataList == null) {
        conditionDataList = [];
    }
    // 検索条件に追加(仕様の情報の非表示項目にセット)
    var conditionData = {};
    conditionData['CTRLID'] = ctrlId;
    $.each(valNos, function (idx, data) {
        var valNo = valNos[idx];
        var value = values[idx];
        conditionData['VAL' + valNo] = value;
    });
    conditionDataList.push(conditionData);
    return conditionDataList;
};

class ParamKeyUtil {

    constructor(pFormInfo) {
        this.formInfo = pFormInfo;
    }
    _getKeyValue(valName) {
        var value = getValueByOtherForm(this.formInfo.No, this.formInfo.Keys.Id, this.formInfo.Keys.Val[valName], 0, CtrlFlag.Label);
        return value;
    }
    getFactoryId() {
        return this._getKeyValue("FactoryId");
    }
    getFactoryIdByCombo() {
        var value = getValueByOtherForm(this.formInfo.No, this.formInfo.Keys.Id, this.formInfo.Keys.Val["FactoryId"], 0, CtrlFlag.Combo);
        return value;
    }

    getSpecId() {
        return this._getKeyValue("SpecId");
    }
    getSpecRelationId() {
        return this._getKeyValue("RelationId");
    }
}

/**
 *【オーバーライド用関数】
 *  戻る処理の前(単票、子画面共用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @codeTransFlg   {int}       ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 *  @return {bool} データ取得する場合はtrue、スキップする場合はfalse（子画面のみ）
 */
function prevBackBtnProcessForMS0020(appPath, btnCtrlId, status, codeTransFlg) {
    var formNo = getFormNo();
    var conditionDataList = [];
    if (formNo == Const_MS0020.FormInfo.SpecItemList.No) {
        // 選択肢一覧画面から機種別仕様登録画面に戻る場合
        // 一覧画面から起動するときと同じキー情報をセットする

        // キー情報取得用クラスを宣言、選択肢一覧画面より値を取得
        var getKey = new ParamKeyUtil(Const_MS0020.FormInfo.SpecItemList);
        // 工場IDを取得し、機種別仕様登録画面のキー情報の項目として検索条件にセット
        var factoryId = getKey.getFactoryId();
        var valNos = [Const_MS0020.FormInfo.SpecRegist.Keys.Val.FactoryId];
        var values = [factoryId];
        setConditionDataList(conditionDataList, Const_MS0020.FormInfo.SpecRegist.Keys.Id, valNos, values);
        // 機種別仕様関連付IDを取得し、一覧画面の選択行の項目として検索条件にセット
        var relationId = getKey.getSpecRelationId();
        valNos = [Const_MS0020.FormInfo.SpecList.List.Val.RelationId];
        values = [relationId];
        setConditionDataList(conditionDataList, Const_MS0020.FormInfo.SpecList.List.Id, valNos, values);

        setSearchCondition(ConductId_MS0020, Const_MS0020.FormInfo.SpecRegist.No, conditionDataList);
    } else if (formNo == Const_MS0020.FormInfo.SpecItemRegist.No) {
        // 選択肢登録画面から選択肢一覧画面に戻る場合
        // 機種別仕様登録画面から起動するときと同じキー情報をセットする

        // キー情報取得用クラスを宣言、選択肢登録画面より値を取得

        var getKey = new ParamKeyUtil(Const_MS0020.FormInfo.SpecItemRegist);
        // 工場IDと機種別仕様関連付IDを取得し、選択肢一覧画面のキー情報の項目として検索条件にセット
        var factoryId = getKey.getFactoryId();
        var relationId = getKey.getSpecRelationId();
        var specId = getKey.getSpecId();
        var valInfo = Const_MS0020.FormInfo.SpecItemList.Keys.Val;
        var valNos = [valInfo.FactoryId, valInfo.SpecId, valInfo.RelationId];
        var values = [factoryId, specId, relationId];
        setConditionDataList(conditionDataList, Const_MS0020.FormInfo.SpecItemList.Keys.Id, valNos, values);
        setSearchCondition(ConductId_MS0020, Const_MS0020.FormInfo.SpecItemList.No, conditionDataList);
    }

    return true;
}

/**
 * 【オーバーライド用関数】画面初期値データ取得前処理(表示中画面用)
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} formNo 画面番号
 * @param {any} btnCtrlId ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
 * @param {any} conditionDataList 条件一覧要素
 * @param {any} listDefines 一覧の定義情報
 * @param {any} pgmId 遷移先のプログラムID
 */
function prevInitFormDataForMS0020(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {
    if (pgmId != ConductId_MS0020) {
        // MS0020以外なら終了
        return [conditionDataList, listDefines];
    }
    if (btnCtrlId == 'Back') {
        // 戻る場合は、prevBackBtnProcessで値をセットするため不要
        return [conditionDataList, listDefines];
    }

    if (formNo == Const_MS0020.FormInfo.SpecRegist.No) {
        // 機種別仕様登録画面
        // 機種別仕様一覧画面で指定された工場IDを引き継ぐ
        var getKey = new ParamKeyUtil(Const_MS0020.FormInfo.SpecList);
        // 工場ID取得
        var factoryId = getKey.getFactoryId();
        // 検索条件に設定
        var valNos = [Const_MS0020.FormInfo.SpecRegist.Keys.Val.FactoryId];
        var values = [factoryId];
        conditionDataList = setConditionDataList(conditionDataList, Const_MS0020.FormInfo.SpecRegist.Keys.Id, valNos, values);
    } else if (formNo == Const_MS0020.FormInfo.SpecItemList.No) {
        // 仕様項目選択肢一覧画面
        // 機種別仕様登録画面で指定された仕様項目IDを引き継ぐ
        var getKey = new ParamKeyUtil(Const_MS0020.FormInfo.SpecRegist);
        // キー値を取得
        var factoryId = getKey.getFactoryIdByCombo();
        var specId = getKey.getSpecId();
        var relationId = getKey.getSpecRelationId();
        // 検索条件に設定
        var valInfo = Const_MS0020.FormInfo.SpecItemList.Keys.Val;
        var valNos = [valInfo.FactoryId, valInfo.SpecId, valInfo.RelationId];
        var values = [factoryId, specId, relationId];
        conditionDataList = setConditionDataList(conditionDataList, Const_MS0020.FormInfo.SpecItemList.Keys.Id, valNos, values);
    } else if (formNo == Const_MS0020.FormInfo.SpecItemRegist.No) {
        // 選択肢登録画面
        // 仕様項目選択肢一覧画面で指定された工場IDを引き継ぐ
        var getKey = new ParamKeyUtil(Const_MS0020.FormInfo.SpecItemList);
        // キー値を取得
        var factoryId = getKey.getFactoryId();
        var specId = getKey.getSpecId();
        var relationId = getKey.getSpecRelationId();
        // 検索条件に設定
        var valInfo = Const_MS0020.FormInfo.SpecItemRegist.Keys.Val;
        var valNos = [valInfo.FactoryId, valInfo.SpecId, valInfo.RelationId];
        var values = [factoryId, specId, relationId];
        conditionDataList = setConditionDataList(conditionDataList, Const_MS0020.FormInfo.SpecItemRegist.Keys.Id, valNos, values);
    }
    return [conditionDataList, listDefines];
}

/**
 *【オーバーライド用関数】
 *  戻る処理の前(子画面用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @status         {any}       ：遷移ステータス
 *  @parentModalNo  {int}       ：親画面のモーダル番号、1以上ならモーダル
 *  @return {int} 親画面がモーダルなら1、そうでなければ0
 */
function prevBackBtnProcessForChildForMS0020(appPath, btnCtrlId, status, parentModalNo) {
    var formNo = getFormNo();
    if (formNo == Const_MS0020.FormInfo.SpecItemRegist.No && btnCtrlId == Const_MS0020.FormInfo.SpecItemRegist.Button.Name.Regist) {
        // 選択肢登録画面から選択肢一覧画面に戻る場合
        // 遷移先は2つめのモーダル画面なので、2
        parentModalNo = 2;
    }
    return parentModalNo;
}

/**
 *【オーバーライド用関数】
 *  検索処理前
 *
 *  @appPath {string} 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn {string} 　　　：対象ボタン
 *  @conductId {string} ：機能ID
 *  @pgmId {string} 　　：プログラムID
 *  @formNo {number} 　 ：画面番号
 *  @conductPtn {number}：処理ﾊﾟﾀｰﾝ
 */
function beforeSearchBtnProcessForMS0020(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    // 検索処理前（マスタメンテナンス用）beforeSearchBtnProcessForMaster を参考

    if (formNo == Const_MS0020.FormInfo.SpecList.No) {
        // 機種別仕様一覧画面

        // 検索条件の工場IDを、非表示項目に退避する　※条件エリアの値は失われるので、明細エリアへ退避

        // 検索条件の工場ID取得
        var factoryId = getValue(Const_MS0020.FormInfo.SpecList.Condition.Id, Const_MS0020.FormInfo.SpecList.Condition.Val.FactoryId, 1, CtrlFlag.Combo);
        if (!factoryId) {
            // 未選択の場合は検索エラーとなるため終了
            return;
        }

        // 非表示情報の設定
        setValue(Const_MS0020.FormInfo.SpecList.Keys.Id, Const_MS0020.FormInfo.SpecList.Keys.Val.FactoryId, 1, CtrlFlag.Label, factoryId);
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
function setComboOtherValuesForMS0020(appPath, combo, datas, selected, formNo, ctrlId, valNo) {
    if (formNo == Const_MS0020.FormInfo.SpecRegist.No && ctrlId == Const_MS0020.FormInfo.SpecRegist.Keys.Id && valNo == Const_MS0020.FormInfo.SpecRegist.Keys.Val.InputType) {
        // 機種別仕様登録画面の入力形式コンボが変更されたとき、数値項目の入力項目の表示状態を切り替える処理
        var exParamValue = selected == null ? '' : selected.EXPARAM1;

        // 登録用に拡張項目の値を退避
        setValue(ctrlId, Const_MS0020.FormInfo.SpecRegist.Keys.Val.InputTypeExtension, 0, CtrlFlag.Label, exParamValue);

        // 画面の項目の表示切替
        var switchCtrls = function (ctrlInfo, isInfo) {
            // 選択された要素の拡張項目の値が指定された値のの場合、非表示
            // それ以外の場合は表示
            var isHide = ctrlInfo.DisplaySwitch.ExParams.includes(exParamValue);
            // メソッド側でFormNoを取得しないメソッド用に取得
            var ctrlIdWithForm = Const_MS0020.FormInfo.SpecRegist.Keys.Id + getAddFormNo();
            // 新規登録の場合、仕様項目選択肢ボタンは常に非表示
            var isNew = getIsNew();
            $.each(ctrlInfo.DisplaySwitch.HideVals, function (idx, val) {
                if (isInfo) {
                    // 入力項目の表示切替
                    changeDispColumn(ctrlIdWithForm, val, !isHide);
                } else {
                    // 仕様項目選択肢ボタンの表示切替
                    setHideButton(Const_MS0020.FormInfo.SpecRegist.Button.Name.SpecImtes, isHide || isNew);
                    // 一度入力が行われるとボタンは非活性になる
                    setDisableSpecItemsBtn(true);
                }
            });
        }
        // 入力項目の表示切替
        switchCtrls(Const_MS0020.FormInfo.SpecRegist.Keys, true);
        // 仕様項目選択肢ボタンの表示切替
        switchCtrls(Const_MS0020.FormInfo.SpecRegist.Button, false);
    }
}

/**
 * 機種別仕様登録画面　新規登録かどうか判定
 * */
function getIsNew() {
    // 非表示項目の機種別仕様関連付けIDの有無で新規かどうかを取得
    var value = getValue(Const_MS0020.FormInfo.SpecRegist.Keys.Id, Const_MS0020.FormInfo.SpecRegist.Keys.Val.RelationId, 0, CtrlFlag.Label);
    var result = (value == null || value == '');
    return result;
}

/**
 * 機種別仕様登録画面　仕様項目選択肢画面の活性制御
 * @param {any} isDisable 非活性にする場合TRUE
 */
function setDisableSpecItemsBtn(isDisable) {
    var btn = getButtonCtrl(Const_MS0020.FormInfo.SpecRegist.Button.Name.SpecImtes);
    setDisableBtn(btn, isDisable);
}