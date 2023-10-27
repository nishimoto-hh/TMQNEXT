/* ========================================================================
 *  機能名　    ：   【MP0001】保全実績評価
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)MP0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");

// 機能ID
const ConductId_MP0001 = "MP0001";

// 保全実績評価の定義
// 指定対象年月の列情報
const TargetDateRow = {
    Months: 1
};

// 概要の列情報
const OverviewRow = {
    Item: 1, StopSystem: 2, StopTime: 3, Malfunction1: 4, Malfunction2: 5, 
    Repair: 6, Number: 7,Workrate: 8, CallCnt: 9
};

// 詳細の列情報
const DetailRow = {
    Item1: 1, Item2: 2, Guide: 3, Term: 4, Machine: 5,
    Electricity: 6, Instrumentation: 7, Other: 8, Total: 9
};

// 指定対象年月の定義
const TargetDateList = {
    No: 0
    , List: { Id: "BODY_020_00_LST_0" } // 一覧
}

// 概要の定義
const OverviewList = {
    No: 0
    , List: { Id: "BODY_040_00_LST_0" } // 一覧
}

// 詳細の定義
const DetailList = {
    No: 0
    , StopSystemList: { Id: "BODY_050_00_LST_0" } 
    , RepairsList: { Id: "BODY_060_00_LST_0" } 
    , WorkPlanList1: { Id: "BODY_070_00_LST_0" } 
    , WorkPlanList2: { Id: "BODY_080_00_LST_0" }
    , WorkPersonalityList: { Id: "BODY_090_00_LST_0" } 
    , OthersList: { Id: "BODY_100_00_LST_0" } 
}

// 職種コード(10：機械、20：電気、30：計装、99:その他、null：全て)
const JobCode = {
    Machine: "10"
    , Electricity: "20"
    , Instrumentation: "30"
    , Other: "99"
    , All: null
}

// 系停止(10：保全要因、20：製造要因、10|20：保全要因と製造要因両方)
const StopSystemCode = {
    Maintenance: "10"
    , Manufacture: "20"
    , All: "10|20"
}

// MQ分類(1、2、1|2、3、4、3|4、5、null(全て)：識別コード(拡張データ4))
const MqClassCodeRepairs = {
    TbmFailure1: "1"
    , CbmFailure1: "2"
    , Failure1: "1|2"
    , CbmFailure2: "3"
    , BdmFailure2: "4"
    , Failure2: "3|4"
    , PredictiveRepair: "5"
    , All: "1|2|3|4|5"
}

// MQ分類(10、20|30|40、10|20|30|40：識別コード(拡張データ2))
const MqClassCodeWorkPlan = {
    PreventiveMaintenance: "10"
    , Other: "20|30|40"
    , All: "10|20|30|40"
}

// 突発区分(10：計画、20：計画外、30：突発)
const SuddenClassCode = {
    Plan: "10"
    , UnPlanned: "20"
    , Sudden: "30"
    , UnPlannedSudden: "20|30"
    , All: "10|20|30"
}

// MQ分類(10、20、30、40、10|20|30|40：作業性格分類コード(拡張データ2))
const MqClassCodeWorkPersonality = {
    Preventive: "10"
    , Failure: "20"
    , Other: "30"
    , Manufacture: "40"
    , All: "10|20|30|40"
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
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    //*対象年月度初期値設定
    //前日日付取得
    var datetime = getBeforeYMDString(new Date());

    //画面表示
    setValue(TargetDateList.List.Id, TargetDateRow.Months, 1, CtrlFlag.Input, datetime.slice(0, 7));
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
    // 検索条件
    var conditionDataList = [];
    // exData判定用
    let exDataJudge, exDataValue, exDataJudge2, exDataValue2;
    // startDate判定用
    let sDateJudge, stDateRowno;

    // 作業計画性分類の場合はTrue
    var planFlg = false;
    // その他の場合はTrue
    var otherFlg = false;

    switch (ctrlId) {
        // 系の停止の場合
        case DetailList.StopSystemList.Id:
            // exData
            // rowNo判定に使用する、該当する行番号を配列の有無で判定する
            exDataJudge = { Maintenance: ["1", "2", "3","10", "11", "12"], Manufacture: ["4", "5", "6"], All: ["7", "8", "9"] };
            // 設定するexDataの値の紐づけに使用する、キーはexDataJudgeと同じにする
            exDataValue = {
                Maintenance: StopSystemCode.Maintenance, // 保全要因
                Manufacture: StopSystemCode.Manufacture, // 製造要因
                All: StopSystemCode.All                  // 全て
            };

            // rowNo
            // exDataと同じ理屈
            sDateJudge = { Month: ["1", "4", "7", "10"], Half: ["2", "5", "8", "11"], Year: ["3", "6", "9", "12"] };
            stDateRowno = { Month: "1", Half: "2", Year: "3" };
            break;
        // 故障修理件数の場合
        case DetailList.RepairsList.Id:
            // exData
            // rowNo判定に使用する、該当する行番号を配列の有無で判定する
            exDataJudge = {
                TbmFailure1: ["1", "2", "3"], CbmFailure1: ["4", "5", "6"], Failure1: ["7", "8", "9"],
                CbmFailure2: ["10", "11", "12"], BdmFailure2: ["13", "14", "15"], Failure2: ["16", "17", "18"],
                PredictiveRepair: ["19", "20", "21"], All: ["22", "23", "24"]
            };
            // 設定するexDataの値の紐づけに使用する、キーはexDataJudgeと同じにする
            exDataValue = {
                TbmFailure1: MqClassCodeRepairs.TbmFailure1,           // 故障1(TBM故障)
                CbmFailure1: MqClassCodeRepairs.CbmFailure1,           // 故障1(CBM想定外故障)
                Failure1: MqClassCodeRepairs.Failure1,                 // 故障1
                CbmFailure2: MqClassCodeRepairs.CbmFailure2,           // 故障2(CBM想定外故障)
                BdmFailure2: MqClassCodeRepairs.BdmFailure2,           // 故障2(BDM故障)
                Failure2: MqClassCodeRepairs.Failure2,                 // 故障2
                PredictiveRepair: MqClassCodeRepairs.PredictiveRepair, // 予知修理
                All: MqClassCodeRepairs.All                            // 全て
            };

            // rowNo
            // exDataと同じ理屈
            sDateJudge = {
                Month: ["1", "4", "7", "10", "13", "16", "19", "22"],
                Half: ["2", "5", "8", "11", "14", "17", "20", "23"],
                Year: ["3", "6", "9", "12", "15", "18", "21", "24"]
            };
            stDateRowno = { Month: "1", Half: "2", Year: "3" };
            break;
        // 作業計画性分類の場合
        case DetailList.WorkPlanList1.Id:
            // exData
            // rowNo判定に使用する、該当する行番号を配列の有無で判定する
            exDataJudge = {
                PreventiveMaintenance: ["1", "2", "3"],
                Other: ["4", "5", "6"],
                All: ["7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21"]
            };
            // 設定するexDataの値の紐づけに使用する、キーはexDataJudgeと同じにする
            exDataValue = {
                PreventiveMaintenance: MqClassCodeWorkPlan.PreventiveMaintenance,           // 計画作業件数
                Other: MqClassCodeWorkPlan.Other,                                           // 計画外作業件数
                All: MqClassCodeWorkPlan.All                 　　　　　　　　　　　　       // 作業総件数
            };

            // exData2
            // rowNo判定に使用する、該当する行番号を配列の有無で判定する
            exDataJudge2 = {
                Plan: ["1", "2", "3", "4", "5", "6", "7", "8", "9"],
                Sudden: ["10", "11", "12"],
                UnPlanned: ["13", "14", "15"],
                UnPlannedSudden: ["16", "17", "18"],
                All: ["19", "20", "21"]
            };
            // 設定するexDataの値の紐づけに使用する、キーはexDataJudgeと同じにする
            exDataValue2 = {
                Plan: SuddenClassCode.Plan,                             // 計画作業
                Sudden: SuddenClassCode.Sudden,                         // 突発作業
                UnPlanned: SuddenClassCode.UnPlanned,                 　// 計画外作業
                UnPlannedSudden: SuddenClassCode.UnPlannedSudden,       // 突発・計画外作業
                All: SuddenClassCode.All                                // 作業総件数
            };

            // rowNo
            // exDataと同じ理屈
            sDateJudge = {
                Month: ["1", "4", "7", "10", "13", "16", "19"],
                Half: ["2", "5", "8", "11", "14", "17", "20"],
                Year: ["3", "6", "9", "12", "15", "18", "21"]
            };
            stDateRowno = { Month: "1", Half: "2", Year: "3" };

            planFlg = true;
            break;
        // 作業性格分類の場合
        case DetailList.WorkPersonalityList.Id:
            // exData
            // rowNo判定に使用する、該当する行番号を配列の有無で判定する
            exDataJudge = {
                Preventive: ["1", "2", "3"], Failure: ["4", "5", "6"], Other: ["7", "8", "9"],
                Manufacture: ["10", "11", "12"], All: ["13", "14", "15"]
            };
            // 設定するexDataの値の紐づけに使用する、キーはexDataJudgeと同じにする
            exDataValue = {
                Preventive: MqClassCodeWorkPersonality.Preventive,           // 予防保全
                Failure: MqClassCodeWorkPersonality.Failure,                 // 故障修理
                Other: MqClassCodeWorkPersonality.Other,                     // その他
                Manufacture: MqClassCodeWorkPersonality.Manufacture,         // 製造関連
                All: MqClassCodeWorkPersonality.All                          // 全て
            };

            // rowNo
            // exDataと同じ理屈
            sDateJudge = {
                Month: ["1", "4", "7", "10", "13"],
                Half: ["2", "5", "8", "11", "14"],
                Year: ["3", "6", "9", "12", "15"]
            };
            stDateRowno = { Month: "1", Half: "2", Year: "3" };

            break;
        // その他の場合
        case DetailList.OthersList.Id:
            // rowNo
            // exDataと同じ理屈
            sDateJudge = {Month: ["1"],Half: ["2"],Year: ["3"]};
            stDateRowno = { Month: "1", Half: "2", Year: "3" };

            otherFlg = true;
            break;
        default:

    }

    // 行番号が所定のリストに含まれるかどうかで判定する処理
    var rowNoIncludeJudge = function (rowNo, judge, value, num) {
        // rowNo判定に使用する連想配列をループ
        $.each(judge, function (key, rowNoList) {
            // key:連想配列のキー、rowNoList:連想配列の値(配列)
            // 配列に含まれる場合(switchと一緒)
            if (rowNoList.includes(rowNo) && num == 0) {
                // 含まれるときのキーの値で、valueから値を取得
                exData = value[key];
                return false; // break
            }
            else if (rowNoList.includes(rowNo) && num == 1) {
                // 含まれるときのキーの値で、valueから値を取得
                exData2 = value[key];
                return false; // break
            }
            else if (rowNoList.includes(rowNo) && num == 2) {
                // 含まれるときのキーの値で、valueから値を取得
                startDateRowNo = value[key];
                return false; // break
            }
        });
    }

    let exData, exData2, startDateRowNo;

    // その他の場合は詳細条件なし
    if (!otherFlg)
    {
        // exDataの判定
        rowNoIncludeJudge(rowNo, exDataJudge, exDataValue, 0);

        // 作業計画性分類の場合は取得条件追加
        if (planFlg)
        {
            rowNoIncludeJudge(rowNo, exDataJudge2, exDataValue2, 1);
        }
    }

    // startDateの判定
    rowNoIncludeJudge(rowNo, sDateJudge, stDateRowno, 2);
    let startDate = getValue(OverviewList.List.Id, 10, Number(startDateRowNo) - 1, CtrlFlag.TextBox);

    // 当月の場合は月初を設定
    if (sDateJudge.Month.includes(startDateRowNo))
    {
        startDate = getValue(TargetDateList.List.Id, 1, 1, CtrlFlag.TextBox);
        startDate += "/01"; // 1日固定
    }

    // 終了日
    let endDate = getValue(OverviewList.List.Id, 10, 0, CtrlFlag.TextBox);

    // VAL値取得
    var valNo = $(element).closest('td').data('name');
    var jobCode = "0";
    switch (valNo) {
        // 機械
        case "VAL5":
            jobCode = JobCode.Machine;
            break;
        // 電気
        case "VAL6":
            jobCode = JobCode.Electricity;
            break;
        // 計装
        case "VAL7":
            jobCode = JobCode.Instrumentation;
            break;
        // その他
        case "VAL8":
            jobCode = JobCode.Other;
            break;
        // 合計
        case "VAL9":
            jobCode = JobCode.All;
            break;
    }

    // パラメータを渡す
    switch (ctrlId) {
        // 系の停止の場合
        case DetailList.StopSystemList.Id:
            conditionDataList = getParamToMA0001Maintenance(startDate, endDate, jobCode, exData);
            break;
        // 故障修理件数の場合
        case DetailList.RepairsList.Id:
            conditionDataList = getParamToMA0001Failure(startDate, endDate, jobCode, exData);
            break;
        // 作業計画性分類の場合
        case DetailList.WorkPlanList1.Id:
            conditionDataList = getParamToMA0001WorkPlanning(startDate, endDate, jobCode, exData, exData2);
            break;
        // 作業性格分類の場合
        case DetailList.WorkPersonalityList.Id:
            conditionDataList = getParamToMA0001WorkPersonality(startDate, endDate, jobCode, exData);
            break;
        // その他の場合
        case DetailList.OthersList.Id: 
            conditionDataList = getParamToMA0001Other(startDate, endDate, jobCode);
            break;
    }
    return [true, conditionDataList];
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
    //詳細(系の停止)集計
    totallingSystemStop();

    //詳細(故障修理件数)集計
    totallingGetNumberOfRepairsDetail();

    //詳細(作業計画性分類1)集計
    totallingGetWorkPlan1Detail();

    // 詳細(作業性格分類)集計(合計列)
    totallingGetWorkPdetailtotal();

    // 詳細(作業性格分類)集計(f+h)
    totallingGetWorkPersonalityDetail_fh();

    //詳細(作業計画性分類2)集計
    totallingGetWorkPlan2Detail_1();        //計画作業率
    totallingGetWorkPlan2Detail_2();        //予防保全作業率
    totallingGetWorkPlan2Detail_3();        //突発作業率

    //詳細(その他)集計
    totallingGetOther();

    //概要集計
    totallingGetOverview();
}

//詳細(系の停止)集計
function totallingSystemStop()
{
    //系の停止全体行取得
    var sStop = $(P_Article).find("#" + DetailList.StopSystemList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //機械計
    var mTotal = { This: 0, Half: 0, Total: 0 };

    //電気合計
    var eTotal = { This: 0, Half: 0, Total: 0 };

    //計装合計
    var iTotal = { This: 0, Half: 0, Total: 0 };

    //その他合計
    var oTotal = { This: 0, Half: 0, Total: 0 };

    //合計
    var tTotal = { This: 0, Half: 0, Total: 0 };

    //ループして集計(機械、電気、計装、その他、合計)
    $.each($(sStop), function (i, tr) {

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a' );
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0"}

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        //合計
        var total = parseFloat(machine) + parseFloat(electricity) + parseFloat(instrumentation) + parseFloat(other);

        //当月の場合
        if (i == 0 || i == 3) {
            mTotal.This += parseFloat(machine);              //当月の機械合計
            eTotal.This += parseFloat(electricity);          //当月の電気合計
            iTotal.This += parseFloat(instrumentation);      //当月の計装合計
            oTotal.This += parseFloat(other);                //当月のその他合計
            tTotal.This += parseFloat(total);                //当月の合計
        }
        //半期の場合
        else if (i == 1 || i == 4) {
            mTotal.Half += parseFloat(machine);              //半期の機械合計
            eTotal.Half += parseFloat(electricity);          //半期の電気合計
            iTotal.Half += parseFloat(instrumentation);      //半期の計装合計
            oTotal.Half += parseFloat(other);                //半期のその他合計
            tTotal.Half += parseFloat(total);                //半期の合計
        }
        //年度の場合
        else if (i == 2 || i == 5) {
            mTotal.Total += parseFloat(machine);              //年度の機械合計
            eTotal.Total += parseFloat(electricity);          //年度の電気合計
            iTotal.Total += parseFloat(instrumentation);      //年度の計装合計
            oTotal.Total += parseFloat(other);                //年度のその他合計
            tTotal.Total += parseFloat(total);                //年度の合計
        }
        //保全要因、製造要因の合計値を表示
        else if (i == 6) {
            setValue(DetailList.StopSystemList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.This).toLocaleString()); //機械
            setValue(DetailList.StopSystemList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.This).toLocaleString()); //電気
            setValue(DetailList.StopSystemList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.This).toLocaleString()); //計装
            setValue(DetailList.StopSystemList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.This).toLocaleString()); //その他
            setValue(DetailList.StopSystemList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.This).toLocaleString()); //合計
        }
        //半期の場合
        else if (i == 7)
        {
            setValue(DetailList.StopSystemList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Half).toLocaleString()); //機械
            setValue(DetailList.StopSystemList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Half).toLocaleString()); //電気
            setValue(DetailList.StopSystemList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Half).toLocaleString()); //計装
            setValue(DetailList.StopSystemList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Half).toLocaleString()); //その他
            setValue(DetailList.StopSystemList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Half).toLocaleString()); //合計

        }
        //年度の場合
        else if (i == 8)
        {
            setValue(DetailList.StopSystemList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Total).toLocaleString()); //機械
            setValue(DetailList.StopSystemList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Total).toLocaleString()); //電気
            setValue(DetailList.StopSystemList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Total).toLocaleString()); //計装
            setValue(DetailList.StopSystemList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Total).toLocaleString()); //その他
            setValue(DetailList.StopSystemList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Total).toLocaleString()); //合計
        }

        //系停止時間であれば小数点二桁表示
        if (i == 9 || i == 10 || i == 11) {
            var ma = parseFloat(machine).toLocaleString().toString();
            if (!ma.includes(".")) { ma += ".00" }

            var el = parseFloat(electricity).toLocaleString().toString();
            if (!el.includes(".")) { el += ".00" }

            var ins = parseFloat(instrumentation).toLocaleString().toString();
            if (!ins.includes(".")) { ins += ".00" }

            var ot = parseFloat(other).toLocaleString().toString();
            if (!ot.includes(".")) { ot += ".00" }

            var to = parseFloat(total).toLocaleString().toString();
            if (!to.includes(".")) { to += ".00" }

            //小数点以下2桁表示
            setValue(DetailList.StopSystemList.Id, DetailRow.Machine, i, CtrlFlag.Link, ma); //機械
            setValue(DetailList.StopSystemList.Id, DetailRow.Electricity, i, CtrlFlag.Link, el); //電気
            setValue(DetailList.StopSystemList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, ins); //計装
            setValue(DetailList.StopSystemList.Id, DetailRow.Other, i, CtrlFlag.Link, ot); //その他
            setValue(DetailList.StopSystemList.Id, DetailRow.Total, i, CtrlFlag.Link, to); //合計
        }
        else if (i != 6 && i != 7 && i != 8) {

            //整数部のみ表示 
            setValue(DetailList.StopSystemList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(machine).toLocaleString()); //機械
            setValue(DetailList.StopSystemList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(electricity).toLocaleString()); //電気
            setValue(DetailList.StopSystemList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(instrumentation).toLocaleString()); //計装
            setValue(DetailList.StopSystemList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(other).toLocaleString()); //その他
            setValue(DetailList.StopSystemList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(total).toLocaleString()); //合計
        }
    });
}

//詳細(故障修理件数)集計
function totallingGetNumberOfRepairsDetail()
{
    //故障修理件数全体行取得
    var repair = $(P_Article).find("#" + DetailList.RepairsList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //機械合計
    var mTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0 };

    //電気合計
    var eTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0 };

    //計装合計
    var iTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0 };

    //その他合計
    var oTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0 };

    //合計
    var tTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0 };

    //当月行
    let thisRow1 = [0, 3];              //故障1
    let thisRow2 = [9, 12];             //故障2

    //半期行
    let halfRow1 = [1, 4];              //故障1
    let halfRow2 = [10, 13];            //故障2

    //年度行
    let totalRow1 = [2, 5];             //故障1
    let totalRow2 = [11, 14];           //故障2

    //ループして集計(機械、電気、計装、その他、合計)
    $.each($(repair), function (i, tr) {

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0" }

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        //合計
        var total = parseFloat(machine) + parseFloat(electricity) + parseFloat(instrumentation) + parseFloat(other);

        //******************************故障1 故障2 計行 集計***************************************
        //故障1の当月の場合(0,3行目)
        if (thisRow1.includes(i)) {
            mTotal.This1 += parseFloat(machine);              //故障1当月の機械合計
            eTotal.This1 += parseFloat(electricity);          //故障1当月の電気合計
            iTotal.This1 += parseFloat(instrumentation);      //故障1当月の計装合計
            oTotal.This1 += parseFloat(other);                //故障1当月のその他合計
            tTotal.This1 += parseFloat(total);                //故障1当月の合計
        }
        //故障1の半期の場合(1,4行目)
        else if (halfRow1.includes(i)) {
            mTotal.Half1 += parseFloat(machine);              //故障1半期の機械合計
            eTotal.Half1 += parseFloat(electricity);          //故障1半期の電気合計
            iTotal.Half1 += parseFloat(instrumentation);      //故障1半期の計装合計
            oTotal.Half1 += parseFloat(other);                //故障1半期のその他合計
            tTotal.Half1 += parseFloat(total);                //故障1半期の合計
        }
        //故障1の年度の場合(2,5行目)
        else if (totalRow1.includes(i)) {
            mTotal.Total1 += parseFloat(machine);             //故障1年度の機械合計
            eTotal.Total1 += parseFloat(electricity);         //故障1年度の電気合計
            iTotal.Total1 += parseFloat(instrumentation);     //故障1年度の計装合計
            oTotal.Total1 += parseFloat(other);               //故障1年度のその他合計
            tTotal.Total1 += parseFloat(total);               //故障1年度の合計
        }
        //故障2の当月の場合(9,12行目)
        else if (thisRow2.includes(i)) {
            mTotal.This2 += parseFloat(machine);              //故障2当月の機械合計
            eTotal.This2 += parseFloat(electricity);          //故障2当月の電気合計
            iTotal.This2 += parseFloat(instrumentation);      //故障2当月の計装合計
            oTotal.This2 += parseFloat(other);                //故障2当月のその他合計
            tTotal.This2 += parseFloat(total);                //故障2当月の合計
        }
        //故障2の半期の場合(10,13行目)
        else if (halfRow2.includes(i)) {
            mTotal.Half2 += parseFloat(machine);              //故障2半期の機械合計
            eTotal.Half2 += parseFloat(electricity);          //故障2半期の電気合計
            iTotal.Half2 += parseFloat(instrumentation);      //故障2半期の計装合計
            oTotal.Half2 += parseFloat(other);                //故障2半期のその他合計
            tTotal.Half2 += parseFloat(total);                //故障2半期の合計
        }
        //故障2の年度の場合(11,14行目)
        else if (totalRow2.includes(i)) {
            mTotal.Total2 += parseFloat(machine);             //故障2年度の機械合計
            eTotal.Total2 += parseFloat(electricity);         //故障2年度の電気合計
            iTotal.Total2 += parseFloat(instrumentation);     //故障2年度の計装合計
            oTotal.Total2 += parseFloat(other);               //故障2年度のその他合計
            tTotal.Total2 += parseFloat(total);               //故障2年度の合計
        }

        //******************************故障1 故障2 計行 画面表示***************************************
        //故障1の合計値を表示
        if (i == 6) {
            setValue(DetailList.RepairsList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.This1).toLocaleString()); //機械
            setValue(DetailList.RepairsList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.This1).toLocaleString()); //電気
            setValue(DetailList.RepairsList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.This1).toLocaleString()); //計装
            setValue(DetailList.RepairsList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.This1).toLocaleString()); //その他
            setValue(DetailList.RepairsList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.This1).toLocaleString()); //合計
        }
        //故障1の半期の場合
        else if (i == 7) {
            setValue(DetailList.RepairsList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Half1).toLocaleString()); //機械
            setValue(DetailList.RepairsList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Half1).toLocaleString()); //電気
            setValue(DetailList.RepairsList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Half1).toLocaleString()); //計装
            setValue(DetailList.RepairsList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Half1).toLocaleString()); //その他
            setValue(DetailList.RepairsList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Half1).toLocaleString()); //合計

        }
        //故障1の年度の場合
        else if (i == 8) {
            setValue(DetailList.RepairsList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Total1).toLocaleString()); //機械
            setValue(DetailList.RepairsList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Total1).toLocaleString()); //電気
            setValue(DetailList.RepairsList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Total1).toLocaleString()); //計装
            setValue(DetailList.RepairsList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Total1).toLocaleString()); //その他
            setValue(DetailList.RepairsList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Total1).toLocaleString()); //合計
        }
        //故障2の合計値を表示
        else if (i == 15) {
            setValue(DetailList.RepairsList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.This2).toLocaleString()); //機械
            setValue(DetailList.RepairsList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.This2).toLocaleString()); //電気
            setValue(DetailList.RepairsList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.This2).toLocaleString()); //計装
            setValue(DetailList.RepairsList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.This2).toLocaleString()); //その他
            setValue(DetailList.RepairsList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.This2).toLocaleString()); //合計
        }
        //故障2の半期の場合
        else if (i == 16) {
            setValue(DetailList.RepairsList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Half2).toLocaleString()); //機械
            setValue(DetailList.RepairsList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Half2).toLocaleString()); //電気
            setValue(DetailList.RepairsList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.This2).toLocaleString()); //計装
            setValue(DetailList.RepairsList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Half2).toLocaleString()); //その他
            setValue(DetailList.RepairsList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Half2).toLocaleString()); //合計

        }
        //故障2の年度の場合
        else if (i == 17) {
            setValue(DetailList.RepairsList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Total2).toLocaleString()); //機械
            setValue(DetailList.RepairsList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Total2).toLocaleString()); //電気
            setValue(DetailList.RepairsList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Total2).toLocaleString()); //計装
            setValue(DetailList.RepairsList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Total2).toLocaleString()); //その他
            setValue(DetailList.RepairsList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Total2).toLocaleString()); //合計
        }
        else {
            //整数部のみ表示 
            setValue(DetailList.RepairsList.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(machine).toLocaleString()); //機械
            setValue(DetailList.RepairsList.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(electricity).toLocaleString()); //電気
            setValue(DetailList.RepairsList.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(instrumentation).toLocaleString()); //計装
            setValue(DetailList.RepairsList.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(other).toLocaleString()); //その他
            setValue(DetailList.RepairsList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(total).toLocaleString()); //合計
        }

    });

}

 //詳細(作業計画性分類1)集計
function totallingGetWorkPlan1Detail()
{
    //作業計画性分類行取得
    var plan = $(P_Article).find("#" + DetailList.WorkPlanList1.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //機械合計
    var mTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0, This: 0, Half: 0, Total: 0 };

    //電気合計
    var eTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0, This: 0, Half: 0, Total: 0 };

    //計装合計
    var iTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0, This: 0, Half: 0, Total: 0 };

    //その他合計
    var oTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0, This: 0, Half: 0, Total: 0 };

    //合計
    var tTotal = { This1: 0, Half1: 0, Total1: 0, This2: 0, Half2: 0, Total2: 0, This: 0, Half: 0, Total: 0 };

    //当月行
    let thisRow1 = [0, 3];              //計画作業件数
    let thisRow2 = [9, 12];             //計画外作業件数

    //半期行
    let halfRow1 = [1, 4];              //計画作業件数
    let halfRow2 = [10, 13];            //計画外作業件数

    //年度行
    let totalRow1 = [2, 5];              //計画作業件数
    let totalRow2 = [11, 14];            //計画外作業件数

    //ループして集計(機械、電気、計装、その他、合計)
    $.each($(plan), function (i, tr) {

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0" }

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        //合計
        var total = parseFloat(machine) + parseFloat(electricity) + parseFloat(instrumentation) + parseFloat(other);

        //******************************計画作業件数 計画外作業件数 小計行 集計***************************************
        //計画作業件数の当月の場合(0,3行目)
        if (thisRow1.includes(i)) {
            mTotal.This1 += parseFloat(machine);              //計画作業件数当月の機械合計
            eTotal.This1 += parseFloat(electricity);          //計画作業件数当月の電気合計
            iTotal.This1 += parseFloat(instrumentation);      //計画作業件数当月の計装合計
            oTotal.This1 += parseFloat(other);                //計画作業件数当月のその他合計
            tTotal.This1 += parseFloat(total);                //計画作業件数当月の合計
        }
        //計画作業件数の半期の場合(1,4行目)
        else if (halfRow1.includes(i)) {
            mTotal.Half1 += parseFloat(machine);              //計画作業件数半期の機械合計
            eTotal.Half1 += parseFloat(electricity);          //計画作業件数半期の電気合計
            iTotal.Half1 += parseFloat(instrumentation);      //計画作業件数半期の計装合計
            oTotal.Half1 += parseFloat(other);                //計画作業件数半期のその他合計
            tTotal.Half1 += parseFloat(total);                //計画作業件数半期の合計
        }
        //計画作業件数の年度の場合(2,5行目)
        else if (totalRow1.includes(i)) {
            mTotal.Total1 += parseFloat(machine);             //計画作業件数年度の機械合計
            eTotal.Total1 += parseFloat(electricity);         //計画作業件数年度の電気合計
            iTotal.Total1 += parseFloat(instrumentation);     //計画作業件数年度の計装合計
            oTotal.Total1 += parseFloat(other);               //計画作業件数年度のその他合計
            tTotal.Total1 += parseFloat(total);               //計画作業件数年度の合計
        }
        //計画外作業件数の当月の場合(9,12行目)
        else if (thisRow2.includes(i)) {
            mTotal.This2 += parseFloat(machine);              //計画外作業件数当月の機械合計
            eTotal.This2 += parseFloat(electricity);          //計画外作業件数当月の電気合計
            iTotal.This2 += parseFloat(instrumentation);      //計画外作業件数当月の計装合計
            oTotal.This2 += parseFloat(other);                //計画外作業件数当月のその他合計
            tTotal.This2 += parseFloat(total);                //計画外作業件数当月の合計
        }
        //計画外作業件数の半期の場合(10,13行目)
        else if (halfRow2.includes(i)) {
            mTotal.Half2 += parseFloat(machine);              //計画外作業件数半期の機械合計
            eTotal.Half2 += parseFloat(electricity);          //故障2半期の電気合計
            iTotal.Half2 += parseFloat(instrumentation);      //故障2半期の計装合計
            oTotal.Half2 += parseFloat(other);                //故障2半期のその他合計
            tTotal.Half2 += parseFloat(total);                //故障2半期の合計
        }
        //計画外作業件数の年度の場合(11,14行目)
        else if (totalRow2.includes(i)) {
            mTotal.Total2 += parseFloat(machine);             //計画外作業件数年度の機械合計
            eTotal.Total2 += parseFloat(electricity);         //計画外作業件数年度の電気合計
            iTotal.Total2 += parseFloat(instrumentation);     //計画外作業件数年度の計装合計
            oTotal.Total2 += parseFloat(other);               //計画外作業件数年度のその他合計
            tTotal.Total2 += parseFloat(total);               //計画外作業件数年度の合計
        }

        //******************************故障1 故障2 小計行 画面表示***************************************
        //計画作業件数の合計値を表示
        if (i == 6) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.This1).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.This1).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.This1).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.This1).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.This1).toLocaleString()); //合計
        }
        //計画作業件数の半期の場合
        else if (i == 7) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Half1).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Half1).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Half1).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Half1).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Half1).toLocaleString()); //合計

        }
        //計画作業件数の年度の場合
        else if (i == 8) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Total1).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Total1).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Total1).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Total1).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Total1).toLocaleString()); //合計
        }
        //計画外作業件数の合計値を表示
        else if (i == 15) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.This2).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.This2).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.This2).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.This2).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.This2).toLocaleString()); //合計
        }
        //計画外作業件数の半期の場合
        else if (i == 16) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Half2).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Half2).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Half2).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Half2).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Half2).toLocaleString()); //合計

        }
        //計画外作業件数の年度の場合
        else if (i == 17) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Total2).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Total2).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Total2).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Total2).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Total2).toLocaleString()); //合計
        }
        else {
            //整数部のみ表示 
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(machine).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(electricity).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(instrumentation).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(other).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(total).toLocaleString()); //合計
        }

        //******************************故障1+故障2+予知修理合計 画面表示***************************************
        //計の合計値を表示
        if (i == 18) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.This1 + mTotal.This2).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.This1 + eTotal.This2).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.This1 + iTotal.This2).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.This1 + oTotal.This2).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.This1 + tTotal.This2).toLocaleString()); //合計

        }
        //計の半期の場合
        else if (i == 19) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Half1 + mTotal.Half2).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Half1 + eTotal.Half2).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Half1 + iTotal.Half2).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Half1 + oTotal.Half2).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Half1 + tTotal.Half2).toLocaleString()); //合計
        }
        //計の年度の場合
        else if (i == 20) {
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Machine, i, CtrlFlag.Link, Math.trunc(mTotal.Total1 + mTotal.Total2).toLocaleString()); //機械
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Electricity, i, CtrlFlag.Link, Math.trunc(eTotal.Total1 + eTotal.Total2).toLocaleString()); //電気
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Instrumentation, i, CtrlFlag.Link, Math.trunc(iTotal.Total1 + iTotal.Total2).toLocaleString()); //計装
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Other, i, CtrlFlag.Link, Math.trunc(oTotal.Total1 + oTotal.Total2).toLocaleString()); //その他
            setValue(DetailList.WorkPlanList1.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(tTotal.Total1 + tTotal.Total2).toLocaleString()); //合計
        }

    });
}

// 詳細(作業性格分類)集計(合計列)
function totallingGetWorkPdetailtotal()
{
    //作業性格分類全体行取得
    var workp = $(P_Article).find("#" + DetailList.WorkPersonalityList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //ループして集計(機械、電気、計装、その他、合計)
    $.each($(workp), function (i, tr) {

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0"; $(mval).text("0");}

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0"; $(eval).text("0"); }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0"; $(ival).text("0"); }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0"; $(oval).text("0"); }

        //合計
        var total = parseFloat(machine) + parseFloat(electricity) + parseFloat(instrumentation) + parseFloat(other);
        //表示合計行取得
        var tval = $(tr).find('td[data-name="VAL' + DetailRow.Total + '"] a');

        //合計表示
        $(tval).text(Math.trunc(total).toLocaleString());

    });
}

// 詳細(作業性格分類)集計(合計列)
function totallingGetWorkPersonalityDetail_fh()
{
    //作業計画性分類行取得
    var plan = $(P_Article).find("#" + DetailList.WorkPlanList1.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //機械合計
    var mTotal = { This: 0, Half: 0, Total: 0 };

    //電気合計
    var eTotal = { This: 0, Half: 0, Total: 0 };

    //計装合計
    var iTotal = { This: 0, Half: 0, Total: 0 };

    //その他合計
    var oTotal = { This: 0, Half: 0, Total: 0 };

    //合計行合計
    var tTotal = { This: 0, Half: 0, Total: 0 };

    //作業計画性分類をループして取得(機械、電気、計装、その他、合計)
    $.each($(plan), function (i, tr) {

        //取得したい行以外飛ばす
        if (i < 18) { return true; }//continueと同等の処理

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a' );
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0"}

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        // 合計取得
        var tval = $(tr).find('td[data-name="VAL' + DetailRow.Total + '"] a');
        var total = getCellVal(tval).replace(/,/g, '');
        if (total == "") { total = "0" }

        //当月行
        if (i == 18) {
            mTotal.This = parseFloat(machine);
            eTotal.This = parseFloat(electricity);
            iTotal.This = parseFloat(instrumentation);
            oTotal.This = parseFloat(other);
            tTotal.This = parseFloat(total);
        }
        //半期行
        else if (i == 19) {
            mTotal.Half = parseFloat(machine);
            eTotal.Half = parseFloat(electricity);
            iTotal.Half = parseFloat(instrumentation);
            oTotal.Half = parseFloat(other);
            tTotal.Half = parseFloat(total);
        }
        //年度行
        else if (i == 20) {
            mTotal.Total = parseFloat(machine);
            eTotal.Total = parseFloat(electricity);
            iTotal.Total = parseFloat(instrumentation);
            oTotal.Total = parseFloat(other);
            tTotal.Total = parseFloat(total);
        }

    });

    //作業性格分類全体行取得
    var workp = $(P_Article).find("#" + DetailList.WorkPersonalityList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //作業性格分類全体行取得をループして集計値表示
    $.each($(workp), function (i, tr) {

        //表示したい行以外飛ばす
        if (i < 12) { return true; }//continueと同等の処理

        //作業性格分類当月の(f+h)表示
        if (i == 12) {
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Machine, 12, CtrlFlag.Link, Math.trunc(mTotal.This).toLocaleString()); //機械
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Electricity, 12, CtrlFlag.Link, Math.trunc(eTotal.This).toLocaleString()); //電気
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Instrumentation, 12, CtrlFlag.Link, Math.trunc(iTotal.This).toLocaleString()); //計装
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Other, 12, CtrlFlag.Link, Math.trunc(oTotal.This).toLocaleString()); //その他
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Total, 12, CtrlFlag.Link, Math.trunc(tTotal.This).toLocaleString()); //合計
        }
        //半期の(f+h)表示
        else if (i == 13) {
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Machine, 13, CtrlFlag.Link, Math.trunc(mTotal.Half).toLocaleString()); //機械
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Electricity, 13, CtrlFlag.Link, Math.trunc(eTotal.Half).toLocaleString()); //電気
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Instrumentation, 13, CtrlFlag.Link, Math.trunc(iTotal.Half).toLocaleString()); //計装
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Other, 13, CtrlFlag.Link, Math.trunc(oTotal.Half).toLocaleString()); //その他
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Total, 13, CtrlFlag.Link, Math.trunc(tTotal.Half).toLocaleString()); //合計
        }
        //年度の(f+h)表示
        else {
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Machine, 14, CtrlFlag.Link, Math.trunc(mTotal.Total).toLocaleString()); //機械
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Electricity, 14, CtrlFlag.Link, Math.trunc(eTotal.Total).toLocaleString()); //電気
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Instrumentation, 14, CtrlFlag.Link, Math.trunc(iTotal.Total).toLocaleString()); //計装
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Other, 14, CtrlFlag.Link, Math.trunc(oTotal.Total).toLocaleString()); //その他
            setValue(DetailList.WorkPersonalityList.Id, DetailRow.Total, 14, CtrlFlag.Link, Math.trunc(tTotal.Total).toLocaleString()); //合計
        }

    });
}

//詳細(作業計画性分類2)集計
function totallingGetWorkPlan2Detail_1()
{
    //作業計画性分類行取得
    var plan = $(P_Article).find("#" + DetailList.WorkPlanList1.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //機械合計
    var mTotal = { This_f: 0, Half_f: 0, Total_f: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //電気合計
    var eTotal = { This_f: 0, Half_f: 0, Total_f: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //計装合計
    var iTotal = { This_f: 0, Half_f: 0, Total_f: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //その他合計
    var oTotal = { This_f: 0, Half_f: 0, Total_f: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //合計行合計
    var tTotal = { This_f: 0, Half_f: 0, Total_f: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //取得しない行
    let notGetRow = [0, 1, 2, 3, 4, 5, 9, 10, 11, 12, 13, 14, 15, 16, 17];         

    //作業計画性分類をループして取得(機械、電気、計装、その他、合計)
    $.each($(plan), function (i, tr) {

        if (notGetRow.includes(i)) { return true; }//continueと同じ処理

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0" }

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        // 合計取得
        var tval = $(tr).find('td[data-name="VAL' + DetailRow.Total + '"] a');
        var total = getCellVal(tval).replace(/,/g, '');
        if (total == "") { total = "0" }

        //f 当月行
        if (i == 6) {
            mTotal.This_f = parseFloat(machine);
            eTotal.This_f = parseFloat(electricity);
            iTotal.This_f = parseFloat(instrumentation);
            oTotal.This_f = parseFloat(other);
            tTotal.This_f = parseFloat(total);
        }
        //f 半期行
        else if (i == 7) {
            mTotal.Half_f = parseFloat(machine);
            eTotal.Half_f = parseFloat(electricity);
            iTotal.Half_f = parseFloat(instrumentation);
            oTotal.Half_f = parseFloat(other);
            tTotal.Half_f = parseFloat(total);
        }
        //f 年度行
        else if (i == 8) {
            mTotal.Total_f = parseFloat(machine);
            eTotal.Total_f = parseFloat(electricity);
            iTotal.Total_f = parseFloat(instrumentation);
            oTotal.Total_f = parseFloat(other);
            tTotal.Total_f = parseFloat(total);
        }
        //i 当月行
        else if (i == 18) {
            mTotal.This_i = parseFloat(machine);
            eTotal.This_i = parseFloat(electricity);
            iTotal.This_i = parseFloat(instrumentation);
            oTotal.This_i = parseFloat(other);
            tTotal.This_i = parseFloat(total);
        }
        //i 半期行
        else if (i == 19) {
            mTotal.Half_i = parseFloat(machine);
            eTotal.Half_i = parseFloat(electricity);
            iTotal.Half_i = parseFloat(instrumentation);
            oTotal.Half_i = parseFloat(other);
            tTotal.Half_i = parseFloat(total);
        }
        //i 年度行
        else {
            mTotal.Total_i = parseFloat(machine);
            eTotal.Total_i = parseFloat(electricity);
            iTotal.Total_i = parseFloat(instrumentation);
            oTotal.Total_i = parseFloat(other);
            tTotal.Total_i = parseFloat(total);
        }

    });

    //作業計画性分類2行取得
    var plan2 = $(P_Article).find("#" + DetailList.WorkPlanList2.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //作業性格分類全体行取得をループして集計値表示
    $.each($(plan2), function (i, tr) {

        //表示したい行以外飛ばす
        if (i > 2) { return true; }//continueと同等の処理

        //当月の(f/i*100)表示
        if (i == 0) {
            if (mTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.This_f / mTotal.This_i * 100) + "%"); //機械
            }

            if (eTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.This_f / eTotal.This_i * 100) + "%"); //電気
            }

            if (iTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.This_f / iTotal.This_i * 100) + "%"); //計装
            }

            if (oTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.This_f / oTotal.This_i * 100) + "%"); //その他
            }

            if (tTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.This_f / tTotal.This_i * 100) + "%"); //合計
            }
        }
        //半期の(f/i*100)表示
        else if (i == 1) {
            if (mTotal.Half_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.Half_f / mTotal.Half_i * 100) + "%"); //機械
            }

            if (eTotal.Half_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.Half_f / eTotal.Half_i * 100) + "%"); //電気
            }

            if (iTotal.Half_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.Half_f / iTotal.Half_i * 100) + "%"); //計装
            }

            if (oTotal.Half_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.Half_f / oTotal.Half_i * 100) + "%"); //その他
            }

            if (tTotal.Half_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.Half_f / tTotal.Half_i * 100) + "%"); //合計
            }
        }
        //年度の(f/i*100)表示
        else {
            if (mTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.Total_f / mTotal.Total_i * 100) + "%"); //機械
            }

            if (eTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.Total_f / eTotal.Total_i * 100) + "%"); //電気
            }

            if (iTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.Total_f / iTotal.Total_i * 100) + "%"); //計装
            }

            if (oTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.Total_f / oTotal.Total_i * 100) + "%"); //その他
            }

            if (tTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.Total_f / tTotal.Total_i * 100) + "%"); //合計
            }
        }
    });
}
//詳細(作業計画性分類2)集計
function totallingGetWorkPlan2Detail_2() {

    //機械合計
    var mTotal = { This_i: 0, Half_i: 0, Total_i: 0, This_e: 0, Half_e: 0, Total_e: 0 };

    //電気合計
    var eTotal = { This_i: 0, Half_i: 0, Total_i: 0, This_e: 0, Half_e: 0, Total_e: 0 };

    //計装合計
    var iTotal = { This_i: 0, Half_i: 0, Total_i: 0, This_e: 0, Half_e: 0, Total_e: 0 };

    //その他合計
    var oTotal = { This_i: 0, Half_i: 0, Total_i: 0, This_e: 0, Half_e: 0, Total_e: 0 };

    //合計行合計
    var tTotal = { This_i: 0, Half_i: 0, Total_i: 0, This_e: 0, Half_e: 0, Total_e: 0 };

    //作業計画性分類行取得
    var plan = $(P_Article).find("#" + DetailList.WorkPlanList1.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //作業計画性分類をループして取得(機械、電気、計装、その他、合計)
    $.each($(plan), function (i, tr) {

        if (i < 18) { return true; }//continueと同じ処理

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0" }

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        // 合計取得
        var tval = $(tr).find('td[data-name="VAL' + DetailRow.Total + '"] a');
        var total = getCellVal(tval).replace(/,/g, '');
        if (total == "") { total = "0" }

        //i 当月行
        if (i == 18) {
            mTotal.This_i = parseFloat(machine);
            eTotal.This_i = parseFloat(electricity);
            iTotal.This_i = parseFloat(instrumentation);
            oTotal.This_i = parseFloat(other);
            tTotal.This_i = parseFloat(total);
        }
        //i 半期行
        else if (i == 19) {
            mTotal.Half_i = parseFloat(machine);
            eTotal.Half_i = parseFloat(electricity);
            iTotal.Half_i = parseFloat(instrumentation);
            oTotal.Half_i = parseFloat(other);
            tTotal.Half_i = parseFloat(total);
        }
        //i 年度行
        else {
            mTotal.Total_i = parseFloat(machine);
            eTotal.Total_i = parseFloat(electricity);
            iTotal.Total_i = parseFloat(instrumentation);
            oTotal.Total_i = parseFloat(other);
            tTotal.Total_i = parseFloat(total);
        }

    });

    //作業性格分類行取得
    var personality = $(P_Article).find("#" + DetailList.WorkPersonalityList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //作業性格分類をループして取得(機械、電気、計装、その他、合計)
    $.each($(personality), function (i, tr) {

        if (i > 2) { return true; } //continueと同じ処理

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0" }

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        // 合計取得
        var tval = $(tr).find('td[data-name="VAL' + DetailRow.Total + '"] a');
        var total = getCellVal(tval).replace(/,/g, '');
        if (total == "") { total = "0" }

        //i 当月行
        if (i == 0) {
            mTotal.This_e = parseFloat(machine);
            eTotal.This_e = parseFloat(electricity);
            iTotal.This_e = parseFloat(instrumentation);
            oTotal.This_e = parseFloat(other);
            tTotal.This_e = parseFloat(total);
        }
        //i 半期行
        else if (i == 1) {
            mTotal.Half_e = parseFloat(machine);
            eTotal.Half_e = parseFloat(electricity);
            iTotal.Half_e = parseFloat(instrumentation);
            oTotal.Half_e = parseFloat(other);
            tTotal.Half_e = parseFloat(total);
        }
        //i 年度行
        else {
            mTotal.Total_e = parseFloat(machine);
            eTotal.Total_e = parseFloat(electricity);
            iTotal.Total_e = parseFloat(instrumentation);
            oTotal.Total_e = parseFloat(other);
            tTotal.Total_e = parseFloat(total);
        }

    });

    //表示しない行
    let notViewRow = [0, 1, 2, 6, 7, 8];

    //作業計画性分類行取得
    var plan2 = $(P_Article).find("#" + DetailList.WorkPlanList2.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //作業性格分類全体行取得をループして集計値表示
    $.each($(plan2), function (i, tr) {

        //表示したい行以外飛ばす
        if (notViewRow.includes(i)) { return true; }//continueと同等の処理

        //当月の(e'/i*100)表示
        if (i == 3) {
            if (mTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.This_e / mTotal.This_i * 100) + "%"); //機械
            }

            if (eTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.This_e / eTotal.This_i * 100) + "%"); //電気
            }

            if (iTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.This_e / iTotal.This_i * 100) + "%"); //計装
            }

            if (oTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.This_e / oTotal.This_i * 100) + "%"); //その他
            }

            if (tTotal.This_i == 0)             //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.This_e / tTotal.This_i * 100) + "%"); //合計
            }
        }
        //半期の(e'/i*100)表示
        else if (i == 4) {
            if (mTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.Half_e / mTotal.Half_i * 100) + "%"); //機械
            }

            if (eTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.Half_e / eTotal.Half_i * 100) + "%"); //電気
            }

            if (iTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.Half_e / iTotal.Half_i * 100) + "%"); //計装
            }

            if (oTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.Half_e / oTotal.Half_i * 100) + "%"); //その他
            }

            if (tTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.Half_e / tTotal.Half_i * 100) + "%"); //合計
            }
        }
        //年度の(e'/i*100)表示
        else {
            if (mTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.Total_e / mTotal.Total_i * 100) + "%"); //機械
            }

            if (eTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.Total_e / eTotal.Total_i * 100) + "%"); //電気
            }

            if (iTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.Total_e / iTotal.Total_i * 100) + "%"); //計装
            }

            if (oTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.Total_e / oTotal.Total_i * 100) + "%"); //その他
            }

            if (tTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.Total_e / tTotal.Total_i * 100) + "%"); //合計
            }
        }
    });
}
//詳細(作業計画性分類2)集計
function totallingGetWorkPlan2Detail_3() {
    //作業計画性分類行取得
    var plan = $(P_Article).find("#" + DetailList.WorkPlanList1.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //機械合計
    var mTotal = { This_g: 0, Half_g: 0, Total_g: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //電気合計
    var eTotal = { This_g: 0, Half_g: 0, Total_g: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //計装合計
    var iTotal = { This_g: 0, Half_g: 0, Total_g: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //その他合計
    var oTotal = { This_g: 0, Half_g: 0, Total_g: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //合計行合計
    var tTotal = { This_g: 0, Half_g: 0, Total_g: 0, This_i: 0, Half_i: 0, Total_i: 0 };

    //取得しない行
    let notGetRow = [0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 14, 15, 16, 17];

    //作業計画性分類をループして取得(機械、電気、計装、その他、合計)
    $.each($(plan), function (i, tr) {

        if (notGetRow.includes(i)) { return true; }//continueと同じ処理

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0" }

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        // 合計取得
        var tval = $(tr).find('td[data-name="VAL' + DetailRow.Total + '"] a');
        var total = getCellVal(tval).replace(/,/g, '');
        if (total == "") { total = "0" }

        //g 当月行
        if (i == 9) {
            mTotal.This_g = parseFloat(machine);
            eTotal.This_g = parseFloat(electricity);
            iTotal.This_g = parseFloat(instrumentation);
            oTotal.This_g = parseFloat(other);
            tTotal.This_g = parseFloat(total);
        }
        //g 半期行
        else if (i == 10) {
            mTotal.Half_g = parseFloat(machine);
            eTotal.Half_g = parseFloat(electricity);
            iTotal.Half_g = parseFloat(instrumentation);
            oTotal.Half_g = parseFloat(other);
            tTotal.Half_g = parseFloat(total);
        }
        //g 年度行
        else if (i == 11) {
            mTotal.Total_g = parseFloat(machine);
            eTotal.Total_g = parseFloat(electricity);
            iTotal.Total_g = parseFloat(instrumentation);
            oTotal.Total_g = parseFloat(other);
            tTotal.Total_g = parseFloat(total);
        }
        //i 当月行
        else if (i == 18) {
            mTotal.This_i = parseFloat(machine);
            eTotal.This_i = parseFloat(electricity);
            iTotal.This_i = parseFloat(instrumentation);
            oTotal.This_i = parseFloat(other);
            tTotal.This_i = parseFloat(total);
        }
        //i 半期行
        else if (i == 19) {
            mTotal.Half_i = parseFloat(machine);
            eTotal.Half_i = parseFloat(electricity);
            iTotal.Half_i = parseFloat(instrumentation);
            oTotal.Half_i = parseFloat(other);
            tTotal.Half_i = parseFloat(total);
        }
        //i 年度行
        else {
            mTotal.Total_i = parseFloat(machine);
            eTotal.Total_i = parseFloat(electricity);
            iTotal.Total_i = parseFloat(instrumentation);
            oTotal.Total_i = parseFloat(other);
            tTotal.Total_i = parseFloat(total);
        }

    });

    //作業計画性分類2行取得
    var plan2 = $(P_Article).find("#" + DetailList.WorkPlanList2.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //作業性格分類全体行取得をループして集計値表示
    $.each($(plan2), function (i, tr) {

        //表示したい行以外飛ばす
        if (i < 6) { return true; }//continueと同等の処理

        //当月の(g/I*100)表示
        if (i == 6) {
            if (mTotal.This_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.This_g / mTotal.This_i * 100) + "%"); //機械
            }

            if (eTotal.This_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.This_g / eTotal.This_i * 100) + "%"); //電気
            }

            if (iTotal.This_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.This_g / iTotal.This_i * 100) + "%"); //計装
            }

            if (oTotal.This_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.This_g / oTotal.This_i * 100) + "%"); //その他
            }

            if (tTotal.This_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.This_g / tTotal.This_i * 100) + "%"); //合計
            }
        }
        //半期の(g/I*100)表示
        else if (i == 7) {
            if (mTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.Half_g / mTotal.Half_i * 100) + "%"); //機械
            }

            if (eTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.Half_g / eTotal.Half_i * 100) + "%"); //電気
            }

            if (iTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.Half_g / iTotal.Half_i * 100) + "%"); //計装
            }

            if (oTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.Half_g / oTotal.Half_i * 100) + "%"); //その他
            }

            if (tTotal.Half_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.Half_g / tTotal.Half_i * 100) + "%"); //合計
            }
        }
        //年度の(g/I*100)表示
        else {
            if (mTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, "0%"); //機械
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Machine, i, CtrlFlag.Label, Math.round(mTotal.Total_g / mTotal.Total_i * 100) + "%"); //機械
            } 

            if (eTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, "0%"); //電気
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Electricity, i, CtrlFlag.Label, Math.round(eTotal.Total_g / eTotal.Total_i * 100) + "%"); //電気
            } 

            if (iTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, "0%"); //計装
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Instrumentation, i, CtrlFlag.Label, Math.round(iTotal.Total_g / iTotal.Total_i * 100) + "%"); //計装
            } 

            if (oTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, "0%"); //その他
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Other, i, CtrlFlag.Label, Math.round(oTotal.Total_g / oTotal.Total_i * 100) + "%"); //その他
            } 

            if (tTotal.Total_i == 0) //0割を防ぐ(NaN表示)
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, "0%"); //合計
            }
            else
            {
                setValue(DetailList.WorkPlanList2.Id, DetailRow.Total, i, CtrlFlag.Label, Math.round(tTotal.Total_g / tTotal.Total_i * 100) + "%"); //合計
            } 
        }
    });
}

//詳細(その他)集計
function totallingGetOther()
{
    //詳細(その他)全体行取得
    var otherRow = $(P_Article).find("#" + DetailList.OthersList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //ループして集計(機械、電気、計装、その他、合計)
    $.each($(otherRow), function (i, tr) {

        //機械取得
        var mval = $(tr).find('td[data-name="VAL' + DetailRow.Machine + '"] a');
        var machine = getCellVal(mval).replace(/,/g, '');
        if (machine == "") { machine = "0" }

        //電気取得
        var eval = $(tr).find('td[data-name="VAL' + DetailRow.Electricity + '"] a');
        var electricity = getCellVal(eval).replace(/,/g, '');
        if (electricity == "") { electricity = "0" }

        //計装取得
        var ival = $(tr).find('td[data-name="VAL' + DetailRow.Instrumentation + '"] a');
        var instrumentation = getCellVal(ival).replace(/,/g, '');
        if (instrumentation == "") { instrumentation = "0" }

        //その他取得
        var oval = $(tr).find('td[data-name="VAL' + DetailRow.Other + '"] a');
        var other = getCellVal(oval).replace(/,/g, '');
        if (other == "") { other = "0" }

        //合計
        var total = parseFloat(machine) + parseFloat(electricity) + parseFloat(instrumentation) + parseFloat(other);

        //合計表示
        setValue(DetailList.OthersList.Id, DetailRow.Total, i, CtrlFlag.Link, Math.trunc(total).toLocaleString());

    });
}
//詳細(概要)集計
function totallingGetOverview()
{
    //詳細(概要)全体行取得
    var overview = $(P_Article).find("#" + OverviewList.List.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //詳細(系の停止)全体行取得
    var stopSystem = $(P_Article).find("#" + DetailList.StopSystemList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //当月
    var thisMonth = { Cnt1: 0, Cnt2: 0, Cnt3: 0};

    //半期
    var half = { Cnt1: 0, Cnt2: 0, Cnt3: 0 };

    //年度
    var total = { Cnt1: 0, Cnt2: 0, Cnt3: 0 };

    //取得したい行
    let GetRow50 = [0,1,2,9,10,11];

    //ループして取得(合計行)*************************************************************************
    $.each($(stopSystem), function (i, tr) {

        //取得したい行以外飛ばす
        if (!GetRow50.includes(i)) { return true; }//continueと同等の処理

        //表示合計行取得
        var tval = $(tr).find('td[data-name="VAL9"] a');

        //系停止回数(当月)
        if (i == 0) { thisMonth.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //系停止回数(半期)
        else if (i == 1) { half.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //系停止回数(年度)
        else if (i == 2) { total.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //系停止時間(当月)
        else if (i == 9) { thisMonth.Cnt2 = Number(getCellVal(tval).replace(/,/g, '')); }
        //系停止時間(半期)
        else if (i == 10) { half.Cnt2 = Number(getCellVal(tval).replace(/,/g, '')); }
        //系停止時間(年度)
        else { total.Cnt2 = Number(getCellVal(tval).replace(/,/g, '')); }
    });

    //ループして表示(系停止回数,系停止時間)
    $.each($(overview), function (i, tr) {

        //当月
        if (i == 0)
        {
            // 系停止回数
            setValue(OverviewList.List.Id, OverviewRow.StopSystem, 0, CtrlFlag.Label, thisMonth.Cnt1.toLocaleString());
            // 系停止時間(整数の場合は0を付ける)
            var tm = parseFloat(thisMonth.Cnt2).toLocaleString().toString();
            if (!tm.includes(".")) { tm += ".00" }
            setValue(OverviewList.List.Id, OverviewRow.StopTime, 0, CtrlFlag.Label, tm);
        }
        //半期
        else if (i == 1)
        {
            // 系停止回数
            setValue(OverviewList.List.Id, OverviewRow.StopSystem, 1, CtrlFlag.Label, half.Cnt1.toLocaleString());
            // 系停止時間(整数の場合は0を付ける)
            var th = parseFloat(half.Cnt2).toLocaleString().toString();
            if (!th.includes(".")) { th += ".00" }
            setValue(OverviewList.List.Id, OverviewRow.StopTime, 1, CtrlFlag.Label, th);
        }
        //年度
        else
        {
            // 系停止回数
            setValue(OverviewList.List.Id, OverviewRow.StopSystem, 2, CtrlFlag.Label, total.Cnt1.toLocaleString());
            // 系停止時間(整数の場合は0を付ける)
            var tt = parseFloat(total.Cnt2).toLocaleString().toString();
            if (!tt.includes(".")) { tt += ".00" }
            setValue(OverviewList.List.Id, OverviewRow.StopTime, 2, CtrlFlag.Label, tt);
        }
    });

    //初期化
    thisMonth.Cnt1 = 0;
    half.Cnt1 = 0;
    total.Cnt1 = 0;
    thisMonth.Cnt2 = 0;
    half.Cnt2 = 0;
    total.Cnt2 = 0;

    //詳細(故障修理件数)全体行取得********************************************************************
    var repair = $(P_Article).find("#" + DetailList.RepairsList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //取得したい行
    let GetRow60 = [6,7,8,15,16,17,18,19,20];

    //ループして取得(合計行)
    $.each($(repair), function (i, tr) {

        //取得したい行以外飛ばす
        if (!GetRow60.includes(i)) { return true; }//continueと同等の処理

        //表示合計行取得
        var tval = $(tr).find('td[data-name="VAL9"] a');

        //故障1計(当月)
        if (i == 6) { thisMonth.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //故障1計(半期)
        else if (i == 7) { half.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //故障1計(年度)
        else if (i == 8) { total.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //故障2計(当月)
        else if (i == 15) { thisMonth.Cnt2 = Number(getCellVal(tval).replace(/,/g, '')); }
        //故障2計(半期)
        else if (i == 16) { half.Cnt2 = Number(getCellVal(tval).replace(/,/g, '')); }
        //故障2計(年度)
        else if (i == 17) { total.Cnt2 = Number(getCellVal(tval).replace(/,/g, '')); }
        //予知修理(当月)
        else if (i == 18) { thisMonth.Cnt3 = Number(getCellVal(tval).replace(/,/g, '')); }
        //予知修理(半期)
        else if (i == 19) { half.Cnt3 = Number(getCellVal(tval).replace(/,/g, '')); }
        //予知修理(年度)
        else { total.Cnt3 = Number(getCellVal(tval).replace(/,/g, '')); }
    });

    //ループして表示(故障,故障2,予知修理)
    $.each($(overview), function (i, tr) {

        //当月
        if (i == 0) {
            setValue(OverviewList.List.Id, OverviewRow.Malfunction1, 0, CtrlFlag.Label, Math.trunc(thisMonth.Cnt1).toLocaleString());
            setValue(OverviewList.List.Id, OverviewRow.Malfunction2, 0, CtrlFlag.Label, Math.trunc(thisMonth.Cnt2).toLocaleString());
            setValue(OverviewList.List.Id, OverviewRow.Repair, 0, CtrlFlag.Label, Math.trunc(thisMonth.Cnt3).toLocaleString());
        }
        //半期
        else if (i == 1) {
            setValue(OverviewList.List.Id, OverviewRow.Malfunction1, 1, CtrlFlag.Label, Math.trunc(half.Cnt1).toLocaleString());
            setValue(OverviewList.List.Id, OverviewRow.Malfunction2, 1, CtrlFlag.Label, Math.trunc(half.Cnt2).toLocaleString());
            setValue(OverviewList.List.Id, OverviewRow.Repair, 1, CtrlFlag.Label, Math.trunc(half.Cnt3).toLocaleString());
        }
        //年度
        else {
            setValue(OverviewList.List.Id, OverviewRow.Malfunction1, 2, CtrlFlag.Label, Math.trunc(total.Cnt1).toLocaleString());
            setValue(OverviewList.List.Id, OverviewRow.Malfunction2, 2, CtrlFlag.Label, Math.trunc(total.Cnt2).toLocaleString());
            setValue(OverviewList.List.Id, OverviewRow.Repair, 2, CtrlFlag.Label, Math.trunc(total.Cnt3).toLocaleString());
        }
    });

    //初期化
    thisMonth.Cnt1 = 0;
    half.Cnt1 = 0;
    total.Cnt1 = 0;

    //詳細(作業計画性分類)全体行取得*******************************************************************
    var plan = $(P_Article).find("#" + DetailList.WorkPlanList1.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //ループして取得(合計行)
    $.each($(plan), function (i, tr) {

        //取得したい行以外飛ばす
        if (i < 18) { return true; }//continueと同等の処理

        //表示合計行取得
        var tval = $(tr).find('td[data-name="VAL9"] a');

        //作業総件数(当月)
        if (i == 18) { thisMonth.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //作業総件数(半期)
        else if (i == 19) { half.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //作業総件数(年度)
        else { total.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
    });

    //ループして表示(作業総件数)
    $.each($(overview), function (i, tr) {

        //当月
        if (i == 0)
        {
            setValue(OverviewList.List.Id, OverviewRow.Number, 0, CtrlFlag.Label, Math.trunc(thisMonth.Cnt1).toLocaleString());
        }
        //半期
        else if (i == 1)
        {
            setValue(OverviewList.List.Id, OverviewRow.Number, 1, CtrlFlag.Label, Math.trunc(half.Cnt1).toLocaleString());
        }
        //年度
        else
        {
            setValue(OverviewList.List.Id, OverviewRow.Number, 2, CtrlFlag.Label, Math.trunc(total.Cnt1).toLocaleString());
        }
    });

    //初期化
    thisMonth.Cnt1 = 0;
    half.Cnt1 = 0;
    total.Cnt1 = 0;

    //詳細(作業計画性分類2)全体行取得******************************************************************
    var plan2 = $(P_Article).find("#" + DetailList.WorkPlanList2.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //ループして取得(合計行)
    $.each($(plan2), function (i, tr) {

        //取得したい行以外飛ばす
        if (i > 2) { return true; }//continueと同等の処理

        //表示合計行取得
        var tval = $(tr).find('td[data-name="VAL9"]');

        //計画作業率(当月)
        if (i == 0) { thisMonth.Cnt1 = Number(getCellVal(tval).replace("%", "")); }
        //計画作業率(半期)
        else if (i == 1) { half.Cnt1 = Number(getCellVal(tval).replace("%", "")); }
        //計画作業率(年度)
        else { total.Cnt1 = Number(getCellVal(tval).replace("%", "")); }
    });

    //ループして表示(計画作業率)
    $.each($(overview), function (i, tr) {

        //当月
        if (i == 0)
        {
            setValue(OverviewList.List.Id, OverviewRow.Workrate, 0, CtrlFlag.Label, Math.trunc(thisMonth.Cnt1));
        }
        //半期
        else if (i == 1)
        {
            setValue(OverviewList.List.Id, OverviewRow.Workrate, 1, CtrlFlag.Label, Math.trunc(half.Cnt1));
        }
        //年度
        else
        {
            setValue(OverviewList.List.Id, OverviewRow.Workrate, 2, CtrlFlag.Label, Math.trunc(total.Cnt1));
        }
    });

    //初期化
    thisMonth.Cnt1 = 0;
    half.Cnt1 = 0;
    total.Cnt1 = 0;

    //詳細(その他)全体行取得***************************************************************************
    var other = $(P_Article).find("#" + DetailList.OthersList.Id + getAddFormNo() + " tbody tr:not([class^='base_tr'])");

    //ループして取得(合計行)
    $.each($(other), function (i, tr) {

        //表示合計行取得
        var tval = $(tr).find('td[data-name="VAL9"] a');

        //呼び出し回数(当月)
        if (i == 0) { thisMonth.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //呼び出し回数(半期)
        else if (i == 1) { half.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
        //呼び出し回数(年度)
        else { total.Cnt1 = Number(getCellVal(tval).replace(/,/g, '')); }
    });

    //ループして表示(呼び出し回数)
    $.each($(overview), function (i, tr) {

        //当月
        if (i == 0)
        {
            setValue(OverviewList.List.Id, OverviewRow.CallCnt, 0, CtrlFlag.Label, Math.trunc(thisMonth.Cnt1).toLocaleString());
        }
        //半期
        else if (i == 1)
        {
            setValue(OverviewList.List.Id, OverviewRow.CallCnt, 1, CtrlFlag.Label, Math.trunc(half.Cnt1).toLocaleString());
        }
        //年度
        else
        {
            setValue(OverviewList.List.Id, OverviewRow.CallCnt, 2, CtrlFlag.Label, Math.trunc(total.Cnt1).toLocaleString());
        }
    });

}