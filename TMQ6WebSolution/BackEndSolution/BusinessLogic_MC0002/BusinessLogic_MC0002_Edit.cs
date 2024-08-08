using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonWebTemplate.CommonDefinitions;
using static CommonTMQUtil.CommonTMQUtil.StructureLayerInfo;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_MC0002.BusinessLogicDataClass_MC0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

using ComDao = CommonTMQUtil.TMQCommonDataClass;
using static CommonTMQUtil.CommonTMQConstants.MsStructure;

namespace BusinessLogic_MC0002
{
    /// <summary>
    /// 機器別管理基準標準 詳細編集画面
    /// </summary>
    public partial class BusinessLogic_MC0002 : CommonBusinessLogicBase
    {
        #region 検索処理
        /// <summary>
        /// 詳細編集画面 検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchEdit()
        {
            // クリックされたボタン(新規・複写・修正)を判定
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsNew())
            {
                // 新規 ボタンで開かれた場合
                return initNew();

            }
            else
            {
                // 複写・修正 ボタンで開かれた場合
                return initEditCopy();
            }

            return true;

            // 新規 ボタンで開かれた場合の初期検索処理
            bool initNew()
            {
                // 画面左側のツリーで選択されている項目を取得
                Dao.searchResultList structureLayer = new();

                // 場所階層ツリーで選択されている項目を取得
                structureLayer.LocationStructureId = getTreeValue(true);

                // 職種機種階層ツリーで選択されている項目を取得
                structureLayer.JobStructureId = getTreeValue(false);

                // 取得した結果に対して、場所階層、職種の情報を設定する
                IList<Dao.searchResultList> structureLayerList = new List<Dao.searchResultList> { structureLayer };
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResultList>(ref structureLayerList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

                // 取得結果を一覧に設定する
                foreach (var ctrlId in getResultMappingInfoByGrpNo(ConductInfo.FormEdit.GroupNo).CtrlIdList)
                {
                    // 画面項目に値を設定
                    if (!SetFormByDataClass(ctrlId, new List<Dao.searchResultList> { structureLayer }))
                    {
                        // エラーの場合
                        return false;
                    }
                }

                return true;
            }

            // 複写・修正 ボタンで開かれた場合の初期検索処理
            bool initEditCopy()
            {
                // 検索条件を取得
                Dao.searchCondition condition = getDetailSearchCondition(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsInfo, new List<string> { "ManagementStandardsId" });

                // 標準件名情報 検索
                if (!searchManagementStandardsInfo(condition, ConductInfo.FormEdit.GroupNo, new List<string> { "IsDetail", nameof(Dao.searchResultList.ManagementStandardsName), nameof(Dao.searchResultList.Memo) }))
                {
                    return false;
                }

                return true;
            }

            // ツリーの階層IDの値が単一の場合その値を返す処理
            int? getTreeValue(bool isLocation)
            {
                var list = isLocation ? GetLocationTreeValues() : GetJobTreeValues();
                if (list != null && list.Count == 1)
                {
                    // 値が単一でもその下に紐づく階層が複数ある場合は初期表示しないので判定
                    // 場所階層の場合、工場階層までをチェック
                    int maxLayerNo = isLocation ? (int)StructureLayerNo.Location.Factory : -1;
                    bool result = TMQUtil.GetButtomValueFromTree(list[0], this.db, this.LanguageId, out int buttomId, maxLayerNo);
                    return result ? buttomId : null;
                }
                return null;
            }
        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 詳細編集画面 登録処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool registEdit()
        {
            // 画面で入力された情報を取得
            DateTime now = DateTime.Now;
            Dao.searchResultList registInfo = getRegistInfo<Dao.searchResultList>(ConductInfo.FormEdit.GroupNo, now);

            // 階層情報の取得
            IList<Dao.searchResultList> registStructureInfo = new List<Dao.searchResultList> { registInfo };
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.searchResultList>(ref registStructureInfo, new List<StructureType> { StructureType.Location, StructureType.Job });

            // ツリーで選択された項目のIDが格納されているのは「○○Name」のプロパティ名なのでテーブルのカラムと一致するプロパティ名に格納しなおす
            // 地区、工場、職種、機種大分類、機種中分類、機種小分類が該当(ツリーで選択されていない場合は「0」を格納)
            registInfo.LocationDistrictStructureId = int.Parse(registInfo.DistrictName);
            registInfo.LocationFactoryStructureId = !string.IsNullOrEmpty(registInfo.FactoryName) ? int.Parse(registInfo.FactoryName) : null;
            registInfo.JobKindStructureId = !string.IsNullOrEmpty(registInfo.JobName) ? int.Parse(registInfo.JobName) : null;
            registInfo.JobLargeClassficationStructureId = !string.IsNullOrEmpty(registInfo.LargeClassficationName) ? int.Parse(registInfo.LargeClassficationName) : null;
            registInfo.JobMiddleClassficationStructureId = !string.IsNullOrEmpty(registInfo.MiddleClassficationName) ? int.Parse(registInfo.MiddleClassficationName) : null;
            registInfo.JobSmallClassficationStructureId = !string.IsNullOrEmpty(registInfo.SmallClassficationName) ? int.Parse(registInfo.SmallClassficationName) : null;

            // 排他チェック(更新のみ)
            if (TransActionDivIsEdit() && !checkExclusiveSingle(ConductInfo.FormEdit.ControlId.ManagementStandardsInfo))
            {
                return false;
            }

            // 登録処理
            if (!registDb(registInfo, out long newManagementStandardsId))
            {
                return false;
            }

            // 再検索処理を実装する
            //登録した機器別管理基準標準IDを設定（詳細画面を表示するための情報設定）
            var pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.ManagementStandardsInfo, this.pageInfoList);
            ComDao.McManagementStandardsEntity registedInfo = new ComDao.McManagementStandardsEntity();

            // 機器別管理基準標準IDを設定(新規・複写の場合は新規採番したID、修正の場合は修正対象のIDを使用)
            registedInfo.ManagementStandardsId = TransActionDivIsEdit() ? registInfo.ManagementStandardsId : newManagementStandardsId;

            // 画面に値を設定
            SetSearchResultsByDataClass<ComDao.McManagementStandardsEntity>(pageInfo, new List<ComDao.McManagementStandardsEntity> { registedInfo }, 1);

            // 一覧画面用のデータ取得（一覧画面に戻った際、再検索をせず一覧表示データを直接更新する）
            getListRowData();

            return true;

            //一覧用のデータと総件数をグローバルリストへ設定
            void getListRowData()
            {
                //検索は行わず、詳細画面の値から取得する（速度改善）
                Dao.searchResultList result = new();
                result.ManagementStandardsId = registedInfo.ManagementStandardsId;

                // ページ情報取得
                PageInfo pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);
                pageInfo.CtrlId = ConductInfo.FormList.ControlId.List;
                var list = ConvertResultsToTmpTableListByDataClassForList(pageInfo, new List<Dao.searchResultList>() { result });
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                //ステータスを設定(１行目)
                dicList.Add(new Dictionary<string, object>() { { "STATUS", TransActionDivIsEdit() ? TMPTBL_CONSTANTS.ROWSTATUS.Edit : TMPTBL_CONSTANTS.ROWSTATUS.New } });
                foreach (var obj in list)
                {
                    //データを設定(２行目)　値はjavascript側で詳細画面の値を取得する
                    Dictionary<string, object> dic = new Dictionary<string, object>(obj as IDictionary<string, object>);
                    dicList.Add(dic);
                }
                //グローバルリストへ設定
                SetGlobalData(GlobalKey.MC0002UpdateListData, dicList);
                if (!TransActionDivIsEdit())
                {
                    //総件数を取得
                    object oldCount = GetGlobalData(GlobalKey.MC0002AllListCount);
                    long count = oldCount == null ? 0 : Convert.ToInt64(oldCount);
                    //グローバルリストへ総件数を設定
                    SetGlobalData(GlobalKey.MC0002AllListCount, count + 1);
                }
            }
        }

        /// <summary>
        /// SQL実行
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="newManagementStandardsId">採番した機器別管理基準標準ID(新規・複写の場合に採番)</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registDb(Dao.searchResultList registInfo, out long newManagementStandardsId)
        {
            // ページ遷移用の機器別管理基準標準ID
            newManagementStandardsId = -1;

            if (TransActionDivIsEdit())
            {
                // UPDATE文
                return TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.UpdateManagementStandardsInfo, SqlName.Edit.SubDirEdit, registInfo, db);

            }
            else
            {
                // INSERT文
                return TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out newManagementStandardsId, SqlName.Edit.InsertManagementStandardsInfo, SqlName.Edit.SubDirEdit, registInfo, db);
            }
        }

        /// <summary>
        /// 画面の起動種類が新規・複写・修正のうち、「修正」かどうか
        /// </summary>
        /// <returns>「修正」の場合はTrue</returns>
        private bool TransActionDivIsEdit()
        {
            // 画面の起動種類が新規・複写・修正のうち、「修正」かどうか(True/False)を返す
            return this.TransActionDiv == LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit;
        }
        #endregion
    }
}
