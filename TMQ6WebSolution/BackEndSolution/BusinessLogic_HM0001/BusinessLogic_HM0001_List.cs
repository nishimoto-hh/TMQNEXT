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
using Dao = BusinessLogic_HM0001.BusinessLogicDataClass_HM0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_HM0001
{
    /// <summary>
    /// 変更管理 機器台帳 一覧画面
    /// </summary>
    public partial class BusinessLogic_HM0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 自分の件名のみ表示
        /// </summary>
        private const int IsDispOnlyMySubject = 1;
        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList(bool isInit)
        {
            // 初期検索時は「自分の件名のみ表示」をチェック状態にする
            if (!getSearchCondition(isInit, out int dispOnlyMySubject))
            {
                return false;
            }

            // 一覧のページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormList.ControlId.List, this.pageInfoList);

            // 「自分の件名のみ表示」がチェックされていたらSQLの該当箇所をアンコメント
            List<string> listUnComment = new();
            if(dispOnlyMySubject == IsDispOnlyMySubject)
            {
                listUnComment.Add("DispOnlyMySubject");
            }

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.List.GetHistoryMachineList, out string baseSql);

            // WITH句は別に取得
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.List.GetHistoryMachineList, out string withSql, listUnComment);

            // 場所分類＆職種機種＆詳細検索条件取得
            if (!GetWhereClauseAndParam2(pageInfo, baseSql, out string whereSql, out dynamic whereParam, out bool isDetailConditionApplied))
            {
                return false;
            }

            // 検索条件を設定
            whereParam.LanguageId = this.LanguageId; // 言語ID
            whereParam.UserId = this.UserId;         // ログインユーザーID

            // SQL、WHERE句、WITH句より件数取得SQLを作成
            string executeSql = TMQUtil.GetSqlStatementSearch(true, baseSql, whereSql, withSql);

            // 総件数を取得
            int cnt = db.GetCount(executeSql, whereParam);

            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, null, cnt, isDetailConditionApplied);
                return false;
            }

            // 一覧検索SQL文の取得
            executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder(executeSql);

            // 機器番号の昇順
            // selectSql.AppendLine("ORDER BY machine_no");

            // 一覧検索実行
            IList<Dao.searchResult> results = db.GetListByDataClass<Dao.searchResult>(selectSql.ToString(), whereParam);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref results, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, results, cnt, isDetailConditionApplied))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;
        }

        /// <summary>
        /// 検索条件を取得(初期検索時は「自分の件名のみ表示」をチェック状態にする)
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool getSearchCondition(bool isInit, out int outDispOnlyMySubject)
        {
            outDispOnlyMySubject = new();

            // 検索条件を取得する一覧のコントロールID
            string ctrlId = ConductInfo.FormList.ControlId.Condition;

            // 検索条件初期化
            Dao.searchCondition searchCondition = new();

            // ページ情報取得
            var pageInfo = GetPageInfo(ctrlId, this.pageInfoList);

            // 初期検索か判定
            if (isInit)
            {
                // 初期値を設定
                searchCondition.DispOnlyMySubject = IsDispOnlyMySubject; // 自分の件名のみ表示
                outDispOnlyMySubject = IsDispOnlyMySubject;

                if (!SetSearchResultsByDataClass<Dao.searchCondition>(pageInfo, new List<Dao.searchCondition> { searchCondition }, 1))
                {
                    return false;
                }
            }
            else
            {
                // コントロールIDにより画面の項目(一覧)を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ctrlId);
                var mappingInfo = getResultMappingInfo(ctrlId);

                // 数値に変換(変換できない場合はエラー)
                if (!int.TryParse(condition[getValNoByParam(mappingInfo, "DispOnlyMySubject")].ToString(), out int dispOnlyMySubject))
                {
                    return false;
                }

                // 取得した値を検索条件に設定
                outDispOnlyMySubject = dispOnlyMySubject; // 自分の件名のみ表示
            }

            return true;

            string getValNoByParam(MappingInfo info, string keyName)
            {
                // 項目キー名と一致する項目番号を返す
                return info.Value.First(x => x.ParamName.Equals(keyName)).ValName;
            }
        }
        #endregion
    }
}
