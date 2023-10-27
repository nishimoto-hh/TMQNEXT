/* ========================================================================
 *  機能名　    ：   【MS0010】ユーザーメンテナンスマスタ
 * ======================================================================== */

// 機能ID
const ConductId_MS0010 = "MS0010";

// 一覧画面 コントロール項目番号
const MS0010_FormList = {
    ConductId: "MS0010",
    ListNo: 1,                                  // 一覧画面番号
    UserList: {
        Id: "BODY_020_00_LST_0",                // ユーザー一覧
        DeleteFlg: 9                            // 削除フラグ
    },
    EditNo: 2,                                  // 詳細画面番号
    UserInfo: {
        Id: "BODY_030_00_LST_1",                // ユーザー情報
        UserId: 1,                              // ユーザーID
        LoginId: 3,                             // ログインID
        AuthLevel: 4,                           // 権限レベル
        PassWord: 5,                            // パスワード
        Language: 6,                            // 言語
        Delete: 10,                             // 削除
        ExData: 13,                             // 拡張データ
        Flg: 14                                 // 制御用フラグ
    },
    MainFactory: {
        Id: "BODY_040_00_LST_1",                // 本務設定
        UserId: 2,                              // ユーザーID隠し項目
        TabNo: 1                                // タブNo
    },
    Local: {
        Id: "BODY_050_00_LST_1"                 // 所属場所階層設定
    },
    Job: {
        Id: "BODY_060_00_LST_1",                // 所属職種機種階層設定
        Factory: 1                              // 工場
    },
    Auth: {
        Id: "BODY_070_00_LST_1",                // 機能権限設定
        Select: 1,                              // 選択
        ConductGrpId: 4,                        // 機能グループID
        TabNo: 2                                // タブNo
    },
    FirstRowNo: 1
};

// 権限レベル拡張項目
const Auth = {
    Guest: "10",                                // ゲストユーザー
    Privilege: "30",                            // 特権ユーザー
    Admin: "99"                                 // システム管理者
};

// マスタメンテナンス機能グループID
const Master = {
    ConductGrpId: "MS0001"                      // 機能グループID
};

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
function initFormOriginalForMS0010(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    // 詳細画面の場合
    if (conductId == MS0010_FormList.ConductId && formNo == MS0010_FormList.EditNo) {
        // ログインIDにフォーカスをセット
        setFocus(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.LoginId, MS0010_FormList.UserInfo.FirstRowNo, CtrlFlag.TextBox, false);

        // 権限レベル拡張項目取得
        var exData = getValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.ExData, MS0010_FormList.FirstRowNo, CtrlFlag.Label, false, false);

        // 権限レベルコンボボックスのインデックスを非表示項目に設定する
        var comboBox = getCtrl(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.AuthLevel, MS0010_FormList.FirstRowNo, CtrlFlag.Combo, false, false);
        setValue(MS0010_FormList.UserInfo.Id, 14, MS0010_FormList.FirstRowNo, CtrlFlag.Label, comboBox.selectedIndex, false, false);

        // 権限レベルコンボボックス変更時イベント
        var ctrl = getCtrl(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.AuthLevel, MS0010_FormList.FirstRowNo, CtrlFlag.Combo, false, false);
        $(ctrl).change(function () {
            // 要素
            var comboBox = getCtrl(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.AuthLevel, MS0010_FormList.FirstRowNo, CtrlFlag.Combo, false, false);
            // インデックス
            var index = comboBox.selectedIndex;
            // 権限レベル
            var authLevel;
            // 非表示項目のインデックス
            var selectedIndex = getValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.Flg, MS0010_FormList.FirstRowNo, CtrlFlag.Label, false, false);

            // インデックスが「-1」の場合は前回値を設定する(コンボ再作成の場合)
            if (index == -1) {
                comboBox.selectedIndex = selectedIndex;
                return;
            }
            else {
                authLevel = $(comboBox)[0][index].attributes[1].value;
            }

            // 選択されたアイテムのインデックスを設定
            setValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.Flg, MS0010_FormList.FirstRowNo, CtrlFlag.Label, index, false, false);

            // 非表示項目に設定する
            setValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.ExData, MS0010_FormList.FirstRowNo, CtrlFlag.Label, authLevel);

            // 場所階層ツリーリンク表示非表示切替
            treeChangeDisabled(authLevel);

        });

        // 新規で起動時は拡張項目がないので初期表示値のゲストユーザーの拡張項目をセット
        if (exData == '') {
            setValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.ExData, MS0010_FormList.FirstRowNo, CtrlFlag.Label, Auth.Guest);
        }
        else {
            //場所階層ツリーリンク表示非表示切替
            treeChangeDisabled(exData);
            // マスタメンテナンス行選択非活性
            //masterChangeDisabled(exData);
        }

        // 折りたたみリンクの非表示
        var grp = $(P_Article).find("a[data-actionkbn='1211']");
        $(grp).remove();

        // 状況に合わせたタイトルの変更
        changeTittle();

        // パスワードの項目設定
        var password = getCtrl(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.PassWord, MS0010_FormList.FirstRowNo, CtrlFlag.TextBox);
        setAttrByNativeJs(password, "type", "password");
    }
}

/**
 * 修正画面の初期処理
 */
