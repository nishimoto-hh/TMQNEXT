using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MA0001.BusinessLogicDataClass_MA0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_MA0001
{
    /// <summary>
    /// 保全活動（機器交換）
    /// </summary>
    public partial class BusinessLogic_MA0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 機器交換画面検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchReplaceMachineList()
        {
            // 画面NO毎に画面定義のコントロールIDを設定
            bool isConfirm = false;
            long? currentMachineId = null;
            long? replaceMachineId = null;
            long? currentEquipmentId = null;
            long? replaceEquipmentId = null;
            long? historyMachineId = null;
            long? historyFailureId = null;
            List<string> currentInfoIdList = new List<string>();
            List<string> replaceInfoIdList = new List<string>();
            Dao.searchCondition conditionObj = new Dao.searchCondition();

            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            if (compareId.IsStartId("Confirm"))
            {
                // 確認ボタンから呼ばれた場合、機器交換確認画面を表示
                isConfirm = true;
            }

            // 機器一覧の選択行を取得
            var machineList = getSelectedRowsByList(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.MachineList);
            // 選択行が無ければエラー
            if (machineList == null || machineList.Count == 0)
            {
                return false;
            }

            // 現場機器の機番ID取得
            currentMachineId = ConvertLong(getDictionaryKeyValue(machineList[0], "machine_id"));
            // 保全履歴機器ID取得
            historyMachineId = ConvertLong(getDictionaryKeyValue(machineList[0], "history_machine_id"));

            // 故障情報を取得
            var failureDictionary = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.FailureInfoIds[0]);
            if (failureDictionary != null)
            {
                // 保全履歴故障情報ID取得
                historyFailureId = ConvertLong(getDictionaryKeyValue(failureDictionary, "history_failure_id"));
            }

            // 機器検索画面で選択した機番IDをグローバル変数から取得
            var selectMachineId = GetGlobalData(GlobalKey.MA0001SelectMachineId, false);
            if (selectMachineId != null)
            {
                // 代替機器の機番IDに設定
                replaceMachineId = ConvertLong(selectMachineId.ToString());
            }

            // 現場機器の機番IDを検索条件に設定
            conditionObj.MachineId = currentMachineId;
            // 現場機器情報の取得
            Dao.ReplaceMachineInfo currentInfo = getMachineInfo<Dao.ReplaceMachineInfo>(conditionObj, SqlName.Replace.GetReplaceMachineInfo);
            // 代替機器の機番IDを検索条件に設定
            conditionObj.MachineId = replaceMachineId;
            // 代替機器情報の取得
            Dao.ReplaceMachineInfo replaceInfo = getMachineInfo<Dao.ReplaceMachineInfo>(conditionObj, SqlName.Replace.GetReplaceMachineInfo);

            // 登録用の関連情報を設定
            if (currentInfo != null)
            {
                // 保全履歴機器ID
                currentInfo.HistoryMachineId = historyMachineId;
                // 保全履歴故障情報ID
                currentInfo.HistoryFailureId = historyFailureId;

                if (replaceInfo != null)
                {
                    // 現場機器の(交換後)製造番号に代替機器の製造番号を設定
                    currentInfo.AfterSerialNo = replaceInfo.SerialNo;
                    // 代替機器の(交換後)製造番号に現場機器の製造番号を設定
                    replaceInfo.AfterSerialNo = currentInfo.SerialNo;
                }
            }

            // 現場機器の画面定義のコントロールIDを設定
            currentInfoIdList.AddRange(ConductInfo.FormReplaceMachine.ControlId.Replace.CurrentInfoIds.ToList());
            currentInfoIdList.AddRange(ConductInfo.FormReplaceMachine.ControlId.Confirm.CurrentInfoIds.ToList());
            // 現場機器情報の設定
            setMachineInfo<Dao.ReplaceMachineInfo>(conditionObj, currentInfoIdList, currentInfo);
            // 代替機器の画面定義のコントロールIDを設定
            replaceInfoIdList.AddRange(ConductInfo.FormReplaceMachine.ControlId.Replace.ReplaceInfoIds.ToList());
            replaceInfoIdList.AddRange(ConductInfo.FormReplaceMachine.ControlId.Confirm.ReplaceInfoIds.ToList());
            // 代替機器情報の設定
            setMachineInfo<Dao.ReplaceMachineInfo>(conditionObj, replaceInfoIdList, replaceInfo);

            return true;

            // 文字列をlong型に変換
            long? ConvertLong(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }

                string num = str.Replace(",", "");
                long? res = null;
                try
                {
                    res = long.Parse(num);
                }
                catch
                {
                    // 有効な数値文字列でない場合
                    res = null;
                }

                return res;
            }
        }

        /// <summary>
        /// 現場機器、代替機器の取得
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="condition">検索条件</param>
        /// <param name="sqlFileName">SQLファイル名</param>
        /// <returns>エラーの場合null</returns>
        private T getMachineInfo<T>(Dao.searchCondition condition, string sqlFileName)
        {
            // 検索実行
            T result = TMQUtil.SqlExecuteClass.SelectEntity<T>(sqlFileName, SqlName.SubDir, condition, this.db);
            if (result == null)
            {
                return default(T);
            }

            return result;
        }

        /// <summary>
        /// 現場機器、代替機器の設定
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="condition">検索条件</param>
        /// <param name="listCtrlIds">一覧のコントロールIDリスト</param>
        /// <param name="list">一覧情報</param>
        private void setMachineInfo<T>(Dao.searchCondition condition, List<string> listCtrlIds, T list)
        {
            // 一覧情報がない場合は処理終了
            if(list == null)
            {
                return;
            }

            // ページ情報取得
            Dictionary<string, PageInfo> pageInfos = new Dictionary<string, PageInfo>();
            foreach (string id in listCtrlIds)
            {
                var pageInfo = GetPageInfo(id, this.pageInfoList);
                pageInfos.Add(id, pageInfo);
            }

            foreach (string id in listCtrlIds)
            {
                // 検索結果の設定
                if (SetSearchResultsByDataClass<T>(pageInfos[id], new List<T> { list }, 1))
                {
                    // 正常終了
                    this.Status = CommonProcReturn.ProcStatus.Valid;
                }
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistReplace()
        {
            // 排他チェック
            string ctrlId;
            // 機器情報(現場機器)の排他チェック
            ctrlId = ConductInfo.FormReplaceMachine.ControlId.Confirm.CurrentInfoIds[0];
            if (!checkExclusiveSingle(ctrlId))
            {
                return false;
            }
            // 機器情報(代替機器)の排他チェック
            ctrlId = ConductInfo.FormReplaceMachine.ControlId.Confirm.ReplaceInfoIds[0];
            if (!checkExclusiveSingle(ctrlId))
            {
                return false;
            }

            // システム日時
            DateTime now = DateTime.Now;

            // 現場機器情報取得
            ctrlId = ConductInfo.FormReplaceMachine.ControlId.Confirm.CurrentInfoIds[0];
            Dao.ReplaceMachineInfo currentInfo = getMachineInfo<Dao.ReplaceMachineInfo>(ctrlId, now);
            // 代替機器情報取得
            ctrlId = ConductInfo.FormReplaceMachine.ControlId.Confirm.ReplaceInfoIds[0];
            Dao.ReplaceMachineInfo replaceInfo = getMachineInfo<Dao.ReplaceMachineInfo>(ctrlId, now);

            // 機番情報(現場機器)更新
            bool returnFlg = registEquipment(currentInfo, replaceInfo, true);
            if (!returnFlg)
            {
                return false;
            }

            // 機番情報(代替機器)更新
            returnFlg = registEquipment(currentInfo, replaceInfo, false);
            if (!returnFlg)
            {
                return false;
            }

            // 保全履歴機器情報更新
            returnFlg = registHistoryMachine(currentInfo, replaceInfo);
            if (!returnFlg)
            {
                return false;
            }

            // 保全履歴故障情報更新
            returnFlg = registHistoryFailure(currentInfo, replaceInfo);
            if (!returnFlg)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機器情報に登録する内容を設定して更新
        /// </summary>
        /// <param name="currentInfo">現場機器情報</param>
        /// <param name="replaceInfo">代替機器情報</param>
        /// <param name="isCurrent">true:現場機器情報、false:代替機器情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registEquipment(Dao.ReplaceMachineInfo currentInfo, Dao.ReplaceMachineInfo replaceInfo, bool isCurrent)
        {
            // 機器ID
            var equipmentId = isCurrent ? currentInfo.EquipmentId : replaceInfo.EquipmentId;
            // 機器IDより機器情報取得
            var registEntity = new ComDao.McEquipmentEntity().GetEntity((long)equipmentId, this.db);

            // 機器情報のコントロールID
            var ctrlId = isCurrent ? ConductInfo.FormReplaceMachine.ControlId.Confirm.ReplaceInfoIds[1] : ConductInfo.FormReplaceMachine.ControlId.Confirm.CurrentInfoIds[1];
            // 交換後使用区分取得
            string useSegmentId = getValueByKeyName(ctrlId, "after_use_segment_structure_id");
            int? afterUseSegmentStructureId = string.IsNullOrEmpty(useSegmentId) ?  null : Convert.ToInt32(getValueByKeyName(ctrlId, "after_use_segment_structure_id"));

            // 機番IDの設定
            registEntity.MachineId = isCurrent ? replaceInfo.MachineId : currentInfo.MachineId;
            // 使用区分の設定
            registEntity.UseSegmentStructureId = afterUseSegmentStructureId;
            // 更新者IDと更新日時を設定
            registEntity.UpdateUserId = currentInfo.UpdateUserId;
            registEntity.UpdateDatetime = currentInfo.UpdateDatetime;

            // 機器情報更新
            bool resultRegist = TMQUtil.SqlExecuteClass.Regist(SqlName.Replace.UpdateEquipmentInfo, SqlName.SubDirMachine, registEntity, this.db);
            if (!resultRegist)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保全履歴機器情報に登録する内容を設定して更新
        /// </summary>
        /// <param name="currentInfo">現場機器情報</param>
        /// <param name="replaceInfo">代替機器情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registHistoryMachine(Dao.ReplaceMachineInfo currentInfo, Dao.ReplaceMachineInfo replaceInfo)
        {
            // 保全履歴機器IDが未設定の場合は処理終了
            if (currentInfo.HistoryMachineId == null)
            {
                return true;
            }

            // 保全履歴機器IDより保全履歴機器情報取得
            var registEntity = new ComDao.MaHistoryMachineEntity().GetEntity((long)currentInfo.HistoryMachineId, this.db);

            // 機器IDに代替機器の機器IDを設定
            registEntity.EquipmentId = replaceInfo.EquipmentId;
            // 更新者IDと更新日時を設定
            registEntity.UpdateUserId = currentInfo.UpdateUserId;
            registEntity.UpdateDatetime = currentInfo.UpdateDatetime;

            // 保全履歴機器情報更新
            bool resultRegist = TMQUtil.SqlExecuteClass.Regist(SqlName.Replace.UpdateHistoryMachineInfoAll, SqlName.SubDir, registEntity, this.db);
            if (!resultRegist)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保全履歴故障情報に登録する内容を設定して更新
        /// </summary>
        /// <param name="currentInfo">現場機器情報</param>
        /// <param name="replaceInfo">代替機器情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registHistoryFailure(Dao.ReplaceMachineInfo currentInfo, Dao.ReplaceMachineInfo replaceInfo)
        {
            // 保全履歴故障情報IDが未設定の場合は処理終了
            if (currentInfo.HistoryFailureId == null)
            {
                return true;
            }

            // 保全履歴故障情報IDより保全履歴故障情報取得
            var registEntity = new ComDao.MaHistoryFailureEntity().GetEntity((long)currentInfo.HistoryFailureId, this.db);

            // 機器IDに代替機器の機器IDを設定
            registEntity.EquipmentId = replaceInfo.EquipmentId;
            // 更新者IDと更新日時を設定
            registEntity.UpdateUserId = currentInfo.UpdateUserId;
            registEntity.UpdateDatetime = currentInfo.UpdateDatetime;

            // 保全履歴機器情報更新
            bool resultRegist = TMQUtil.SqlExecuteClass.Regist(SqlName.Regist.UpdateHistoryFailure, SqlName.SubDir, registEntity, this.db);
            if (!resultRegist)
            {
                return false;
            }
            return true;
        }
    }
}
