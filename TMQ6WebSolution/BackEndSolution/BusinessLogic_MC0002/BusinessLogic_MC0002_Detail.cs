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
using static CommonTMQUtil.CommonTMQUtil.StructureLayerInfo;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MC0002.BusinessLogicDataClass_MC0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_MC0002
{
    /// <summary>
    /// 機器別管理基準標準 詳細画面
    /// </summary>
    public partial class BusinessLogic_MC0002 : CommonBusinessLogicBase
    {
        #region 検索処理
        /// <summary>
        /// 詳細画面 検索処理
        /// </summary>
        /// <param name="targetDic">検索条件を取得するためのディクショナリ</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchDetail(string targetCtrlId, List<Dictionary<string, object>> targetDic)
        {
            // 詳細編集画面の登録後かどうかで検索条件の取得先の一覧のコントロールIDを変更
            object obj = GetGlobalData("MC0002_RegistFormNo", true);
            bool isFormEditRegist = obj != null;
            if (isFormEditRegist)
            {
                // どの画面で登録後の遷移か
                int fromFormNo = Convert.ToInt32(obj);
                switch (fromFormNo)
                {
                    case ConductInfo.FormEdit.FormNo:
                        targetCtrlId = ConductInfo.FormEdit.ControlId.ManagementStandardsInfo;
                        break;
                    default:
                        break;
                }
            }

            // 検索条件を取得
            Dao.searchCondition condition = getDetailSearchCondition(targetDic, targetCtrlId, new List<string> { "ManagementStandardsId" });

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl();

            // 標準件名情報 検索
            if (!searchManagementStandardsInfo(condition, ConductInfo.FormDetail.GroupNo, new List<string> { "IsDetail", nameof(Dao.searchResultList.ManagementStandardsName), nameof(Dao.searchResultList.Memo), nameof(Dao.searchResultList.DistrictName), nameof(Dao.searchResultList.FactoryName), nameof(Dao.searchResultList.JobName), nameof(Dao.searchResultList.LargeClassficationName), nameof(Dao.searchResultList.MiddleClassficationName), nameof(Dao.searchResultList.SmallClassficationName) }))
            {
                return false;
            }

            // 保全項目一覧 検索
            if (!searchManagementStandardsDetailInfo(ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList, condition, new List<string> { "IsGetTrans" }))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 詳細画面・詳細編集画面検索時・標準件名削除時の条件を取得
        /// </summary>
        /// <param name="targetCtrlId">値取得対象のコントロールID</param>
        /// <param name="keyNameList">キー項目名称のリスト</param>
        /// <returns>検索条件</returns>
        private Dao.searchCondition getDetailSearchCondition(List<Dictionary<string, object>> targetDictionary, string targetCtrlId, List<string> keyNameList)
        {
            Dao.searchCondition condition = new();

            // 検索条件を取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(targetDictionary, targetCtrlId);
            if (targetDic == null)
            {
                //取得できない場合、詳細画面の情報を取得（保全項目編集の行追加時/登録後に編集画面の戻る押下時など）
                targetCtrlId = ConductInfo.FormDetail.ControlId.ManagementStandardsInfo;
                targetDic = ComUtil.GetDictionaryByCtrlId(targetDictionary, targetCtrlId);
            }

            // 取得したディクショナリをデータクラスに反映
            SetDataClassFromDictionary(targetDic, targetCtrlId, condition, keyNameList);

            // 検索条件を追加
            condition.LanguageId = this.LanguageId; // 言語ID
            condition.UserId = this.UserId;         // ユーザーID

            return condition;
        }

        /// <summary>
        /// 標準件名情報 検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchManagementStandardsInfo(Dao.searchCondition condition, int groupNo, List<string> listUnComment)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.Common.SubDirCommon, SqlName.Common.GetManagementstandardsList, out string sql, listUnComment);

            // SQL実行
            IList<Dao.searchResultList> results = db.GetListByDataClass<Dao.searchResultList>(sql, condition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果設定対象のグループ番号(画面定義で設定)を判定
            if (groupNo == ConductInfo.FormEdit.GroupNo)
            {
                // 詳細編集画面の場合
                // 機能場所階層IDと職種機種階層IDから上位の階層を設定
                IList<Dao.searchResultList> result = new List<Dao.searchResultList> { results[0] };
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResultList>(ref result, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true, results[0].LocationFactoryStructureId == null ? 0 : (int)results[0].LocationFactoryStructureId);
            }

            // 検索結果を一覧に設定する
            foreach (var ctrlId in getResultMappingInfoByGrpNo(groupNo).CtrlIdList)
            {
                // 画面項目に値を設定
                if (!SetFormByDataClass(ctrlId, new List<Dao.searchResultList> { results[0] }))
                {
                    // エラーの場合
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 保全項目一覧 検索処理(詳細画面に表示する保全項目一覧と、保全項目一覧の詳細リンククリック時の保全項目編集画面の初期検索で使用)
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchManagementStandardsDetailInfo(string targetCtrlId, Dao.searchCondition condition, List<string> listUnComment)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDirDetail, SqlName.Detail.GetManagementstandardsDetailList, out string sql, listUnComment);

            // SQL実行
            IList<Dao.searchResultDetail> results = db.GetListByDataClass<Dao.searchResultDetail>(sql, condition);
            if (results == null || results.Count == 0)
            {
                // 検索結果設定対象の一覧のコントロールIDを判定
                if (targetCtrlId == ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList)
                {
                    // 割当ボタン非活性
                    BtnDisabled(ConductInfo.FormDetail.ButtonId.Quota, ConductInfo.FormDetail.FormNo);
                    // 詳細画面に表示する保全項目一覧の場合、機器別管理基準標準になにも紐付いていない可能性があるため検索結果が0件でもエラーとしない
                    return true;
                }
                else
                {
                    // 保全項目編集画面の場合、詳細リンクがクリックされている(=検索結果は必ず取得できるはず)ため検索結果が0件の場合はエラーとする
                    return false;
                }

            }

            //本務工場を設定（コンボボックスの絞り込み用）
            results[0].FactoryId = TMQUtil.GetUserFactoryId(this.UserId, db);

            if (targetCtrlId == ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList)
            {
                // 割当ボタン活性
                BtnActive(ConductInfo.FormDetail.ButtonId.Quota, ConductInfo.FormDetail.FormNo);
            }
            else
            {
                //保全項目編集画面の場合、未使用アイテムまたは工場個別アイテムが登録されている場合は本務工場が異なるユーザで表示できるように登録されているアイテムをコンボボックスに追加する
                //スケジュール管理基準はメンテナンス不可のため考慮不要
                results[0].InspectionSiteImportanceName = results[0].InspectionSiteImportanceStructureId + "|" + results[0].InspectionSiteImportanceName;
                results[0].InspectionSiteConservationName = results[0].InspectionSiteConservationStructureId + "|" + results[0].InspectionSiteConservationName;
                results[0].MaintainanceDivisionName = results[0].MaintainanceDivision + "|" + results[0].MaintainanceDivisionName;
                results[0].MaintainanceKindName = results[0].MaintainanceKindStructureId + "|" + results[0].MaintainanceKindName;
            }

            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(targetCtrlId, this.pageInfoList);

            // 検索結果を一覧に設定
            if (!SetSearchResultsByDataClass<Dao.searchResultDetail>(pageInfo, results, results.Count))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }
        #endregion

        #region 機器別管理基準標準 削除処理(詳細画面上部の削除ボタン)
        /// <summary>
        /// 機器別管理基準標準 削除処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteManagementStandardsInfo()
        {
            // 削除条件を取得
            Dao.searchCondition condition = getDetailSearchCondition(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsInfo, new List<string> { "ManagementStandardsId", "MaxUpdateDatetime" });

            // 機器別管理基準標準の排他チェック(更新シリアルID)
            if (!checkExclusiveSingle(ConductInfo.FormDetail.ControlId.ManagementStandardsInfo))
            {
                return false;
            }

            // 機器別管理基準標準に紐付く機器別管理基準詳細の排他チェック(最大更新日時)
            if (!checkManagementStandardsDetailExclusive(condition))
            {
                return false;
            }

            // 機器別管理基準標準 削除
            if (!new ComDao.McManagementStandardsEntity().DeleteByPrimaryKey(condition.ManagementStandardsId, this.db))
            {
                return false;
            }

            // 機器別管理基準標準詳細 削除
            // SQL取得
            TMQUtil.GetFixedSqlStatement(SqlName.Detail.SubDirDetail, SqlName.Detail.DeleteManagementStandardsDetail, out string deleteSql);
            if (this.db.Regist(deleteSql, condition) < 0)
            {
                return false;
            }

            //一覧画面のデータ更新用の値を設定（一覧画面に戻った際、再検索をせず一覧表示データを直接削除する）
            setDeleteRowDataToGlobalData();

            return true;

            //一覧画面のデータ更新用の値を設定
            void setDeleteRowDataToGlobalData()
            {
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                //削除
                dicList.Add(new Dictionary<string, object>() { { "STATUS", TMPTBL_CONSTANTS.ROWSTATUS.None } });
                //機器別管理基準標準IDのVAL値を取得
                int itemNo = mapInfoList.Where(x => x.CtrlId.Equals(ConductInfo.FormList.ControlId.List) && x.ParamName.Equals(nameof(Dao.searchResultList.ManagementStandardsId))).Select(x => x.ItemNo).FirstOrDefault();
                dicList.Add(new Dictionary<string, object>() { { "VAL" + itemNo, condition.ManagementStandardsId } });
                //グローバルリストへ設定
                SetGlobalData(GlobalKey.MC0002UpdateListData, dicList);
                //総件数を取得
                object oldCount = GetGlobalData(GlobalKey.MC0002AllListCount);
                long count = oldCount == null ? 0 : Convert.ToInt64(oldCount);
                //グローバルリストへ総件数を設定
                SetGlobalData(GlobalKey.MC0002AllListCount, count - 1);
            }
        }

        /// <summary>
        /// 機器別管理基準標準に紐付く機器別管理基準詳細の排他チェック
        /// </summary>
        /// <param name="condition">最大更新日時を取得するための検索条件</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool checkManagementStandardsDetailExclusive(Dao.searchCondition condition)
        {
            // 画面に表示されている最大更新日時とDBから取得した最大更新日時を用いて排他チェック
            if (!CheckExclusiveStatusByUpdateDatetime(condition.MaxUpdateDatetime, getMaxUpdateDate()))
            {
                // エラーの場合
                return false;
            }

            return true;

            // 機器別管理基準標準に紐付く機器別管理基準標準詳細の最大の更新日時を取得
            DateTime? getMaxUpdateDate()
            {
                // SQL取得
                TMQUtil.GetFixedSqlStatement(SqlName.Common.SubDirCommon, SqlName.Common.GetManagementstandardsList, out string sql, new List<string>() { "IsDetail" });

                // SQL実行
                IList<Dao.searchResultList> results = db.GetListByDataClass<Dao.searchResultList>(sql, condition);
                if (results == null || results.Count == 0)
                {
                    return null;
                }

                return results[0].MaxUpdateDatetime;
            }
        }
        #endregion

        #region 機器別管理基準標準詳細 削除処理(保全項目一覧の行削除ボタン)
        /// <summary>
        /// 機器別管理基準標準詳細 削除処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool deleteManagementStandardsDetailInfo()
        {
            // 削除リスト取得
            var deleteList = getSelectedRowsByList(this.resultInfoDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList);

            // 排他チェック
            if (!checkExclusiveList(ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList, deleteList))
            {
                // 排他エラー
                return false;
            }

            // 該当行を機器別管理基準標準詳細テーブルから削除
            foreach (var deleteRow in deleteList)
            {
                // レコードの情報をデータクラスに格納
                ComDao.McManagementStandardsDetailEntity deleteCondition = new();
                SetDeleteConditionByDataClass(deleteRow, ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList, deleteCondition);

                // 削除SQL実行
                if (!deleteCondition.DeleteByPrimaryKey(deleteCondition.ManagementStandardsDetailId, this.db))
                {
                    // エラーの場合
                    return false;
                }
            }

            // 詳細画面の再検索
            if (!searchDetail(ConductInfo.FormDetail.ControlId.ManagementStandardsInfo, this.resultInfoDictionary))
            {
                // エラーの場合
                return false;
            }

            return true;
        }
        #endregion
    }
}
