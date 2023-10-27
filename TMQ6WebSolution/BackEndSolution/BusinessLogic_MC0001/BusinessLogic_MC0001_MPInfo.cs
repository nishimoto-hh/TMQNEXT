using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MC0001.BusinessLogicDataClass_MC0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_MC0001
{
    /// <summary>
    /// 機器台帳(MP情報タブ)
    /// </summary>
    public partial class BusinessLogic_MC0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlNameMpInfo
        {
            /// <summary>SQL名：MP情報 MP情報一覧取得</summary>
            public const string GetMpInfoList = "GetMpInfoList";
            /// <summary>SQL名：MP情報 MP情報登録処理</summary>
            public const string InsertMpInfo = "InsertMpInfo";
            /// <summary>SQL名：MP情報 MP情報更新処理</summary>
            public const string UpdateMpInfo = "UpdateMpInfo";
            /// <summary>SQL名：MP情報 MP情報削除処理</summary>
            public const string DeleteMpInfoByMpInfoId = "DeleteMpInfoByMpInfoId";
            /// <summary>SQL名：MP情報 MP情報に紐付く文書を検索</summary>
            public const string GetAttachmentCount = "GetAttachmentCount";
            /// <summary>SQL名：MP情報 MP情報に紐付く文書の最大更新日時を取得</summary>
            public const string GetMaxDateByKeyId = "GetMaxDateByKeyId";
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlIdMpInfo
        {
            /// <summary>
            /// MP情報 非表示一覧
            /// </summary>
            public const string MpInfoHideList = "BODY_280_00_LST_1";
            /// <summary>
            /// MP情報 MP情報一覧
            /// </summary>
            public const string MpInfoList = "BODY_290_00_LST_1";
        }

        /// <summary>
        /// MP情報一覧で行削除ボタン(-アイコン)クリック時に連携される削除ボタンの名称
        /// </summary>
        private const string DeleteMpInfo = "DeleteMpInfo";

        /// <summary>
        /// MP情報検索
        /// </summary>
        /// <param name="machineId">機番ID</param>
        /// <returns>正常終了:True 異常終了:False</returns>
        private bool searchMPInfo(long machineId)
        {
            // 一覧検索SQL文の取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameMpInfo.GetMpInfoList, out string sql);

            dynamic whereParam = null; // WHERE句パラメータ
            whereParam = new { MachineId = machineId };

            // 非表示一覧に機番IDをセット
            if (!setMachineId())
            {
                return false;
            }

            // 一覧検索実行
            IList<Dao.mpInfoList> results = db.GetListByDataClass<Dao.mpInfoList>(sql, whereParam);
            if (results == null || results.Count == 0)
            {
                // 正常終了
                return true;
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlIdMpInfo.MpInfoList, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.mpInfoList>(pageInfo, results, results.Count))
            {
                // 異常終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;

            bool setMachineId()
            {
                // 非表示一覧のデータクラス
                Dao.mpInfoHideList resultMachineId = new Dao.mpInfoHideList();
                // データクラスに機番IDを設定
                resultMachineId.MachineId = machineId;             // 機番ID
                IList<Dao.mpInfoHideList> result = new List<Dao.mpInfoHideList> { resultMachineId };

                // ページ情報取得
                var pageInfo = GetPageInfo(TargetCtrlIdMpInfo.MpInfoHideList, this.pageInfoList);

                //　非表示一覧に機番IDを設定
                if (!SetSearchResultsByDataClass<Dao.mpInfoHideList>(pageInfo, result, result.Count))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// MP情報登録処理
        /// </summary>
        /// <param name="result">単票で入力されたデータ</param>
        /// <returns>エラーの場合False</returns>
        private bool registMpInfo(Dictionary<string, object> result)
        {
            // 入力された内容を取得
            Dao.mpInfoList registResult = getRegist<Dao.mpInfoList>(result, TargetCtrlIdMpInfo.MpInfoList);

            // SQL名
            string sql = string.Empty;

            // 非表示一覧の機番IDを取得
            long machineId = getMachineId();
            registResult.MachineId = machineId;

            // 取得した内容のMP情報ID(mp_information_id)で登録か更新を判定
            if (registResult.MpInformationId == null)
            {
                // MP情報IDがnullの場合は新規登録
                sql = SqlNameMpInfo.InsertMpInfo;
            }
            else
            {
                // MP情報IDがnullではない(採番されている)場合は更新
                sql = SqlNameMpInfo.UpdateMpInfo;

                // 排他チェック
                List<Dictionary<string, object>> registResults = new List<Dictionary<string, object>>() { result };
                if (!checkExclusiveList(TargetCtrlIdMpInfo.MpInfoList, registResults))
                {
                    // 排他エラー
                    return false;
                }
            }

            // 登録・更新SQL実行
            if (!registUpdateDb<Dao.mpInfoList>(registResult, sql))
            {
                return false;
            }

            return true;

            long getMachineId()
            {
                string ctrlId = TargetCtrlIdMpInfo.MpInfoHideList;
                // コントロールIDにより画面の項目(非表示一覧)を取得
                Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);

                // 登録対象の画面項目定義の情報
                var mappingInfo = getResultMappingInfo(ctrlId);
                string machineIdVal = getValNoByParam(mappingInfo, "MachineId"); // 機番IDの項目番号

                return long.Parse(result[machineIdVal].ToString());
            }
        }

        /// <summary>
        /// MP情報削除処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool deleteMpInfo()
        {
            // 削除対象一覧のコントロールID
            string ctrlId = TargetCtrlIdMpInfo.MpInfoList;

            // 一覧の選択チェックボックスがチェックされている行を取得
            List<Dictionary<string, object>> resultList = getSelectedRowsByList(this.resultInfoDictionary, ctrlId);

            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ctrlId);
            var lockKeyMaps = GetLockKeyMaps(ctrlId);

            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ctrlId);
            string mpinfoIdVal = getValNoByParam(mappingInfo, "MpInformationId"); // MP情報ID
            string maxUpdateDatetimeVal = getValNoByParam(mappingInfo, "MaxUpdateDatetime"); // 最大更新日時の項目番号

            // 排他チェック
            foreach (var targetDic in resultList)
            {
                // MP情報
                if (!CheckExclusiveStatus(targetDic, lockValMaps, lockKeyMaps))
                {
                    // 排他エラー
                    return false;
                }

                // 添付情報
                DateTime? maxDateOfList = DateTime.TryParse(targetDic[maxUpdateDatetimeVal].ToString(), out DateTime outDate) ? outDate : null;
                if (!CheckExclusiveStatusByUpdateDatetime(maxDateOfList, getMaxDateByMpInfoId(int.Parse(targetDic[mpinfoIdVal].ToString()))))
                {
                    // 排他エラー
                    return false;
                }
            }

            // SQL取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameMpInfo.DeleteMpInfoByMpInfoId, out string sql))
            {
                return false;
            }

            foreach (var resultRow in resultList)
            {
                // 削除条件取得
                Dao.mpInfoList condition = new();
                SetExecuteConditionByDataClass(resultRow, ctrlId, condition, DateTime.Now, this.UserId);

                // MP情報削除
                int result = this.db.Regist(sql, condition);
                if (result < 0)
                {
                    // 削除エラー
                    return false;
                }

                // 指定された機能タイプID、キーIDのデータが存在しない場合は何もしない
                dynamic param = null; // WHERE句パラメータ
                param = new { FunctionTypeId = (int)TMQConsts.Attachment.FunctionTypeId.MpInfo, KeyId = int.Parse(condition.MpInformationId.ToString()) };
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameMpInfo.GetAttachmentCount, out string cntSql);
                if (this.db.GetCount(cntSql, param) <= 0)
                {
                    continue;
                }

                // 添付情報削除
                if (!new ComDao.AttachmentEntity().DeleteByKeyId(TMQConsts.Attachment.FunctionTypeId.MpInfo, int.Parse(condition.MpInformationId.ToString()), this.db))
                {
                    // 削除エラー
                    return false;
                }
            }
            return true;

            DateTime? getMaxDateByMpInfoId(int id)
            {
                // 最大更新日時取得SQL
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlNameMpInfo.GetMaxDateByKeyId, out string outSql);
                Dao.mpInfoList getMaxDateParam = new Dao.mpInfoList();
                getMaxDateParam.KeyId = id;
                getMaxDateParam.FunctionTypeId = (int)TMQConsts.Attachment.FunctionTypeId.MpInfo;
                // SQL実行
                var maxDateResult = db.GetEntity(outSql, getMaxDateParam);

                return maxDateResult.max_update_datetime;
            }
        }

    }
}
