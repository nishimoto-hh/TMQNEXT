
// getCtrl、getValue、setValueの引数に使用するflgの値を定義
var CtrlFlag = { TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8, Search: 9, MultiCheckBox: 10 };
// 戻るボタンのID
const BtnCtrlId_Back = 'Back';

// 添付情報関連
const DM0001_List_CtrlId = "BODY_020_00_LST_0";
// 添付情報　添付元の構成グループID
const AttachmentStructureGroupID =
{
    Machine: 1600,                  // 機器台帳-機番添付
    Equipment: 1610,                // 機器台帳-機器添付
    Content: 1620,                  // 機器台帳-保全項目一覧-ファイル添付
    MpInfo: 1630,                   // 機器台帳-MP情報タブ-ファイル添付
    LongPlan: 1640,                 // 件名別長期計画-件名添付
    Summary: 1650,                  // 保全活動-件名添付
    HistoryFailureDiagram: 1660,    // 保全活動-故障分析情報タブ-略図添付
    HistoryFailureAnalyze: 1670,    // 保全活動-故障分析情報タブ-故障原因分析書添付
    HistoryFailureFactDiagram: 1680,// 保全活動-故障分析情報(個別工場)タブ-略図添付
    HistoryFailureFactAnalyze: 1690,// 保全活動-故障分析情報(個別工場)タブ-故障原因分析書添付
    SpareImage: 1700,               // 予備品管理-画像添付
    SpareDocument: 1750,            // 予備品管理-文書添付
    SpareMap: 1780                  // 予備品管理-予備品地図
};

// 機器台帳 機能ID
const MC0001_ConductId = 'MC0001';

// 件名別長期計画 機能ID
const LN0001_ConductId = 'LN0001';

// 予備品一覧 機能ID
const PT0001_ConductId = 'PT0001';

// 添付情報詳細 機能ID
const DM0002_ConductId = 'DM0002';

// アイテム検索 機能ID
const SI0001_ConsuctId = 'SI0001';

// 担当者検索 機能ID
const SU0001_ConsuctId = 'SU0001';

// 予備品検索 機能ID
const SP0001_ConductId = 'SP0001';

// 入庫入力 機能ID
const PT0005_ConsuctId = 'PT0005';

// 出庫入力 機能ID
const PT0006_ConsuctId = 'PT0006';

// 移庫入力 機能ID
const PT0007_ConsuctId = 'PT0007';

// 帳票出力 機能ID
const RM00001_ConductId = "RM0001";

// マスタメンテナンス一覧 機能ID
const MS0001_ConductId = "MS0001";

// 変更管理機器台帳 機能ID
const ConductId_HM0001 = "HM0001";

// 変更管理長期計画 機能ID
const ConductId_HM0002 = "HM0002";

// 申請状況変更 機能ID
const ConductId_HM0003 = "HM0003";

// 変更管理帳票出力 機能ID
const ConductId_HM0004 = "HM0004";

// 一覧フィルタ(付加情報の遷移先に設定)
const ListFilter_TransTarget = 'FILTER';

// 棚卸▶受払履歴
const PT0003_ConditionInfo = "COND_010_00_LST_0";
// 予備品一覧▶入庫入力
const PT0005_List_CtrlId = "BODY_030_00_LST_0";
// 予備品一覧▶出庫入力
const PT0001_List_CtrlId = "BODY_020_00_LST_0";
// 予備品一覧▶移庫入力(棚番移庫)
const PT0007_SubjectList_CtrlId = "BODY_070_00_LST_0";
// 予備品一覧▶移庫入力(部門移庫)
const PT0007_DepartmentList_CtrlId = "BODY_090_00_LST_0";

// 機器台帳変更管理一覧
const HM0001_List_CtrlId = "BODY_040_00_LST_0";
// 長期計画変更管理一覧
const HM0002_List_CtrlId = "BODY_040_00_LST_0";
// 申請状況変更
const HM0003_List_CtrlId = "CBODY_000_00_LST_0";
// 変更管理帳票出力
const HM0004_List_CtrlId = "CCOND_080_00_LST_0";
/**
 * スケジュール表示の表示単位
 */
const ScheduleDispUnit =
{
    /** 月度単位*/
    Month: 1,
    /** 年度単位*/
    Year: 2
}

/**
 * スケジュールのマークの種類
 */
const ScheduleStatus = {
    // 分かりやすくするためにコメントにマークを書いていますが、設定により変更し得るので、注意
    // 値はアイテム拡張項目の値で、表示優先順位とは別の値ですが、定義は初期仕様の表示優先順位と合わせています

    /** 保全履歴完了、●*/
    Complete: 5,
    /** 上位ランクが履歴完了済み、▲*/
    UpperComplete: 3,
    /** 計画作成済み、◎*/
    Created: 4,
    /** スケジュール済み、○*/
    NoCreate: 1,
    /** 上位ランクがスケジュール済み、△*/
    UpperScheduled: 2,
    /** なし、非表示*/
    NoSchedule: -1
}

/**
 * スケジュールのマークのCSS class名
 */
const ScheduleClass = {
    // 分かりやすくするためにコメントにマークを書いていますが、設定により変更し得るので、注意
    // 値はアイテム拡張項目の値で、表示優先順位とは別の値ですが、定義は初期仕様の表示優先順位と合わせています

    /** 保全履歴完了、●*/
    Complete: "sc-complete",
    /** 上位ランクが履歴完了済み、▲*/
    UpperComplete: "sc-upper-complete",
    /** 計画作成済み、◎*/
    Created: "sc-created",
    /** スケジュール済み、○*/
    NoCreate: "sc-no-create",
    /** 上位ランクがスケジュール済み、△*/
    UpperScheduled: "sc-upper-scheduled",
    /** なし、非表示*/
    NoSchedule: ""
}

/**
 * 移動可能なスケジュール種類配列
 */
const MovableScheduleStatus = [
    ScheduleStatus.Created, // 計画作成済み、◎
    ScheduleStatus.NoCreate // スケジュール済み、○
];

/** スケジュールのリンクの識別用のID */
const ScheduleLinkId = "ScheduleLink";

/** グローバル変数：スケジュール移動フラグ(true:移動中/false:移動中でない) */
var P_ScIsMoving = false;
/** グローバル変数：スケジュール移動元セル */
var P_ScSrcCell = null;
/** グローバル変数：スケジュール移動先セル */
var P_ScDstCell = null;
/** グローバル変数：スケジュール移動対象行番号 */
var P_ScTargetRowNo = -1;
/** グローバル変数：スケジュール移動マウスストーカー */
var P_ScStalker = null;

/** 保全実績評価から保全活動への遷移時の検索条件(職種)に設定する文字列 */
const JobAll = "All";

// 丸め処理区分
const RoundDivision =
{
    Round: 1,   // 四捨五入
    Ceiling: 2, // 切り上げ
    Floor: 3    // 切り捨て
}

// 変更管理 申請状況(拡張項目)
const ApplicationStatus =
{
    Making: "10",  // 申請データ作成中
    Request: "20", // 承認依頼中
    Return: "30",  // 差戻中
    Approved: "40" // 承認済み
}

// 変更管理 申請区分(拡張項目)
const ApplicationDivision =
{
    New: "10",    // 新規登録申請
    Update: "20", // 変更申請
    Delete: "30"  // 削除申請
}

// 申請区分に応じた背景色設定のCSSクラス名
const BackGroundStyleInfo =
{
    10: "hmApplicationDivisionNew",    // 新規登録申請
    20: "hmApplicationDivisionUpdate", // 変更申請
    30: "hmApplicationDivisionDelete"  // 削除申請
}

// 変更管理帳票出力機能
const HistoryOutputDivision = {
    HM0001: 1, //機器台帳
    HM0002: 2  //長期計画
}

// SQLで扱うことのできる日付(年)
const SqlYear =
{
    MaxYear: 9999, // 最大値
    MinYear: 1753  // 最小値
};

// 予備品一覧 入出庫履歴タブの表示年度を保持するためのキー名称
const DispYearKeyName =
{
    YearFrom: "YearFrom", // 表示年度(From)
    YearTo: "YearTo"      // 表示年度(To)
}

// スケジュールの○リンクから保全活動に遷移したとき、対象機器一覧に表示するデータを決めるもの
const TransParamForMA0001ByLink =
{
    KeyName : "TransParamForMA0001ByLink", // グローバルリストにパラメータを格納する際のキー名称
    DispAll: "0",                          // クリックされた○リンクと同一年月のスケジュールを全て表示する
    DispSelected: "1",                     // クリックされた○リンクのスケジュールのみを表示する
    MaintainanceScheduleDetailId: "VAL60", // クリックされた○リンクの保全スケジュール詳細ID
    //ScheduleCtrlNo: "VAL62"                // 確認メッセージを表示するかどうか判定するためのもの(スケジュールアイコンの種類)
    TabNoCtrlNo: "VAL62",                  // クリックされた○リンクのタブ番号
    ScheduleCtrlNo: "ScheduleType"         // 確認メッセージを表示するかどうか判定するためのもの(スケジュールアイコンの種類)
}


/**
 * 対象コントロールを取得し、返す。
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目、6:テキストエリア、7:パスワード
 * @param isModal モーダルのコントロールの場合True
 * @param tableHeader ヘッダーの値を取得する場合はtrue
 * 
 * @return 取得した値
 */
function getCtrl(ctrlId, val, rowNo, flg, isModal, tableHeader) {
    var trs;
    if (isModal) {
        trs = $(P_Article).find("#" + ctrlId + getAddFormNo() + "_edit");
    } else {
        trs = $(P_Article).find("#" + ctrlId + getAddFormNo());
    }
    if ($(trs).hasClass('vertical_tbl')) {
        // 縦方向一覧
        var tag = tableHeader ? "th" : "td";
        if (flg == CtrlFlag.TextBox) {
            return $(trs).find(tag + "[data-name='VAL" + val + "'] input[type='text']")[0];
        } else if (flg == CtrlFlag.Label) {
            return $(trs).find(tag + "[data-name='VAL" + val + "']")[0];
        } else if (flg == CtrlFlag.Combo) {
            return $(trs).find(tag + "[data-name='VAL" + val + "'] select")[0];
        } else if (flg == CtrlFlag.Link) {
            return $($(trs).find(tag + "[data-name='VAL" + val + "']")[0]).find("a")[0];
        } else if (flg == CtrlFlag.Input) {
            return $($(trs).find(tag + "[data-name='VAL" + val + "']")[0]).find("input")[0];
        } else if (flg == CtrlFlag.ChkBox) {
            return $($(trs).find(tag + "[data-name='VAL" + val + "']")[0]).find("input")[0];
        } else if (flg == CtrlFlag.Textarea) {
            return $($(trs).find(tag + "[data-name='VAL" + val + "']")[0]).find("textarea")[0];
        } else if (flg == CtrlFlag.Password) {
            return $(trs).find(tag + "[data-name='VAL" + val + "'] input[type='password']")[0];
        } else if (flg == CtrlFlag.Button) {
            return $(trs).find(tag + "[data-name='VAL" + val + "'] input[type='button']")[0];
        } else if (flg == CtrlFlag.Search) {
            return $(trs).find(tag + "[data-name='VAL" + val + "'] input[type='search']")[0];
        } else if (flg == CtrlFlag.MultiCheckBox) {
            return $(trs).find(tag + "[data-name='VAL" + val + "'] div.multisel-parent")[0];
        }

    } else {
        // 横方向一覧
        var tr;
        if ($(trs).data("ctrltype") == ctrlTypeDef.IchiranPtn3) {
            // Tabulatorの一覧の場合
            tr = $(trs).find("div.tabulator-table").find("div[tabulator-field='VAL" + val + "']")[rowNo];
        } else {
            // そうでない場合
            tr = $(trs).find("tbody tr:not([class^='base_tr'])").find("td[data-name='VAL" + val + "']")[rowNo];
        }
        // 子の要素を取得
        return getChildCtrlByFlg(tr, flg);
    }
}

/**
 * 要素の子より指定した種類の要素を取得する
 * @param {any} element 取得元の要素
 * @param {any} flg 取得する要素の種類、CtrlFlag
 */
function getChildCtrlByFlg(element, flg) {
    if (flg == CtrlFlag.TextBox) {
        return $(element).find("input[type='text']")[0];
    } else if (flg == CtrlFlag.Label) {
        return $(element)[0];
    } else if (flg == CtrlFlag.Combo) {
        return $(element).find("select")[0];
    } else if (flg == CtrlFlag.Link) {
        return $(element).find("a")[0];
    } else if (flg == CtrlFlag.Input) {
        return $(element).find("input")[0];
    } else if (flg == CtrlFlag.ChkBox) {
        return $(element).find("input")[0];
    } else if (flg == CtrlFlag.Textarea) {
        return $(element).find("textarea")[0];
    } else if (flg == CtrlFlag.Password) {
        return $(element).find("input[type='password']")[0];
    } else if (flg == CtrlFlag.Button) {
        return $(element).find("input[type='button']")[0];
    }
}

/**
 * 要素のきょうだい(同じ行の別の列)より指定した列の要素を取得する
 * @param {any} element 取得元の要素
 * @param {any} val 取得する列のVAL値
 * @param {any} flg 取得する列の要素の種類
 */
function getSiblingCtrl(element, val, flg) {
    var row = $(element).closest("div").siblings("div[tabulator-field='VAL" + val + "']");
    return getChildCtrlByFlg(row, flg);
}

/**
 * data-rownoにより対象コントロールを取得し、返す。
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param dataRowNo 行のdata-rowno
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目
 * @param isModal モーダルのコントロールの場合True
 * @return 取得した値
 */
function getCtrlDataRow(ctrlId, val, dataRowNo, flg, isModal) {
    var trs = $(P_Article).find("#" + ctrlId + getAddFormNo());
    if ($(trs).hasClass('vertical_tbl')) {
        return getCtrl(ctrlId, val, dataRowNo, flg, isModal);
    } else {
        var td = $(trs).find("input[data-rowno='" + dataRowNo + "']").closest(".tabulator-row").find("div[tabulator-field='VAL" + val + "']")
        return getChildCtrlByFlg(td, flg);
    }
}

/**
 * data-rownoにより対象値を取得し、返す。
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param dataRowNo 行のdata-rowno
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目
 * @param isModal モーダルのコントロールの場合True
 * @return 取得した値
 */
function getValueDataRow(ctrlId, val, dataRowNo, flg, isModal) {
    var target = getCtrlDataRow(ctrlId, val, dataRowNo, flg, isModal);
    return getValueByCtrl(target, flg);
}

/**
 * 他のフォームから対象コントロールを取得する。
 * @param formNo 画面NO
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス
 * @return 取得したコントロール
 */
function getCtrlByOtherForm(formNo, ctrlId, val, rowNo, flg) {
    var tmpArticle = $('article[name="main_area"][data-formno="' + formNo + '"]');
    if (tmpArticle.length < 0) {
        return;
    }
    if (tmpArticle.length > 1) {
        //※複数件見つかった場合、ﾍﾞｰｽﾌｫｰﾑを除外
        tmpArticle = $('article:not([class="base_form"])[name="main_area"][data-formno="' + formNo + '"]');
    }
    var trs = $(tmpArticle).find("#" + ctrlId + "_" + formNo);
    if ($(trs).hasClass('vertical_tbl')) {
        if (flg == CtrlFlag.TextBox) {
            return $(trs).find("td[data-name='VAL" + val + "'] input[type='text']")[0];
        } else if (flg == CtrlFlag.Label) {
            return $(trs).find("td[data-name='VAL" + val + "']")[0];
        } else if (flg == CtrlFlag.Combo) {
            return $(trs).find("td[data-name='VAL" + val + "'] select")[0];
        } else if (flg == CtrlFlag.ChkBox) {
            return $(trs).find("input[type='checkbox']")[0];
        }
    } else {
        //var tr = $(trs).find("tbody tr:not([class^='base_tr'])").find("td[data-name='VAL" + val + "']")[rowNo];
        var tr;
        if ($(trs).data("ctrltype") == ctrlTypeDef.IchiranPtn3) {
            // Tabulatorの一覧の場合
            tr = $(trs).find("div.tabulator-table").find("div[tabulator-field='VAL" + val + "']")[rowNo];
        } else {
            // そうでない場合
            tr = $(trs).find("tbody tr:not([class^='base_tr'])").find("td[data-name='VAL" + val + "']")[rowNo];
        }
        if (flg == CtrlFlag.TextBox) {
            return $(tr).find("input[type='text']")[0];
        } else if (flg == CtrlFlag.Label) {
            return $(tr)[0];
        } else if (flg == CtrlFlag.Combo) {
            return $(tr).find("select")[0];
        } else if (flg == CtrlFlag.ChkBox) {
            return $($(tr).find("input[type='checkbox']")[0]);
        }
    }
}

/**
 * 他のフォームから対象値を取得し、返す。
 * @param formNo 画面NO
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス
 * @return 取得した値
 */
function getValueByOtherForm(formNo, ctrlId, val, rowNo, flg) {
    var target = getCtrlByOtherForm(formNo, ctrlId, val, rowNo, flg);
    // 値を取得して返す
    return getValueByCtrl(target, flg);
}

/**
 * 要素とその種類から値を取得する処理
 * @param {element} target 要素,getCtrlで取得
 * @param {int} flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス
 */
function getValueByCtrl(target, flg) {
    if (!target) { return null; }
    if (flg == CtrlFlag.TextBox || flg == CtrlFlag.Search) {
        return target.value;
    } else if (flg == CtrlFlag.Label || flg == CtrlFlag.Link) {
        return target.innerText;
    } else if (flg == CtrlFlag.Combo) {
        var val = target.value;
        if (!val || val.length == 0) {
            val = $(target).data('value');
        }
        return val;
    } else if (flg == CtrlFlag.ChkBox) {
        //return target.prop("checked");
        return target.checked;
    } else if (flg == CtrlFlag.Textarea) {
        return target.value;
    } else if (flg == CtrlFlag.Password) {
        return target.value;
    }
}

/**
 * 対象値を取得し、返す。
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス
 * @param isModal モーダルのコントロールの場合True
 * @param tableHeader ヘッダーの値を取得する場合はtrue
 * @return 取得した値
 */
function getValue(ctrlId, val, rowNo, flg, isModal, tableHeader) {
    var target = getCtrl(ctrlId, val, rowNo, flg, isModal, tableHeader);
    return getValueByCtrl(target, flg);
}

/**
 * 指定要素の同じ行の指定列から対象値を取得し、返す
 * @param {any} element 対象値を取得する要素と同じ列の要素
 * @param {any} val 指定列のVAL値
 * @param {any} flg 0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス
 */
function getSiblingsValue(element, val, flg) {
    var target = getSiblingCtrl(element, val, flg);
    return getValueByCtrl(target, flg);
}

