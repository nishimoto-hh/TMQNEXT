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
using Dao = BusinessLogic_PT0007.BusinessLogicDataClass_PT0007;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_PT0007
{
    /// <summary>
    /// 部門移庫タブ
    /// </summary>
    public partial class BusinessLogic_PT0007 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 部門移庫タブ 初期化処理
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="dataCnt">部門別在庫一覧の件数</param>
        /// <returns>エラーの場合False</returns>
        private bool initDepartment(Dao.searchCondition searchCondition, out int dataCnt)
        {
            dataCnt = 0;

            // 部門別在庫一覧
            if (!setDepartmentList(searchCondition, ref dataCnt))
            {
                return false;
            }

            // 移庫先情報
            if (!setDepartmentInfoTo(searchCondition))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 部門別在庫一覧検索
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="dataCnt">部門別在庫一覧のデータ件数</param>
        /// <returns>エラーの場合False</returns>
        private bool setDepartmentList(Dao.searchCondition searchCondition, ref int dataCnt)
        {
            // 修正で棚番移庫からの遷移の場合は何もしない
            if (searchCondition.TransFlg == TransFlg.FlgEdit && searchCondition.TransType == TransType.Subject)
            {
                return true;
            }

            // 画面遷移フラグを判定
            string sqlName = string.Empty;

            if (searchCondition.TransFlg == TransFlg.FlgNew)
            {
                // 新規の場合
                sqlName = SqlName.Department.GetDepartmentListNew;
            }
            else
            {
                // 修正の場合
                sqlName = SqlName.Department.GetDepartmentListEdit;
            }

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string outSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

            // SQL実行
            IList<Dao.departmentList> results = db.GetListByDataClass<Dao.departmentList>(outWithSql + outSql, searchCondition);
            if (results == null)
            {
                return false;
            }

            // 部門別在庫一覧のデータ件数
            dataCnt = results.Count;

            // 丸め処理・数量と単位の結合
            results.ToList().ForEach(x => x.JoinStrAndRound());

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.DepartmentControlId.DepartmentInventoryList, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.departmentList>(pageInfo, results, results.Count))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移庫先情報検索
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool setDepartmentInfoTo(Dao.searchCondition searchCondition)
        {
            // 新規または棚番移庫からの遷移の場合は何もしない
            if (searchCondition.TransFlg == TransFlg.FlgNew || searchCondition.TransType == TransType.Subject)
            {
                return true;
            }

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Department.GetDepartmentInfoTo, out string outSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

            // SQL実行
            IList<Dao.departmentInfoTo> results = db.GetListByDataClass<Dao.departmentInfoTo>(outWithSql + outSql, searchCondition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 丸め処理・数量と単位の結合
            results.ToList().ForEach(x => x.JoinStrAndRound(searchCondition.WorkNo));

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.DepartmentControlId.DepartmentTransferInfo, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.departmentInfoTo>(pageInfo, results, results.Count))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登録・取消処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeDepartment()
        {
            // 予備品情報を取得
            Dao.partsInfo partsInfo = getPartsInfo();

            // 移庫先情報を取得
            Dao.departmentInfoTo departmentInfoTo = getDepartmentInfo();

            // 登録条件の作成
            TMQUtil.PartsInventory.MoveDepartment moveDepartment = new(this.db, this.UserId, this.LanguageId);
            TMQDao.PartsInventory.MoveDepartment condition = setNewCondition();

            // ボタンコントロールIDを判定
            if (this.CtrlId == ConductInfo.ButtonCtrlId.RegistDepartment)
            {
                // 登録処理
                if (!registDepartment(partsInfo, departmentInfoTo, moveDepartment, condition))
                {
                    return false;
                }
            }
            else
            {
                // 取消処理
                if (!cancelDepartment(departmentInfoTo, moveDepartment))
                {
                    return false;
                }
            }

            return true;

            TMQDao.PartsInventory.MoveDepartment setNewCondition()
            {
                TMQDao.PartsInventory.MoveDepartment condition = new();
                condition.LotControlId = departmentInfoTo.LotControlId;             // ロット管理ID
                condition.DepartmentStructureId = departmentInfoTo.DepartmentId;    // 部門
                condition.AccountStructureId = departmentInfoTo.SubjectId;          // 勘定科目
                condition.ManagementDivision = departmentInfoTo.ManagementDivision; // 管理区分
                condition.ManagementNo = departmentInfoTo.ManagementNo;             // 管理No.
                condition.InoutDatetime = departmentInfoTo.RelocationDate;          // 移庫日
                condition.UnitPrice = (double?)departmentInfoTo.UnitPrice;          // 移庫単価

                return condition;
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="partsInfo">予備品情報</param>
        /// <param name="departmentInfoTo">移庫先情報</param>
        /// <param name="moveDepartment">SQL実行クラス</param>
        /// <param name="condition">DB登録情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registDepartment(Dao.partsInfo partsInfo, Dao.departmentInfoTo departmentInfoTo, TMQUtil.PartsInventory.MoveDepartment moveDepartment, TMQDao.PartsInventory.MoveDepartment condition)
        {
            // 棚卸確定日を取得するための条件を作成
            List<Dao.getInventoryDate> inventoryCondition = getInventoryDateCondition();

            // 入力チェック
            if (isErrorRegistCommon(partsInfo, ConductInfo.DepartmentControlId.DepartmentTransferInfo, departmentInfoTo.RelocationDate, inventoryCondition, departmentInfoTo.LotControlId))
            {
                return false;
            }

            // タブ内入力チェック
            if (isErrorRegistDepartment(departmentInfoTo))
            {
                return false;
            }

            // 新規か修正を判定
            if (partsInfo.TransFlg == TransFlg.FlgNew)
            {
                // 新規
                if (!moveDepartment.New(condition, out long newWorkNo))
                {
                    return false;
                }
            }
            else
            {
                // 修正

                // 排他チェック
                if (!checkExclusiveDepartment(departmentInfoTo))
                {
                    return false;
                }

                // 移庫先に対する受払が発生している場合はエラー
                if (isAppearInoutData(departmentInfoTo))
                {
                    return false;
                }

                // 登録処理
                if (!moveDepartment.Update(condition, departmentInfoTo.WorkNo, out long newWorkNo))
                {
                    return false;
                }
            }

            return true;

            List<Dao.getInventoryDate> getInventoryDateCondition()
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Department.GetConditionToGetInventoryDate, out string outSql);

                // 棚卸確定日を取得するための条件を取得(移庫元)
                Dao.getInventoryDate conditionBefore = new();
                conditionBefore.PartsId = partsInfo.PartsId;                                 // 予備品ID
                conditionBefore.OldNewStructureId = departmentInfoTo.OldNewStructureId;      // 新旧区分
                conditionBefore.DepartmentStructureId = departmentInfoTo.DepartmentIdBefore; // 部門ID
                conditionBefore.AccountStructureId = departmentInfoTo.AccountIdBefore;       // 勘定科目ID
                // SQL実行
                IList<Dao.getInventoryDate> condListBefore = this.db.GetListByDataClass<Dao.getInventoryDate>(outSql, conditionBefore);

                // 棚卸確定日を取得するための条件を取得(移庫先)
                Dao.getInventoryDate conditionAfter = new();
                conditionAfter.PartsId = partsInfo.PartsId;                            // 予備品ID
                conditionAfter.OldNewStructureId = departmentInfoTo.OldNewStructureId; // 新旧区分
                conditionAfter.DepartmentStructureId = departmentInfoTo.DepartmentId;  // 部門ID
                conditionAfter.AccountStructureId = departmentInfoTo.SubjectId;        // 勘定科目ID
                // SQL実行
                IList<Dao.getInventoryDate> condListAfter = this.db.GetListByDataClass<Dao.getInventoryDate>(outSql, conditionAfter);

                // 条件リスト
                List<Dao.getInventoryDate> list = new();
                // 条件リストに追加(移庫元)
                foreach (Dao.getInventoryDate condition in condListBefore)
                {
                    list.Add(condition);
                }
                // 条件リストに追加(移庫先)
                foreach (Dao.getInventoryDate condition in condListAfter)
                {
                    list.Add(condition);
                }

                return list;
            }
        }

        /// <summary>
        /// 取消処理
        /// </summary>
        /// <param name="departmentInfoTo">移庫先情報</param>
        /// <param name="moveDepartment">SQL実行クラス</param>
        /// <returns>エラーの場合False</returns>
        private bool cancelDepartment(Dao.departmentInfoTo departmentInfoTo, TMQUtil.PartsInventory.MoveDepartment moveDepartment)
        {
            // 排他チェック
            if (!checkExclusiveDepartment(departmentInfoTo))
            {
                return false;
            }

            // 移庫先に対する受払が発生している場合はエラー
            if (isAppearInoutData(departmentInfoTo))
            {
                return false;
            }

            // 取消処理
            if (!moveDepartment.Cancel(departmentInfoTo.WorkNo))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移庫先の取得
        /// </summary>
        /// <returns>登録内容のデータクラス</returns>
        private Dao.departmentInfoTo getDepartmentInfo()
        {
            // コントロールIDにより画面の項目(一覧)を取得
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.DepartmentControlId.DepartmentTransferInfo);

            // TODO:ユーザIDを数値に変換するのは共通化
            Dao.departmentInfoTo departmentInfo = new();
            if (!SetExecuteConditionByDataClass<Dao.departmentInfoTo>(result, ConductInfo.DepartmentControlId.DepartmentTransferInfo, departmentInfo, DateTime.Now, this.UserId, this.UserId))
            {
                // エラーの場合終了
                return departmentInfo;
            }

            departmentInfo.UserFactoryId = TMQUtil.GetUserFactoryId(this.UserId, db); // 本務工場
            return departmentInfo;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="departmentInfoTo">移庫先情報</param>
        /// <returns>エラーの場合False</returns>
        private bool checkExclusiveDepartment(Dao.departmentInfoTo departmentInfoTo)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Department.GetMaxUpdateDateDepartment, out string outSql);

            // 処理時の更新シリアルID
            int updateDatetime = db.GetEntity<int>(outSql, departmentInfoTo);

            // 初期表示時の更新シリアルIDと処理時の更新シリアルIDを比較
            if (departmentInfoTo.UpdateSerialid != updateDatetime)
            {
                // 排他エラー
                setExclusiveError();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="departmentInfoTo">移庫先情報</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorRegistDepartment(Dao.departmentInfoTo departmentInfoTo)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 単一の項目の場合のイメージ
            if (isErrorRegistForSingle(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                // エラー情報格納クラス
                var targetDepartment = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.DepartmentControlId.DepartmentTransferInfo);
                ErrorInfo errorInfoDepartment = new ErrorInfo(targetDepartment);

                // エラー情報を画面に設定するためのマッピング情報リスト
                var infoDepartment = getResultMappingInfo(ConductInfo.DepartmentControlId.DepartmentTransferInfo);

                string errMsg = string.Empty; // エラーメッセージ
                string val = string.Empty;    // エラー項目の項目番号

                // 入庫単価が0以下の場合はエラー
                if (departmentInfoTo.UnitPrice <= 0)
                {
                    // 「入庫単価は0以下で登録できません。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141060007, ComRes.ID.ID111220008 });
                    val = infoDepartment.getValName("UnitPrice");
                    errorInfoDepartment.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfoDepartment.Result); // エラー情報を追加
                    return true;
                }

                // 入庫単価が10桁より多い場合エラー
                if (departmentInfoTo.UnitPrice > 9999999999.99m)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111220008, ComRes.ID.ID911140003, "10" }); // 「入庫単価は整数部10桁以下で入力してください。」
                    val = infoDepartment.getValName("UnitPrice");
                    errorInfoDepartment.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfoDepartment.Result); // エラー情報を追加
                    return true;
                }

                return false;
            }
        }
    }
}
