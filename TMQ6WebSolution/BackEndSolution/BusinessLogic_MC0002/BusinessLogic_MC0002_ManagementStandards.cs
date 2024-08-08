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
using Dao = BusinessLogic_MC0002.BusinessLogicDataClass_MC0002;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_MC0002
{
    /// <summary>
    /// 機器別管理基準標準 保全項目編集画面
    /// </summary>
    public partial class BusinessLogic_MC0002 : CommonBusinessLogicBase
    {
        #region 検索処理
        /// <summary>
        ///  保全項目編集画面 検索処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool searchManagementStandardsDetail()
        {
            // 検索条件を取得
            Dao.searchCondition condition = getDetailSearchCondition(this.searchConditionDictionary, ConductInfo.FormDetail.ControlId.ManagementStandardsDetailList, new List<string> { "ManagementStandardsId", "ManagementStandardsDetailId" });

            // 検索条件の機器別管理基準詳細IDがNULLの場合(保全項目一覧の行追加アイコンがクリックされた場合)
            if (condition.ManagementStandardsDetailId == null)
            {
                // 保全項目詳細編集画面には空のデータを設定
                return setEmptyData();
            }

            // 翻訳の一時テーブルを作成
            createTranslationTempTbl();
            // 保全項目編集画面 検索(未使用アイテム、工場個別アイテムをコンボボックスに表示する為、翻訳も取得する)
            if (!searchManagementStandardsDetailInfo(ConductInfo.FormEditManagemantStandardsDetail.ControlId.ManagementStandardsDetail, condition, new List<string> { "IsManagementStandardsDetail", "IsGetTrans" }))
            {
                return false;
            }

            return true;

            // 保全項目一覧の行追加アイコンがクリックされた場合の初期表示
            bool setEmptyData()
            {
                // 空のデータを設定(該当データなしのメッセージが表示されないように)
                var pageInfo = GetPageInfo(ConductInfo.FormEditManagemantStandardsDetail.ControlId.ManagementStandardsDetail, this.pageInfoList);
                Dao.searchResultDetail result = new();
                result.ManagementStandardsId = condition.ManagementStandardsId;
                //本務工場を設定（コンボボックスの絞り込み用）
                result.FactoryId = TMQUtil.GetUserFactoryId(this.UserId, db);
                IList<Dao.searchResultDetail> results = new List<Dao.searchResultDetail>() { result };
                if (!SetSearchResultsByDataClass<Dao.searchResultDetail>(pageInfo, results, results.Count))
                {
                    return false;
                }

                return true;
            }
        }
        #endregion

        #region 登録処理
        /// <summary>
        /// 保全項目編集画面 登録処理
        /// </summary>
        /// <returns>エラーの場合はFalse</returns>
        private bool registManagementStandardsDetail()
        {
            // 画面情報を取得
            Dictionary<string, object> resultDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormEditManagemantStandardsDetail.ControlId.ManagementStandardsDetail);
            var mapInfo = getResultMappingInfo(ConductInfo.FormEditManagemantStandardsDetail.ControlId.ManagementStandardsDetail);
            // 登録データをデータクラスへ設定
            Dao.searchResultDetail resultInfo = new();
            if (!SetExecuteConditionByDataClass(resultDic, mapInfo.Value, resultInfo, DateTime.Now, this.UserId, this.UserId))
            {
                // エラーの場合終了
                return false;
            }

            //追加の場合true、編集の場合false
            bool isRegist = resultInfo.ManagementStandardsDetailId <= 0 ? true : false;

            // 排他チェック
            if (!isRegist && !checkExclusiveSingle(ConductInfo.FormEditManagemantStandardsDetail.ControlId.ManagementStandardsDetail))
            {
                return false;
            }

            //入力チェック
            if (isErrorRegistManagementStandard(resultInfo, resultDic, ConductInfo.FormEditManagemantStandardsDetail.ControlId.ManagementStandardsDetail))
            {
                return false;
            }

            //登録
            string sqlName = isRegist ? SqlName.ManagementStandardsDetail.InsertManagementStandardsDetail : SqlName.ManagementStandardsDetail.UpdateManagementStandardsDetail;
            bool ret = TMQUtil.SqlExecuteClass.Regist(sqlName, SqlName.ManagementStandardsDetail.SubDirMsd, resultInfo, db);

            return ret;
        }

        /// <summary>
        /// 保全項目編集画面入力チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorRegistManagementStandard(Dao.searchResultDetail result, Dictionary<string, object> dic, string ctrlId)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // チェック
            if (isErrorRegistManagementStandardForSingle(ref errorInfoDictionary, result, ctrlId))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            // メソッド内処理
            bool isErrorRegistManagementStandardForSingle(ref List<Dictionary<string, object>> errorInfoDictionary, Dao.searchResultDetail result, string ctrlId)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持
                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ctrlId);

                // 同一機器、部位、保全項目重複チェック
                if (!checkContentDuplicate(result))
                {
                    // エラー情報格納クラス
                    Dictionary<string, object> targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                    ErrorInfo errorInfo = new ErrorInfo(targetDic);
                    isError = true;
                    this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                    // 入力された部位、保全項目の組み合わせは既に登録されています。
                    string errMsg = GetResMessage(ComRes.ID.ID141220002);
                    string val = info.getValName("inspection_site_name"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    val = info.getValName("inspection_content_name");     // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);
                    errorInfoDictionary.Add(errorInfo.Result);
                }

                return isError;

                // 同一機器、部位、保全項目重複チェック
                bool checkContentDuplicate(Dao.searchResultDetail result)
                {
                    // 検索SQL文の取得
                    string sql = string.Empty;
                    if (!TMQUtil.GetFixedSqlStatement(SqlName.ManagementStandardsDetail.SubDirMsd, SqlName.ManagementStandardsDetail.GetCountManagementStandardDetail, out sql))
                    {
                        this.Status = CommonProcReturn.ProcStatus.Error;
                        return false;
                    }
                    // 総件数を取得
                    int cnt = db.GetCount(sql, new { ManagementStandardsId = result.ManagementStandardsId, ManagementStandardsDetailId = result.ManagementStandardsDetailId, InspectionSiteName = result.InspectionSiteName, InspectionContentName = result.InspectionContentName });
                    if (cnt > 0)
                    {
                        return false;
                    }

                    return true;
                }

            }

        }
        #endregion

    }
}