/**
 * 対象のラベル値を取得（フォーマット等が必要な項目はラベル値を取得すること）
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目、6:テキストエリア、7:パスワード
 * 
 * @return 取得した値
 */
function getItemLabelValue(ctrlId, val, rowNo, flg) {
    //要素取得
    var ele = getCtrl(ctrlId, val, rowNo, flg);
    //表示しているラベルの値を取得
    return $(ele).closest("td").find("span.labeling").text();
}

/**
 * 値を対象の場所に設定する
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目、6:テキストエリア、7:パスワード
 * @param value 設定値
 * @param isModal モーダルのコントロールの場合True
 * @param tableHeader ヘッダーの値を取得する場合はtrue
 * @return 取得した値
 */
function setValue(ctrlId, val, rowNo, flg, value, isModal, tableHeader) {
    var target = getCtrl(ctrlId, val, rowNo, flg, isModal, tableHeader);
    if (!target) { return; }

    if (flg == CtrlFlag.TextBox) {
        target.value = value;
    } else if (flg == CtrlFlag.Label) {
        target.innerText = value;
    } else if (flg == CtrlFlag.Combo) {
        target.value = value;
    } else if (flg == CtrlFlag.Input) {
        target.value = value;
    } else if (flg == CtrlFlag.ChkBox) {
        target.checked = value;
    } else if (flg == CtrlFlag.Textarea) {
        target.value = value;
    } else if (flg == CtrlFlag.Password) {
        target.value = value;
    } else if (flg == CtrlFlag.Link) {
        target.innerText = value;
    }
}

/**
 * 値を対象の場所に設定する
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目、6:テキストエリア、7:パスワード
 * @param value 設定値
 * @param isModal モーダルのコントロールの場合True
 * @param tableHeader ヘッダーの値を取得する場合はtrue
 * @return 取得した値
 */
function setValue2(ctrlId, val, rowNo, flg, value, isModal, tableHeader) {
    var target = getCtrl(ctrlId, val, rowNo, flg, isModal, tableHeader);

    if (!target) {
        return;
    }

    if (flg == CtrlFlag.TextBox) {
        target.value = value;
    } else if (flg == CtrlFlag.Label) {
        target.innerText = value;
    } else if (flg == CtrlFlag.Combo) {
        target.value = value;
    } else if (flg == CtrlFlag.Input) {
        target.value = value;
    } else if (flg == CtrlFlag.ChkBox) {
        target.checked = value;
    } else if (flg == CtrlFlag.Textarea) {
        target.value = value;
    } else if (flg == CtrlFlag.Password) {
        target.value = value;
    } else if (flg == CtrlFlag.Link) {
        target.innerText = value;
    }
}

/**
 * グローバルリストの値を追加・削除する
 * @param {any} key キー名称
 * @param {any} isDelete グローバルリストから削除する場合はtrue
 * @param {any} value グローバルリストに格納する値
 */
function operatePdicIndividual(key, isDelete, value) {

    // 値の追加・削除を判定
    if (isDelete) {
        // グローバルリストから指定されたキーを削除
        delete P_dicIndividual[key];
    } else {
        // グローバルリストの指定されたキーに値を格納
        P_dicIndividual[key] = value;
    }
}

/*
* 項目に値をセットして、その項目の変更時イベントを発生させる
* apcommon.jsのsetValueの拡張
*/
function setValueAndTrigger(ctrlId, val, rowNo, flg, value, isModal, isHeader) {
    setValue(ctrlId, val, rowNo, flg, value, isModal, isHeader);
    // 変更時イベントを発生させる
    $(getCtrl(ctrlId, val, rowNo, flg, isModal)).trigger('change');
}

/*
* 項目に値をセットして、その項目の変更時イベントを発生させる(変更フラグ無し)
* apcommon.jsのsetValueの拡張
*/
function setValueAndTriggerNoChange(ctrlId, val, rowNo, flg, value, isModal) {
    setValue(ctrlId, val, rowNo, flg, value, isModal);
    // 変更時イベントを発生させる
    changeNoEdit(getCtrl(ctrlId, val, rowNo, flg, isModal));
}

/*
* HTMLのIDで、ctrlIdに付与されるformNoを取得する
*/
function getAddFormNo() {
    return '_' + getFormNo();
}

/**
 * 画面の要素からformNoを取得する
 * @param {html} elm 画面の要素
 * @returns {int} フォーム番号
 */
function getFormNoByElement(elm) {
    return $(elm.closest("article")).find("input[name='FORMNO']").val();
}

/**
 * 画面の要素(dataset)からformNoを取得する
 * @param {html} elm 画面の要素
 * @returns {int} フォーム番号
 */
function getFormNoByDataSet(elm) {
    return elm.closest("article").dataset.formno;
}

/**
 * 画面の要素から機能IDを取得する
 * @param {any} elm 画面の要素
 * @return {string} PGMID
 */
function getProgramIdByElement(elm) {
    return $(elm.closest("article")).find("input[name='PGMID']").val();
}

/**
 * フォームの要素から指定したオプションを取得する
 * @param {any} elm フォームの要素
 * @param {string} name PGMID、CONDUCTIDなどフォームのinput要素の名称
 * @return {string} 取得した要素の値
 */
function getArticleInfoByElm(elm, name) {
    return $(elm.closest("article")).find("input[name='" + name + "']").val();
}

/**
 * オートコンプリートが初回遷移時かユーザ入力時なのか判定フラグ 初期化
 */
function deleteInitTransFlg() {
    var target = $("[data-transinit='1']");
    if ($(target).length) {
        $(target).removeAttr("data-transinit");
    }
}

/**
 * オートコンプリートが初回遷移時かユーザ入力時なのか判定
 * true：初回遷移時、false：ユーザ入力時
 */
function checkInitTransFlg(select) {
    if ($(select).attr("data-transinit") != "1") {
        // 新規行、行コピー時は、falseを返す
        if ($(select).closest("tr").data("rowstatus") == dataTypeDef.New) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }
}

/*
 * カンマを除去し、数値に変換する
*/
function removeComma(value) {
    var rtnValue;
    var qty = value.replace(/,/g, "");
    if (!isNaN(qty)) {
        //数量のカンマを取りフロートへ変換
        rtnValue = parseFloat(qty);
    }
    else {
        rtnValue = qty;
    }
    return rtnValue;
}

/**
 * 対象コントロールにフォーカスをセットする。
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目
 * @param isModal モーダルのコントロールの場合True 
 */
function setFocus(ctrlId, val, rowNo, flg, isModal) {
    var target = getCtrl(ctrlId, val, rowNo, flg, isModal);

    $(target).focus();
}

/**
 * 対象コントロールにフォーカスをセットする。共通画面用に、遅延してセット。
 * @param ctrlId コントロールID
 * @param val VAL値
 * @param rowNo 行番号
 * @param flg=0:テキストボックス、1:ラベル、2:コンボボックス、3:チェックボックス、4:リンク、5:入力項目
 * @param isModal モーダルのコントロールの場合True
 */
function setFocusDelay(ctrlId, val, rowNo, flg, isModal) {
    // 引数で与えられたミリ秒後、フォーカスをセット
    // 共通画面では、すぐに画面が表示されないため、表示されるまで待ってセットする
    setTimeout(function () {
        setFocus(ctrlId, val, rowNo, flg, isModal);
    }, 300); //300ミリ秒
}

/*
* 共通画面で、呼出元のFormNoを取得
* @return {string} FormNo
*/
function getParentFormNoCommon() {
    // Common.jsの10863行目より
    return $(P_Article).find("form[id^='formDetail'] input:hidden[name='ORIGINNO']").val();
}


/*
* 編集中フラグを変更せずに、項目のchangeイベントを発生させる処理
* @element イベントを発生させる画面の項目
*/
function changeNoEdit(element) {
    // 現在の編集中フラグを退避
    var tempFlg = dataEditedFlg;
    // changeイベント
    $(element).trigger('change');
    // 編集中フラグを退避した元の値に戻す
    dataEditedFlg = tempFlg;
}

/*
* 日時をyyyy/MM/dd HH:MMの文字列に変換する
* @date 変換する日時{date}
* @return {string} yyyy/MM/dd HH:mm の文字列
*/
function formatDateToYMDHM(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minutes = date.getMinutes();

    return year + "/" + zeroPadding(month, 2) + "/" + zeroPadding(day, 2) + " " + zeroPadding(hour, 2) + ":" + zeroPadding(minutes, 2);
}

/*
* 日時をyyyy/MM/ddの文字列に変換する
* @date 変換する日時{date}
* @return {string} yyyy/MM/ddの文字列
*/
function formatDateToYMD(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();

    return year + "/" + zeroPadding(month, 2) + "/" + zeroPadding(day, 2);
}

/**
 * 入力制御(活性/非活性)を行う。要素を読み取り専用に変更する。
 * @param elm  制御対象
 * @param flg true:活性/false:非活性(読み取り専用に変更)
 */
function changeInputControl(elm, flg) {
    if (!flg) {
        setAttrByNativeJs(elm, 'disabled', true);
    }
    else {
        $(elm).removeAttr('disabled');
    }
}


/**
 * 入力制御(活性/非活性)を行う。要素をラベルに変更する。
 * @param elm  制御対象
 * @param flg true:活性/false:非活性(ラベルに変更)
 */
function changeInputReadOnly(elm, flg) {
    if (!flg) {
        $($(elm).closest("td")).addClass('readonly');
    }
    else {
        $($(elm).closest("td")).removeClass('readonly');
    }
}

/**
 * 入力制御(活性/非活性)を行う。要素をラベルに変更する。数量など、右寄せにする項目ではこちらを使用すること。
 * @param elm  制御対象
 * @param flg true:活性/false:非活性(ラベルに変更)
 */
function changeInputReadOnlyRight(elm, flg) {
    if (!flg) {
        $($(elm).closest("td")).addClass('readonly').addClass('right');
    }
    else {
        $($(elm).closest("td")).removeClass('readonly').addClass('right');
    }
}

/**
 * 入力制御(活性/非活性)を行う。要素をラベルに変更する。左右を指定する場合はこちらを使用。
 * @param elm  制御対象
 * @param isVisible true:活性/false:非活性(ラベルに変更)
 * @param isRight true:右寄せ/false:そのまま
 */
function changeInputReadOnlyLR(elm, isVisible, isRight) {
    if (isRight) {
        changeInputReadOnlyRight(elm, isVisible);
    } else {
        changeInputReadOnly(elm, isVisible);
    }
}

/**
 * 指定要素の表示/非表示を切り替える
 * @param {any} elm 表示を切り替える要素
 * @param {any} isDisplay 表示する場合True
 */
function changeCtrlDisplay(elm, isDisplay) {
    // isDisplayで表示を切り替えるのにshowとhideをいちいち書くのが煩雑なので作成
    if (isDisplay) {
        // 表示
        elm.show();
    } else {
        // 非表示
        elm.hide();
    }
}

/**
 * 一覧の表示状態を切り替える
 * @param {any} ctrlId 一覧のコントロールID
 * @param {any} isDisplay 表示する場合True
 * @param {any} isModal モーダルの場合True(省略時False)
 */
function changeListDisplay(ctrlId, isDisplay, isModal) {
    // セレクタの文言
    var selector = "#" + ctrlId + getAddFormNo();
    if (isModal) {
        // モーダルの場合追加
        selector = selector + "_edit";
    }
    // 一覧取得、行追加なども一緒に非表示なので親要素
    var list = $(P_Article).find(selector + "_div");
    // 表示状態切り替え
    changeCtrlDisplay(list, isDisplay);
}

/*
* 一覧の列の表示/非表示切替を行う
* @param ctrlId 一覧のコントロールID
* @param valNo VALの値
* @param isDisplay 表示する場合True
*/
var changeColumnDisplay = function (ctrlId, valNo, isDisplay) {
    var column = $("#" + ctrlId + getAddFormNo()).find("*[data-name='VAL" + valNo + "']");
    // 表示状態切り替え
    changeCtrlDisplay(column, isDisplay);
}

/*
* Tabulator一覧の列の表示/非表示切替を行う
* @param ctrlId 一覧のコントロールID
* @param valNo VALの値
* @param isDisplay 表示する場合True
*/
function changeTabulatorColumnDisplay(ctrlId, valNo, isDisplay) {
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (table) {
        if (isDisplay) {
            table.showColumn("VAL" + valNo);
        } else {
            table.hideColumn("VAL" + valNo);
        }
        table.redraw();
        table = null;
    }
}

/*
* 一覧の行追加などと選択列の表示/非表示切替を行う
* @param ctrlId 一覧のコントロールID
* @param isDisplay 表示する場合True
*/
function changeRowControl(ctrlId, isDisplay) {
    // 行追加など
    var items = [actionkbn.AddNewRow, actionkbn.DeleteRow, actionkbn.SelectAll, actionkbn.CancelAll];
    $.each(items, function (i, item) {
        var target = $("#" + ctrlId + getAddFormNo() + "_div").find('[data-actionkbn=' + item + ']');
        setHide(target, !isDisplay);
    });

    // 選択列の表示制御
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (isDisplay) {
        table.showColumn("SELTAG");
    } else {
        table.hideColumn("SELTAG");
    }
    table.redraw();
    table = null;
}

/*
* 一覧の行追加などと選択列の表示/非表示切替を行う
* @param ctrlId 一覧のコントロールID
* @param isDisplay 表示する場合True
*/
function changeRowControlAndDispRowNo(ctrlId, isDisplay) {
    // 行追加など
    var items = [actionkbn.AddNewRow, actionkbn.DeleteRow, actionkbn.SelectAll, actionkbn.CancelAll];
    $.each(items, function (i, item) {
        var target = $("#" + ctrlId + getAddFormNo() + "_div").find('[data-actionkbn=' + item + ']');
        setHide(target, !isDisplay);
    });

    // 選択列の表示制御
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (isDisplay) {
        table.showColumn("ROWNO");
    } else {
        table.hideColumn("ROWNO");
    }
    table.redraw();
    table = null;
}

/*
 * 列の表示/非表示 切替処理
 * @param　ctrlId　一覧のコントロールID
 * @param　valNo　VALの値
 * @param　isDisplay　表示する場合True
 */
function changeDispColumn(ctrlId, valNo, isDisplay) {
    // 対象コントロールIDの列を取得
    var column = $("#" + ctrlId).find("*[data-name='VAL" + valNo + "']");
    if (isDisplay) {
        // 列を表示
        column.show();
    } else {
        // 列を非表示
        column.hide();
    }
}

/*
* 一覧の鉛筆リンクの表示/非表示を切り替える処理
* @param ctrlId コントロールID
* @param rowNo 行番号
* @param isDisplay 表示する場合True
*/
function changePencilLink(ctrlId, rowNo, isDisplay) {
    var pencilLink = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("tbody tr:not([class^='base_tr'])").find("td[data-name='ROWNO']")[rowNo];
    if (isDisplay) {
        $(pencilLink).removeClass("ws_pre_wrap");
    } else {
        $(pencilLink).addClass("ws_pre_wrap");
    }
    setHide($(pencilLink).find("a"), !isDisplay);
}


/*
* ボタン名でボタンを取得する
* @param name ボタン名称
* @return ボタンのコントロール
*/
function getButtonCtrl(name) {
    return $(P_Article).find("input[type='button'][name='" + name + "']");
}

/*
* 名称でファイル選択コントロールを取得する
* @param name ファイル選択のコントロール名称
* @return ファイル選択のコントロール
*/
function getFileCtrl(name) {
    return $(P_Article).find("input[type='file'][name='" + name + "']");
}

/*
* ボタン名を指定して、フォーカスをセットする
* @param name ボタン名称
*/
function setFocusButton(name) {
    $(getButtonCtrl(name)).focus();
}

/**
 * ボタン名を二つ指定して、可能なボタンにフォーカスをセットする
 * @param {any} priBtn 優先ボタン名
 * @param {any} secBtn 優先ボタンが押下不能な場合にセットするボタン名
 */
function setFocusButtonAvailable(priBtn, secBtn) {
    var isPri = !isUnAvailableButton(priBtn);
    setFocusButton(isPri ? priBtn : secBtn);
}

/*
* ボタン名を指定して、フォーカスをセットする(変更管理の詳細画面用)
* @param processMode 処理モード
*/
function setFocusButtonHistory(processMode) {
    //フォーカス設定するボタン
    var focusBtn = HistoryFormDetailCommonButton.CopyRequest; //複写申請
    if (ProcessMode.History == processMode) {
        //変更管理モード

        //ボタン押下可能なボタンを設定（承認依頼、承認依頼引戻、承認、戻る）
        var btnList = [HistoryFormDetailCommonButton.ChangeApplicationRequest, HistoryFormDetailCommonButton.PullBackRequest, HistoryFormDetailCommonButton.ChangeApplicationApproval, HistoryFormDetailCommonButton.Back];
        $.each(btnList, function (i, btn) {
            if (!isUnAvailableButton(btn)) {
                //押下可能であれば設定
                focusBtn = btn;
                return false;
            }
        });
    }
    // 押下可能なボタンにフォーカスをセット
    setFocusButton(focusBtn);
}

/**
 *  指定要素が非活性か判定する。
 *  @param element   {string} ：<div>要素指定文字列
 *  @return 非活性の場合True
 */
function isDisabled(element) {

    if ($(element).prop('disabled')) {
        // 非活性の場合
        return true;
    } else {
        // そうでない場合
        return false;
    }
}

/*
* 指定されたリンクをラベルに変更する
* @param link aタグの要素
*/
function setLinkToLabel(link) {
    $(link).addClass("link-to-label");
    $(link).prop("tabindex", -1);
    $(link).parent().removeClass("transLink");
}


/*
* ボタンが押下不能な状態ならTrueを返す処理
* @btnName ボタン名称 getButtonCtrlの引数
* @return ボタンが押下不能(非表示、非活性、権限なし)
*/
function isUnAvailableButton(btnName) {
    // ボタンを取得
    var button = getButtonCtrl(btnName);
    var td = getButtonCtrl(btnName).parent('td');
    // ボタンの表示状態を取得
    // ボタンが非表示ならTrue
    var isHideButton = isHide(td);
    // ボタンが非活性ならTrue
    var isDisabledButton = isDisabled(button);
    //　ボタンが画面より取得できないならTrue
    var isNoLengthButton = td.length == 0;
    // いずれかがTrueなら押下不能としてTrue
    return isNoLengthButton || isHideButton || isDisabledButton;
}

/*
* コード+翻訳の項目のラベルよりコードを取得する処理
* @value 取得する項目の値
* @return コードの値、取得不能な場合Null
*/
function getCodeFromCodeTrans(value) {
    var separator = value.indexOf(":");
    if (separator < 0) {
        return null;
    }
    var code = value.slice(0, separator);
    return code;
}


/**
 * グループ化した要素の表示/非表示を切り替える
 * @param groupNo グループ番号
 * @param flg true(非表示)、false(表示)
 */
