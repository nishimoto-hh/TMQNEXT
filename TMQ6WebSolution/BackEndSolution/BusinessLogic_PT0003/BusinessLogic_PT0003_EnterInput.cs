using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Const = CommonTMQUtil.CommonTMQConstants;
using Dao = BusinessLogic_PT0003.BusinessLogicDataClass_PT0003;
using STDData = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace BusinessLogic_PT0003
{
    /// <summary>
    /// 予備品管理　棚卸(入庫単価入力)
    /// </summary>
    public partial class BusinessLogic_PT0003 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchEnterInputList()
        {
            // 遷移元のディクショナリー情報取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.EnterList.List);

            // 検索条件
            Dao.inoutList conditionObj = new Dao.inoutList();

            // 遷移元の一覧より棚差調整IDの取得
            SetDataClassFromDictionary(targetDic, ConductInfo.FormList.EnterList.List, conditionObj, new List<string> { "InventoryDifferenceId" });

            // 言語ID設定
            conditionObj.LanguageId = this.LanguageId;

            // 入庫単価入力画面検索
            var enterInputResult = TMQUtil.SqlExecuteClass.SelectEntity<Dao.enterInput>(SqlName.GetEnterInput, SqlName.SubDir, conditionObj, this.db);

            // 数量と単位結合
            enterInputResult.InoutQuantityDisplay = TMQUtil.CombineNumberAndUnit(TMQUtil.roundDigit(enterInputResult.InoutQuantity.ToString(), enterInputResult.UnitDigit, enterInputResult.UnitRoundDivision), enterInputResult.UnitName, false);

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.FormEdit.EnterInput, this.pageInfoList);
            pageInfo.CtrlId = ConductInfo.FormEdit.EnterInput;                     // GetPageInfoでは取得出来ないのでコントロールID設定
            pageInfo.CtrlType = FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn1;        // 一覧パターン

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.enterInput>(pageInfo, new List<Dao.enterInput> { enterInputResult }, 1, true))
            {
                // 正常終了
                return true;
            }
            // エラー
            return false;
        }

        /// <summary>
        /// 入庫単価入力画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool enterInputRegist()
        {
            // 排他チェック
            if (isErrorEnterInput())
            {
                return false;
            }

            // 入力チェック
            if (isCheckEnterInput())
            {
                return false;
            }

            // 画面情報取得
            DateTime now = DateTime.Now;
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            Dao.enterInput registInfo = getEnterInputRegistInfo<Dao.enterInput>(TargetGrpNo.EnterInput, now);

            // 登録
            if (!inputRegistDb(registInfo))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorEnterInput()
        {
            // 単一の場合の排他チェック
            if (!checkExclusiveSingle(ConductInfo.FormEdit.EnterInput))
            {
                // エラーの場合
                return true;
            }

            // 排他チェックOK
            return false;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isCheckEnterInput()
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

            // 正常
            return false;

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持

                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(ConductInfo.FormEdit.EnterInput);

                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 単一の内容を取得
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormEdit.EnterInput);

                // Dictionaryをデータクラスに変換
                Dao.enterInput result = new();
                SetDataClassFromDictionary(targetDic, ConductInfo.FormEdit.EnterInput, result);

                // エラー情報格納クラス
                ErrorInfo errorInfo = new ErrorInfo(targetDic);

                //エラーメッセージ
                string errMsg = string.Empty;

                // 入庫単価の入力がない場合
                if (result.UnitPrice == null)
                {
                    isError = true;
                    // 「入力してください。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941220009 });
                }
                // 入庫単価が0以下の場合
                else if (result.UnitPrice <=  0)
                {
                    isError = true;
                    // 「入庫単価は0より大きい値を入力してください。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941260012, ComRes.ID.ID111220008, "0" });
                }
                // 10桁以上入力された場合
                else if (result.UnitPrice < 0 || result.UnitPrice > 9999999999.99m)
                {
                    isError = true;
                    // 「入庫単価は整数部10桁以下で入力してください。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111220008, ComRes.ID.ID911140003, "10" });

                }

                // エラーであればエラー情報追加
                if (isError)
                {
                    string val = info.getValName("unit_price"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                    errorInfo.setError(errMsg, val);            // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                }

                return isError;
            }
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getEnterInputRegistInfo<T>(short groupNo, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

            T resultInfo = new();

            // コントロールIDより画面の項目を取得
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ConductInfo.FormEdit.EnterInput);
            var mapInfo = grpMapInfo.selectByCtrlId(ConductInfo.FormEdit.EnterInput);
            // 登録データの設定
            if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
            {
                // エラーの場合終了
                return resultInfo;
            }
            return resultInfo;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <returns>エラーの場合False</returns>
        private bool inputRegistDb(Dao.enterInput registInfo)
        {
            // 登録SQL実行
            bool result = TMQUtil.SqlExecuteClass.Regist(SqlName.UpdateInoutQuantity, SqlName.SubDir, registInfo, this.db);
            return result;
        }
    }
}