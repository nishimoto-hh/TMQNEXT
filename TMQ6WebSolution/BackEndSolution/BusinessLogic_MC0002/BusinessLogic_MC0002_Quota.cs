using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonTMQUtil;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static BusinessLogic_MC0002.BusinessLogicDataClass_MC0002;
using static CommonTMQUtil.CommonTMQConstants.MsStructure;
using static CommonTMQUtil.CommonTMQUtil;
using static CommonTMQUtil.CommonTMQUtil.StructureLayerInfo;
using static CommonWebTemplate.Models.Common.COM_CTRL_CONSTANTS;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MC0002.BusinessLogicDataClass_MC0002;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_MC0002
{
    /// <summary>
    /// 機器別管理基準標準 標準割当画面
    /// </summary>
    public partial class BusinessLogic_MC0002 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 検索条件の「機器別管理基準」コンボボックスアイテム「なし」
        /// </summary>
        private const int HasNotManagementStandards = 1;
        /// <summary>
        /// 検索条件の「機器別管理基準」コンボボックスアイテム「あり」
        /// </summary>
        private const int HasManagementStandards = 2;

        /// <summary>
        /// 割当時に登録する日時(登録日時・更新日時)のフォーマット
        /// </summary>
        private const string DateFormat = "yyyy/MM/dd HH:mm:ss.fff";
        /// <summary>
        /// 割当時に登録する日時(スケジュール日)のフォーマット
        /// </summary>
        private const string ScheduleDateFormat = "yyyy/MM/dd";
        #endregion

        #region 初期表示処理
        /// <summary>
        /// 標準割当画面 初期表示処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool initQuota()
        {
            // 検索条件の初期値を設定
            Dao.searchConditionQuota condition = new();

            // 詳細画面の隠し項目に表示されていた機器別管理基準標準IDを取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsInfo);
            SetDataClassFromDictionary(targetDic, ConductInfo.FormDetail.ControlId.ManagementStandardsInfo, condition, new List<string> { "ManagementStandardsId" });

            // 「機器別管理基準」コンボボックスの初期値を「なし」に設定
            condition.IsManagementStandards = HasNotManagementStandards;

            // グループより対象のコントロールIDを取得
            List<string> ctrlIdList = getResultMappingInfoByGrpNo(ConductInfo.FormQuota.GroupNo).CtrlIdList;
            foreach (var ctrlId in ctrlIdList)
            {
                // 検索条件設定対象のページ情報取得
                var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);
                // 検索条件を画面に設定
                if (!SetSearchResultsByDataClass<Dao.searchConditionQuota>(pageInfo, new List<Dao.searchConditionQuota> { condition }, 1))
                {
                    return false;
                }
            }

            // 詳細画面の保全項目一覧で選択されていたレコードを取得
            var selectedList = getSelectedRowsByList(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList);

            // 割当画面に非表示で定義している機器別管理基準標準詳細IDリスト
            List<Dao.searchResultDetail> managementStandardsDetailIdList = new();
            Dao.searchResultDetail managementStandardsDetailId = new();

            // 機器別管理基準標準詳細IDが設定されている項目番号(VAL○)を取得する
            string valDetailId = getResultMappingInfo(ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList).getValName("ManagementStandardsDetailId");

            // ディクショナリから機器別管理基準標準詳細IDを取得し、画面に表示するためのリストに格納
            foreach (var selected in selectedList)
            {
                managementStandardsDetailIdList.Add(new Dao.searchResultDetail() { ManagementStandardsDetailId = int.Parse(selected[valDetailId].ToString()) });
            }

            // 機器別管理基準標準詳細IDリストを画面に設定
            var pageInfoDetailId = GetPageInfo(ConductInfo.FormQuota.ControlId.ManagementStandardsDetailIdList, this.pageInfoList);
            SetSearchResultsByDataClass<Dao.searchResultDetail>(pageInfoDetailId, managementStandardsDetailIdList, managementStandardsDetailIdList.Count);

            return true;
        }
        #endregion

        #region 検索処理(検索ボタンクリック後)
        /// <summary>
        /// 標準割当画面 検索処理(検索ボタンクリック後)
        /// </summary>
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchMachineList()
        {
            // 入力された検索条件を取得
            getSearchCondition(out Dao.searchConditionQuota condition, out List<string> listUnComment);

            // 検索SQL実行
            if (!setMachineList(condition, listUnComment, false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 機器一覧 検索条件を取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">アンコメントリスト</param>
        private void getSearchCondition(out Dao.searchConditionQuota condition, out List<string> listUnComment)
        {
            // 入力された検索条件を取得
            condition = GetConditionInfoByGroupNo<Dao.searchConditionQuota>(ConductInfo.FormQuota.GroupNo);

            // 検索条件の地区・職種は各階層に値が設定されているが、検索には「指定された最下層の値」以下の全ての階層IDを用いるので設定
            setStructureLayerInfo(ref condition);

            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchConditionQuota>(condition);

            // 検索条件の「機器別管理基準」コンボボックスで選択されている値を判定
            if (condition.IsManagementStandards == HasNotManagementStandards)
            {
                // 「なし」
                // 機器別管理基準が紐付いていない機器を検索対象とする
                listUnComment.Add("NotExistsManagementStandards");
            }
            else if (condition.IsManagementStandards == HasManagementStandards)
            {
                // 「あり」
                // 機器別管理基準が紐付いている機器を検索対象とする
                listUnComment.Add("ExistsManagementStandards");
            }
            else
            {
                // 「すべて」が選択されていた場合は指定しないためアンコメントリストに何も追加しない
            }
        }

        /// <summary>
        /// 機器一覧 検索SQL実行～画面に設定
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="listUnComment">アンコメントリスト</param>
        /// <param name="isRedisp">割当後の再検索の場合はtrue</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool setMachineList(Dao.searchConditionQuota condition, List<string> listUnComment, bool isRedisp)
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormQuota.ControlId.MachineList, this.pageInfoList);

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.Quota.SubDirQuota, SqlName.Quota.GetMachineList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.Quota.SubDirQuota, SqlName.Quota.GetMachineList, out string withSql, listUnComment);

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl();

            /*
             * 場所階層・職種機種階層の構成IDをカンマ区切りにする(パラメータ数が2100個以上だとエラーになるため)
             * カンマ区切りしたものを検索SQL内で一時テーブルに格納する
             */
            condition.StrLocationStructureIdList = string.Join(',', condition.LocationStructureIdList);
            condition.StrJobStcuctureIdList = condition.JobStructureIdList != null ? string.Join(',', condition.JobStructureIdList) : string.Empty;

            // 総件数取得SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);
            // 総件数を取得
            int cnt = db.GetCount(executeSql, condition);

            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                if (isRedisp)
                {
                    // 再検索の場合は検索結果が無くてもエラーにしない
                    return true;
                }
                else
                {
                    return false;
                }

            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);
            var selectSql = new StringBuilder(executeSql);
            selectSql.AppendLine("ORDER BY ");       // 並び順を指定
            selectSql.AppendLine("machine_no　ASC"); // 機器番号の昇順

            // 一覧検索実行
            IList<Dao.MachineList> results = db.GetListByDataClass<Dao.MachineList>(selectSql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.MachineList>(pageInfo, results, results.Count))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 画面より取得した検索条件は地区と職種が各階層の値なので、階層IDを取得し設定する
        /// </summary>
        /// <param name="condition">ref 画面より取得した検索条件</param>
        /// <remarks>指定された階層ID配下の階層IDリストを検索条件に設定</remarks>
        private void setStructureLayerInfo(ref Dao.searchConditionQuota condition)
        {
            // 共通処理で階層IDを設定するが、画面の検索条件は画面定義の関係でプロパティ名にIDでなく名称を使用しているので置き換える
            Dao.StructureLayerCondition structureInfo = new();
            // 地区
            structureInfo.DistrictId = condition.DistrictName;
            structureInfo.FactoryId = condition.FactoryName;
            structureInfo.PlantId = condition.PlantName;
            structureInfo.SeriesId = condition.SeriesName;
            structureInfo.StrokeId = condition.StrokeName;
            structureInfo.FacilityId = condition.FacilityName;
            // 職種
            structureInfo.JobId = condition.JobName;
            structureInfo.LargeClassficationId = condition.LargeClassficationName;
            structureInfo.MiddleClassficationId = condition.MiddleClassficationName;
            structureInfo.SmallClassficationId = condition.SmallClassficationName;
            // 地区と職種の階層IDを取得
            IList<Dao.StructureLayerCondition> tempList = new List<Dao.StructureLayerCondition> { structureInfo };
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.StructureLayerCondition>(ref tempList, new List<StructureType> { StructureType.Location, StructureType.Job });

            // 取得した階層ID配下の階層IDリストを検索条件に設定
            if (tempList[0].LocationStructureId != null)
            {
                // 機能場所階層ID
                condition.LocationStructureIdList = GetLowerStructureIdList(new List<int> { tempList[0].LocationStructureId ?? -1 });
            }
            if (tempList[0].JobStructureId != null)
            {
                // 職種機種階層ID
                condition.JobStructureIdList = GetLowerStructureIdList(new List<int> { tempList[0].JobStructureId ?? -1 });
            }
        }
        #endregion

        #region 割当処理
        /// <summary>
        /// 標準割当画面 割当処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool quotaMachine()
        {
            // 再検索を行うための検索条件を取得
            getSearchCondition(out Dao.searchConditionQuota condition, out List<string> listUnComment);

            // 機器一覧で機番IDと開始日が設定されている項目番号(VAL○)を取得する
            string valMachineNo = getResultMappingInfo(ConductInfo.FormQuota.ControlId.MachineList).getValName("MachineId");
            string valStartDate = getResultMappingInfo(ConductInfo.FormQuota.ControlId.MachineList).getValName("StartDate");

            // 機器一覧で選択されたレコードを取得
            List<Dictionary<string, object>> machineList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormQuota.ControlId.MachineList);

            // 機番IDと開始日をパイプ「|」区切りにしたものをレコード分をし、それをカンマ区切りにして1つの文字列にする
            StringBuilder strIdDate = new();
            foreach (var machineDic in machineList)
            {
                strIdDate.Append(machineDic[valMachineNo]);
                strIdDate.Append("|");
                strIdDate.Append(machineDic[valStartDate]);
                strIdDate.Append(",");
            }

            // 機番IDと開始日を一時テーブルに格納するためのSQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.Quota.SubDirQuota, SqlName.Quota.CreateIdDateForProc, out string sql);
            this.db.Regist(sql);

            // 一時テーブルに登録
            TMQUtil.GetFixedSqlStatement(SqlName.Quota.SubDirQuota, SqlName.Quota.InsertIdDateForProc, out sql);
            if (this.db.Regist(sql, new { StrIdDate = strIdDate.ToString().TrimEnd(',') }) < 0)
            {
                return false;
            }

            // 非表示の機器別管理基準標準詳細IDを取得
            List<Dictionary<string, object>> managementStandardsDetailIdList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormQuota.ControlId.ManagementStandardsDetailIdList);

            // 機器別管理基準標準IDの項目番号(VAL○)を取得する
            string valDetailIdNo = getResultMappingInfo(ConductInfo.FormQuota.ControlId.ManagementStandardsDetailIdList).getValName("ManagementStandardsDetailId");

            // 機器別管理基準標準詳細IDをリストに格納
            List<string> detailIdList = new();
            foreach(var detailId in managementStandardsDetailIdList)
            {
                detailIdList.Add(detailId[valDetailIdNo].ToString());
            }

            // 詳細画面の保全項目一覧で選択されたレコードの機器別管理基準標準詳細IDを一時テーブルに格納するためのSQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.Quota.SubDirQuota, SqlName.Quota.CreateTempTargetId, out sql);
            this.db.Regist(sql);

            // 一時テーブルに登録
            TMQUtil.GetFixedSqlStatement(SqlName.Quota.SubDirQuota, SqlName.Quota.InsertTempTargetId, out sql);
            if (this.db.Regist(sql, new { ManagementStandardsDetailIdList = string.Join(',', detailIdList) }) < 0)
            {
                return false;
            }

            // プロシージャを実行しt機器別管理基準部位、機器別管理基準内容、保全スケジュール、保全スケジュール詳細テーブルにデータを登録
            TMQUtil.GetFixedSqlStatement(SqlName.Quota.SubDirQuota, SqlName.Quota.InsertManagementStandardsByProc, out sql);
            if (this.db.GetCount(sql, new { ManagementStandardsId = condition.ManagementStandardsId, DateTime = DateTime.Now.ToString(DateFormat), UserId = this.UserId, LanguageId = this.LanguageId }) < 0)
            {
                return false;
            }

            // 機器一覧の再検索
            if (!setMachineList(condition, listUnComment, true))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