function toggleHideGroup(groupNo, flg) {
    //グループのタイトル要素
    toggleHideGroupTitle(groupNo, flg);
    //グループ要素
    setHide(getGroupId(groupNo), flg);
}

/**
 * グループ化した要素のタイトルの表示/非表示を切り替える
 * @param groupNo グループ番号
 * @param flg true(非表示)、false(表示)
 */
function toggleHideGroupTitle(groupNo, flg) {
    var id = "grp_id_detail" + groupNo;
    var ele = $(P_Article).find('[data-switchid="' + id + '"]');
    if (ele) {
        //グループのタイトル要素
        var titleEle = $(ele).closest('div.tbl_title');
        setHide(titleEle, flg);
    }
}

/**
 * グループ化した要素のIDを取得
 * @param groupNo グループ番号
 */
function getGroupId(groupNo) {
    var id = "#grp_id_detail" + groupNo + getAddFormNo();
    return id;
}

/**
 * 明細がチェックされているか確認し、未チェックの場合メッセージを表示しFalseを返す
 * @param {string} ctrlId 一覧の項目ID
 * @param {string} message メッセージ
 * @return 未チェックの場合False、いずれかがチェックされた場合True
 */
function isCheckedList(ctrlId, message) {
    // メッセージをクリア
    clearMessage();
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (table) {
        var trs = table.getRows();
        var strMessage = message != null && message != "" ? message : P_ComMsgTranslated[941160003]; // 「対象行が選択されていません。」
        if (trs != null && trs.length > 0) {
            // 検索結果一覧を参照し、更新対象が選択されているかどうかをチェック
            $(trs).each(function (i, tr) {
                if (tr.getData().SELTAG == 1) {
                    strMessage = "";
                    return false;
                }
            });
        }
        if (strMessage != null && strMessage != "") {
            // エラーメッセージを設定する
            setMessage(strMessage, procStatus.Error);
            table = null;
            return false;
        }
        table = null;
    }
    return true;
}

/**
 * 明細がチェックされているか確認し、複数選択されている場合メッセージを表示しFalseを返す
 * @param {string} ctrlId 一覧の項目ID
 * @param {string} message メッセージ
 * @return 複数チェックの場合False、未チェックまたは1件チェックされた場合True
 */
function isMultipleCheckedList(ctrlId, message) {
    // メッセージをクリア
    clearMessage();
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (table) {
        var trs = table.getRows();
        var strMessage = message;
        var checkCount = 0;
        if (trs != null && trs.length > 0) {
            // 検索結果一覧を参照し、更新対象が選択されているかどうかをチェック
            $(trs).each(function (i, tr) {
                var ele = tr.getElement();
                if ($($(ele).find("div[tabulator-field='SELTAG'] input[type='checkbox']")[0]).prop('checked')) {
                    checkCount++;
                }
            });
        }
        if (checkCount > 1) {
            // エラーメッセージを設定する
            setMessage(strMessage, procStatus.Error);
            table = null;
            return false;
        }
        table = null;
    }
    return true;
}

/**
 * Ajax通信共通処理
 * @param {string} controlId 処理のコントロールID(ExecuteImplで分岐する処理名)
 * @param {string} appPath 呼出元処理の引数を渡す
 * @param {string} formNo フォーム番号
 * @param {string} conductId 機能ID
 * @param {bool} isPopUp ポップアップ画面の場合True
 * @param {Dictionary} postデータ(listData)に追加するデータ
 * @param {function} eventFunc 正常終了時の処理
 * @return {bool} エラー発生時はTrue
 */
function ajaxCommon(controlId, appPath, formNo, conductId, isPopUp, data, eventFunc) {
    // 戻り値　エラー発生時はTrue
    var isError = false;

    listData = getListDataAll(formNo, 0); // 画面情報を取得
    if (data) {
        listData = listData.concat(data);
    }

    // サーバにアクセスを行う
    // POSTデータを生成
    var postdata = {
        conductId: conductId, // メニューの機能ID
        pgmid: conductId,     // メニューのプログラムID
        formNo: formNo,       // フォーム番号
        ctrlId: controlId, // コントロールID
        listDefines: P_listDefines,             // 一覧定義情報
        listData: listData,   // 条件データ
        browserTabNo: P_BrowserTabNo,   // ブラウザタブ識別番号
        ListIndividual: P_dicIndividual, // グローバルリスト
    };

    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.Execute, // 実行
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false,
        async: false // 同期処理に
    }).then(
        function (resultInfo) {
            // 正常終了
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"

            eventFunc(status, data);
        },
        function (resultInfo) {
            // 異常終了

            // 2つ目は通信時のコールバック
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                clickRegistBtnConfirmOK(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

            isError = true;
        }
    ).always(
        // 通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            if (data) {
                data = null;
            }
            return isError;
        }
    );
}

/**
 * 要素の表示状態を切り替える
 * @param {any} element 要素
 * @param {any} flg true(非活性)、false(活性)
 */
function setDispMode(element, flg) {
    //要素の活性・非活性を切り替える
    setDisableBtn(element, flg);

    //要素の表示・非表示を切り替える
    //setHide(element, flg);
}

/**
 * 行削除共通処理
 * preDeleteRowで呼出、この処理の戻り値をreturnする
 * @param {any} targetId メソッドの引数のid 処理対象の一覧のコントロールID
 * @param {any} arrListIds 行削除処理を行う対象の一覧のコントロールIDの配列
 */
function preDeleteRowCommon(targetId, arrListIds) {
    // 削除を行っている一覧が引数に合致する場合True
    var isMatch = false;
    // 合致するか確認
    $.each(arrListIds, function (index, value) {
        // FormNoを付与
        if (targetId == value + getAddFormNo()) {
            // 合致する場合
            isMatch = true;
            return false;
        }
    })

    if (isMatch) {
        // 削除ボタン(非表示)をクリック
        var button = $("#" + targetId).closest("form").find("input[type='button'][name='Delete']");
        $(button).click();
        // 元々の行削除処理はキャンセル
        return false;
    }
    // 合致しない場合は通常の処理
    return true;
}

/**
 * 行削除共通処理
 * preDeleteRowで呼出、この処理の戻り値をreturnする
 * @param {any} targetId メソッドの引数のid 処理対象の一覧のコントロールID
 * @param {any} arrListIds 行削除処理を行う対象の一覧のコントロールIDの配列
 * @param {any} btnName クリックされたボタン名
 */
function preDeleteRowCommonDesignateBtn(targetId, arrListIds, btnName) {
    // 削除を行っている一覧が引数に合致する場合True
    var isMatch = false;
    // 合致するか確認
    $.each(arrListIds, function (index, value) {
        // FormNoを付与
        if (targetId == value + getAddFormNo()) {
            // 合致する場合
            isMatch = true;
            return false;
        }
    })

    if (isMatch) {
        // 削除ボタン(非表示)をクリック
        var button = $("#" + targetId).closest("form").find("input[type='button'][name='" + btnName + "']")
        $(button).click();
        // 元々の行削除処理はキャンセル
        return false;
    }
    // 合致しない場合は通常の処理
    return true;
}

/**
 * ファイル選択共通処理
 * preRegistで呼出、この処理の戻り値をreturnする
 * @param {any} targetId メソッドの引数のid 処理対象の一覧のコントロールID
 * @param {any} arrListIds 行削除処理を行う対象の一覧のコントロールIDの配列
 */
function preSelectFileCommon(targetId, arrListIds, fileCtrl) {
    // クリックイベントを行う一覧が引数に合致する場合True
    var isMatch = false;
    // 合致するか確認
    $.each(arrListIds, function (index, value) {
        // FormNoを付与
        var a = getAddFormNo()
        var b = value + getAddFormNo()
        if (targetId == value) {
            // 合致する場合
            isMatch = true;
            return false;
        }
    })

    if (isMatch) {
        // ファイル選択コントロールをクリック
        var selectFile = getFileCtrl(fileCtrl);
        $(selectFile).click();

        return false;
    }
    // 合致しない場合は通常の処理
    return true;
}

/**
 * ボタンコントロールIDを指定して表示/非表示を切り替える
 * @param {any} name ボタンのコントロール
 * @param {any} flg true(非表示)、false(表示)
 */
function setHideButton(name, flg) {
    //ボタン要素取得
    var button = getButtonCtrl(name);
    //ボタン要素の親（td）
    var ele = $(button).parent();
    setHide(ele, flg);
}

/**
 * ボタンコントロールIDを指定して表示/非表示を切り替える
 * @param {any} name ボタンのコントロール
 * @param {any} flg true(非表示)、false(表示)
 */
function setHideButtonExcludeModal(name, flg) {
    //ボタン要素取得（モーダル画面の閉じるボタンは対象外とする）
    var button = $(P_Article).find("input[type='button'][name='" + name + "']:not([data-dismiss='modal'])");
    //ボタン要素の親（td）
    var ele = $(button).parent();
    setHide(ele, flg);
}

/**
 * 一覧の上部にあるボタン（行追加、行削除、全選択、全解除等）の表示/非表示を切り替える
 * @param {any} ctrlId 一覧のコントロールID
 * @param {any} buttonType ボタンのアクション区分（actionkbn）
 * @param {any} flg true(非表示)、false(表示)
 */
function setHideButtonTopForList(ctrlId, buttonType, flg) {
    var ele = $(P_Article).find('a[data-actionkbn="' + buttonType + '"][data-parentid="' + ctrlId + '"]');
    setHide(ele, flg);
}

/*
* 必須項目の「＊」をセットする
* @param elm セットする項目
* @param isSet true:セットする/false:セットされたのを外す
*/
function setRequiredElement(elm, isSet) {
    if (isSet) {
        elm.addClass('validate_required ');
    } else {
        elm.removeClass('validate_required ');
    }
}

/*
* 引当時要素に対して着色(水色)を行うメソッド
* @param elm セットする項目
* @param isSet true:セットする/false:セットされたのを外す
*/
function setHikiate(elm, isSet) {
    if (isSet) {
        $(elm).addClass('hikiate');
    } else {
        $(elm).removeClass('hikiate');
    }
}

/**
 * 文字列の真偽値を真偽値に変換する処理
 * @param {str} str 変換する真偽値の文字列
 */
function convertStrToBool(str) {
    // TRUE,true,1 →true
    // FALSE,false,0→false

    if (typeof str != 'string') {
        // 文字列以外なら真偽値に変換
        return Boolean(str);
    }
    try {
        // JSONに変換して値を判定
        var obj = JSON.parse(str.toLowerCase());
        return obj == true;
    } catch (e) {
        return str != '';
    }
}

/**
 * 添付情報詳細画面への遷移パラメータを作成する
 * @param {string} functionTypeId 機能タイプID
 * @param {string} keyId キーID
 * @returns {Dictionary} 画面の遷移情報、リストへpushする
 */
function getParamToDM0002(functionTypeId, keyId) {
    var conditionData = {};
    conditionData['CTRLID'] = DM0001_List_CtrlId;
    conditionData['FORMNO'] = 1;
    conditionData['VAL1'] = functionTypeId;
    conditionData['VAL25'] = keyId;
    return conditionData;
}

// 予備品一覧から入庫・出庫・移庫 入力画面に遷移する際の遷移フラグ
const PartsTransFlg = {
    New: 0,　　　      //新規
    Edit: 1,　　       //修正
    EditDetail: 2,     // 予備品の詳細画面から呼び出す場合
    Reference: 3,　　  //参照
}

// 移庫入力画面遷移タイプ
const TransType =
{
    Location: 2,  // 棚番
    Department: 3 // 部門
}

/**
 * 棚卸(受払履歴)への遷移パラメータを作成する
 * @param {string} functionTypeId 機能タイプID
 * @param {string} keyId キーID
 * @returns {Dictionary} 画面の遷移情報、リストへpushする
 */
function getParamToPT0003_1(Date) {
    var conditionData = {};
    conditionData['CTRLID'] = PT0003_ConditionInfo;
    conditionData['FORMNO'] = 1;
    conditionData['VAL1'] = Date;
    return conditionData;
}

/**
 * 入庫入力画面への遷移パラメータを作成する
 * @param {any} PartsId 予備品ID
 * @param {any} HistoryId 受払履歴ID
 * @param {any} Flg フラグ(0:新規、1：修正)
 */
function getParamToPT0005(PartsId, HistoryId, Flg) {
    var conditionData = {};
    conditionData['CTRLID'] = PT0001_List_CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL38'] = PartsId;
    conditionData['VAL40'] = HistoryId;
    conditionData['VAL41'] = Flg;
    return conditionData;
}
/**
 * 出庫入力画面への遷移パラメータを作成する
 * @param {string} PartsId  予備品ID
 * @param {string} InoutHistoryId  受払履歴ID
 * @param {const} Flg       起動モード(新規/編集)
 * @returns {Dictionary} 画面の遷移情報、リストへpushする
 */
function getParamToPT0006(PartsId, InoutHistoryId, Flg) {
    var conditionData = {};
    conditionData['CTRLID'] = PT0001_List_CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL38'] = PartsId;
    conditionData['VAL40'] = InoutHistoryId;
    conditionData['VAL41'] = Flg;
    return conditionData;
}

/**
 * 移庫入力画面への遷移パラメータを作成する
 * @param {any} CtrlId 一覧のコントロールID
 * @param {any} PartsId 予備品ID
 * @param {any} workNo 作業No.
 */
function getParamToPT0007(CtrlId, PartsId, workNo) {
    var conditionData = {};
    conditionData['CTRLID'] = CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL19'] = PartsId;
    conditionData['VAL20'] = workNo;
    return conditionData;
}

/**
 * 予備品一覧から移庫入力画面への遷移パラメータを作成する
 * @param {any} CtrlId 一覧のコントロールID
 * @param {any} PartsId 予備品ID
 */
function getParamToPT0007FromPT0001(CtrlId, PartsId) {
    var conditionData = {};
    conditionData['CTRLID'] = CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL38'] = PartsId;
    return conditionData;
}

/**
 * 機器台帳(MC0001)の詳細画面から、機器台帳変更管理 詳細画面への遷移パラメータを作成する
 * @param {any} MachineId           機番ID
 */
function getParamToHM0001FormDetail(MachineId) {
    var conditionData = {};
    conditionData['CTRLID'] = HM0001_List_CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL40'] = 0;         // 変更管理ID
    conditionData['VAL42'] = MachineId; // 機番ID
    return conditionData;
}

/**
 * 件名別長期計画(LN0001)の詳細画面から、長期計画変更管理 詳細画面への遷移パラメータを作成する
 * @param {any} LongPlanId           長期計画ID
 */
function getParamToHM0002FormDetail(LongPlanId) {
    var conditionData = {};
    conditionData['CTRLID'] = HM0002_List_CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL53'] = 0;         // 変更管理ID
    conditionData['VAL51'] = LongPlanId; // 長期計画ID
    return conditionData;
}

/**
 * 申請状況変更画面への遷移パラメータを作成する
 * @param {any} flg 承認依頼の場合true、否認の場合false
 * @param {any} HistoryManagementId 変更管理ID
 */
