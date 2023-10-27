/* ========================================================================
 *  機能名　    ：   【MS0001】マスタメンテナンスメニュー
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)MS0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/tmqmaster.js\"></script>");
document.write("<script src=\"" + getPath() + "/SI0001.js\"></script>");
document.write("<script src=\"" + getPath() + "/MS1000.js\"></script>");
document.write("<script src=\"" + getPath() + "/MS1001.js\"></script>");
document.write("<script src=\"" + getPath() + "/MS1010.js\"></script>");
document.write("<script src=\"" + getPath() + "/MS1020.js\"></script>");
document.write("<script src=\"" + getPath() + "/MS1040.js\"></script>");
document.write("<script src=\"" + getPath() + "/MS0010.js\"></script>");
document.write("<script src=\"" + getPath() + "/MS0020.js\"></script>");

// 一覧画面
const FormList = {
    // 画面No
    No: 0,
    // メニュー一覧
    MenuList: {
        Id: "BODY_010_00_LST_0",
        // 機能ID列
        ConductId: 1,
    },
}

//// 拡張項目列
//const ItemExCol = { ExData1: 11, ExData2: 12, ExData3: 13, ExData4: 14, ExData5: 15 }
//// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
//const ItemExCtrlCol = { ExData1: 0, ExData2: 2, ExData3: 2, ExData4: 2, ExData5: 2 }

// 拡張項目列
var ItemExCol = { }
// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
var ItemExCtrlCol = { }

// 機能別拡張項目列
// 拡張項目列
//--【MS1060】予算性格区分マスタ
var ItemExCol_MS1060 = { ExData1: 11 }
var ItemExCtrlCol_MS1060 = { ExData1: 2 }
//--【MS1130】系停止マスタ
var ItemExCol_MS1130 = { ExData1: 11 }
var ItemExCtrlCol_MS1130 = { ExData1: 2 }
//--【MS1170】機器レベルマスタ
var ItemExCol_MS1170 = { ExData1: 11 }
var ItemExCtrlCol_MS1170 = { ExData1: 2 }
//--【MS1210】使用区分マスタ
var ItemExCol_MS1210 = { ExData1: 11 }
var ItemExCtrlCol_MS1210 = { ExData1: 2 }
//--【MS1230】保全区分マスタ
var ItemExCol_MS1230 = { ExData1: 11 }
var ItemExCtrlCol_MS1230 = { ExData1: 2 }
//--【MS1240】点検種別マスタ
var ItemExCol_MS1240 = { ExData1: 11 }
var ItemExCtrlCol_MS1240 = { ExData1: 0 }
//--【MS1540】依頼番号採番パターンマスタ
var ItemExCol_MS1540 = { ExData1: 11 }
var ItemExCtrlCol_MS1540 = { ExData1: 2 }
//--文書種類
//  【MS1600】文書種類_機器台帳_機番添付マスタ
//  【MS1610】文書種類_機器台帳_機器添付マスタ
//  【MS1620】文書種類_機器台帳_保全項目一覧マスタ
//  【MS1630】文書種類_機器台帳_MP情報タブマスタ
//  【MS1640】文書種類_件名別長期計画_件名添付マスタ
//  【MS1650】文書種類_保全活動_件名添付マスタ
//  【MS1660】文書種類_保全活動_故障分析_略図添付マスタ
//  【MS1670】文書種類_保全活動_故障分析_故障原因分析書添付マスタ
//  【MS1680】文書種類_保全活動_故障分析(個別工場)_略図添付マスタ
//  【MS1690】文書種類_保全活動_故障分析(個別工場)_故障原因分析書添付マスタ
//  【MS1700】文書種類_予備品一覧_画像添付マスタ
//  【MS1750】文書種類_予備品一覧_文書添付マスタ
//  【MS1780】文書種類_予備品一覧_予備品地図マスタ
var ItemExCol_MS1600 = { ExData1: 14, ExData2: 12, ExData3: 13 }
var ItemExCtrlCol_MS1600 = { ExData1: 0, ExData2: 0, ExData3: 0 }
//--【MS1710】添付種類
var ItemExCol_MS1710 = { ExData1: 11 }
var ItemExCtrlCol_MS1710 = { ExData1: 2 }
//--【MS1720】仕入先マスタ
var ItemExCol_MS1720 = { ExData1: 11 }
var ItemExCtrlCol_MS1720 = { ExData1: 0 }
//--【MS1730】数量管理単位マスタ
var ItemExCol_MS1730 = { ExData1: 11, ExData2: 12 }
var ItemExCtrlCol_MS1730 = { ExData1: 0, ExData2: 2 }
//--【MS1740】金額管理単位マスタ
var ItemExCol_MS1740 = { ExData1: 11, ExData2: 12 }
var ItemExCtrlCol_MS1740 = { ExData1: 0, ExData2: 2 }
//--【MS1760】部門（工場・部門）マスタ
var ItemExCol_MS1760 = { ExData1: 11, ExData2: 12 }
var ItemExCtrlCol_MS1760 = { ExData1: 0, ExData2: 3 }
//--【MS1770】勘定科目マスタ
var ItemExCol_MS1770 = { ExData1: 11, ExData2: 12 }
var ItemExCtrlCol_MS1770 = { ExData1: 0, ExData2: 2 }
//--【MS1850】MQ分類マスタ
var ItemExCol_MS1850 = { ExData1: 11, ExData2: 12, ExData3: 13, ExData4: 14, ExData5: 15 }
var ItemExCtrlCol_MS1850 = { ExData1: 0, ExData2: 2, ExData3: 2, ExData4: 2, ExData5: 2 }
//--【MS1940】新旧区分マスタ
var ItemExCol_MS1940 = { ExData1: 11 }
var ItemExCtrlCol_MS1940 = { ExData1: 2 }
//--【MS1980】予備品出庫区分マスタ
var ItemExCol_MS1980 = { ExData1: 11 }
var ItemExCtrlCol_MS1980 = { ExData1: 2 }
//--【MS2010】棚卸時間マスタ
var ItemExCol_MS2010 = { ExData1: 11 }
var ItemExCtrlCol_MS2010 = { ExData1: 0 }
//--【MS2020】工場毎年度期首月マスタ
var ItemExCol_MS2020 = { ExData1: 11, ExData2: 12 }
var ItemExCtrlCol_MS2020 = { ExData1: 2, ExData2: 2 }
//--【MS2040】予備品発注基準判定条件マスタ
var ItemExCol_MS2040 = { ExData1: 11 }
var ItemExCtrlCol_MS2040 = { ExData1: 2 }
//--【MS2050】丸め処理区分マスタ
var ItemExCol_MS2050 = { ExData1: 11 }
var ItemExCtrlCol_MS2050 = { ExData1: 2 }
//--【MS2070】工場毎会計提出表出力条件マスタ
var ItemExCol_MS2070 = { ExData1: 11, ExData2: 12 }
var ItemExCtrlCol_MS2070 = { ExData1: 3, ExData2: 0 }
//--【MS2080】長期計画メール送信情報マスタ
var ItemExCol_MS2080 = { ExData1: 11, ExData2: 12, ExData3: 13, ExData4: 14, ExData5: 15, ExData6: 16, ExData7: 17, ExData8: 18, ExData9: 19 }
var ItemExCtrlCol_MS2080 = { ExData1: 0, ExData2: 0, ExData3: 0, ExData4: 0, ExData5: 0, ExData6: 0, ExData7: 0, ExData8: 0, ExData9: 0 }
//--【MS9020】言語マスタ
var ItemExCol_MS9020 = { ExData1: 11 }
var ItemExCtrlCol_MS9020 = { ExData1: 0 }

var TargetConductId = "";

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
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    TargetConductId = conductId;
    if (conductId == MS0001_ConductId) {
        // マスタメンテナンス一覧の場合は実行しない
        return;
    } else if (conductId == ConductIdMS0010) {
        // 初期化処理（ユーザーマスタ用）
        initFormOriginalForMS0010(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    } else if (conductId == ConductIdMS0020) {
        // 初期化処理（機種別仕様用）
        initFormOriginalForMS0020(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    } else if (conductId == ConductIdMS1000) {
        // 初期化処理（地区/工場用）
        initFormOriginalForMS1000(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    } else if (conductId == ConductIdMS1001) {
        // 初期化処理（場所階層用）
        initFormOriginalForMS1001(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    } else if (conductId == ConductIdMS1010) {
        // 初期化処理（職種・機種用）
        initFormOriginalForMS1010(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    } else if (conductId == ConductIdMS1020) {
        // 初期化処理（原因性格用）
        initFormOriginalForMS1020(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    } else if (conductId == ConductIdMS1040) {
        // 初期化処理（予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)用）
        initFormOriginalForMS1040(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    } else {
        /*-------------------------------------------------------------
        // 拡張項目列の列番号、コントロールを指定（拡張項目ありの場合）
          -------------------------------------------------------------*/
        var chkExItem = true;
        if (conductId == ConductIdMS1060) { //1060:予算性格区分マスタ
            ItemExCol = ItemExCol_MS1060;
            ItemExCtrlCol = ItemExCtrlCol_MS1060;
        }
        else if (conductId == ConductIdMS1130) { //1130:系停止マスタ
            ItemExCol = ItemExCol_MS1130;
            ItemExCtrlCol = ItemExCtrlCol_MS1130;
        }
        else if (conductId == ConductIdMS1170) { //1170:機器レベルマスタ
            ItemExCol = ItemExCol_MS1170;
            ItemExCtrlCol = ItemExCtrlCol_MS1170;
        }
        else if (conductId == ConductIdMS1210) { //1210:使用区分マスタ
            ItemExCol = ItemExCol_MS1210;
            ItemExCtrlCol = ItemExCtrlCol_MS1210;
        }
        else if (conductId == ConductIdMS1230) { //1230:保全区分マスタ
            ItemExCol = ItemExCtrlCol_MS1230;
            ItemExCtrlCol = ItemExCtrlCol_MS1230;
        }
        else if (conductId == ConductIdMS1240) { //1240:点検種別マスタ
            ItemExCol = ItemExCol_MS1240;
            ItemExCtrlCol = ItemExCtrlCol_MS1240;
        }
        else if (conductId == ConductIdMS1540) { //1540:依頼番号採番パターンマスタ
            ItemExCol = ItemExCol_MS1540;
            ItemExCtrlCol = ItemExCtrlCol_MS1540;
        }
        else if ((conductId == ConductIdMS1600) || (conductId == ConductIdMS1610) || (conductId == ConductIdMS1620) || (conductId == ConductIdMS1630)
             || (conductId == ConductIdMS1640) || (conductId == ConductIdMS1650) || (conductId == ConductIdMS1660) || (conductId == ConductIdMS1670)
             || (conductId == ConductIdMS1680) || (conductId == ConductIdMS1690) || (conductId == ConductIdMS1700) || (conductId == ConductIdMS1750)
            || (conductId == ConductIdMS1780)) { //1600:文書種類_機器台帳_機番添付マスタ～MS1780:文書種類_予備品一覧_予備品地図マスタ
            ItemExCol = ItemExCol_MS1600;
            ItemExCtrlCol = ItemExCtrlCol_MS1600;
        }
        else if (conductId == ConductIdMS1710) { //1710:添付種類
            ItemExCol = ItemExCol_MS1710;
            ItemExCtrlCol = ItemExCtrlCol_MS1710;
        }
        else if (conductId == ConductIdMS1720) { //1720:仕入先マスタ
            ItemExCol = ItemExCol_MS1720;
            ItemExCtrlCol = ItemExCtrlCol_MS1720;
        }
        else if (conductId == ConductIdMS1730) { //1730:数量管理単位マスタ
            ItemExCol = ItemExCol_MS1730;
            ItemExCtrlCol = ItemExCtrlCol_MS1730;
        }
        else if (conductId == ConductIdMS1740) { //1740:金額管理単位マスタ
            ItemExCol = ItemExCol_MS1740;
            ItemExCtrlCol = ItemExCtrlCol_MS1740;
        }
        else if (conductId == ConductIdMS1760) { //1760:部門（工場・部門）マスタ
            ItemExCol = ItemExCol_MS1760;
            ItemExCtrlCol = ItemExCtrlCol_MS1760;
        }
        else if (conductId == ConductIdMS1770) { //1770:勘定科目マスタ
            ItemExCol = ItemExCol_MS1770;
            ItemExCtrlCol = ItemExCtrlCol_MS1770;
        }
        else if (conductId == ConductIdMS1850) { //1850:MQ分類
            ItemExCol = ItemExCol_MS1850;
            ItemExCtrlCol = ItemExCtrlCol_MS1850;
        }
        else if (conductId == ConductIdMS1940) { //1940:新旧区分マスタ
            ItemExCol = ItemExCol_MS1940;
            ItemExCtrlCol = ItemExCtrlCol_MS1940;
        }
        else if (conductId == ConductIdMS1980) { //1980:予備品出庫区分マスタ
            ItemExCol = ItemExCol_MS1980;
            ItemExCtrlCol = ItemExCtrlCol_MS1980;
        }
        else if (conductId == ConductIdMS2010) { //2010:棚卸時間マスタ
            ItemExCol = ItemExCol_MS2010;
            ItemExCtrlCol = ItemExCtrlCol_MS2010;
        }
        else if (conductId == ConductIdMS2020) { //2020:工場毎年度期首月マスタ
            ItemExCol = ItemExCol_MS2020;
            ItemExCtrlCol = ItemExCtrlCol_MS2020;
        }
        else if (conductId == ConductIdMS2040) { //2040:予備品発注基準判定条件マスタ
            ItemExCol = ItemExCol_MS2040;
            ItemExCtrlCol = ItemExCtrlCol_MS2040;
        }
        else if (conductId == ConductIdMS2050) { //2050:丸め処理区分マスタ
            ItemExCol = ItemExCol_MS2050;
            ItemExCtrlCol = ItemExCtrlCol_MS2050;
        }
        else if (conductId == ConductIdMS2070) { //2070:工場毎会計提出表出力条件マスタ
            ItemExCol = ItemExCol_MS2070;
            ItemExCtrlCol = ItemExCtrlCol_MS2070;
        }
        else if (conductId == ConductIdMS2080) { //2080:長期計画メール送信情報マスタ
            ItemExCol = ItemExCol_MS2080;
            ItemExCtrlCol = ItemExCtrlCol_MS2080;
        }
        else if (conductId == ConductIdMS9020) { //9020:言語マスタ
            ItemExCol = ItemExCol_MS9020;
            ItemExCtrlCol = ItemExCtrlCol_MS9020;
        }
        else {
            chkExItem = false;
        }
        // 初期化処理（マスタメンテナンス用）
        initFormOriginalForMaster(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data, ItemExCol, ItemExCtrlCol);
        if (chkExItem == true) {
            // 拡張項目あり
            // アイテム検索画面用
            if (formNo == MasterFormEdit.No) {
                // 登録・修正画面

                // 工場ID取得
                var factoryId = getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FactoryId, 1, CtrlFlag.Label);
                // アイテム情報一覧の翻訳用工場IDに工場IDを設定する
                setValue(MasterFormEdit.ItemList.Id, MasterFormList.ItemList.TranFactoryId, 1, CtrlFlag.Label, factoryId)
            }
        }
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
    if (TargetConductId == ConductIdMS1001) {
        // コンボボックス変更時イベント（場所階層用）
        setComboOtherValuesForMS1001(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    } else if (TargetConductId == ConductIdMS1010) {
        // コンボボックス変更時イベント（職種・機種用）
        setComboOtherValuesForMS1010(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    } else if (TargetConductId == ConductIdMS1020) {
        // コンボボックス変更時イベント（原因性格用）
        setComboOtherValuesForMS1020(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    } else if (TargetConductId == ConductIdMS1040) {
        // コンボボックス変更時イベント（予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)用）
        setComboOtherValuesForMS1040(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    } else if (TargetConductId == ConductIdMS0020) {
        // コンボボックス変更時イベント（機種別仕様用）
        setComboOtherValuesForMS0020(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    }
}

/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginal(tabNo, tableId) {
    if (TargetConductId == ConductIdMS0010) {
        // コンボボックス変更時イベント（ユーザーマスタ用）
        initTabOriginalForMS0010(tabNo, tableId);
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
    const conductId = getConductId();
    if (conductId == ConductIdMS0010 || conductId == ConductIdMS0020) {
        // ユーザーマスタ・機種別仕様マスタの場合は何もしない
    }
    else if (conductId == MS0001_ConductId) {
        // マスタメンテナンス一覧の場合
        if (ctrlId == FormList.MenuList.Id) {
            if (transTarget == '-') {
                //// Noリンクをクリック
                //getSiblingCtrl(element, FormList.MenuList.ConductId, CtrlFlag.Link).click();

                // 遷移先機能IDを取得
                const targetConductId = $(getSiblingCtrl(element, FormList.MenuList.ConductId, CtrlFlag.Link)).text();

                // 画面レイアウトデータを取得
                getArticleElements(appPath, transPtn, transDiv, 1, transDispPtnDef.Shift, formNo, ctrlId, btn_ctrlId, rowNo, element, targetConductId);

                // 処理をキャンセル
                return [false, conditionDataList];
            }
        }
    } else {
        // マスタメンテナンス一覧以外の場合
        if (transTarget == SI0001_ConsuctId) {
            // 遷移先が「アイテム検索」の場合

            // 工場ID取得
            var factoryId = getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FactoryId, 1, CtrlFlag.Label);

            // 画面遷移時のパラメータ
            var conditionDataList = [];
            var conditionData = {};
            conditionData['CTRLID'] = 'InitCondition';                                 // コントロールID
            conditionData['STRUCTUREGROUPID'] = SearchItemStructureGroupID.Budget;	   // 構成グループID(1060)
            conditionData['FACTORYID'] = factoryId;　　　                              // 工場ID
            conditionDataList.push(conditionData);
            return [true, conditionDataList];
        } else if (conductId == ConductIdMS1000) {
            // 遷移処理の前処理（地区/工場用）
            return prevTransFormForMS1000(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        } else if (conductId == ConductIdMS1001) {
            // 遷移処理の前処理（場所階層用）
            return prevTransFormForMS1001(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        } else if (conductId == ConductIdMS1010) {
            // 遷移処理の前処理（職種・機種用）
            return prevTransFormForMS1010(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        } else if (conductId == ConductIdMS1020) {
            // 遷移処理の前処理（原因性格用）
            return prevTransFormForMS1020(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        } else if (conductId == ConductIdMS1040) {
            // 遷移処理の前処理（予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)）
            return prevTransFormForMS1040(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        } else {
            // 遷移処理の前処理（マスタメンテナンス用）
            return prevTransFormForMaster(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
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
 *  @return {bool} データ取得する場合はtrue、スキップする場合はfalse（子画面のみ）
 */
function prevBackBtnProcess(appPath, btnCtrlId, status, codeTransFlg) {
    const conductId = getConductId();
    if (conductId == ConductIdMS0020) {
        // 機種別仕様の場合
        return prevBackBtnProcessForMS0020(appPath, btnCtrlId, status, codeTransFlg);
    }
    else if (conductId != ConductIdMS0010 && conductId == MS0001_ConductId) {
        // マスタメンテナンス一覧の場合
        if (codeTransFlg != 1) {
            codeTransFlg = 0;
        }
    } else {
        // マスタメンテナンス一覧以外の場合
        // 戻る処理の前(単票、子画面共用)（マスタメンテナンス用）
        return prevBackBtnProcessForMaster(appPath, btnCtrlId, status, codeTransFlg);
    }
    return true;
}

/**
 *【オーバーライド用関数】行追加の前
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {html}       element     ：ｲﾍﾞﾝﾄ発生要素
 *  @param {boolean}    isCopyRow   ：行コピーフラグ(true：行コピー、false：行追加)
 *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、3：単票参照、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {int}        rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {boolean}    confirmFlg  ：遷移前確認ﾌﾗｸﾞ(true：確認する、false：確認しない)
 */
//function prevAddNewRow(appPath, element, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, confirmFlg) {
function prevAddNewRow(appPath, element, isCopyRow, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, confirmFlg) {
    return true;    // 個別実装で以後の処理の実行可否を制御 true：続行、false：中断
}

/**
 *【オーバーライド用関数】
 *  行削除の前
 *
 *  @appPath    {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn        {<a>}       ：行削除ﾘﾝｸのa要素
 *  @id         {string}    ：行削除ﾘﾝｸの対象一覧のCTRLID
 *  @checkes    {要素}      ：削除対象の要素リスト(tabulator一覧の場合、削除対象のデータリスト)
 */
function preDeleteRow(appPath, btn, id, checkes) {
    const conductId = getConductId();
    if (conductId == MS0001_ConductId) {
        // マスタメンテナンス一覧の場合
        return true;   // 個別実装で以後の処理の実行可否を制御 true：続行、false：中断
    } else {
        // マスタメンテナンス一覧以外の場合
        // 共通処理により非表示の削除ボタンを押下
        return preDeleteRowCommon(id, [MasterFormList.StandardItemList.Id, MasterFormList.FactoryItemList.Id]);
    }
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
    // マスタメンテナンス一覧の場合は実行しない
    if (conductId == MS0001_ConductId) { return; }

    // ユーザーマスタ・機種別仕様マスタでない場合
    if (conductId != ConductIdMS0010 && conductId != ConductIdMS0020) {
        // 共通-アイテム検索画面を閉じたとき
        SI0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
    }
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
function beforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    // マスタメンテナンス一覧の場合は実行しない
    if (conductId == MS0001_ConductId) { return; }

    if (conductId == ConductIdMS1000) {
        // 検索処理前（地区/工場用）
        beforeSearchBtnProcessForMS1000(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS1001) {
        // 検索処理前（場所階層用）
        beforeSearchBtnProcessForMS1001(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS1010) {
        // 検索処理前（職種・機種用）
        beforeSearchBtnProcessForMS1010(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS1020) {
        // 検索処理前（原因性格用）
        beforeSearchBtnProcessForMS1020(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS1040) {
        // 検索処理前（予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)用）
        beforeSearchBtnProcessForMS1040(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS0020) {
        // 検索処理前（機種別仕様用）
        beforeSearchBtnProcessForMS0020(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS0010) {
        // 検索処理前（ユーザーマスタ用　何もしない）
    } else {
        // 検索処理前（マスタメンテナンス用）
        beforeSearchBtnProcessForMaster(appPath, btn, conductId, pgmId, formNo, conductPtn);
    }
}

/**
 *【オーバーライド用関数】
 *  検索処理後
 *
 *  @appPath {string} 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn {string} 　　　：対象ボタン
 *  @conductId {string} ：機能ID
 *  @pgmId {string} 　　：プログラムID
 *  @formNo {number} 　 ：画面番号
 *  @conductPtn {number}：処理ﾊﾟﾀｰﾝ
 */
function afterSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    // マスタメンテナンス一覧の場合は実行しない
    if (conductId == MS0001_ConductId) { return; }

    if (conductId == ConductIdMS1001) {
        // 検索処理後（場所階層用）
        afterSearchBtnProcessForMS1001(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS1010) {
        // 検索処理後（職種・機種用）
        afterSearchBtnProcessForMS1010(appPath, btn, conductId, pgmId, formNo, conductPtn);
    } else if (conductId == ConductIdMS1040) {
        // 検索処理後（予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)）
        afterSearchBtnProcessForMS1040(appPath, btn, conductId, pgmId, formNo, conductPtn);
    }
}

/**
 *【オーバーライド用関数】
 *  共通機能用選択ボタン押下時処理
 *  @param {string}     appPath     :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}     cmConductId :共通機能ID
 *  @param {article}    cmArticle   :共通機能articleタグ
 *  @param {button}     btn         :選択ボタン要素
 *  @param {string}     fromCtrlId  :共通機能遷移時に押下したﾎﾞﾀﾝ/一覧のｺﾝﾄﾛｰﾙID
 *  @param {string}     fromConductId  :共通機能遷移元の機能ID
 */
function clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, fromConductId) {
    // アイテム検索画面
    SI0001_clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, fromConductId);
}

/*==1:実行処理==*/

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function registCheckPre(appPath, conductId, formNo, btn) {
    return true;
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
    // 独自の確認メッセージを表示するために使用、関連情報パラメータは0に設定してください。
    return true;
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
function postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {
    // マスタメンテナンス一覧の場合は実行しない
    if (conductId == MS0001_ConductId) { return; }

    // 更新対象の構成グループID
    const grpId = parseInt(getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.StructureGroupId, 1, CtrlFlag.Label), 10);

    if (conductId == ConductIdMS1000 || conductId == ConductIdMS1001 || conductId == ConductIdMS1010 || conductId == ConductIdMS1020) {
        // 地区/工場、場所階層、職種・機種、原因性格の場合

        // ツリービューの再作成
        refreshTreeView(appPath, conductId, grpId);
    }
    //// コンボボックスの再作成
    //refreshComboBox(appPath, grpId);

    if (conductId == ConductIdMS1001 || conductId == ConductIdMS1010) {
        // 場所階層、職種・機種の場合
        var btnName = $(btn).attr("name");
        if (btnName == MasterFormList.ButtonId.Delete) {
            //  行削除アイコン押下時
            // コンボデータをクリア
            clearSavedComboBoxData(grpId);
            if (conductId == ConductIdMS1001) {
                // 実行正常終了後処理（場所階層用）
                postRegistProcessForMS1001(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
            } else if (conductId == ConductIdMS1010) {
                // 実行正常終了後処理（職種・機種用）
                postRegistProcessForMS1010(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
            }
        }
    } else {
        // コンボボックスの再作成
        refreshComboBox(appPath, grpId);
    }
}

/*==5:出力処理==*/

/*==9:削除処理==*/
/**
 * 【オーバーライド用関数】削除ボタン前処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} conductId   ：機能ID
 *  @param {string} pgmId       ：プログラムID
 *  @param {number} formNo      ：画面番号
 *  @param {byte}   conductPtn  ：画面ﾊﾟﾀｰﾝ
 *  @param {element}btn         ：ﾎﾞﾀﾝ要素
 *  @param {boolean}isEdit      ：単票ｴﾘｱﾌﾗｸﾞ
 *  @param {int}    confirmNo   ：確認番号(ﾃﾞﾌｫﾙﾄ：0)
 *  @return {bool} 処理続行フラグ Trueなら続行、Falseなら処理終了
 */
function preDeleteProcess(appPath, conductId, pgmId, formNo, conductPtn, btn, isEdit, confirmNo) {
    // 独自の確認メッセージを表示するために使用、関連情報パラメータは0に設定してください。
    return true;
}

/*==8:取込処理==*/


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
function passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId) {
    // アイテム検索画面
    SI0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);
}

/**
 *【オーバーライド用関数】子画面新規遷移前ﾁｪｯｸ処置
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevTransChildForm(appPath, conductId, formNo, btn) {
    return true;
}

/**
 *【オーバーライド用関数】他機能遷移時パラメータを個別実装で作成
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transTarget ：遷移先情報（機能ID|画面NO）
 *  @param {number} formNo      ：遷移元画面NO
 *  @param {string} ctrlId      ：遷移元の一覧ctrlid
 *  @param {string} btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {int}    rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {html}   element     ：ｲﾍﾞﾝﾄ発生要素
 *
 *  @return {List<Dictionary<string, object>>}  conditionDataList
 */
function createParamTransOtherIndividual(appPath, transTarget, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];

    return conditionDataList;
}

/**
 *【オーバーライド用関数】他機能遷移先の戻る時再検索用退避条件(P_SearchCondition)を作成
 *  @param {string} appPath         ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transTarget     ：遷移先情報（機能ID|画面NO|親画面NO）
 *  @param {string} transConductId  ：遷移先の機能ID
 *  @param {number} formNo          ：遷移元画面NO
 *  @param {string} ctrlId          ：遷移元の一覧ctrlid
 *  @param {string} btn_ctrlId      ：ボタンのbtn_ctrlid
 *  @param {int}    rowNo           ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {html}   element         ：ｲﾍﾞﾝﾄ発生要素
 *
 *  @return {Dictionary<string, object>}  dicBackConditions
 */
function createParamTransOtherBack(appPath, transTarget, transConductId, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var dicBackConditions = {};

    return dicBackConditions;
}

/**
 *【オーバーライド用関数】共通機能ポップアップ画面遷移前ﾁｪｯｸ処置
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：遷移先情報
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevTransCmForm(appPath, transTarget, formNo, btn) {
    return true;
}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function reportCheckPre(appPath, conductId, formNo, btn) {
    return true;
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 *  
 *  @return {Dictionary<string, object>}  追加する条件リスト
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {
    if (conductId == MS0001_ConductId) {
        // マスタメンテナンス一覧の場合
        var conditionDataList = [];
        return conditionDataList;
    } else {
        // マスタメンテナンス一覧以外の場合
        // 登録前追加条件取得処理（マスタメンテナンス用）
        return addSearchConditionDictionaryForRegistForMaster(appPath, conductId, formNo, btn);
    }
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
    var isAutoBackFlg = true;

    return [isContinue, isError, isAutoBackFlg];
}

/**
 * 【オーバーライド用関数】Tabuator一覧の描画前処理
 * @param {string} appPath  ：アプリケーションルートパス
 * @param {string} id       ：一覧のID(#,_FormNo付き)
 * @param {string} options  ： 一覧のオプション情報
 * @param {object} header   ：ヘッダー情報
 * @param {object} dispData ：データ
 */
function prevCreateTabulator(appPath, id, options, header, dispData) {
    const conductId = getConductId();
    // マスタメンテナンス一覧の場合は実行しない
    if (conductId == MS0001_ConductId) { return; }

    // Tabuator一覧の描画前処理 (マスタメンテナンス用)
    prevCreateTabulatorForMaster(appPath, id, options, header, dispData)
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {
    const conductId = getConductId();
    // マスタメンテナンス一覧の場合は実行しない
    if (conductId == MS0001_ConductId) {
        return;
    } else if (conductId == ConductIdMS1000) {
        // Tabulatorの描画が完了時の処理  (地区/工場用)
        postBuiltTabulatorForMS1000(tbl, id);
    } else if (conductId == ConductIdMS1001) {
        // Tabulatorの描画が完了時の処理  (場所階層用)
        postBuiltTabulatorForMS1001(tbl, id);
    } else if (conductId == ConductIdMS1010) {
        // Tabulatorの描画が完了時の処理  (職種・機種用)
        postBuiltTabulatorForMS1010(tbl, id);
    } else if (conductId == ConductIdMS1040) {
        // Tabulatorの描画が完了時の処理  (予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)用)
        postBuiltTabulatorForMS1040(tbl, id);
    } else if (conductId == ConductIdMS0010) {
        // Tabulatorの描画が完了時の処理  (ユーザーマスタ用)
        postBuiltTabulatorForMS0010(tbl, id);
    } else if (conductId == ConductIdMS0020) {
        // Tabulatorの描画が完了時の処理  (機種別仕様用　何もしない)
    } else {
        // Tabulatorの描画が完了時の処理 (マスタメンテナンス用)
        postBuiltTabulatorForMaster(tbl, id);
    }
}

/**
 * 【オーバーライド用関数】画面初期値データ取得前処理(表示中画面用)
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} formNo 画面番号
 * @param {any} btnCtrlId ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
 * @param {any} conditionDataList 条件一覧要素
 * @param {any} listDefines 一覧の定義情報
 * @param {any} paramPgmId 遷移先のプログラムID
 */
function prevInitFormData(appPath, formNo, btnCtrlId, conditionDataList, listDefines, paramPgmId) {
    const conductId = getConductId();
    if (conductId == ConductIdMS0020) {
        // 機種別仕様の場合
        return prevInitFormDataForMS0020(appPath, formNo, btnCtrlId, conditionDataList, listDefines, paramPgmId);
    }
    else if (conductId != ConductIdMS0010 && conductId == MS0001_ConductId) {
        // マスタメンテナンス一覧の場合
        const pgmId = getConductId();
        if (paramPgmId == pgmId) {
            return [conditionDataList, listDefines];
        } else {
            // 子画面からマスタメンテナンス一覧へ戻る場合、機能IDとプログラムIDを修正する
            return [conditionDataList, listDefines, conductId, pgmId];
        }
    } else {
        // マスタメンテナンス一覧以外の場合
        // 画面初期値データ取得前処理(表示中画面用) (マスタメンテナンス用)
        return prevInitFormDataForMaster(appPath, formNo, btnCtrlId, conditionDataList, listDefines, paramPgmId);
    }

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
function prevBackBtnProcessForChild(appPath, btnCtrlId, status, parentModalNo) {
    if (TargetConductId == ConductIdMS0020) {
        // 機種別仕様の場合
        parentModalNo = prevBackBtnProcessForChildForMS0020(appPath, btnCtrlId, status, parentModalNo);
    }
    return parentModalNo;
}

/**
 * 【オーバーライド用関数】階層選択モーダル画面の選択ボタン実行後
 * @param {any} appPath アプリケーションパス
 * @param {any} btn 選択ボタン要素
 * @param {any} ctrlId コントロールID
 * @param {any} structureGrpId 構成グループID
 * @param {any} maxStructureNo 最下層階層番号
 * @param {any} node 設定値取得先のノード
 */
function afterSelectBtnForTreeView(appPath, btn, ctrlId, structureGrpId, maxStructureNo, node) {
    if (TargetConductId == ConductIdMS0010) {
        // ユーザーマスタの場合
        afterSelectBtnForTreeViewForMS0010(appPath, btn, ctrlId, structureGrpId, maxStructureNo, node);
    }
}