function changeTittle() {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル取得
        var title = $(titleEle).text();
        // 新規・修正
        var status = "";

        // ユーザーIDが無ければ新規登録
        var userId = getValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.UserId, MS0010_FormList.FirstRowNo, CtrlFlag.Label, false, false);
        if (userId == "") {
            status = P_ComMsgTranslated[111200003]; // 登録
        } else {
            status = P_ComMsgTranslated[111120007]; // 修正
        }
        // タイトル編集
        $(titleEle).text(title + status);
    }
}

/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginalForMS0010(tabNo, tableId) {

    // 権限レベル拡張項目取得
    var exData = getValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.ExData, MS0010_FormList.FirstRowNo, CtrlFlag.Label, false, false);

    // 所属設定タブの場合
    if (tabNo == MS0010_FormList.MainFactory.TabNo) {
        //場所階層ツリーリンク表示非表示切替
        treeChangeDisabled(exData);
    }
    // 機能権限設定タブの場合
    else if (tabNo == MS0010_FormList.Auth.TabNo) {
        // マスタメンテナンス行選択非活性
        //masterChangeDisabled(exData);
    }
}

/**場所階層ツリーリンク表示非表示切替
 * */
function treeChangeDisabled(exData) {
    // 所属場所階層のツリーリンク
    var getLocalLink = $(P_Article).find("#" + MS0010_FormList.Local.Id + getAddFormNo() + "_div .tbl_title").find("a[data-actionkbn='1260']");

    // 所属職種機種のツリーリンク
    var getJobLink = $(P_Article).find("#" + MS0010_FormList.Job.Id + getAddFormNo() + "_div .tbl_title").find("a[data-actionkbn='1260']");
    // システム管理者であれば場所階層、機種、権限設定非活性
    if (exData == Auth.Admin) {
        // 非表示
        changeCtrlDisplay($(getLocalLink), false);
        changeCtrlDisplay($(getJobLink), false);
    }
    else {
        // 再表示
        changeCtrlDisplay($(getLocalLink), true);
        changeCtrlDisplay($(getJobLink), true);
    }
}

/**機能権限設定マスタメンテナンス表示非表示切替
 * */
function masterChangeDisabled(exData) {

    // 一覧データ取得
    var tbl = P_listData['#' + MS0010_FormList.Auth.Id + getAddFormNo()];
    if (tbl) {
        var trs = tbl.getRows();
        $(trs).each(function (i, tr) {

            // 選択コントロール取得
            //var select = getChildCtrlByFlg(ele, CtrlFlag.Label);
            var select = getCtrl(MS0010_FormList.Auth.Id, MS0010_FormList.Auth.Select, i, CtrlFlag.ChkBox);

            // 機能グループID取得
            //var grpId = getSiblingsValue(ele, MS0010_FormList.Auth.ConductGrpId, CtrlFlag.Label);
            var grpId = getValue(MS0010_FormList.Auth.Id, MS0010_FormList.Auth.ConductGrpId, i, CtrlFlag.Label, false, false);

            // 機能グループIDがMS0001かつシステム管理者の場合、選択行活性
            if (grpId == Master.ConductGrpId) {
                if (exData == Auth.Admin) {
                    // 選択行活性
                    changeInputControl(select, true);
                }
                else {
                    // システム管理者でない場合
                    // 既に選択済みの場合はチェックを外す
                    setValue(MS0010_FormList.Auth.Id, MS0010_FormList.Auth.Select, i, CtrlFlag.ChkBox, 0);
                    // 選択行非活性
                    changeInputControl(select, false);
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
function postBuiltTabulatorForMS0010(tbl, id) {

    // 機能権限設定の場合
    if (id == '#' + MS0010_FormList.Auth.Id + getAddFormNo()) {
        var rows = tbl.getRows();
        $.each(rows, function (i, row) {
            // 隠し項目の選択を取得
            var hideRowNo = row.getData().VAL1;
            if (hideRowNo == 1) {
                //行番号
                var rowNo = row.getData().ROWNO;
                //SELTAGに選択状態を設定
                var item = { ROWNO: rowNo, SELTAG: 1 };
                tbl.updateData([item]);
            }
        });
    }
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
function afterSelectBtnForTreeViewForMS0010(appPath, btn, ctrlId, structureGrpId, maxStructureNo, node) {
    if (structureGrpId == structureGroupDef.LocationForUserMst) {
        // 所属場所階層選択時、職種をクリアする
        //所属職種階層設定
        var targetDiv = $(P_Article).find('div.ctrlId[data-ctrlid="' + MS0010_FormList.Job.Id + '"]');
        // 先頭行以外削除
        $(targetDiv).find("div.vertical_list:gt(0)").remove();
        // 先頭行の選択値をクリア
        var tds = $(targetDiv).find("div.vertical_list").find('td');
        $.each(tds, function (index, td) {
            setStructureInfoToTreeLabel(td, '', '');
        });
    }
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
    // 詳細画面の場合
    if (conductId == MS0010_FormList.ConductId && formNo == MS0010_FormList.EditNo) {
        // ボタン押下時メッセージを一旦クリア
        setAttrByNativeJs(btn, 'data-message', '');

        if (dataEditedFlg) {
            // 画面編集フラグONの場合
            var isDelete = getValue(MS0010_FormList.UserInfo.Id, MS0010_FormList.UserInfo.Delete, MS0010_FormList.FirstRowNo, CtrlFlag.ChkBox, false, false);
            if (isDelete) {
                // 削除にチェックが入っている場合、確認メッセージを変更
                // 『削除してよろしいですか？』
                setAttrByNativeJs(btn, 'data-message', P_ComMsgTranslated[941110001]);
            }
        }
    }
    return true;
}