function getParamToHM0003(flg, HistoryManagementId) {
    var conditionDataList = [];
    var conditionData = {};
    conditionData['CTRLID'] = HM0003_List_CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL3'] = flg; // 承認依頼の場合true、否認の場合false
    conditionData['VAL4'] = HistoryManagementId; // 変更管理ID
    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * 変更管理帳票出力画面への遷移パラメータを作成する
 * @param {any} conductId 機能ID
 */
function getParamToHM0004(conductId) {
    //帳票区分（出力機能）
    var conductCode = conductId == ConductId_HM0001 ? HistoryOutputDivision.HM0001 : HistoryOutputDivision.HM0002;

    var conditionDataList = [];
    var conditionData = {};
    conditionData['CTRLID'] = HM0004_List_CtrlId;
    conditionData['FORMNO'] = 0;
    conditionData['VAL1'] = conductCode; // 帳票区分
    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * URL直接起動のパラメータを設定する
 * @param {any} conductId 機能ID
 * @param {any} conditionDataList 遷移情報リスト
 */
function getParamByUrl(conductId, conditionDataList) {

    // 遷移先キーの存在チェック
    var conditionData = conditionDataList.filter(function (val, idx) {
        return 'KEY' in val;
    });

    // 遷移先キーが存在しない場合は何もしない
    if (conditionData == null || conditionData.length <= 0) {
        return conditionDataList;
    }

    // キー情報を取得
    var key = conditionData[0].KEY;

    // 機能IDを判定
    if (conductId == MC0001_ConductId) {
        // 機器台帳
        var machineId = key[0]; // 機番ID
        var tabNo = key[1];     // タブ番号
        conditionDataList.push(getParamToMC0001(machineId, tabNo)[0]);
    }
    else if (conductId == LN0001_ConductId) {
        // 件名別長期計画
        var longPlanId = key[0]; // 長計件名ID
        conditionDataList.push(getParamToLN0001(longPlanId)[0]);
    }
    else if (conductId == PT0001_ConductId) {
        // 予備品一覧
        var partsId = key[0]; // 予備品ID
        conditionDataList.push(getParamToPT0001(partsId)[0]);
    }

    return conditionDataList;
}



/**
 * 共通画面を閉じた際、指定された画面・ボタンの場合、呼出元画面で再検索を行うための処理
 * 【オーバーライド用関数】画面再表示ﾃﾞｰﾀ取得関数呼出前（beforeCallInitFormData）で呼び出す
 * 引数は全てbeforeCallInitFormDataの引数と同一
 * @param {any} appPath        beforeCallInitFormDataの引数
 * @param {any} conductId      beforeCallInitFormDataの引数
 * @param {any} pgmId          beforeCallInitFormDataの引数
 * @param {any} formNo         beforeCallInitFormDataの引数
 * @param {any} originNo       beforeCallInitFormDataの引数
 * @param {any} btnCtrlId      beforeCallInitFormDataの引数
 * @param {any} conductPtn     beforeCallInitFormDataの引数
 * @param {any} selectData     beforeCallInitFormDataの引数
 * @param {any} targetCtrlId   beforeCallInitFormDataの引数
 * @param {any} listData       beforeCallInitFormDataの引数
 * @param {any} skipGetData    beforeCallInitFormDataの引数
 * @param {any} status         beforeCallInitFormDataの引数
 * @param {any} selFlg         beforeCallInitFormDataの引数
 * @param {any} backFrom       beforeCallInitFormDataの引数
 */
function InitFormDataByCommonModal(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom) {
    // 再取得フラグがONの場合は再検索を行わない
    if (skipGetData == skipGetDataDef.GetData) {
        return;
    }
    // 対象の共通画面リスト
    //var listCommonConductId = [DM0002_ConductId, PT0005_ConsuctId, PT0006_ConsuctId, PT0007_ConsuctId, ConductId_HM0003];
    var listCommonConductId = [PT0005_ConsuctId, PT0006_ConsuctId, PT0007_ConsuctId, ConductId_HM0003];
    if (listCommonConductId.indexOf(backFrom) > -1 && btnCtrlId == BtnCtrlId_Back) {
        // 対象の共通画面から戻るボタンが押下された場合、再検索
        initFormData(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, selectData, listData, status);
    }
}

/**
 * 自身の単票画面を指定したボタン押下で閉じたときに再検索を行う処理
 * appPath以降は全てbeforeCallInitFormDataの引数と同一
 * @param {any} myConductId    自身の機能ID
 * @param {any} myFormNo       単票画面の一覧のFormNo
 * @param {any} myTargetCtrlId 単票画面の一覧のCtrlId
 * @param {any} myBtnCtrlId    単票画面のボタンのID
 * @param {any} appPath        beforeCallInitFormDataの引数
 * @param {any} conductId      beforeCallInitFormDataの引数
 * @param {any} pgmId          beforeCallInitFormDataの引数
 * @param {any} formNo         beforeCallInitFormDataの引数
 * @param {any} originNo       beforeCallInitFormDataの引数
 * @param {any} btnCtrlId      beforeCallInitFormDataの引数
 * @param {any} conductPtn     beforeCallInitFormDataの引数
 * @param {any} selectData     beforeCallInitFormDataの引数
 * @param {any} targetCtrlId   beforeCallInitFormDataの引数
 * @param {any} listData       beforeCallInitFormDataの引数
 * @param {any} skipGetData    beforeCallInitFormDataの引数
 * @param {any} status         beforeCallInitFormDataの引数
 * @param {any} selFlg         beforeCallInitFormDataの引数
 * @param {any} backFrom       beforeCallInitFormDataの引数
 */
function InitFormDataByOwnModal(myConductId, myFormNo, myTargetCtrlId, myBtnCtrlId, appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom) {
    // 再取得フラグがONの場合は再検索を行わない
    if (skipGetData == skipGetDataDef.GetData) {
        return;
    }
    // 機能ID、フォームNo、単票のコントロールID、ボタンのIDが全て一致する場合再検索
    if (conductId == myConductId && formNo == myFormNo && targetCtrlId == myTargetCtrlId && myBtnCtrlId == btnCtrlId) {
        // 再検索
        initFormData(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, selectData, listData, status);
    }
}

/**
 * タブの表示/非表示を切り替える
 * @param {any} ctrlIds コントロールID
 * @param {any} formNo  フォーム番号
 * @param {any} flg     true(非表示)、false(表示)
 */
function setHideTab(ctrlIds, formNo, flg) {
    if (!$.isArray(ctrlIds)) { ctrlIds = [ctrlIds]; }
    // タブ情報を取得する
    var tab_contents = $(P_Article).find("div.tab_contents");
    $(tab_contents).each(function (i, tab_content) {
        var checkFLg = false; // デフォルト表示
        // タブ内に該当のコントロールIDが含まれているかどうかチェック
        var tabInfo = null;
        $.each(ctrlIds, function (i, ctrlId) {
            tabInfo = $(tab_content).find("#" + ctrlId + "_" + formNo);
            if (tabInfo != null && tabInfo.length > 0) {
                checkFLg = true;
                return false;
            }
        });
        if (!checkFLg) { return true; } // タブ内にコントロールが存在しない場合、次へ
        // 対象コントロールIDのtabnoを取得
        var tabno = $(tab_content).data("tabno");
        setHide(tab_content, flg);
        var tab = $(P_Article).find(".tab_btn.detail").find("a[data-tabno='" + tabno + "']");
        setHide(tab, flg);
        return false;
    });
}

/**
 * タブを選択する
 * @param {any} selectTabNo タブ番号
 */
function selectTab(selectTabNo) {
    // タブ情報を取得する
    var tabBtns = $(P_Article).find(".tab_btn.detail a");
    if (tabBtns != null && tabBtns.length > 0) {
        $.each($(tabBtns), function (i, btn) {
            //タブ番号
            var tabNo = $(btn).data('tabno');
            if (tabNo == selectTabNo) {
                //タブを選択状態にする
                $(btn).click();
                return false; //break
            }
        });
    }
}

/**
 * Tabulatorのグループで折り畳み表示できるようにリストを変換する処理
 * @param {any} options prevCreateTabulatorの引数、Tabulatorのコンストラクタに渡すテーブルオプション
 * @param {number} groupKeyVal 集計するキーの列のVAL値
 * @param {array[number]} groupDispVals 表示するキーの列のVAL値
 * @param {array[number]} copyVals 削除せずコピーする列のVAL値
 * @param {number} headerFlgVal ヘッダ行ならTRUEをセットする列のVAL値
 */
function convertTabulatorListToGroup(options, groupKeyVal, groupDispVals, copyVals, headerFlgVal) {
    /*
     1 A TestA1
     2 A TestA2
     3 B TestB1
     4 B TestB2
     5 B TestB3
     ↓同一の集計キーの先頭行を親、残りを子に設定することでTabulatorがグループ化する
     1 A
       _children A TestA2
     2 B
       _children B TestB2
       _children B TestB3
     */

    const childName = "_children"; // 子の行を設定する名前(デフォルトは_children)
    var datas = options["data"];    // テーブルオプションよりデータを取得
    var rtnDatas = [];  // 設定する新しいデータ
    var rtnDataRow; // 設定する新しいデータの1行
    var groupKeys = []; // 集計キーの処理済みの値

    /**
     * 行のデータを新たな集計行として追加する処理
     * @param {any} data 行のデータ
     */
    var getNewRow = function (data) {
        // 行のデータよりコピーして新規行を作成
        var newRow = $.extend(true, {}, data);
        var keys = Object.keys(newRow); // 行のキー(ROWNOやVAL1)を取得

        var removeVal = function (valName) {
            return valName.replace("VAL", "") - 0;
        }

        // 集計キーと集計表示対象行以外の値をクリアする(集計行はグループで同じ値のみを表示する)
        for (var idx in keys) {
            var keyValue = keys[idx]; // キーの値(ROWNOやVAL1)
            if (!keyValue.startsWith("VAL")) {
                // VALで無ければ次へ
                continue;
            }
            if (removeVal(keyValue) == groupKeyVal) {
                // 集計キーの行の値をクリアすると、罫線の調整が動かないので行わない
                continue;
            }
            if (copyVals.indexOf(removeVal(keyValue)) > -1) {
                // クリアしないよう指定された行はクリアしない
                continue;
            }

            // 表示対象の列以外のVALnの列の場合、値をクリア
            var isMatch = false;
            $.each(groupDispVals, function (index, valNo) {
                if ("VAL" + valNo == keyValue) {
                    isMatch = true;
                    return true;
                }
            });
            if (isMatch == false) {
                newRow[keyValue] = "";
            }
        }
        // 集計行なので、子を追加
        newRow[childName] = [];

        newRow["VAL" + headerFlgVal] = true;
        return newRow;
    }
    /**
     * 行のデータを集計行の子に追加する
     * @param {any} data 行のデータ
     */
    var addChild = function (data) {
        rtnDataRow[childName].push(data); //追加
    }
    // データを繰り返し
    $.each(datas, function (index, data) {
        // キーの値を取得
        var key = data['VAL' + groupKeyVal];
        if (groupKeys.indexOf(key) < 0) {
            // キーが含まれないとき(新しいキー)
            groupKeys.push(key); // キーの値を処理済みに追加
            rtnDataRow = getNewRow(data); // 新しいデータの1行を設定
            rtnDatas.push(rtnDataRow);  // データに追加
            return true; // continue;
        }
        // キーが含まれるとき
        addChild(data) // 現在の行を新しいデータの子にセット
    });
    // 設定した新しいデータを設定
    options["data"] = rtnDatas;
}

/**
 * 一覧をグループごとに書式設定する処理
 * @param {any} listId 一覧のID
 * @param {any} groupValNos グループかどうか判定するキーのVAL値のリスト
 * @param {any} changeCols 書式設定する列のVAL値のリスト
 */
function setListGroupStyle(listId, groupValNos, changeCols) {

    // 一覧から値を取得する処理
    var getListValue = function (index) {
        var value = '';
        $.each(groupValNos, function (groupValIndex, groupValNo) {
            value = value + '_' + getValue(listId, groupValNo, index, CtrlFlag.Label);
        });

        return value;
    }
    // グループの先頭かどうかを取得
    var getIsTopGroup = function (prevValue, nowValue) {
        var isTop = false;
        if (prevValue != nowValue) {
            // 前の値と異なる場合は先頭
            isTop = true;
        }
        return isTop;
    }
    // グループの末尾かどうかを取得
    var getIsBottomGroup = function (index, list, nowValue) {
        var isBottom = false;
        var nextValue;  // 次の行の値
        if (index + 1 != list.length) {
            // 最終行でない場合は値を取得
            nextValue = getListValue(index + 1);
        }
        if (nowValue != nextValue) {
            // 次の行の値と異なる場合は末尾
            isBottom = true;
        }
        return isBottom;
    }
    // グループの状態に応じてスタイルを変更
    var setGroupToCols = function (index, targetCols, isTop, isBottom) {
        $.each(targetCols, function (colIndex, colVal) {
            var targetCell = getCtrl(listId, colVal, index, CtrlFlag.Label);
            if (isTop) {
                // 先頭、先頭行以外のスタイルを削除
                $(targetCell).removeClass("group-untop");
            } else {
                // 先頭でない、先頭行以外のスタイルを設定
                $(targetCell).addClass("group-untop");
            }
            if (isBottom) {
                // 末尾、末尾行以外のスタイルを削除
                $(targetCell).removeClass("group-unbottom");
            } else {
                // 末尾、末尾行以外のスタイルを設定
                $(targetCell).addClass("group-unbottom");
            }
        });
    }
    // 比較した結果を取得
    var getGroupCompareInfo = function (prevValue, index, list) {
        var newPrevValue = prevValue; // 直前行の値
        var nowValue = getListValue(index); // この行の値
        var isTop = getIsTopGroup(prevValue, nowValue); // 比較して先頭行かどうか判定
        if (isTop) {
            newPrevValue = nowValue; // 先頭行の場合、次回以降の比較でこの行の値が直前行の値となる
        }
        var isBottom = getIsBottomGroup(index, list, nowValue); // 末尾行かどうか判定
        return [newPrevValue, isTop, isBottom]; // 直前行の値、先頭かどうか、末尾かどうかを返す
    }

    // グループのキー値比較用
    var prevGroupId;
    // 一覧
    var list = $(P_Article).find("div#" + listId + getAddFormNo() + ".ctrlId").find("div:not([class^='tabulator-header'])").find("div.tabulator-row");
    // 一覧を繰り返し
    $.each(list, function (index, row) {
        // 比較
        var [newPrevGroupId, isTop, isBottom] = getGroupCompareInfo(prevGroupId, index, list);
        // 比較用の値を変更
        prevGroupId = newPrevGroupId;
        // 書式設定
        setGroupToCols(index, changeCols, isTop, isBottom);
    });
}

/**
 * 一覧のフィルタ処理実行
 * @param {string} listId 絞込を行う一覧のID
 * @param {string} filterId フィルタの要素を持つID
 * @param {string} filterVal フィルタの要素のValNo
 */
function callExecuteListFilter(listId, filterId, filterVal) {
    executeListFilter(ListFilter_TransTarget, listId, filterId, filterVal);
}

/**
 * 文字が空白(半角と全角どちらも)かどうか判定する処理
 * @param {any} target 判定する文字
 * @return {boolean} 空白の場合True
 */
function isSpace(target) {
    var result = target.match(/^( |　)$/);
    return result != null;
}

/**
 * 一覧のフィルタ処理(prevTransFormで呼び出し)
 * @param {string} transTarget prevTransFormの引数、ListFilter_TransTargetの値の場合、処理を行いTrueを返す、そうでない場合は処理を行わずFalseを返す
 * @param {string} listId 絞込を行う一覧のID
 * @param {string} filterId フィルタの要素を持つID
 * @param {string} filterVal フィルタの要素のValNo
 * @return {bool} フィルタ処理の場合True、処理中断する。そうでない場合はfalse、処理続行
 */
function executeListFilter(transTarget, listId, filterId, filterVal) {
    if (transTarget != ListFilter_TransTarget) {
        // フィルタ処理でない場合は終了
        return false;
    }

    // フィルタ処理に用いる情報の取得
    // 一覧の取得
    const tbl = P_listData["#" + listId + getAddFormNo()];
    // 現在のヘッダによるフィルタを削除(列フィルタは削除されない)
    tbl.clearFilter();
    // 後続の処理で頻出するので、わざわざメソッドの引数で渡さない、移動不可
    // 一覧の各列の定義情報
    const colInfos = tbl.getColumnDefinitions();
    // フィルタ内容の取得(ヘッダ)
    const filterText = getValue(filterId, filterVal, 0, CtrlFlag.Search) + '';
    if (!filterText) {
        // フィルタ内容が設定されていないなら終了
        return true;
    }

    // 表示中の列、かつフィルタ指定可能な列に対してフィルタ処理を行う

    // 一覧の列のうち、表示中の列を取得
    // 非表示の列だけでなく、一覧項目カスタマイズにより非表示の列も考慮
    var getVisibleColumnNames = function () {
        var colNames = []; // 列の名称(SELTAG,VAL1など)
        var columns = tbl.columnManager.getColumns(); // 一覧の表示状態
        $.each(columns, function (index, col) { // 一覧の表示状態で繰り返し
            if (col.visible) {
                colNames.push(col.field); // 表示中なら追加
            }
        });
        return colNames;
    }
    const visibleColNames = getVisibleColumnNames();

    // 表示中の列に対して、フィルタの指定がある列を取得
    var getFilterColumnNames = function (colNames) {
        var colNamesFilter = []; // 列の名称(SELTAG,VAL1など)
        // 表示中の列で繰り返し処理
        $.each(colNames, function (index, visibleColName) {
            // 列の定義より表示中の列の情報を取得
            var colInfo = colInfos.find(x => x.field == visibleColName);
            if (colInfo == null) { return true; } // 取得できない場合(無いはず)、次へ
            var filterType = colInfo.headerFilter; // 列のフィルタの種類
            if (filterType == "select" || filterType == "input") {
                // select(コンボ)かinput(テキスト)ならフィルタなので追加
                colNamesFilter.push(visibleColName);
            }
        });

        return colNamesFilter;
    }
    const colNamesFilter = getFilterColumnNames(visibleColNames);

    // フィルタ関連クラス

    // 単語ごとのフィルタ条件
    var filterClass = function () { };
    filterClass.prototype = {
        KeyId: 0 // 識別用キー
        , Keyword: "" // 検索キーワード
        , IsExactMatch: false // 完全一致フラグ
        , IsStartMulti: false // 複数条件開始フラグ
        , IsEndMulti: false // 複数条件終了フラグ
        , IsOr: false // OR条件フラグ
        , IsNot: false // 否定条件フラグ
        , Result: false // 比較結果(フィルタ処理で使用)
    }

    // フィルタリストクラス、フィルタ文字列より単語ごとに区切り、リストを作成する
    var filterListClass = function () { };
    filterListClass.prototype = {
        list: [] // フィルタリスト
        , filter: new filterClass() // フィルタリストのメンバ
        , startIdx: -1 // フィルタ文字列からの切り出しに用いるインデックス
        , addFilter: function (idx) {
            if (this.startIdx < 0) {
                return;
            }
            // 開始インデックスから現在インデックス-1までの位置の文字を切り取り
            var length = idx - this.startIdx;
            if (length > 0) {
                // リストへ追加
                this.filter.Keyword = filterText.substr(this.startIdx, length); // 検索文字列
                this.filter.KeyId = this.list.length + 1; // 識別用キー、1オリジン
                this.list.push(this.filter); // リストへ追加
                this.filter = new filterClass(); // 新たなフィルタ
            }
            this.startIdx = -1;
        }
        , execDoubleQuat: function (idx) {
            // ダブルクォーテーションの場合の処理
            if (!this.filter.IsExactMatch) {
                // 開始
                this.filter.IsExactMatch = true;
                this.startIdx = idx + 1; // キーワードは次の文字から開始
            } else {
                // 終了
                // 現在のキーワードを追加
                this.addFilter(idx);
            }
        }
        , execStartKakko: function () {
            // 左括弧の場合の処理
            if (this.filter.IsExactMatch) {
                return;
            }
            // 完全一致フラグOFFの場合、複数条件開始フラグON
            this.filter.IsStartMulti = true;
        }
        , execEndKakko: function (idx) {
            // 右括弧の場合の処理
            if (this.filter.IsExactMatch) {
                return;
            }
            // 完全一致フラグOFFの場合、複数条件終了フラグON
            this.filter.IsEndMulti = true;
            // 現在のキーワードを追加
            this.addFilter(idx);
        }
        , execNot: function (idx) {
            // 除外の場合の処理
            if (this.filter.IsExactMatch) {
                return;
            }
            // 完全一致フラグOFFの場合
            if (idx == 0) {
                // 先頭文字の場合、否定条件フラグON
                this.filter.IsNot = true;
            } else {
                var prevChar = filterText.charAt(idx - 1); // 1文字前の文字
                if (prevChar == ')' || prevChar == '"' || isSpace(prevChar)) {
                    // 右括弧、ダブルクォーテーション、スペースの場合
                    this.filter.IsNot = true; // 否定条件フラグON
                }
            }
        }
        , execSpace: function (idx) {
            // 空白文字の場合の処理
            if (this.filter.IsExactMatch) {
                return;
            }
            // 現在のキーワードを追加
            this.addFilter(idx);
        }
        , execOr: function (idx) {
            // O(オー)の場合の処理
            // Oの場合、次のRの分カウンタをインクリメントするので値を返す
            var returnIdx = idx;
            if (this.filter.IsExactMatch) {
                return returnIdx;
            }

            var getNextChar = function (nextCnt) {
                // nextCnt文字先の文字を取得する処理
                var isExists = idx < (filterText.length - (nextCnt + 1)); // 文字が存在するか確認
                if (isExists) {
                    return filterText.charAt(idx + nextCnt); // 取得できる場合
                } else {
                    return 'GetError';
                }
            }

            if (idx > 1) {
                var prevChar = filterText.charAt(idx - 1); // 1文字前の文字
                // 1文字前の文字が終了か判定
                var isPrevEnd = prevChar == ')' || prevChar == '"' || isSpace(prevChar);
                var nextChar = getNextChar(1); // 1文字先の文字
                if (isPrevEnd && nextChar == 'R') {
                    // 1文字前が右括弧、ダブルクォーテーション、空白
                    // かつ1文字先がR
                    nextChar = getNextChar(2); // 2文字先の文字
                    if (nextChar == '(' || nextChar == '"' || isSpace(nextChar)) {
                        // 2文字先が左括弧、ダブルクォーテーション、空白
                        this.filter.IsOr = true; // OR条件フラグON
                        // Rの分インデックスをインクリメント
                        returnIdx = idx + 1;
                    }
                }
            }
            if (this.startIdx < 0 && !this.filter.IsOr) {
                // OR条件の「O」でない場合、開始インデックスをセット
                this.startIdx = idx;
            }
            return returnIdx;
        }
        , execKeyword: function (idx) {
            if (this.startIdx < 0) {
                // 開始インデックスをセット
                this.startIdx = idx;
            }
        }
        , execRest: function () {
            // 残りの部分
            if (this.startIdx >= 0) {
                var length = filterText.length - this.startIdx;
                if (length > 0) {
                    this.filter.Keyword = filterText.substr(this.startIdx, length);
                    this.list.push(this.filter);
                }
            }
        }
    }

    // フィルタ文字列よりフィルタ条件リストを作成
    var convertFilter = function () {
        // 上記の2クラスと合わせて、CommonTMQUtilのAnalysisConditionStringメソッドより移行
        var convFilterList = new filterListClass();
        convFilterList.filter = new filterClass();
        convFilterList.startIdx = -1;
        for (var i = 0; i < filterText.length; i++) {
            var char = filterText.charAt(i);
            if (char == '"') {
                // ダブルクォーテーション
                convFilterList.execDoubleQuat(i);
            } else if (char == '(') {
                // 左括弧
                convFilterList.execStartKakko();
            } else if (char == ')') {
                // 右括弧
                convFilterList.execEndKakko(i);
            } else if (char == '-') {
                // 除外
                convFilterList.execNot(i);
            } else if (isSpace(char)) {
                // スペース
                convFilterList.execSpace(i);
            } else if (char == 'O') {
                // 大文字のO(オー)、ORの開始
                // Oの場合、カウンタiがインクリメントされる
                i = convFilterList.execOr(i);
            } else {
                // それ以外(通常の検索条件)
                convFilterList.execKeyword(i);
            }
        }
        // 残りの内容を条件に設定
        convFilterList.execRest();
        // 作成したリストを返す
        return convFilterList.list;
    }
    const inputFilters = convertFilter(); // フィルタ条件リストの取得

    /**
     * 部分一致検索を行う処理
     * @param {string} target チェックする文字列
     * @param {string} compare フィルタの文字列
     * @param {bool} isNot 一致しないことが条件の場合True
     * @return {bool} フィルタの内容が正しい場合、True(一致しないことが条件の場合、一致しなければTrueが返る)
     */
    var compareLike = function (target, compare, isNot) {
        var isLike = false;
        if (target) {
            // チェック文字列が空でなければ部分一致検索
            isLike = isMatchCharacterNoDiff(target, compare, false);
        }
        if (isNot) {
            // 一致しないことが条件の場合、値を反転
            return !isLike;
        }
        return isLike;
    }
    /**
    * 完全一致検索を行う処理
    * @param {string} target チェックする文字列
    * @param {string} compare フィルタの文字列
    * @param {bool} isNot 一致しないことが条件の場合True
    * @return {bool} フィルタの内容が正しい場合、True(一致しないことが条件の場合、一致しなければTrueが返る)
    */
    var compareEqual = function (target, compare, isNot) {
        var isEqual = false;
        if (target) {
            // チェック文字列が空でなければ完全一致検索
            isEqual = isMatchCharacterNoDiff(target, compare, true);
        }
        if (isNot) {
            // 一致しないことが条件の場合、値を反転
            return !isEqual;
        }
        return isEqual;
    }

    // それぞれのフィルタをTabulatorで用いるフィルタに変換するためのクラス
    // フィルタ条件の比較結果とAND/ORフラグを持つ
    var FilterBoolClass = function () { };
    FilterBoolClass.prototype = {
        value: false
        , IsOr: false
        , New: function (value, isOr) {
            this.value = value;
            this.IsOr = isOr;
        }
    };
    /**
     * FilterBoolClassのコンストラクタ代わり、引数の値をメンバに持つFilterBoolClassを作成する
     * @param {bool} value 条件が正しい場合True
     * @param {bool} isOr 前の項目とORで結合する場合True
     */
    var getNewFilterBool = function (value, isOr) {
        var filter = new FilterBoolClass();
        filter.New(value, isOr);
        return filter;
    }

    /**
     * FilterBoolClassのリストを結合し、TrueかFalseを取得する
     * @param {array[FilterBoolClass]} list FilterBoolClassのリスト
     * @returns {bool} listのメンバを結合した結果
     */
    var getBoolFromFilterBoolList = function (list) {
        var returnCond; // 結果
        $.each(list, function (index, cond) { // listで繰り返し
            if (index == 0) {
                // 初回はそのままセット
                returnCond = cond.value;
                return true;
            }
            // 2回目以降は結合、AND or OR
            if (cond.IsOr) {
                // OR
                returnCond = returnCond || cond.value;
            } else {
                // AND
                returnCond = returnCond && cond.value;
            }
        });
        return returnCond;
    }

    /**
     * 括弧で囲まれた中のFilterBoolClassのリストを結合し、一つのFilterBoolClassを返す
    * @param {array[FilterBoolClass]} list FilterBoolClassのリスト
     */
    var summaryKakkoConds = function (list) {
        // listの真偽を判定
        var result = getBoolFromFilterBoolList(list);
        // FilterBoolClassをその値で作成、ORかどうかは先頭の要素の値
        return getNewFilterBool(result, list[0].IsOr);
    }

    /**
     * 比較対象の値とフィルタリストで、真偽を判定する処理
     * @param {any} targetValue 比較対象の値
     * @param {list[filterClass]} filters フィルタリスト
     */
    var compareValues = function (targetValue, filters) {
        // 各フィルタの真偽値をそれぞれ判定する
        for (var i = 0; i < filters.length; i++) {
            var filter = new filterClass();
            filter = filters[i];
            // 各条件の判定をセット
            if (filter.IsExactMatch) {
                // 完全一致
                filter.Result = compareEqual(targetValue, filter.Keyword, filter.IsNot);
            }
            else {
                // 部分一致
                filter.Result = compareLike(targetValue, filter.Keyword, filter.IsNot);
            }
        }

        var conds = []; // 全てのフィルタの条件を格納
        var kakkoConds = []; // 括弧内のフィルタの条件を格納
        // 全てのフィルタを結合し、真偽値を取得する
        for (var i = 0; i < filters.length; i++) {
            var filter = new filterClass(); // 特に宣言する意味は無い、後続のコードでIDEの入力補完を行うため
            filter = filters[i];

            if (filter.IsStartMulti) {
                // 括弧が開始の場合、括弧内の条件に格納し、次へ
                kakkoConds.push(getNewFilterBool(filter.Result, filter.IsOr));
                continue;
            }

            // 括弧開始でない場合
            if (kakkoConds.length == 0) {
                // 括弧条件が空の場合、全てのフィルタ条件の方へ追加
                conds.push(getNewFilterBool(filter.Result, filter.IsOr));
            } else {
                // 括弧の中の条件なので、括弧の条件に追加
                kakkoConds.push(getNewFilterBool(filter.Result, filter.IsOr));
            }

            if (filter.IsEndMulti) {
                // 括弧終了の場合、現在の括弧を一つのFilterBoolClassへ集約し、全てのフィルタ条件へ追加
                conds.push(summaryKakkoConds(kakkoConds));
                kakkoConds = []; // 初期化
            }
        }
        // すべての条件を集約し真偽値を取得
        var cond = getBoolFromFilterBoolList(conds);
        return cond;
    }

    /**
     * Tabulatorのカスタムフィルタ
     * @param {list[string]} data Tabulatorの1行のデータ
     * @param {list[filterClass]} param パラメータ、処理対象列名のリストとフィルタ内容を持つ
     */
    function customFilter(data, param) {
        const colNames = param.colNames; // 対象列
        const filters = param.filters; // フィルタ内容
        // 行の判定結果
        var allColConds = false;

        // 処理対象列で繰り返し
        $.each(colNames, function (index, colName) {
            var colInfo = colInfos.find(x => x.field == colName); // 列の定義情報を取得
            var colValue = data[colName] + ""; // 行のデータより列の値を取得

            // 一覧画面のフィルタはselectの作成候補が変わったため、処理を通らないよう変更
            if (false && colInfo.headerFilter == "list") {

                // selectの場合、その列のコンボのメンバから、フィルタ条件と合致するメンバを全て選び、そのコード値リストを条件とする
                // フィルタが「京」で、コンボのメンバが「1:東京,2:京都,3:大阪」の場合、1or2が検索条件となる
                var targetIds = [];
                var selectValues = colInfo.headerFilterParams.values; // その列のフィルタのコンボのリスト(連想配列、キーがID、値が名称)
                // フィルタ条件に合致するコンボのメンバを取得し、配列に追加
                Object.keys(selectValues).forEach(function (value) {
                    if (compareValues(this[value], filters)) {
                        targetIds.push(value);
                    }
                }, selectValues);

                var cond = false;
                if (targetIds.length > 0) {
                    // 合致するコンボのリストに含まれる場合、True
                    cond = targetIds.indexOf(colValue) > -1;
                }
                // 行の判定結果とOR(いずれかの列の条件に合えば表示するため)
                allColConds = allColConds || cond;

                // 以降はテキストボックスの時の処理なのでcontinue
                return true;
            }
            // テキストボックスの場合は、値とフィルタで比較
            var cond = compareValues(colValue, filters);
            // 行の判定結果とOR(いずれかの列の条件に合えば表示するため)
            allColConds = allColConds || cond;
        });
        // 全ての行を比較した結果
        return allColConds;
    }

    // 絞り込み条件に上記のフィルタを「追加」、既存の列フィルタは有効のまま
    tbl.addFilter(customFilter, { colNames: colNamesFilter, filters: inputFilters });

    return true;
}

/**
 * 機器台帳詳細画面への画面遷移パラメータを設定する処理
 * @param {string} machineId 機番ID
 */
function getParamToMC0001(machineId, tabNo) {
    var conditionDataList = [];
    var conditionData = {};
    conditionData['CTRLID'] = 'BODY_020_00_LST_0';
    conditionData['VAL32'] = machineId;
    conditionData['VAL34'] = tabNo;
    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * 他の機能より新しいタブで開く画面で実行する、戻る/閉じるボタン表示制御処理
 * @param {any} transPtn 画面遷移のパターン、transPtnDef
 */
function setDisplayCloseBtn(transPtn) {
    // ボタンの表示制御
    var isDisplayCloseBtn = transPtn == transPtnDef.OtherTab;
    if (isDisplayCloseBtn) {
        // 新しいタブで開いた場合、戻るボタンを非表示にする
        setHideButtonExcludeModal("Back", true);
        setHideButtonExcludeModal("Close", false);
        return;
    }
    // 戻るボタンと閉じるボタンの表示状態を取得
    var isUnAvaibleBack = isUnAvailableButton("Back");
    var isUnAvaibleClose = isUnAvailableButton("Close");
    if (!isUnAvaibleBack && !isUnAvaibleClose) {
        // どちらも表示されているとき、一覧→参照画面に遷移なので、閉じるボタンを非表示にする
        setHideButtonExcludeModal("Close", true);
    }
    // ポップアップ→削除→一覧へ戻る→参照画面の場合の制御は？
}

/**
 * 件名別長期計画参照画面への画面遷移パラメータを設定する処理
 * @param {string} longplanId 長期計画ID
 */
function getParamToLN0001(longplanId) {
    var conditionDataList = [];
    var conditionData = {};
    conditionData['CTRLID'] = 'BODY_040_00_LST_0';
    conditionData['VAL51'] = longplanId;
    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * 保全活動参照画面への画面遷移パラメータを設定する処理
 * @param {string} summaryId 保全活動件名ID
 * @param {number} tabNo タブ番号（依頼タブ：1、履歴タブ：3）
 * @param {number} scheduleType スケジュール種類
 */
function getParamToMA0001(summaryId, tabNo, scheduleType) {
    var conditionDataList = [];
    var conditionData = {};
    conditionData['CTRLID'] = 'BODY_020_00_LST_0';
    conditionData['VAL60'] = summaryId;
    conditionData['VAL62'] = tabNo;
    if (scheduleType) {
        conditionData['ScheduleType'] = scheduleType;
    }
    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * 予備品一覧参照画面への画面遷移パラメータを設定する処理
 * @param {string} partsId 予備品ID
 */
function getParamToPT0001(partsId) {
    var conditionDataList = [];
    var conditionData = {};
    conditionData['CTRLID'] = 'BODY_020_00_LST_0';
    conditionData['VAL34'] = partsId;
    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * 機器台帳-詳細画面-保全活動タブから保全活動詳細編集画面への画面遷移パラメータを設定する処理
 * @param {string} machineId 遷移元の機器の機番ID
 */
function getParamToMA0001FromMachine(machineId) {
    var conditionDataList = [];
    var conditionData = {};
    conditionData['MaintainanceTabNew'] = machineId;

    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * コンボの変更時に拡張項目の値を隠し項目に設定するコンボで、
 * 初期表示時にイベントを発生させる処理
 * @param {any} conditionDataList オーバーライド関数「PrevInitFormData」の引数、隠し項目の値をセットする
 * @param {any} formNo コンボの画面のFormNo
 * @param {any} listCtrlId コンボと隠し項目の一覧のコントロールID
 * @param {any} comboVal コンボのVAL値
 * @param {any} extVal 隠し項目のVAL値
 * @param {any} extCtrlFlag 隠し項目のコントロールフラグ(GetValueの引数)
 * @returns {any} conditionDataListに隠し項目の値をセットした内容、これをPrevInitFormDataで返す
 */
function setComboExValueOnPrevInitFormData(conditionDataList, formNo, listCtrlId, comboVal, extVal, extCtrlFlag) {
    // 連動元コンボの変更時イベントを発生させる
    var combo = getCtrl(listCtrlId, comboVal, 0, CtrlFlag.Combo);
    changeNoEdit(combo);
    // コンボの変更イベントによって取得した拡張項目の値
    var optValue = getValue(listCtrlId, extVal, 0, extCtrlFlag);
    // 検索条件に設定
    conditionDataList = setValueToConditionDataList(conditionDataList, formNo, listCtrlId, extVal, optValue);
    return conditionDataList;
}

/**
 * 検索条件を格納したリストに値を設定する処理(PrevInitFormDataでの値変更用)
 * @param {any} conditionDataList オーバーライド関数「PrevInitFormData」の引数、隠し項目の値をセットする
 * @param {any} formNo 検索条件の画面のFormNo
 * @param {any} listCtrlId 検索条件の一覧のコントロールID
 * @param {any} valNo 設定する項目のVAL値
 * @param {any} value 設定する値
 */
function setValueToConditionDataList(conditionDataList, formNo, listCtrlId, valNo, value) {
    if ($(conditionDataList).length) {
        $.each($(conditionDataList), function (idx, conditionData) {
            if (conditionData.FORMNO == formNo && conditionData.CTRLID == listCtrlId) {
                // 画面に渡す情報にセット
                conditionData["VAL" + valNo] = value;
            }
        });
    }
    return conditionDataList;
}

///**
// * コンボ変更時、拡張項目の値(EXPARAM1)を隠し項目にセットする処理
// * @param {any} listCtrlId コンボと隠し項目の一覧のコントロールID
// * @param {any} ExtVal 隠し項目のVAL値
// * @param {any} ExtCtrlFlag 隠し項目のコントロールフラグ(SetValueの引数)
// * @param {any} selected コンボの選択行の取得値、setComboOtherValuesの引数
// * @param {any} isModal モーダル画面の場合true
// */
//function setComboExValue(listCtrlId, ExtVal, ExtCtrlFlag, selected, isModal) {
//    var extValue = selected == null ? '' : selected.EXPARAM1;
//    setValue(listCtrlId, ExtVal, 0, ExtCtrlFlag, extValue, isModal);
//}

/**
 * コンボ変更時、拡張項目の値を隠し項目にセットする処理
 * @param {any} listCtrlId コンボと隠し項目の一覧のコントロールID
 * @param {any} ExtVal 隠し項目のVAL値
 * @param {any} ExtCtrlFlag 隠し項目のコントロールフラグ(SetValueの引数)
 * @param {any} selected コンボの選択行の取得値、setComboOtherValuesの引数
 * @param {any} isModal モーダル画面の場合true
 * @param {any} exparamNo 拡張項目番号
 */
function setComboExValue(listCtrlId, ExtVal, ExtCtrlFlag, selected, isModal, exparamNo) {
    var paramName = "EXPARAM" + (exparamNo ? exparamNo : "1");
    var extValue = selected == null ? '' : selected[paramName];
    setValue(listCtrlId, ExtVal, 0, ExtCtrlFlag, extValue, isModal);
}

/**
 * スケジュール表示エリアのTabulatorヘッダー情報を設定
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {Array.<object>} header   ：Tabulatorヘッダー情報
 * @param {Element} headerElement   ：ヘッダー要素
 */
function setScheduleHeaderInfo(appPath, header, headerElement, layoutNo = null) {
    // 個別実装用汎用リストからスケジュールレイアウト情報を取得
    var layoutName = "scheduleLayout";
    if (layoutNo != null) {
        layoutName = "scheduleLayout" + layoutNo;
    }
    var layoutInfo = P_dicIndividual[layoutName]; // Dictionary<string, object>
    if (!layoutInfo || layoutInfo.length == 0) {
        return;
    }

    const unit = layoutInfo['unit'];
    const layout = layoutInfo['layout'];
    const move = layoutInfo['move'] && layoutInfo['move'] == "1" ? true : false;
    if (!unit || !layout) {
        return;
    }

    const modalNo = getCurrentModalNo(headerElement);
    var zIndex = 1;
    if (modalNo > 0) {
        zIndex = 1040 + (10 * modalNo) + 1;
    }

    switch (unit) {
        case ScheduleDispUnit.Month: // 月度単位
            $.each(layout, function (yearTitle, monthList) {
                // 下段の月単位の定義を生成
                var monthHeaderList = [];
                $.each(monthList, function (key, title) {
                    monthHeaderList.push(createScheduleTabulatorHeader(appPath, "YM" + key, title, move, zIndex, true));
                });

                // 上段の年度の定義を生成
                var yearHeader = {
                    title: yearTitle,
                    field: 'YMColumn',
                    // ヘッダーの中央寄せ
                    headerHozAlign: "center",
                    // 列の表示/非表示
                    visible: true,
                    // 下段の月単位の定義を入れ子で設定
                    columns: monthHeaderList,
                };
                header.push(yearHeader);
            });

            break;
        case ScheduleDispUnit.Year: // 年度単位
            $.each(layout, function (key, title) {
                header.push(createScheduleTabulatorHeader(appPath, "Y" + key, title, move, zIndex, false));
            });
            break;
    }
}

/**
 * スケジュールエリア用のTabulatorヘッダー情報の生成
 * @param {string} appPath  ：アプリケーションルートパス
 * @param {string} field    ：項目名
 * @param {string} title    ：ヘッダータイトル
 * @param {bool} isYm:年月単位の場合True
 * @return {object} Tabulatorヘッダー情報
 */
function createScheduleTabulatorHeader(appPath, field, title, move, zIndex, isYm) {
    var header = {
        // ヘッダータイトル
        title: title,
        // ヘッダー項目名
        field: field,
        // ヘッダーの中央寄せ
        headerHozAlign: "center",
        // データ部ラベルの中央寄せ
        hozAlign: "center",
        // 列幅
        width: isYm ? 50 : 75,
        // フォーマット
        formatter: "scheduleLabel",
        formatterParams: {
            appPath: appPath,
            move: move,
            zindex: zIndex,
        },
        // セルクリック時イベント
        //cellClickVal: cellClick,
        // 列の表示/非表示
        visible: true,
        // 列のフィルター無効
        headerFilter: false,
        // ソート無効
        headerSort: false,
        // ツールチップ無効
        tooltip: false,
    };
    return header;
}

/**
 * Tabulatorのカスタム列のヘッダー設定処理
 * @param {number} formNo   ：画面NO
 * @param {string} id       ：一覧のID
 * @param {Element} head    ：Tabulatorヘッダー
 */
function setCustomTabulatorHeader(formNo, id, head) {
    if (head.formatter == "scheduleLabel") {
        // スケジュール表示列
        head.formatter = function (cell, formatterParams, onRendered) {
            const row = cell.getRow();
            const rowNo = row.getData().ROWNO;
            var val = cell.getValue();
            if (!val) { val = ""; }
            const field = cell.getField();

            // ラベル値を分解
            // [スケジュール種類]|[点検種別]|[機能ID]|[画面番号]|[タブ番号]|[遷移パラメータ]|[変更前年月](移動後のみ付加される)
            var aryVal = val.split('|');
            var status = aryVal[0] ? parseInt(aryVal[0], 10) : ScheduleStatus.NoSchedule;

            const targetYM = field.replace('Y', '').replace('M', '');
            const maintainanceKind = aryVal.length > 1 ? aryVal[1] : "";
            const existsLink = aryVal.length > 2 && aryVal.length <= 6;
            //const transParam = aryVal.length > 2 ? aryVal.slice(2).join('|') : "";
            const transParam = aryVal.length > 2 ? aryVal[0] + '|' + aryVal.slice(2).join('|') : "";
            const appPath = head.formatterParams.appPath;
            const move = head.formatterParams.move;
            const zIndex = head.formatterParams.zindex;

            onRendered(function () {
                //値設定
                var elm = cell.getElement();
                if (move) {
                    // 移動有り
                    $(elm).on("contextmenu", function (e) {
                        // 既存のコンテキストメニューを無効化
                        e.preventDefault();

                        var [mark, status] = getScheduleMarkAndStatus(this);
                        if (P_ScIsMoving || MovableScheduleStatus.indexOf(status) >= 0) {
                            if (!P_ScIsMoving) {
                                P_ScSrcCell = elm;
                            } else {
                                P_ScDstCell = elm;
                            }
                            // 移動中でない場合は移動可能なスケジュール種類の場合のみコンテキストメニュー表示
                            var contextMenu = $('#main_contents').find('#sc_contextmenu');
                            if (!contextMenu || contextMenu.length == 0) {
                                // コンテキストメニューの生成
                                contextMenu = $(createScheduleContextMenu()).appendTo($('#main_contents'));
                            }
                            // コンテキストメニューの項目の設定
                            setScheduleContextMenuItem(contextMenu, elm, rowNo, field, status);
                            setScheduleContextMenuEvent(contextMenu, formNo, id, rowNo);
                            // コンテキストメニューの表示
                            $(contextMenu).finish().toggle(100).
                                // マウスの右側に表示
                                css({
                                    top: e.pageY + "px",
                                    left: e.pageX + "px"
                                });
                        }
                    });
                    // 
                    $(document).on("mousedown", function (e) {
                        if (!$(e.target).parents("#sc_contextmenu").length > 0) {
                            // メニューではない場所をクリックした場合、メニューを非表示にする
                            $("#sc_contextmenu").hide(100);
                        }
                    });
                }
                if (existsLink) {
                    // 画面遷移有りの場合
                    mark = $(elm).find('[class^="sc-mark"]');
                    $(mark).addClass('sc-link');
                    $(mark).on('click', function (e) {
                        if ($(this).hasClass('sc-link-canceled')) {
                            // 画面遷移キャンセルのclassを付与されている場合は画面遷移を実行しない
                            return false;
                        }
                        // 画面遷移実行
                        confirmScrapBeforeTrans(appPath, transPtnDef.OtherTab, transDivDef.None, transParam, transDispPtnDef.Popup, formNo, id, ScheduleLinkId, rowNo, null, false);
                    });
                }

            });

            if (status == ScheduleStatus.NoSchedule) {
                // スケジュール無し
                return "";
            }

            // マーク要素の生成
            return createScheduleMarkElements(zIndex, status, targetYM, maintainanceKind);
        }
    }
}

/**
 * スケジュール種類からマーク用のCSSclass名を取得
 * @param {number} status   ：スケジュール種類
 */
function getScheduleCssClass(status) {
    switch (status) {
        case ScheduleStatus.Complete:
            return ScheduleClass.Complete;
        case ScheduleStatus.UpperComplete:
            return ScheduleClass.UpperComplete;
        case ScheduleStatus.Created:
            return ScheduleClass.Created;
        case ScheduleStatus.NoCreate:
            return ScheduleClass.NoCreate;
        case ScheduleStatus.UpperScheduled:
            return ScheduleClass.UpperScheduled;
        case ScheduleStatus.NoSchedule:
            return ScheduleClass.NoSchedule;
        default:
            return "";
    }
}

/**
 * スケジュール用マーク要素の生成
 * @param {number} zIndex           ：z-index
 * @param {ScheduleStatus} status   ：スケジュール種類
 * @param {string} targetYM         ：対象年月
 * @param {string} maintainanceKind ：点検種別
 */
function createScheduleMarkElements(zIndex, status, targetYM, maintainanceKind) {
    const markCss = getScheduleCssClass(status);
    var parent = $('<div>');
    var mark = $('<span class="sc-mark"></span>').appendTo(parent);
    $(mark).addClass(markCss);
    $(mark).css('z-index', zIndex + 1);
    setAttrByNativeJs(mark, 'data-sc-status', status);
    setAttrByNativeJs(mark, 'data-sc-origindate', targetYM);
    if (status == ScheduleStatus.UpperScheduled) {
        // △の場合、枠線表示用span要素を追加
        var markBorder = $('<span class="sc-mark-border"></span>').appendTo(parent);
        $(markBorder).addClass(markCss);
    }
    // 点検種別表示用span要素を追加
    var markText = $('<span class="sc-mark-text"></span>').appendTo(parent);
    $(markText).addClass(markCss);
    $(markText).text(maintainanceKind);
    $(markText).css('z-index', zIndex + 2);
    var elementText = $(parent).html();
    return elementText;
}

/**
 * スケジュール移動用コンテキストメニュー要素の生成
 */
function createScheduleContextMenu() {
    var contextmenu = $('<div id="sc_contextmenu" ></div>');
    var ul = $('<ul></ul>').appendTo(contextmenu);
    $('<li data-sc-action="prev">' + P_ComMsgTranslated[911140004] + '</li>').appendTo(ul);    // 前月へ移動
    $('<li data-sc-action="next">' + P_ComMsgTranslated[911120021] + '</li>').appendTo(ul);    // 次月へ移動
    $('<li data-sc-action="select">' + P_ComMsgTranslated[911020004] + '</li>').appendTo(ul);  // 移動先を指定
    $('<li data-sc-action="decide">' + P_ComMsgTranslated[911090005] + '</li>').appendTo(ul);  // 決定
    $('<li data-sc-action="cancel">' + P_ComMsgTranslated[911020005] + '</li>').appendTo(ul);  // 移動解除
    return contextmenu;
}

/**
 * スケジュール移動用コンテキストメニューの表示設定
 * @param {Element} contextMenu     ：コンテキストメニュー要素
 * @param {Element} elm             ：対象セル要素
 * @param {number} rowNo            ：表示対象行
 * @param {string} field            ：表示列名
 * @param {ScheduleStatus} status   ：対象セルのスケジュール種類
 */
function setScheduleContextMenuItem(contextMenu, elm, rowNo, field, status) {
    var now = new Date();
    var nowYear = now.getFullYear();
    var nowMonth = ('00' + (now.getMonth() + 1)).slice(-2);
    var nowYM = nowYear + (field.indexOf('YM') >= 0 ? nowMonth : '');
    var targetYM = field.replace('Y', '').replace('M', '');
    var prevCell = getPrevOrNextScheduleCell(elm, false);
    var nextCell = getPrevOrNextScheduleCell(elm, true);

    if (P_ScIsMoving) {
        // 移動中
        $(contextMenu).find('li[data-sc-action="prev"]').hide();
        $(contextMenu).find('li[data-sc-action="next"]').hide();
        $(contextMenu).find('li[data-sc-action="select"]').hide();
        $(contextMenu).find('li[data-sc-action="cancel"]').show();
        if (status != ScheduleStatus.NoSchedule || rowNo != P_ScTargetRowNo || targetYM < nowYM) {
            // スケジュールが設定済み、または移動対象行と同一でないセル、
            // または対象年月が現在年月より過去の場合、「決定」メニューを非表示
            $(contextMenu).find('li[data-sc-action="decide"]').hide();
        } else {
            const srcYM = getScheduleYM(P_ScSrcCell);
            const isNext = targetYM > srcYM;
            var tmpCell = getPrevOrNextScheduleCell(P_ScSrcCell, isNext)[0];
            while (tmpCell != P_ScDstCell) {
                const [mark, tmpStatus] = getScheduleMarkAndStatus(tmpCell);
                if (tmpStatus != ScheduleStatus.NoSchedule) {
                    // 移動元と移動先のセルの間にスケジュールが存在している場合は「決定」メニューを非表示
                    $(contextMenu).find('li[data-sc-action="decide"]').hide();
                    return;
                }
                tmpCell = getPrevOrNextScheduleCell(tmpCell, isNext)[0];
            }
            // 上記以外の場合、「決定」メニューを表示
            $(contextMenu).find('li[data-sc-action="decide"]').show();
        }
    } else {
        // 移動中以外
        $(contextMenu).find('li[data-sc-action="select"]').show();
        $(contextMenu).find('li[data-sc-action="decide"]').hide();
        $(contextMenu).find('li[data-sc-action="cancel"]').hide();
        if (prevCell) {
            const prevYM = getScheduleYM(prevCell);
            const [mark, prevStatus] = getScheduleMarkAndStatus(prevCell);
            if (!prevYM || targetYM <= nowYM || prevStatus != ScheduleStatus.NoSchedule) {
                // 対象セルが開始年月、または対象年月が現在年月以前、
                // または前月にスケジュール設定済みの場合、「前月に移動」メニューを非表示
                $(contextMenu).find('li[data-sc-action="prev"]').hide();
            } else {
                // 上記以外の場合、「前月に移動」メニューを表示
                $(contextMenu).find('li[data-sc-action="prev"]').show();
            }
        }
        if (nextCell) {
            const nextYM = getScheduleYM(nextCell);
            const [mark, nextStatus] = getScheduleMarkAndStatus(nextCell);
            if (!nextYM || nextYM < nowYM || nextStatus != ScheduleStatus.NoSchedule) {
                // 対象セルが終了年月、または翌月が現在年月より過去、または翌月にスケジュール設定済みの場合、「翌月に移動」メニューを非表示
                $(contextMenu).find('li[data-sc-action="next"]').hide();
            } else {
                // 上記以外の場合、「翌月に移動」メニューを表示
                $(contextMenu).find('li[data-sc-action="next"]').show();
            }
        }
    }
}

/**
 * スケジュールセルの前後のセルを取得
 * @param {Element} elm     ：対象セル要素
 * @param {boolean} isNext  ：対象セルの後ろの要素かどうか(true:後/false:前)
 */
function getPrevOrNextScheduleCell(elm, isNext) {
    var targetElm;
    if (isNext) {
        targetElm = $(elm).next();
        if ($(targetElm).hasClass('tabulator-col-resize-handle')) {
            targetElm = $(targetElm).next();
        }
    } else {
        targetElm = $(elm).prev();
        if ($(targetElm).hasClass('tabulator-col-resize-handle')) {
            targetElm = $(targetElm).prev();
        }
    }
    return targetElm;
}

/**
 * スケジュールセル要素から対象年月を取得
 * @param {Element} elm ：対象セル要素
 * @return {string} 対象年月    ：yyyyMM(月度単位の場合)/yyyy(年度単位の場合)
 */
function getScheduleYM(elm) {
    const field = $(elm).attr('tabulator-field');
    if (!field || field.indexOf('Y') < 0) {
        return "";
    }
    return field.replace('Y', '').replace('M', '');
}

/**
 * スケジュールセル要素から変更前年月を取得
 * @param {Element} elm ：対象セル要素
 * @return {string} 変更前年月    ：yyyyMM(月度単位の場合)/yyyy(年度単位の場合)
 */
function getScheduleOriginalYM(elm) {
    const mark = $(elm).find('.sc-mark');
    if (!mark || mark.length == 0) {
        return "";
    }
    return $(mark).data('sc-origindate');
}

/**
 * スケジュール表示用マーク要素とスケジュール種類の取得
 * @param {Element} elm ：対象セル要素
 * @return {Array} [マーク要素, スケジュール種類]
 */
function getScheduleMarkAndStatus(elm) {
    const mark = $(elm).find('[class^="sc-mark"]');
    if (!mark || mark.length == 0) {
        return [null, ScheduleStatus.NoSchedule];
    }
    return [mark, $(elm).find('.sc-mark').data('sc-status')];
}

/**
 * スケジュール移動時マウスストーカー生成＆イベント追加
 */
function setScheduleMouseStalker(cellElement) {
    // 追随するマウスストーカー要素を生成
    P_ScStalker = $('<span class="sc-stalker"></span>')[0];
    // スタイルを設定
    var style = P_ScStalker.style;
    // fixed（画面左上を基点、スクロール無視）を設定
    $(P_ScStalker).css('position', 'fixed');
    $(P_ScStalker).css('display', 'none');
    // マーク要素を複製してマウスストーカーへ追加
    var marks = $(cellElement).find('.sc-mark', '.sc-mark-border').clone();
    $(marks).removeClass('sc-link');
    $(marks).appendTo($(P_ScStalker));
    $(P_ScStalker).appendTo('body');

    // イベント追加
    $(document).on("mousemove", setScheduleMouseStalkerPosition);
}

/**
 * スケジュール移動時マウスストーカーの表示位置設定
 * @param {Event} e ：イベント
 */
function setScheduleMouseStalkerPosition(e) {
    if (!P_ScStalker) {
        return;
    }
    $(P_ScStalker).css('display', 'block');
    $(P_ScStalker).css('left', (e.clientX + 15) + 'px');
    $(P_ScStalker).css('top', (e.clientY + 15) + 'px');
}

/**
 * スケジュール移動時マウスストーカー削除＆イベント削除
 */
function removeScheduleMouseStalker() {
    if (!P_ScStalker) return;
    $(document).off('mousemove', setScheduleMouseStalkerPosition);
    $(P_ScStalker).remove();
    P_ScStalker = null;
}

/**
 * スケジュール移動用コンテキストメニュー項目の選択イベント処理
 * @param {Element} contextMenu ：コンテキストメニュー要素
 * @param {number} rowNo        ：対象行番号
 */
function setScheduleContextMenuEvent(contextMenu, formNo, id, rowNo) {
    $(contextMenu).find("li").off("click");
    $(contextMenu).find("li").on("click", function () {
        const action = $(this).data('sc-action');
        switch (action) {
            case "prev":
            case "next":
                P_ScIsMoving = false;
                const isNext = action == 'next';
                if (P_ScSrcCell) {
                    P_ScDstCell = getPrevOrNextScheduleCell(P_ScSrcCell, isNext);	// 前後のセル
                    if (P_ScDstCell) {
                        // 移動元のマーク要素を取得して複製
                        var [mark, status] = getScheduleMarkAndStatus(P_ScSrcCell);
                        var markDst = $(mark).clone(true);
                        // 移動先へマークを追加
                        $(markDst).appendTo(P_ScDstCell);
                        // 画面遷移リンクの設定/解除
                        setScheduleLink(P_ScDstCell, markDst);
                    }
                    $(P_ScSrcCell).removeClass('sc-moving');
                    $(P_ScSrcCell).removeClass('sc-moved');
                    $(P_ScSrcCell).empty();
                    // 変更値を保存
                    var tbl = P_listData[id];
                    updateTabulatorDataForChangeVal(tbl, id, formNo, getProgramIdByElement(this));
                    tbl = null;
                }
                P_ScSrcCell = null;
                P_ScDstCell = null;
                break;
            case "select":
                P_ScIsMoving = true;
                P_ScTargetRowNo = rowNo;
                P_ScDstCell = null;
                if (P_ScSrcCell) {
                    $(P_ScSrcCell).addClass('sc-moving');
                    $(P_ScSrcCell).removeClass('sc-moved');
                }
                // マウスストーカーを設定
                setScheduleMouseStalker(P_ScSrcCell);
                break;
            case "decide":
                P_ScIsMoving = false;
                P_ScTargetRowNo = -1;
                if (P_ScSrcCell && P_ScDstCell) {
                    var [mark, status] = getScheduleMarkAndStatus(P_ScSrcCell);
                    var markDst = $(mark).clone(true);
                    // 移動先へマークを追加
                    $(markDst).appendTo(P_ScDstCell);
                    // 画面遷移リンクの設定/解除
                    setScheduleLink(P_ScDstCell, markDst);
                    $(P_ScSrcCell).removeClass('sc-moving');
                    $(P_ScSrcCell).empty();
                    // 変更値を保存
                    var tbl = P_listData[id];
                    updateTabulatorDataForChangeVal(tbl, id, formNo, getProgramIdByElement(this));
                    tbl = null;
                }
                // マウスストーカーを解除
                removeScheduleMouseStalker();
                P_ScSrcCell = null;
                P_ScDstCell = null;
                break;
            case "cancel":
                P_ScIsMoving = false;
                P_ScTargetRowNo = -1;
                if (P_ScSrcCell) {
                    $(P_ScSrcCell).removeClass('sc-moving');
                    if (isScheduleDateChanged(P_ScSrcCell)) {
                        $(P_ScSrcCell).addClass('sc-moved');
                    }
                }
                // マウスストーカーを解除
                removeScheduleMouseStalker();
                P_ScSrcCell = null;
                P_ScDstCell = null;
                break;
        }
        // コンテキストメニューを非表示に
        $("#sc_contextmenu").hide(100);
    });
}

/**
 * スケジュール年月が変更されているかどうか
 * @param {Element} cell    ：対象セル要素
 */
function isScheduleDateChanged(cell) {
    const targetYM = getScheduleYM(cell);
    const originalYM = getScheduleOriginalYM(cell);
    return targetYM != originalYM;
}

/**
 * スケジュール表示用マークへの画面遷移リンクの設定処理
 * @param {Element} cell    ：設定対象セル要素
 * @param {Element} mark    ：設定対象マーク要素
 */
function setScheduleLink(cell, mark) {
    if (isScheduleDateChanged(cell)) {
        $(cell).addClass('sc-moved');
        if ($(mark).hasClass('sc-link')) {
            // リンクを解除
            $(mark).removeClass('sc-link');
            $(mark).addClass('sc-link-canceled');
        }
    } else {
        if ($(mark).hasClass('sc-link-canceled')) {
            // リンクを再設定
            $(mark).removeClass('sc-link-canceled');
            $(mark).addClass('sc-link');
        }
    }
}

/**
 * Tabulatorのカスタム列のデータ取得処理
 * @param {Array.<object>} rowData      ：Tabulator行データ
 * @param {Element} rowEle              ：行要素
 * @param {boolean} isCalledUpdateFunc  ：更新値保存関数からの呼び出しかどうか
 */
function getCustomTempDataForTabulator(rowData, rowEle, isCalledUpdateFunc) {
    // 各セル毎にデータをセット(スケジュールデータ)
    $.each($(rowEle).find(".tabulator-cell[tabulator-field^='Y']"), function (i, cell) {
        //ｾﾙの値(表示値、入力値)を取得
        const fieldName = $(cell).attr("tabulator-field");
        var val = getScheduleCellVal(cell, fieldName, rowData, isCalledUpdateFunc);
        if (val) {
            // スケジュールデータが存在するセルのみデータをセットする
            rowData[$(cell).attr("tabulator-field")] = val;
        }
    });

}

/**
 * スケジュール列のセルデータ取得処理
 * @param {Element} td                  ：セル要素
 * @param {string} fieldName            ：列名
 * @param {Array.<object>} rowData      ：Tabulator行データ
 * @param {boolean} isCalledUpdateFunc  ：更新値保存関数からの呼び出しかどうか
 */
function getScheduleCellVal(td, fieldName, rowData, isCalledUpdateFunc) {

    var val = '';
    //スケジュール表示ラベル
    var mark = $(td).find('.sc-mark');
    if (mark.length > 0) {
        const status = $(mark).data('sc-status');
        const originDate = $(mark).data('sc-origindate');
        const targetDate = fieldName.replace('Y', '').replace('M', '');
        const isMoved = originDate != targetDate;
        const dataName = !isMoved ? fieldName : fieldName.replace(targetDate, originDate);
        if (isCalledUpdateFunc) {
            val = rowData[dataName];
        } else {
            val = status;
        }
        if (isMoved) {
            // 移動元のデータをクリア
            rowData[dataName] = '';
        }
        // 変更前年月と対象年月が異なる場合のみ変更前年月を付加する
        val += (!isMoved ? "" : ('|' + originDate));
    }
    return val;
}

/**
 * リンクのコントロールIDがスケジュールのリンクかどうかを判定する処理
 * @param {string} btn_ctrlId 画面遷移前処理のbtn_ctrlId
 * @return {bool} スケジュールのリンクの場合True
 */
function isScheduleLink(btn_ctrlId) {
    return btn_ctrlId == ScheduleLinkId;
}

/**
 * スケジュールのリンクの画面遷移の内容より保全活動画面への遷移パラメータを作成する処理
 * @param {string} transTarget 画面遷移前処理のtransTarget
 * @return {array} 画面遷移パラメータ
 */
function getParamToMA0001BySchedule(transTarget) {
    // 機能ID|フォームNo|タブNo|保全活動ID
    var paramList = transTarget.split('|');
    //return getParamToMA0001(paramList[3], paramList[2]);
    // スケジュール種類|機能ID|フォームNo|タブNo|保全活動ID
    return getParamToMA0001(paramList[4], paramList[3], paramList[0]);
}

/**
 * 入れ子一覧の場合、子一覧の表示/非表示切り替えボタン列を追加
 * @param {any} id 一覧のID(#,_FormNo付き)
 * @param {any} header ヘッダー情報
 * @param {any} parentId 親一覧のID
 * @param {any} childrenId 子一覧のID
 * @param {any} subTableIdList 子一覧のIDを設定するリスト
 */
function setNestedTable(id, header, parentId, childrenId, subTableIdList) {
    if (id.indexOf(parentId) <= 0) {
        return;
    }

    //子一覧ID
    subTableIdList.push(childrenId);

    //アイコン設定
    var icon = function (cell, formatterParams, onRendered) {
        return '<i class="fas fa-ellipsis-v" aria-hidden="true"></i>';//縦三点リーダー
    };

    //一番左に子一覧の表示非表示を切り替えるボタン列を追加
    header.unshift({
        field: "SubTableToggle", formatter: icon, hozAlign: "center", title: "", frozen: true, width: 50, headerSort: false,
        cellClick: function (e, row, formatterParams) {
            const rowNo = row.getData().ROWNO;
            $(".subTable_" + childrenId + '_' + rowNo).toggle();
            //子テーブルの再描画
            var subTable = P_listData['#' + childrenId + '_' + rowNo + getAddFormNo()];
            if (subTable) {
                subTable.redraw();
                subTable = null;
            }
            //親テーブルの再描画
            row.getTable().redraw();
        }
    });
}

/**
 * 入れ子一覧のデータを成型する
 * @param {any} id 一覧のID(#,_FormNo付き)
 * @param {any} parentId 親一覧のID
 * @param {any} childrenId 子一覧のID
 * @param {any} parentKeyVal 親一覧のキー列のVAL値
 * @param {any} childrenKeyVal 子一覧のキー列のVAL値
 */
function convertTabulatorListToNestedTable(id, parentId, childrenId, parentKeyVal, childrenKeyVal) {
    var pTableId = "#" + parentId + getAddFormNo();
    var cTableId = "#" + childrenId + getAddFormNo();
    if (id != pTableId && id != cTableId) {
        //親一覧、子一覧以外は処理なし
        return;
    }

    //親一覧
    var pTable = P_listData[pTableId];
    //子一覧
    var cTable = P_listData[cTableId];

    if (!pTable || !cTable) {
        //親一覧、子一覧のどちらか生成されていない場合は終了
        return;
    }

    //親一覧のデータ
    var pData = pTable.getData();
    //子一覧のデータ
    var cData = cTable.getData();

    if (!pData || pData.length <= 0) {
        //親一覧のデータが無い場合終了
        pTable = null;
        cTable = null;
        return;
    }

    var pSubData = $.grep(pData, function (obj, index) {
        return (obj.SubData);
    });
    if (pSubData.length > 0) {
        //親データに子データが入れ子になっていれば終了
        pTable = null;
        cTable = null;
        return;
    }

    if (!cData || cData.length <= 0) {
        //子一覧のデータが無い場合終了
        pTable = null;
        cTable = null;
        return;
    }

    //親データに"SubData"をキーに子データを入れ子にする
    $.each(pData, function (i, pRowData) {
        //親データのキー値
        var key = pRowData["VAL" + parentKeyVal];

        var childrenData = $.grep(cData, function (obj, index) {
            //親データに紐づく子データを取得
            return (obj["VAL" + childrenKeyVal] == key);
        });
        pRowData["SubData"] = childrenData;
    });

    pTable.setData(pData);
    pTable = null;
    cTable = null;
}

/**
 * テキストボックスのIDを取得
 * @param {any} ctrlId コントロールID
 * @param {any} val VAL値
 */
function getTextBoxId(ctrlId, val) {
    return ctrlId + getAddFormNo() + "VAL" + val;
}

/**
 * 担当者項目にユーザマスタから取得できないユーザ名を設定
 * @param {any} ele コード+翻訳の要素
 * @param {any} name 表示する値
 */
function setNameToCodeTrans(ele, name) {
    //翻訳を設定するspan
    var span = $(ele).parent().find('.honyaku');
    //値を設定
    $(span).text(name);
    //ラベル表示用spanにも設定
    $(ele).closest("td,.tabulator-cell").find("span.labeling").text(name);
}

/**
 * コンボボックスの拡張項目の値を指定して、その要素を選択する処理
 * @param {any} ctrlId コンボのある一覧のID
 * @param {any} valNo コンボのVAL値
 * @param {any} paramNo 拡張項目の値の番号
 * @param {any} selectValue 拡張項目の選択する値
 */
function selectComboByExparam(ctrlId, valNo, paramNo, selectValue) {
    var combo = getCtrl(ctrlId, valNo, 0, CtrlFlag.Combo);
    var paramName = "exparam" + paramNo;
    $(combo).find('option').each(function (index, element) {
        if ($(element).attr(paramName) == selectValue) {
            selectComboIdx(combo, index);
        }
    });
}

/**
 * 条件エリアを一覧定義情報に追加する処理
 * @param {any} listDefines 一覧定義情報、これに追加して返す
 * @param {any} transPtn 画面の表示パターン Common.jsのtransPtnDef
 * @param {any} formNo 表示する画面のフォーム番号
 * @param {any} parentFormNo transPtnが画面遷移なし以外の場合、遷移元の画面のフォーム番号
 * @returns {any} listDefinesに条件エリアの定義を追加したリスト
 */
function addSearchConditionToListDefine(listDefines, transPtn, formNo, parentFormNo) {

    // initFormの処理の真似
    // 条件エリアも同様に追加する
    var areaId = "formSearch" + getAddFormNo();
    var lists = $(P_Article).find("#" + areaId + " .ctrlId:not([id$='edit'],[id$='_tablebase'])");
    if ($(lists).length <= 0) {
        return listDefines;
    }
    searchConditions = [];
    $.each($(lists), function (i, list) {
        // 一覧の画面定義条件を生成
        var ctrlId = $(list).attr("data-ctrlid");
        var ctrlType = $(list).attr("data-ctrltype");
        var maxrows = "0";
        var pagerows = "0";

        if (true == $(list).hasClass("vertical_tbl")) {
            // ※縦方向一覧の場合
            maxrows = 1;
            pagerows = 1;
        }
        else {
            //※横方向一覧の場合
            if ($(list).is("[data-maxrows]")) {
                maxrows = $(list).data("maxrows");
            }
            if ($(list).is("[data-pagerows]")) {
                pagerows = $(list).data("pagerows");
            }
            if (ctrlId == null || ctrlId == undefined) {
                return true;    //continue
            }
        }

        var pageNo = '1';
        if (!(transPtn == transPtnDef.Child && parentFormNo >= 0)) {
            //※子画面初期表示 以外の場合は現在ﾍﾟｰｼﾞ番号を取得
            pageNo = getCurrentPageNo(ctrlId, 'def');
        }

        var tmpData = {
            CTRLID: ctrlId,                 // 一覧の画面定義のコントロールID
            FORMNO: formNo,                 // 画面NO
            CTRLTYPE: ctrlType,             // 一覧のｺﾝﾄﾛｰﾙﾀｲﾌﾟ
            VAL1: pageNo,                   // 現在ﾍﾟｰｼﾞ番号（先頭ﾍﾟｰｼﾞ）
            VAL2: maxrows,   				// 一覧の画面定義の出力最大件数
            VAL3: pagerows,  				// 一覧の画面定義の1ページ行数
            VAL4: '',                       // 一覧のソートキー
        };
        searchConditions.push(tmpData);
    });

    var resultList = listDefines.concat(searchConditions);

    return resultList;
}

/**
 * 保全活動一覧画面への画面遷移パラメータを設定する処理（検索条件：系停止）
 * @param {string} startDate 開始日
 * @param {string} endDate 終了日
 * @param {string} jobCode 職種コード(10：機械、20：電気、30：計装、99:その他、null：全て)
 * @param {string} stopSystem 系停止(10：保全要因、20：製造要因、10|20：保全要因と製造要因両方)
 */
function getParamToMA0001Maintenance(startDate, endDate, jobCode, stopSystem) {
    //MQ分類：拡張データ2(「設備工事」または「撤去工事」を除く)
    var mqClass = '10|20|30|40';
    return getParamToMA0001FromMP0001(startDate + '|' + endDate, jobCode, mqClass, stopSystem, "");
}

/**
 * 保全活動一覧画面への画面遷移パラメータを設定する処理（検索条件：故障修理件数）
 * @param {string} startDate 開始日
 * @param {string} endDate 終了日
 * @param {string} jobCode 職種コード(10：機械、20：電気、30：計装、99:その他、null：全て)
 * @param {string} mqClass MQ分類(1、2、3、4、5：識別コード(拡張データ4))
 */
function getParamToMA0001Failure(startDate, endDate, jobCode, mqClass) {
    //MQ分類：拡張データ2と拡張データ4
    var paramMqClass = '20||' + mqClass;
    return getParamToMA0001FromMP0001(startDate + '|' + endDate, jobCode, paramMqClass, "", "");
}

/**
 * 保全活動一覧画面への画面遷移パラメータを設定する処理（検索条件：作業計画性分類）
 * @param {string} startDate 開始日
 * @param {string} endDate 終了日
 * @param {string} jobCode 職種コード(10：機械、20：電気、30：計装、99:その他、null：全て)
 * @param {string} mqClass MQ分類(10、20、30、40：作業性格分類コード(拡張データ2))
 * @param {string} sudden 突発区分(10：計画、20：計画外、30：突発)
 */
function getParamToMA0001WorkPlanning(startDate, endDate, jobCode, mqClass, sudden) {
    return getParamToMA0001FromMP0001(startDate + '|' + endDate, jobCode, mqClass, "", sudden);
}

/**
 * 保全活動一覧画面への画面遷移パラメータを設定する処理（検索条件：作業性格分類）
 * @param {string} startDate 開始日
 * @param {string} endDate 終了日
 * @param {string} jobCode 職種コード(10：機械、20：電気、30：計装、99:その他、null：全て)
 * @param {string} mqClass MQ分類(10、20、30、40：作業性格分類コード(拡張データ2))
 */
function getParamToMA0001WorkPersonality(startDate, endDate, jobCode, mqClass) {
    return getParamToMA0001FromMP0001(startDate + '|' + endDate, jobCode, mqClass, "", "");
}

/**
 * 保全活動一覧画面への画面遷移パラメータを設定する処理（検索条件：その他）
 * @param {string} startDate 開始日
 * @param {string} endDate 終了日
 * @param {string} jobCode 職種コード(10：機械、20：電気、30：計装、99:その他、null：全て)
 */
function getParamToMA0001Other(startDate, endDate, jobCode) {
    //MQ分類：拡張データ2(「設備工事」または「撤去工事」を除く)
    var mqClass = '10|20|30|40';
    return getParamToMA0001FromMP0001(startDate + '|' + endDate, jobCode, mqClass, "", "", "1|");
}

/**
 * 保全実績評価から保全活動一覧画面への画面遷移パラメータを設定する処理
 * @param {string} date 完了日(開始|終了)
 * @param {string} jobCode 職種コード(10：機械、20：電気、30：計装、99:その他、null：全て)
 * @param {string} MQ分類 「|」と「||」区切りで指定(拡張データ2、拡張データ4)
 * @param {string} stopSystem 系停止(10：保全要因、20：製造要因、10|20：保全要因と製造要因両方)
 * @param {string} sudden 突発区分(10：計画、20：計画外、30：突発)
 * @param {string} callCount 呼出回数
 */
function getParamToMA0001FromMP0001(date, jobCode, mqClass, stopSystem, sudden, callCount) {
    var conditionDataList = [];
    //下記VAL値は保全活動の一覧（BODY_020_00_LST_0）と対応
    var conditionData = {};
    conditionData['CTRLID'] = 'MA0001_DetailSearchCondition';
    conditionData['VAL2'] = date; //完了日
    conditionData['VAL2_checked'] = true;
    conditionData['VAL4'] = mqClass; //MQ分類 例：10|20||2|3(拡張データ2[10or20]且つ拡張データ4[2or3])
    conditionData['VAL4_checked'] = true;
    conditionData['VAL13'] = jobCode ? jobCode : JobAll; //職種
    conditionData['VAL13_checked'] = false;
    conditionData['VAL17'] = stopSystem; //系停止
    conditionData['VAL17_checked'] = stopSystem ? true : false;
    conditionData['VAL20'] = sudden; //突発区分
    conditionData['VAL20_checked'] = sudden ? true : false;
    conditionData['VAL81'] = callCount; //呼出回数
    conditionData['VAL81_checked'] = callCount ? true : false;
    conditionDataList.push(conditionData);
    return conditionDataList;
}

/**
 * 詳細検索条件への値設定
 * @param {any} setData 検索条件
 */
function setDetailSearchCondition(setData) {
    var tables = $('nav#detail_search table.detailsearch');
    if (tables == null || tables.length == 0) { return; }

    $.each($(tables), function (i, tbl) {
        // 値を設定
        var trs = $(tbl).find('tr');
        $.each($(trs), function (i, tr) {
            const valNo = 'VAL' + $(tr).data('itemno');
            // チェック状態を設定
            const checked = setData[valNo + '_checked'];
            $(tr).find('td.select input[type="checkbox"]').prop('checked', convertStrToBool(checked));

            // 値を条件コントロールへ設定
            const val = setData[valNo];
            if (!val) { return true; }

            const td = $(tr).find('td.input_td');
            // 複数選択チェックボックス
            var msul = $(td).find("ul.multiSelect");
            if (msul.length > 0) {
                // 選択状態をクリア
                var checks = $(msul).find('> li:not(.hide) :checkbox');
                $.each(checks, function () {
                    //選択状態を解除
                    $(this).prop('checked', false);
                });
                //値セット
                setAttrByNativeJs(msul, 'data-value', val);
                setDataForMultiSelect(td, msul, val);
                return true;
            }

            // チェックボックス
            var checkbox = $(td).find(":checkbox");
            if (checkbox.length > 0) {
                $(checkbox).prop("checked", val == 1);
                return true;
            }

            //ラジオボタン
            var radio = $(td).find(":radio");
            if (radio.length > 0) {
                $(radio).find('[value="' + val + '"]').prop('checked', true);
                return true;
            }

            //ﾃｷｽﾄ、数値、ｺｰﾄﾞ＋翻訳
            var input = $(td).find('input[type="text"], input[type="hidden"]');
            if (input.length > 0) {
                if (input.length == 1) {
                    $(input).val(val);
                } else {
                    var vals = val.split('|');
                    $(input[0]).val(vals[0]);
                    if (vals.length > 1) {
                        $(input[1]).val(vals[1]);
                    }
                }
                return true;
            }

            // 日付（ブラウザ標準）
            var input = $(td).find("input[type='date']");
            if (input != null && input.length > 0) {
                setDataForTypeDate(td, input, val);   //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日付）
                return true;
            }

            // 時刻（ブラウザ標準）
            var input = $(td).find("input[type='time']");
            if (input != null && input.length > 0) {
                setDataForTypeTime(td, input, val);   //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準時刻）
                return true;
            }

            // 日時（ブラウザ標準）
            var input = $(td).find("input[type='datetime-local']");
            if (input != null && input.length > 0) {
                setDataForTypeDatetimelocal(td, input, val);  //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日時）
                return true;
            }
        });
    });
}


/**
 * 小数点以下桁数 丸め処理
 * @param {any} val 対象数値
 * @param {any} digitNum 小数点以下桁数
 * @param {any} roundDivision 丸め処理区分
 */
function roundDigit(val, digitNum, roundDivision) {

    // 数値ではない場合
    if (isNaN(val)) {
        // 値をそのまま返す
        return val;
    }

    var calcVal = 0;
    var roundNum = Math.pow(10, digitNum);

    // 丸め処理区分を判定
    if (roundDivision == RoundDivision.Round) {
        // 四捨五入
        calcVal = Math.round(val * roundNum) / roundNum;
    }
    else if (roundDivision == RoundDivision.Ceiling) {
        // 切り上げ
        calcVal = digitNum == 0 ? Math.ceil(val) : Math.ceil(val * roundNum) / roundNum;
    }
    else if (roundDivision == RoundDivision.Floor) {
        // 切り捨て
        calcVal = digitNum == 0 ? Math.floor(val) : Math.floor(val * roundNum) / roundNum;
    }
    else {
        // 値をそのまま返す
        return val;
    }

    // 3桁カンマ区切り
    return calcVal.toLocaleString(undefined, { maximumFractionDigits: digitNum });
}

/**
 * 数量と単位を結合する
 * @param {any} val 数量
 * @param {any} unit 単位
 * @param {any} isUseSpace 結合に半角スペースを使用する場合はtrue
 */
function combineNumberAndUnit(val, unit, isUseSpace) {

    if (val.length <= 0) {
        return val;
    }

    // 結合時に半角スペースを含めるか判定
    if (isUseSpace) {
        return val + ' ' + unit;
    }
    else {
        return val + unit
    }
}

/**
 * 明細（tabulator一覧）の入力値が変更されているかチェックする
 * @param {string} ctrlId 一覧の項目ID
 * @param {string} isSelectedOnly 選択行のみ判定する場合はTRUE
 * @return 変更されている場合True、変更なしの場合False
 */
function isChangeList(ctrlId, isSelectedOnly) {
    var flg = false;

    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (table) {
        var trs = table.getRows();

        if (trs != null && trs.length > 0) {
            // 検索結果一覧を参照し、変更されている（UPDTAG=1）かどうかをチェック
            $(trs).each(function (i, tr) {
                var rowData = tr.getData();
                if (isSelectedOnly && rowData.SELTAG != 1) {
                    // 選択行のみを取得する場合は、選択の値が1でなければcontinue
                    return true;
                }

                var ele = tr.getCell('UPDTAG').getElement();
                if ($(ele).find("input[data-name='UPDTAG']").val()) {
                    flg = true;
                    return false;
                }
                ele = null;
            });
        }
        table = null;
    }
    return flg;
}


// 変更管理 処理モード
const ProcessMode =
{
    Transaction: "0", // トランザクションモード
    History: "1"      // 変更管理モード
}

/**
 * 変更管理 背景色変更共通処理
 * @param {any} tbl                        :検索結果
 * @param {any} applicationDivisionCodeVal :申請区分の拡張項目を表示している列の項目番号
 * @param {any} valueChangedVal            :変更のあった項目を表示している列の項目番号
 * @param {any} columnNoList               :項目名と項目番号(検索結果の項目名をキーに、項目番号を値に定義した連想配列)
 * @param {any} rows                       :フィルター適応後の行情報
 */
function commonChangeBackGroundColorHistory(tbl, applicationDivisionCodeVal, valueChangedVal, columnNoList, rows) {

    // 検索結果のレコードを取得
    var table = tbl.getRows("active");
    if (rows) {
        table = rows;
    }

    // 取得できなければ何もしない
    if (!$(table).length) {
        return;
    }

    $.each($(table), function (idx, row) {

        // 申請区分(拡張項目)の値を取得
        var data = row.getData();
        var applicationDivisionCode = data["VAL" + applicationDivisionCodeVal];

        // 背景色を変更
        if (applicationDivisionCode == ApplicationDivision.New || applicationDivisionCode == ApplicationDivision.Delete) {
            // 申請区分が「10：新規登録申請」、「30：削除申請」
            setTimeout(function () {
                // 背景色変更処理
                changeBackGroundColorTabulatorHistory(row, applicationDivisionCode);
            }, 0);

        }
        else if (applicationDivisionCode == ApplicationDivision.Update) {

            // 申請区分が「20：変更申請」
            // 変更のあった項目名を取得(「MachineNo_10|MachineName_30」 のようにパイプ「|」区切りになっているので分割)
            var valueChanged = data["VAL" + valueChangedVal].split("|");

            valueChanged.forEach(function (columnDetail, colIdx) {

                // 変更のあった項目の列名が存在する場合
                if ((columnDetail.trim()).length) {

                    // 背景色を変更する項目名(列名と申請区分に分割) ※JavaScriptで定義している項目名とSQLで取得する項目名は統一させる
                    var colName = columnDetail.split("_");

                    if (columnNoList[colName[0]]) {
                        setTimeout(function () {
                            // 背景色変更処理
                            changeBackGroundColorTabulatorHistory(row, colName[1], columnNoList[colName[0]]);
                        }, 200);
                    }
                }
            }, row);
        }
    });
    table = null;
}

/**
 * 変更管理 申請区分に応じて背景色の色を変更(Tabulator)
 * @param {any} row                     :対象行
 * @param {any} applicationDivisionCode :申請区分(10：新規登録申請、20：変更申請、30：削除申請)
 * @param {any} val                     :項目番号
 */
function changeBackGroundColorTabulatorHistory(row, applicationDivisionCode, val) {

    // 申請区分に応じて背景色スタイルを設定
    var backGroundStyle = BackGroundStyleInfo[applicationDivisionCode];

    // 項目番号があれば指定のセル、無ければ行全体
    if (val) {
        // セルを取得
        var cell = row.getCell("VAL" + val).getElement();
        // セルの背景色を変更
        $(cell).addClass(backGroundStyle);
    }
    else {
        // 行全体の背景色を変更
        var ele = row.getElement();
        $(ele).addClass(backGroundStyle);
    }
}

/**
 * 変更管理 申請区分に応じて背景色の色を変更(縦一覧)
 * @param {any} ctrlId                  :一覧のコントロールグループID
 * @param {any} val                     :項目番号
 * @param {any} applicationDivisionCode :申請区分(10：新規登録申請、20：変更申請、30：削除申請)
 */
function changeBackGroundColorHistory(ctrlId, val, applicationDivisionCode) {

    // 申請区分に応じて背景色スタイルを設定
    var backGroundStyle = BackGroundStyleInfo[applicationDivisionCode];

    // 背景色変更対象のセルを取得
    var cell = getCtrl(ctrlId, val, 1, CtrlFlag.Label, false, false);

    // セルの背景色を変更
    $(cell).addClass(backGroundStyle);
}

// 変更管理詳細画面 ボタンコントロールID
const HistoryFormDetailCommonButton =
{
    CopyRequest: "CopyRequest",                             // 複写申請
    ChangeRequest: "ChangeRequest",                         // 変更申請
    DeleteRequest: "DeleteRequest",                         // 削除申請
    ChangeApplicationRequest: "ChangeApplicationRequest",   // 承認依頼
    EditRequest: "EditRequest",                             // 修正
    CancelRequest: "CancelRequest",                         // 取消
    PullBackRequest: "PullBackRequest",                     // 引戻
    ChangeApplicationApproval: "ChangeApplicationApproval", // 承認
    ChangeApplicationDenial: "ChangeApplicationDenial",     // 否認
    BeforeChange: "BeforeChange",                           // 変更前
    Back: "Back"                                            // 戻る
}
/**
 * 変更管理 詳細画面ボタン非表示 制御
 * @param {any} isTransactionMode                :トランザクションモードの場合True、変更管理モードの場合False
 * @param {any} applicationStatusCode            :申請状況(10：申請データ作成中、20：承認依頼中、30：差戻中、40：承認済み)
 * @param {any} applicationDivisionCode          :申請区分(10：新規登録申請、20：変更申請、30：削除申請)
 * @param {any} isCertified                      :申請の申請者またはシステム管理者の場合True、それ以外はFalse
 * @param {any} isCertifiedFactory               :工場の承認ユーザの場合True
 */
function commonButtonHideHistory(isTransactionMode, applicationStatusCode, applicationDivisionCode, isCertified, isCertifiedFactory) {

    // 全てのボタンを非表示状態にする
    Object.keys(HistoryFormDetailCommonButton).forEach(function (button, idx) {
        setHideButton(button, true);
    });


    // 条件によりボタンを表示する

    // 複写申請、変更申請・削除申請
    // ①トランザクションモードの場合
    if (isTransactionMode) {
        setHideButton(HistoryFormDetailCommonButton.CopyRequest, false);
        setHideButton(HistoryFormDetailCommonButton.ChangeRequest, false);
        setHideButton(HistoryFormDetailCommonButton.DeleteRequest, false);
    }

    // 承認依頼、申請内容取消
    // ①申請状況が「10：申請データ作成中」かつ、申請の申請者(システム管理者含む)である場合
    // ②申請状況が「30：差戻中」かつ、申請の申請者(システム管理者含む)である場合
    if (!isTransactionMode && ((applicationStatusCode == ApplicationStatus.Making && isCertified) || (applicationStatusCode == ApplicationStatus.Return && isCertified))) {
        setHideButton(HistoryFormDetailCommonButton.ChangeApplicationRequest, false);
        setHideButton(HistoryFormDetailCommonButton.CancelRequest, false);
    }

    // 申請内容修正
    // ①申請状況が「10：申請データ作成中」かつ、申請の申請者(システム管理者含む)であるかつ、申請区分が「30：削除申請」以外の場合
    // ②申請状況が「30：差戻中」かつ、申請の申請者(システム管理者含む)であるかつ、申請区分が「30：削除申請」以外の場合
    if (!isTransactionMode &&
        ((applicationStatusCode == ApplicationStatus.Making && isCertified && applicationDivisionCode != ApplicationDivision.Delete)
            || (applicationStatusCode == ApplicationStatus.Return && isCertified && applicationDivisionCode != ApplicationDivision.Delete))) {
        setHideButton(HistoryFormDetailCommonButton.EditRequest, false);
    }

    // 承認依頼引戻
    // ①申請状況が「20：承認依頼中」かつ、申請の申請者(システム管理者含む)である場合
    if (!isTransactionMode && applicationStatusCode == ApplicationStatus.Request && isCertified) {
        setHideButton(HistoryFormDetailCommonButton.PullBackRequest, false);
    }

    // 承認・否認
    // ①申請状況が「20：承認依頼中」かつ、工場の承認ユーザである場合
    if (!isTransactionMode && applicationStatusCode == ApplicationStatus.Request && isCertifiedFactory) {
        setHideButton(HistoryFormDetailCommonButton.ChangeApplicationApproval, false);
        setHideButton(HistoryFormDetailCommonButton.ChangeApplicationDenial, false);
    }

    // 変更前
    // ①変更管理モード(新規登録申請以外)の場合
    if (!isTransactionMode && applicationDivisionCode != ApplicationDivision.New) {
        setHideButton(HistoryFormDetailCommonButton.BeforeChange, false);
    }

    // 「戻る」ボタンを表示
    setHideButton(HistoryFormDetailCommonButton.Back, false);
}

/**
 * 変更管理 ボタンを非表示にする
 * @param {any} button ボタン要素
 * @param {any} isHide 非表示にする場合はTrue
 */
function dispNoneElementHistory(button, isHide) {

    // .hide() だと親要素のwidthが残って隙間ができるため以下の方法で非表示にする

    // フラグを判定
    if (isHide) {
        // ボタンの親要素を非表示にする
        $(button).parent().css("display", "none");
    }
    else {
        // ボタンの親要素を表示する
        $(button).parent().css("display", "");
    }
}

/**
 * 変更管理関連のコントロールの表示制御
 * @param {any} isHistoryManagement 変更管理の場合はTrue
 * @param {any} historyManagementBtnName 変更管理ボタンの名前
 * @param {any} hideBtns 変更管理の時に非表示にするボタンの名前のリスト
 * @param {any} hideLists 変更管理の時に入力コントロールを非表示にする一覧のIDのリスト
 * @param {any} isNeedHideRowNo ROWNO列を非表示にする必要がある場合True
 */
function setHistoryManagementCtrlDisplay(isHistoryManagement, historyManagementBtnName, hideBtns, hideLists, isNeedHideRowNo) {
    var historyManagementBtn = getButtonCtrl(historyManagementBtnName);
    dispNoneElementHistory(historyManagementBtn, !isHistoryManagement);

    $.each(hideBtns, function (i, name) {
        var btn = getButtonCtrl(name);
        dispNoneElementHistory(btn, isHistoryManagement);
    });

    $.each(hideLists, function (i, listId) {
        if (isNeedHideRowNo) {
            changeRowControlAndDispRowNo(listId, !isHistoryManagement);
        } else {
            changeRowControl(listId, !isHistoryManagement);
        }
    });
}

/**
 * 画面遷移前のチェック
 * @param {any} appPath アプリケーションルートパス
 * @param {any} formNo 画面No
 * @param {any} conductId 機能ID
 * @param {any} processName 処理のコントロールID(ExecuteImplで分岐する処理名)
 * @param {any} val VAL値
 */
function checkTransition(appPath, formNo, conductId, processName, val, message) {
    var flg = false;
    var eventFunc = function (status, data) {
        if (!data || data.length <= 0) {
            // 処理終了
            return;
        }

        // データ
        var data = data.slice(1);//先頭行削除
        if (!data || data.length <= 0) {
            // 処理終了
            return;
        }
        //チェック結果を取得
        var value = data[0]["VAL" + val];
        flg = convertStrToBool(value);

        if (!flg) {
            setMessage(message, procStatus.Error);
        }
    }
    //Ajaxでチェックを行い、チェック結果（真偽値）を取得する
    ajaxCommon(processName, appPath, formNo, conductId, false, null, eventFunc);
    return flg;
}


/**
 * 変更管理詳細画面 背景色変更
 * @param {any} applicationDivisionCode 申請区分
 * @param {any} columnList 背景色を変更するセルのリスト
 * @param {any} ctrlId 変更のあった項目が設定されている一覧のID
 * @param {any} val 変更のあった項目が設定されている項目番号
 */
function changeBackGroundColorHistoryDetail(applicationDivisionCode, columnList, ctrlId, val) {

    var valueChanged = ""; // 変更のあった項目名

    // 取得した申請区分を判定
    if (applicationDivisionCode == ApplicationDivision.New || applicationDivisionCode == ApplicationDivision.Delete) { // 新規登録申請・削除申請

        // 変更管理対象項目の全セルの背景色を変更
        valueChanged = getValueChangedAllItemForHistory(applicationDivisionCode, columnList);
    }
    else { // 変更申請

        // 変更のあった項目名を取得(「|」区切りになっているので分割)
        valueChanged = getValue(ctrlId, val, 1, CtrlFlag.Label, false, false).split("|");
    }

    // 変更対象項目より背景色を変更
    valueChanged.forEach(function (columnDetail, colIdx) {

        // 変更のあった項目の列名が存在する場合
        if ((columnDetail.trim()).length) {

            // 列名と申請区分に分割
            colName = columnDetail.split("_");

            //列名が一致するものを取得
            var column = $.grep(columnList, function (item, idx) {
                return item.Key == colName[0];
            });
            if (column.length == 0) {
                return false;
            }

            // 背景色変更処理
            changeBackGroundColorHistory(column[0].CtrlId, column[0].Val, colName[1]);
        }
    });
}

/**
 * 変更管理詳細画面 背景色変更時の「変更対象項目」を作成(新規登録申請・削除申請の場合)
 * @param {any} applicationDivisionCode 申請区分
 * @param {any} columnList 背景色を変更するセルのリスト
 */
function getValueChangedAllItemForHistory(applicationDivisionCode, columnList) {
    // 「項目名_背景色設定値」のリストを作成
    var valueChanged = [];
    $.each(columnList, function (i, col) {
        valueChanged.push(col.Key + "_" + applicationDivisionCode);
    });

    return valueChanged;
}

/**
 * 一覧画面のページングを再設定(詳細から一覧へ戻る際に再検索を行わない機能で使用)
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId   ：機能ID
 *  @param pgmId       ：プログラムID
 *  @param ctrlId      ：一覧のコントロールID
 *  @param formNo      ：一覧の画面No
 *  @param allListCountKey：グローバルリストに保存している総件数のキー
 */
function setListPagination(appPath, conductId, pgmId, status, ctrlId, formNo, allListCountKey) {
    // 読込件数エリアの総件数の設定
    var div = $(P_Article).find('div[data-relation-id="' + ctrlId + '"]');
    var label = $(div).find('td[data-name="VAL3"]');
    if (label != null && label.length > 0 && P_dicIndividual[allListCountKey]) {
        var oldCount = $(label).text(); //「/123」のような先頭にスラッシュが付与された値
        if (oldCount.slice(1) == P_dicIndividual[allListCountKey]) {
            //件数の変更はないため処理なし
            return;
        }
        $(label).text('/' + P_dicIndividual[allListCountKey]);
    }

    //一覧画面のデータ
    var table = P_listData["#" + ctrlId + "_" + formNo];
    //ページャーの再設定
    if (table) {
        var count = getTotalRowCount(table);
        setupPagination(appPath, conductId, pgmId, formNo, ctrlId, count);
    }
    //ページャーの総ページ数が1件の場合、ページャーを非表示
    setHidePagination("#" + ctrlId + "_" + formNo, table);

    //メッセージの設定
    if (status != null && status.MESSAGE != null && status.MESSAGE.length > 0) {
        // 削除時の成功メッセージ
        addMessage(status.MESSAGE, status.STATUS);
    }
}

/**
 * 詳細検索条件エリアを表示状態にする
 */
function dispDetailConditionArea() {

    // グローバル変数にキー「InitDispDetailCondition」が格納されているか判定
    // →メニューからの初期表示かつ、有効な詳細検索条件が設定されていない場合はバックエンド側の検索処理でキーが格納される
    if ("InitDispDetailCondition" in P_dicIndividual) {

        // 格納されている場合
        // グローバル変数からキーを削除
        operatePdicIndividual("InitDispDetailCondition", true);

        // 検索結果一覧の虫眼鏡マークをクリックして詳細検索条件エリアを表示状態にする
        $(P_Article).find("a[data-actionkbn='" + actionkbn.ComDetailSearch + "']").click();
    }

    // 格納されていない場合は何もしない(詳細検索条件エリアは非表示のまま)
    return;
}