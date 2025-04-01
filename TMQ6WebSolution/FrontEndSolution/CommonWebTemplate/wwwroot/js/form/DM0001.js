/* ========================================================================
 *  機能名　    ：   【DM0001】文書管理
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)DM0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/DM0002.js\"></script>");

// 一覧のVALの値をJavaScriptではよく使いますが、値をそのまま記載すると後でどの項目を見ているか分からなくなります。
// また、VALの値を詰めるなどで変更することが大変になりますので、まとめて定義してください。

/** 機能ID*/
const ConductId_DM0001 = "DM0001";

/** グローバル変数のキー、一覧画面の表示データを更新する用*/
const DM0001_UpdateListData = "DM0001_UpdateListData";
/** グローバル変数のキー、文書管理画面で添付情報が更新された場合true*/
const DM0001_UpdateAttachmentFlg = "DM0001_UpdateAttachmentFlg";
/** グローバル変数のキー、一覧画面用の総件数*/
const DM0001_AllListCount = "DM0001_AllListCount";
/** グローバル変数のキー、一覧画面の表示データ更新用のキー */
const DM0001_UpdateKey = "DM0001_UpdateKey";
/** グローバル変数のキー、一覧画面の選択行番号 */
const DM0001_SelectedRowNo = "DM0001_SelectedRowNo";

/**
 * 独自のコンボの列フィルター設定用
 * (別のTableのselectタグから取得して生成する場合に定義する)
 */
const OriginalHeaderOptionsField = {
    /** 対象列名*/
    TargetItem: "VAL5", // 文書種類
    /** 取得先テーブルコントロールID*/
    FromCtrlId: "BODY_010_00_LST_0", // 文書管理 文書管理一覧 フィルタ(非表示列)
    /** 取得先列名*/
    FromItem: "VAL2",    // 文書種類(非表示列)
    /** 翻訳で集約されているかどうか(true:集約されている/false:集約されていない)*/
    Marged: true
};

/**
 *  初期化処理(表示中画面用)
 *  @param appPath   　　 ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId 　　 ：機能ID
 *  @param formNo 　　　　：画面番号
 *  @param articleForm    ：表示画面ｴﾘｱ要素
 *  @param curPageStatus  ：画面表示ｽﾃｰﾀｽ
 *  @param actionCtrlId   :Action(ﾎﾞﾀﾝなど)CTRLID
 *  @param data           :初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    // 共通-文書管理詳細画面の初期化処理
    DM0002_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    if (conductId == DM0001_FormList.ConductId) {
        // 一覧画面の場合

        // 「出力」ボタンにフォーカス設定(ボタンを画面に表示しないのでコメント)
        //setFocusButton(DM0001_FormList.ButtonOutputId);

    } else if (conductId == DM0002_FormDetail.ConductId) {
        // 詳細画面の場合
        // 更新データを一覧画面に反映する（再検索を行わず、一覧データに反映）
        DM0002_setUpdateDataForList(conductId, false, false);
   }
}

/**
 *  遷移処理の後
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param formNo      ：遷移元formNo
 *  @param ctrlId      ：遷移元の一覧ctrlid
 *  @param btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param element     ：ｲﾍﾞﾝﾄ発生要素
 */
function postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    // 共通-文書管理詳細画面の遷移後処理
    DM0002_postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);

    if (transTarget == DM0001_FormList.Id) {
        // 一覧画面

        // モーダル画面の文書種類コンボボックスにフォーカスをセット
        setFocusDelay(DM0001_FormList.Id, DM0001_FormList.Document, 0, CtrlFlag.Combo, true);

        // ファイル列・リンク列の表示/非表示 切替処理
        DM0002_changeDisp(DM0001_FormList.ModalId, DM0001_FormList.FileName, DM0001_FormList.File, DM0001_FormList.Link, DM0001_FormList.AttachmentExData, DM0001_FormList.Id, formNo, DM0001_FormList.ConductId);
    }
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

    // 共通-文書管理詳細画面の行削除前処理
    var comRtn = DM0002_preDeleteRow(appPath, btn, id, checkes);
    if (!comRtn) {
        // 処理終了の場合は終了
        return false;
    }

    // 共通処理により非表示の削除ボタンを押下
    return preDeleteRowCommon(id, [DM0001_FormList.Id]);
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
 *  @param pctrlId      ：遷移元の一覧ctrlid
 *  @param btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param element     ：ｲﾍﾞﾝﾄ発生要素
 */
function prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    // 共通-文書管理詳細画面の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecDM0002_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return DM0002_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }

    var conditionDataList = [];

    // 一覧フィルタ処理実施
    if (executeListFilter(transTarget, DM0001_FormList.Id, DM0001_FormList.FilterId, DM0001_FormList.Filter)) {
        return [false, conditionDataList];
    }

    // それ以外の場合は自身の機能の遷移前処理を行う

    if (btn_ctrlId == DM0001_FormList.ButtonOutputId) {

        // 一覧画面で「出力」ボタン押下時、一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(DM0001_FormList.Id)) {
            return [false, conditionDataList]
        }
    }
    return [true, conditionDataList];
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
 *  @param status      ：処理ステータス
*/
function postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data, status) {
    // 共通-文書管理詳細画面の実行正常終了後処理
    DM0002_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    var btnName = $(btn).attr('name');
    if (btnName == DM0002_FormDetail.BtnDeleteId) {
        // 削除ボタンの場合、削除データを一覧画面から削除
        DM0002_setUpdateDataForList(conductId, true, false);
    }

    if (conductId == DM0001_FormList.ConductId) {
        // 一覧画面

        // 「出力」ボタンにフォーカス設定(ボタンを画面に表示しないのでコメント)
        //setFocusButton(DM0001_FormList.ButtonOutputId);

        if (btnName == DM0002_FormDetail.BtnUploadId) {
            // 登録ボタンの場合、更新データを一覧画面へ反映
            DM0002_setUpdateDataForList(conductId, false, false);
            if (status.MESSAGE) {
                // 処理メッセージを一覧画面へ設定
                setMessage(status.MESSAGE, status.STATUS);
            }
        }
    }
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {
    // 共通-文書管理詳細画面の登録前追加条件取得処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecDM0002_AddSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn)) {
        return DM0002_addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);
    }
    // それ以外の場合
    var conditionDataList = [];
    return conditionDataList;
}

/**
 *【オーバーライド用関数】取込処理個別入力チェック
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 */
function preInputCheckUpload(appPath, conductId, formNo) {
    // 取込処理は添付情報のみのため他機能の考慮不要
    return DM0002_preInputCheckUpload(appPath, conductId, formNo);
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
    // 共通-文書管理詳細画面のコンボボックス変更処理
    DM0002_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // 文書管理以外の機能の場合は何もしない
    if (!DM0002_JudgeConductId()) {
        return;
    }

    // 単票の添付種類コンボボックスのアイテムが変更された時に変更されたアイテムに応じてコントロールの表示/非表示を切り替える
    if (ctrlId == DM0001_FormList.Id && valNo == DM0001_FormList.Attachment) {

        // 添付種類コンボボックスの選択されているアイテムの拡張項目を設定する
        setComboExValue(DM0001_FormList.Id, DM0001_FormList.AttachmentExData, CtrlFlag.Label, selected, true);

        // 一覧画面
        DM0002_changeDisp(DM0001_FormList.ModalId, DM0001_FormList.FileName, DM0001_FormList.File, DM0001_FormList.Link, DM0001_FormList.AttachmentExData, DM0001_FormList.Id, DM0001_FormList.No, DM0001_FormList.ConductId);
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
 */
function beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom) {
    // 共通-文書管理詳細画面の画面再表示前処理
    // 共通-文書管理詳細画面を閉じたときの再表示処理はこちらで行うので、各機能での呼出は不要
    DM0002_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
    // 単票画面を閉じた場合
    // 単票で、「登録」ボタンを閉じたときに再検索を行う⇒再検索は行わない
    //InitFormDataByOwnModal(DM0001_FormList.ConductId, DM0001_FormList.No, DM0001_FormList.Id, DM0002_FormDetail.BtnUploadId, appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    if (btnCtrlId == DM0002_FormDetail.BtnBackId) {
        //文書管理画面を閉じた場合
        if (conductId == DM0001_FormList.ConductId && formNo == DM0001_FormList.No) {
            // グローバル変数から削除
            delete P_dicIndividual[DM0001_SelectedRowNo];
           //ページング再設定
            setListPagination(appPath, conductId, pgmId, status, DM0001_FormList.Id, DM0001_FormList.No, DM0001_AllListCount);
        }
    }
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {

    DM0002_postBuitTabulator(tbl, id);

    if (id == '#' + DM0001_FormList.Id + getAddFormNo()) {
        // 一覧画面の場合
        // 一覧フィルタ処理実施
        callExecuteListFilter(DM0001_FormList.Id, DM0001_FormList.FilterId, DM0001_FormList.Filter);

    } else if (id == '#' + DM0002_FormDetail.Id + getAddFormNo()) {

        // 詳細画面の場合
        // 更新データを一覧画面に反映する（再検索を行わず、一覧データに反映。新規登録時に詳細一覧の添付IDの値を参照する為ここで行う）
        DM0002_setUpdateDataForList(getConductId(), false, true);
    }
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
function passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId) {

    // 文書管理詳細画面
    DM0002_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);
}
