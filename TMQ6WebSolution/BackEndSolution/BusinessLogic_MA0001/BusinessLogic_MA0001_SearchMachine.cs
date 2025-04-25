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
    /// 保全活動（機器検索）
    /// </summary>
    public partial class BusinessLogic_MA0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 機器検索画面検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchMachineList()
        {
            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormSearchMachine.ControlId.SearchResult, this.pageInfoList);

            // 検索条件の取得
            Dao.searchMachineSearchCondition condition = GetConditionInfoByGroupNo<Dao.searchMachineSearchCondition>(ConductInfo.FormSearchMachine.GroupNo.SearchCondition);

            // 検索条件の地区・職種は各階層に値が設定されているが、検索には「指定された最下層の値」以下の全ての階層IDを用いるので設定
            setStructureLayerInfo(ref condition);

            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            List<string> listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchMachineSearchCondition>(condition);

            /*
             * 場所階層・職種機種階層の構成IDをカンマ区切りにする(パラメータ数が2100個以上だとエラーになるため)
             * カンマ区切りしたものを検索SQL内で一時テーブルに格納する
             */
            condition.StrLocationStructureIdList = string.Join(',', condition.LocationStructureIdList);
            condition.LocationStructureIdList = null;
            listUnComment.Add("LocationSelected");
            condition.StrJobStcuctureIdList = condition.JobStructureIdList != null ? string.Join(',', condition.JobStructureIdList) : string.Empty;

            // 職種機種が選択されていた場合
            if (condition.JobStructureIdList != null)
            {
                listUnComment.Add("JobSelected");    // 一時テーブルの使用箇所をアンコメント
                condition.JobStructureIdList = null; // パラメータのリストをNULLにする
            }

            // SQL取得(上記で取得したNullでないプロパティ名をアンコメント)
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Search.GetSearchMachineList, out string baseSql, listUnComment);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.Search.GetSearchMachineList, out string withSql, listUnComment);

            // 総件数取得SQL文の取得
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, string.Empty, withSql);
            // 総件数を取得
            int cnt = db.GetCount(executeSql, condition);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchMachineList>(pageInfo, null, cnt);
                return false;
            }

            // 一覧検索SQL文の取得
            StringBuilder sql = new StringBuilder(TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql));
            sql.Append("ORDER BY machine_no");

            // 一覧検索実行
            IList<Dao.searchMachineList> results = db.GetListByDataClass<Dao.searchMachineList>(sql.ToString(), condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchMachineList>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchMachineList>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 画面より取得した検索条件は地区と職種が各階層の値なので、階層IDを取得し設定する
        /// </summary>
        /// <param name="condition">ref 画面より取得した検索条件</param>
        /// <remarks>指定された階層ID配下の階層IDリストを検索条件に設定</remarks>
        private void setStructureLayerInfo(ref Dao.searchMachineSearchCondition condition)
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
    }
}
