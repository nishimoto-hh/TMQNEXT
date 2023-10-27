using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using APComUtil = APCommonUtil.APCommonUtil.APCommonUtil;
using APConsts = APConstants.APConstants;
using APFunc = CommonAPUtil.APStoredFncUtil.APStoredFncUtil;
using APMaster = CommonAPUtil.APMasterCheckUtil.APMasterCheckUtil;
using APResources = CommonAPUtil.APCommonUtil.APResources;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComSTDUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonAPUtil.APStoredPrcUtil
{
    /// <summary>
    /// 現行AP「PD_PLAN_PACKAGE」を移植
    /// </summary>
    public class ProPlanPackage
    {
        /// <summary>
        /// SQL定義
        /// </summary>
        public static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = "PlanPackage";
            // 登録処理
            /// <summary>SQL名：処方プロシージャ取得</summary>
            public const string GetRecipeProcedure = "PlanPackage_GetRecipeProcedure";
            /// <summary>SQL名：処方フォーミュラ取得</summary>
            public const string GetRecipeFormula = "PlanPackage_GetRecipeFormula";
            // 製造指図ヘッダ登録
            /// <summary>SQL名：製造指図ヘッダ取得</summary>
            public const string GetCountDirectionHeader = "PlanPackage_GetCountDirectionHeader";
            /// <summary>SQL名：製造指図ヘッダ登録</summary>
            public const string InsertDirectionHeader = "PlanPackage_InsertDirectionHeader";
            /// <summary>SQL名：製造指図ヘッダ更新</summary>
            public const string UpdateDirectionHeader = "PlanPackage_UpdateDirectionHeader";
            /// <summary>SQL名：製造指図ヘッダプルーフ</summary>
            public const string DirectionHeaderProof = "Direction_Header_Proof";
            // 製造指図プロシージャ登録
            /// <summary>SQL名：製造指図プロシージャ取得</summary>
            public const string GetCountDirectionProcedure = "PlanPackage_GetCountDirectionProcedure";
            /// <summary>SQL名：製造指図プロシージャプルーフ</summary>
            public const string DirectionProcedureProof = "Direction_Procedure_Proof";
            /// <summary>SQL名：製造指図プロシージャ登録</summary>
            public const string InsertDirectionProcedure = "PlanPackage_InsertDirectionProcedure";
            /// <summary>SQL名：製造指図プロシージャ更新</summary>
            public const string UpdateDirectionProcedure = "PlanPackage_UpdateDirectionProcedure";
            // 製造指図フォーミュラ登録
            /// <summary>SQL名：製造指図フォーミュラ取得</summary>
            public const string GetCountDirectionFormula = "PlanPackage_GetCountDirectionFormula";
            /// <summary>SQL名：製造指図フォーミュラプルーフ</summary>
            public const string DirectionFormulaProof = "Direction_Formula_Proof";
            /// <summary>SQL名：製造指図フォーミュラ登録</summary>
            public const string InsertDirectionFormula = "PlanPackage_InsertDirectionFormula";
            /// <summary>SQL名：製造指図フォーミュラ更新</summary>
            public const string UpdateDirectionFormula = "PlanPackage_UpdateDirectionFormula";
        }

        /// <summary>
        /// 最終製品
        /// </summary>
        private const decimal FINISH_LINE_NO = 1;
        /// <summary>
        /// 部品の場合の品目タイプ
        /// </summary>
        private const int PARTS_LINE_TYPE = -1;

        /// <summary>
        /// ロット番号のシーケンス
        /// </summary>
        private const string SeqLotSeries = "LOT_SERIES_NO";
        /// <summary>
        /// 戻り値クラス
        /// </summary>
        public class ResultInfo
        {
            /// <summary>Gets or sets a value indicating whether gets or sets エラー有無</summary>
            /// <value>エラー有無</value>
            public bool IsError { get; set; }
            /// <summary>Gets or sets メッセージ</summary>
            /// <value>メッセージ</value>
            public string Message { get; set; }
            /// <summary>
            /// コンストラクタ(エラーなし)
            /// </summary>
            public ResultInfo()
            {
                this.IsError = false;
                this.Message = string.Empty;
            }
            /// <summary>
            /// コンストラクタ(値指定)
            /// </summary>
            /// <param name="isError">エラーフラグ</param>
            /// <param name="message">メッセージ</param>
            public ResultInfo(bool isError, string message)
            {
                this.IsError = isError;
                this.Message = message;
            }
        }

        /// <summary>
        /// 製造指図登録処理
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>戻り値クラスのデータクラス</returns>
        /// <remarks>FUN_PD_PLAN_DIRECTION_SET</remarks>
        public static ResultInfo RegistDirection(ParamRegistDirection param, ComDB db)
        {
            // システム日付
            DateTime now = DateTime.Now;
            // 処理で使用する、DBから取得した定数
            RegistDirectionConst constValues = new RegistDirectionConst(db);

            // 処方マスタヘッダより取得
            var getRecipeHeader = new ComDao.RecipeHeaderEntity().GetEntity(param.Header.RecipeId, db);
            var recipeHeader = new ResultGetRecipeHeader(getRecipeHeader);
            if (recipeHeader == null)
            {
                // 基本処方が見つかりません 品目='{0}' 仕様='{1}'
                string errMsg = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M10034, param.Header.ItemCd, param.Header.SpecificationCd }, param.Common.LanguageId, null, db);
                return new ResultInfo(true, errMsg);
            }

            // 生産量の計算
            bool isCalcError;
            param.Header.PlanedQty = calcPlanedQty(recipeHeader.UnitQty, param.Header.PlanedQty, recipeHeader.MinQty, recipeHeader.MaxQty, out isCalcError);
            if (isCalcError)
            {
                // 生産量が最小と最大の範囲に無い場合
                // 生産量エラー 品目='{0}' 仕様='{1}' 生産数={2}
                string errMsg = ComSTDUtil.GetPropertiesMessage(new string[] { APResources.ID.M10035, param.Header.ItemCd, param.Header.SpecificationCd, param.Header.PlanedQty.ToString() }, param.Common.LanguageId, null, db);
                return new ResultInfo(true, errMsg);
            }

            // ヘッダ登録処理
            ResultInfo headerResult = callRegistDrectionHeader(param, recipeHeader, db, now);
            if (headerResult.IsError)
            {
                return headerResult;
            }

            // 処方プロシージャより取得
            var recipeProList = db.GetListByOutsideSql<ResultGetRecipeProcedure>(SqlName.GetRecipeProcedure, SqlName.SubDir, new { RecipeId = param.Header.RecipeId });
            foreach (var recipePro in recipeProList)
            {
                int orgStepNo = recipePro.StepNo;
                recipePro.StepNo = GetDirectionStepNo(recipePro.StepNo);
                // 製造指図プロシージャ登録
                ParamRegistDirectionProcedure paramProcedure = new ParamRegistDirectionProcedure(param, recipePro);
                if (!RegistDirectionProcedure(paramProcedure, db, now))
                {
                    // 製造指図プロシージャ登録処理失敗 品目=仕様=指図数=
                    string errMsg = ComSTDUtil.GetPropertiesMessage(
                        new string[] { APResources.ID.M10036, APResources.ID.M10038, param.Header.ItemCd, param.Header.SpecificationCd, param.Header.PlanedQty.ToString() }, param.Common.LanguageId, null, db);
                    return new ResultInfo(true, errMsg);
                }

                // フォーミュラ(部品)登録処理
                ResultInfo partsResult = registDirectionFormulaParts(param, orgStepNo, recipeHeader.StdQty, constValues, db, now);
                if (partsResult.IsError)
                {
                    return partsResult;
                }

            }
            // フォーミュラ(仕上)登録処理
            ResultInfo finishResult = registDirectionFormulaFinish(param, recipeHeader.StdQty, constValues, db, now);
            if (finishResult.IsError)
            {
                return finishResult;
            }

            return new ResultInfo();
        }

        #region Publicメソッド 既存プロシージャの転向
        /// <summary>
        /// 製造指図ヘッダ登録
        /// </summary>
        /// <param name="paramHeader">RegistDirectionHeaderの引数</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramNow">現在日</param>
        /// <returns>処理失敗時、false</returns>
        /// <remarks>FUN_PD_PLAN_DIRECTION_HDREG</remarks>
        public static bool RegistDirectionHeader(ParamRegistDirectionHeader paramHeader, ComDB db, DateTime? paramNow = null)
        {
            // システム日付
            DateTime now = paramNow ?? DateTime.Now;

            // 製造指図ヘッダに登録する情報の主キー
            PkDirectionHeader prikeyValue = new PkDirectionHeader(paramHeader.Common.DirectionDivision, paramHeader.Common.DirectionNo);

            bool isInsert = true;
            // 製造指図ヘッダの件数取得
            if (db.GetCountByOutsideSql(SqlName.GetCountDirectionHeader, SqlName.SubDir, prikeyValue) > 0)
            {
                // 取得できた場合、更新
                isInsert = false;
                // 更新前プルーフ登録
                if (!RegistDirectionProof(SqlName.DirectionHeaderProof, APConsts.PROOF_STATUS.PRE_UPDATE, prikeyValue, paramHeader.Common.InputUserId, db))
                {
                    return false;
                }
            }

            // 登録情報作成
            ComDao.DirectionHeaderEntity registCondition = getDirectionHeaderCondition(paramHeader, now, db);
            // 登録
            int result = db.RegistByOutsideSql(isInsert ? SqlName.InsertDirectionHeader : SqlName.UpdateDirectionHeader, SqlName.SubDir, registCondition);
            if (result <= 0)
            {
                return false;
            }

            // プルーフ登録
            // 新規 or 更新後
            decimal proofStatus = isInsert ? APConsts.PROOF_STATUS.NEW : APConsts.PROOF_STATUS.POST_UPDATE;
            if (!RegistDirectionProof(SqlName.DirectionHeaderProof, proofStatus, prikeyValue, paramHeader.Common.InputUserId, db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 製造指図プロシージャ登録
        /// </summary>
        /// <param name="paramProc">RegistDirectionProcedureの引数</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramNow">現在日</param>
        /// <returns>処理失敗時、false</returns>
        /// <remarks>FUN_PD_PLAN_DIRECTION_PRREG</remarks>
        public static bool RegistDirectionProcedure(ParamRegistDirectionProcedure paramProc, ComDB db, DateTime? paramNow = null)
        {
            // システム日付
            DateTime now = paramNow ?? DateTime.Now;

            // 製造指図プロシージャに登録する情報の主キー
            PkDirectionProcedure prikeyValue = new PkDirectionProcedure(paramProc.Common.DirectionDivision, paramProc.Common.DirectionNo, paramProc.Recipe.StepNo);

            bool isInsert = true;
            // 製造指図プロシージャの件数取得
            if (db.GetCountByOutsideSql(SqlName.GetCountDirectionProcedure, SqlName.SubDir, prikeyValue) > 0)
            {
                // 取得できた場合、更新
                isInsert = false;
                // 更新前プルーフ登録
                if (!RegistDirectionProof(SqlName.DirectionProcedureProof, APConsts.PROOF_STATUS.PRE_UPDATE, prikeyValue, paramProc.Common.InputUserId, db))
                {
                    return false;
                }
            }

            // 登録情報作成
            ComDao.DirectionProcedureEntity registCondition = getDirectionProcedureCondition(paramProc, now);
            // 登録
            int result = db.RegistByOutsideSql(isInsert ? SqlName.InsertDirectionProcedure : SqlName.UpdateDirectionProcedure, SqlName.SubDir, registCondition);
            if (result <= 0)
            {
                return false;
            }

            // プルーフ登録
            // 新規 or 更新後
            decimal proofStatus = isInsert ? APConsts.PROOF_STATUS.NEW : APConsts.PROOF_STATUS.POST_UPDATE;
            if (!RegistDirectionProof(SqlName.DirectionProcedureProof, proofStatus, prikeyValue, paramProc.Common.InputUserId, db))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 製造指図フォーミュラ登録処理
        /// </summary>
        /// <param name="division">実績区分</param>
        /// <param name="paramFormula">RegistDirectionFormulaの引数</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="paramNow">現在日</param>
        /// <returns>処理失敗時、false</returns>
        /// <remarks>FUN_PD_PLAN_DIRECTION_FMREG</remarks>
        public static bool RegistDirectionFormula(int division, ParamRegistDirectionFormula paramFormula, ComDB db, DateTime? paramNow = null)
        {
            // システム日付
            DateTime now = paramNow ?? DateTime.Now;

            // ロット番号が空の場合、取得する
            if (string.IsNullOrEmpty(paramFormula.Formula.LotNo))
            {
                paramFormula.Formula.LotNo = APFunc.FncGetLotDummy(db);
            }

            // 製造指図フォーミュラに登録する情報の主キー
            PkDirectionFormula prikeyValue = new PkDirectionFormula(paramFormula.Common.DirectionDivision, paramFormula.Common.DirectionNo, paramFormula.Formula.StepNo, (int)division, paramFormula.Recipe.SeqNo);

            bool isInsert = true;
            // 製造指図フォーミュラの件数取得
            if (db.GetCountByOutsideSql(SqlName.GetCountDirectionFormula, SqlName.SubDir, prikeyValue) > 0)
            {
                // 取得できた場合、更新
                isInsert = false;
                // 更新前プルーフ登録
                if (!RegistDirectionProof(SqlName.DirectionFormulaProof, APConsts.PROOF_STATUS.PRE_UPDATE, prikeyValue, paramFormula.Common.InputUserId, db))
                {
                    return false;
                }
            }

            // 登録情報作成
            ComDao.DirectionFormulaEntity registCondition = getDirectionFormulaCondition(paramFormula, division, now, db);
            // 登録
            int result = db.RegistByOutsideSql(isInsert ? SqlName.InsertDirectionFormula : SqlName.UpdateDirectionFormula, SqlName.SubDir, registCondition);
            if (result <= 0)
            {
                return false;
            }

            // プルーフ登録
            // 新規 or 更新後
            decimal proofStatus = isInsert ? APConsts.PROOF_STATUS.NEW : APConsts.PROOF_STATUS.POST_UPDATE;
            if (!RegistDirectionProof(SqlName.DirectionFormulaProof, proofStatus, prikeyValue, paramFormula.Common.InputUserId, db))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region ヘッダ
        /// <summary>
        /// 製造指図ヘッダ登録処理呼び出し
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <param name="recipeHeader">GetRecipeHeaderの結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="now">現在日</param>
        /// <returns>戻り値クラスのデータクラス</returns>
        private static ResultInfo callRegistDrectionHeader(ParamRegistDirection param, ResultGetRecipeHeader recipeHeader, ComDB db, DateTime now)
        {
            // 登録処理の引数作成
            ParamRegistDirectionHeader paramHeader = new ParamRegistDirectionHeader(param, recipeHeader);
            // 製造指図ヘッダ登録
            if (!RegistDirectionHeader(paramHeader, db, now))
            {
                // 製造指図ヘッダー登録処理失敗 品目=仕様=指図数=
                string errMsg = ComSTDUtil.GetPropertiesMessage(
                    new string[] { APResources.ID.M10036, APResources.ID.M10037, param.Header.ItemCd, param.Header.SpecificationCd, param.Header.PlanedQty.ToString() },
                    param.Common.LanguageId, null, db);
                return new ResultInfo(true, errMsg);
            }
            return new ResultInfo();
        }

        /// <summary>
        /// 製造指図ヘッダの登録情報を作成
        /// </summary>
        /// <param name="paramHeader">製造指図ヘッダ登録処理の引数</param>
        /// <param name="now">システム日付</param>
        /// <param name="db">DB接続</param>
        /// <returns>登録・更新処理に使用する情報</returns>
        private static ComDao.DirectionHeaderEntity getDirectionHeaderCondition(ParamRegistDirectionHeader paramHeader, DateTime now, ComDB db)
        {
            // 備考
            // ヘッダ情報の備考、処方マスタヘッダのレシピ備考の順で設定
            paramHeader.Header.Remark = paramHeader.Header.Remark ?? paramHeader.Recipe.RecipeMemo;
            // 注釈
            // ヘッダ情報の注釈、処方マスタヘッダのレシピ注釈の順で設定
            paramHeader.Header.Notes = paramHeader.Header.Notes ?? paramHeader.Recipe.RecipeDescription;

            // 品目名称と仕様名称を取得
            // OrderNoとOrderRowNoが設定されているときにORDER_DETAILより取得する現行仕様は必要？
            ResultGetItemInfo itemInfo = APComUtil.GetItemInfoByActiveDate<ResultGetItemInfo>(paramHeader.Header.ItemCd, paramHeader.Common.ProductionStartDate, paramHeader.Common.LanguageId, db);
            ResultGetSpecificationInfo speInfo = APComUtil.GetItemSpecificationInfoByActiveDate<ResultGetSpecificationInfo>(
                paramHeader.Header.ItemCd, paramHeader.Header.SpecificationCd, paramHeader.Common.ProductionStartDate, paramHeader.Common.LanguageId, db);

            var header = new ComDao.DirectionHeaderEntity();
            header.DirectionDivision = paramHeader.Common.DirectionDivision;
            header.DirectionNo = paramHeader.Common.DirectionNo;
            header.DirectionDate = now;
            header.DirectionStatus = paramHeader.Header.DirectionStatus;
            header.ItemCd = paramHeader.Header.ItemCd;
            header.ItemName = itemInfo.ItemName;
            header.ItemActiveDate = itemInfo.ActiveDate;
            header.SpecificationCd = paramHeader.Header.SpecificationCd;
            header.SpecificationName = speInfo.SpecificationName;
            header.OrderNo = paramHeader.Header.OrderNo;
            header.OrderRowNo = paramHeader.Header.OrderRowNo;
            header.ReferenceNo = paramHeader.Header.ReferenceNo;
            header.PlanedQty = paramHeader.Header.PlanedQty;
            header.PlanedStartDate = paramHeader.Common.ProductionStartDate;
            header.PlanedEndDate = paramHeader.Common.ProductionEndDate;
            header.StampFlg = 1;
            header.InspectionExistence = speInfo.InspectionFlg;
            header.Remark = paramHeader.Header.Remark;
            header.Notes = paramHeader.Header.Notes;
            header.ApprovalStatus = paramHeader.Header.ApprovalStatus;
            header.RecipeId = paramHeader.Header.RecipeId;
            header.RecipeCd = paramHeader.Header.RecipeCd;
            header.RecipeVersion = paramHeader.Header.RecipeVersion;
            header.ProductionLine = paramHeader.Header.ProductionLine;
            header.VenderCd = paramHeader.Header.VenderCd;
            header.InputDate = now;
            header.InputUserId = paramHeader.Common.InputUserId;
            header.UpdateDate = now;
            header.UpdateUserId = paramHeader.Common.InputUserId;
            return header;
        }

        #endregion

        #region プロシージャ

        /// <summary>
        /// 製造指図プロシージャの登録情報を作成
        /// </summary>
        /// <param name="param">製造指図プロシージャ登録処理の引数</param>
        /// <param name="now">システム日付</param>
        /// <returns>登録・更新に使用する情報</returns>
        private static ComDao.DirectionProcedureEntity getDirectionProcedureCondition(ParamRegistDirectionProcedure param, DateTime now)
        {
            var procedure = new ComDao.DirectionProcedureEntity();
            procedure.DirectionDivision = param.Common.DirectionDivision;
            procedure.DirectionNo = param.Common.DirectionNo;
            procedure.StepNo = param.Recipe.StepNo;
            procedure.Seq = param.Recipe.Seq;
            procedure.OperationCd = param.Recipe.OperationCd;
            procedure.Condition = param.Recipe.Condition;
            procedure.Remark = param.Recipe.Remark;
            procedure.Notes = param.Recipe.Notes;
            procedure.StartDate = param.Common.ProductionStartDate;
            procedure.EndDate = param.Common.ProductionEndDate;
            procedure.InputDate = now;
            procedure.InputUserId = param.Common.InputUserId;
            procedure.UpdateDate = now;
            procedure.UpdateUserId = param.Common.InputUserId;
            return procedure;
        }

        #endregion

        #region フォーミュラ
        /// <summary>
        /// 製造指図フォーミュラ(部品)登録処理
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <param name="stepNo">工程番号</param>
        /// <param name="stdQty">標準生産量</param>
        /// <param name="constValues">製造指図登録処理で使用するDBから取得した定数を管理するクラス</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="now">現在日</param>
        /// <returns>戻り値クラスのデータクラス</returns>
        private static ResultInfo registDirectionFormulaParts(ParamRegistDirection param, int stepNo, decimal? stdQty, RegistDirectionConst constValues, ComDB db, DateTime now)
        {
            // 製造指図フォーミュラテーブル登録
            // 処方フォーミュラ取得
            var searchKey = new SelectKeyRecipeFormula(param.Header.RecipeId, stepNo, APConsts.DIRECTION.FORMULA.RESULT_DIVISION.PARTS, new List<string> { constValues.ItemTypeMat });
            var recipeFormulaPartsList = db.GetListByOutsideSql<ResultGetRecipeFormula>(SqlName.GetRecipeFormula, SqlName.SubDir, searchKey);
            foreach (var recipeFormula in recipeFormulaPartsList)
            {

                // 基準保管場所取得
                ComDao.ItemSpecificationEntity specInfo = APComUtil.GetItemSpecificationInfoByActiveDate<ComDao.ItemSpecificationEntity>(
                         recipeFormula.ItemCd, recipeFormula.SpecificationCd, param.Common.ProductionStartDate, param.Common.LanguageId, db);
                string defaultLocation = null;
                if (specInfo != null)
                {
                    defaultLocation = specInfo.DefaultLocation;
                }

                // フォーミュラの数量の計算
                decimal formulaQty = 0;
                if ((stdQty ?? 0) != 0)
                {
                    // 標準生産量がNullでも0でもない場合(分母なので0除算回避)
                    formulaQty = param.Header.PlanedQty * ((recipeFormula.Qty ?? 0) / (decimal)stdQty);
                }

                // 製造指図フォーミュラ(部品)登録
                DirectionFormula directionFormula = new DirectionFormula(GetDirectionStepNo(stepNo), defaultLocation, constValues.DefaultLotNo, formulaQty);
                ParamRegistDirectionFormula paramFormula = new ParamRegistDirectionFormula(param, recipeFormula, directionFormula);
                if (!RegistDirectionFormula(APConsts.DIRECTION.FORMULA.RESULT_DIVISION.PARTS, paramFormula, db, now))
                {
                    // 「製造指図フォーミュラ（部品）登録処理失敗 品目=仕様=指図数=」
                    string errMsg = ComSTDUtil.GetPropertiesMessage(
                        new string[] { APResources.ID.M10036, APResources.ID.M10039, param.Header.ItemCd, param.Header.SpecificationCd, param.Header.PlanedQty.ToString() }, param.Common.LanguageId, null, db);
                }
            }
            return new ResultInfo();
        }
        /// <summary>
        /// 製造指図フォーミュラ(仕上)登録処理
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <param name="stdQty">標準生産量</param>
        /// <param name="constValues">製造指図登録処理で使用するDBから取得した定数を管理するクラス</param>
        /// <param name="db">DB操作クラス</param>
        /// <param name="now">現在日</param>
        /// <returns>戻り値クラスのデータクラス</returns>
        private static ResultInfo registDirectionFormulaFinish(ParamRegistDirection param, decimal? stdQty, RegistDirectionConst constValues, ComDB db, DateTime now)
        {
            // 処方フォーミュラを取得
            var searchKey = new SelectKeyRecipeFormula(param.Header.RecipeId, null, APConsts.DIRECTION.FORMULA.RESULT_DIVISION.FINISH,
                new List<string> { constValues.ItemTypeMain, constValues.ItemTypeSub, constValues.ItemTypeMiddle, constValues.ItemTypeWaste });
            var recipeFormulaFinishList = db.GetListByOutsideSql<ResultGetRecipeFormula>(SqlName.GetRecipeFormula, SqlName.SubDir, searchKey);
            foreach (var recipeFormula in recipeFormulaFinishList)
            {
                recipeFormula.StepNo = GetDirectionStepNo(recipeFormula.StepNo);
                // 品目タイプが製品かどうか
                bool checkLineType = ComSTDUtil.ConvertDecimalToString(recipeFormula.LineType) == constValues.ItemTypeMain;
                // 行番号のチェック
                bool checkLineNo = recipeFormula.SeqNo == FINISH_LINE_NO;
                // 処方の品目仕様が引数と一致するかどうか
                bool checkItem = recipeFormula.ItemCd == param.Header.ItemCd && recipeFormula.SpecificationCd == param.Header.SpecificationCd;
                string defaultLocation = param.LocationCd;
                if (((checkLineType || checkLineNo) && checkItem) == false)
                {
                    ComDao.ItemSpecificationEntity specInfo = APComUtil.GetItemSpecificationInfoByActiveDate<ComDao.ItemSpecificationEntity>(
                        recipeFormula.ItemCd, recipeFormula.SpecificationCd, param.Common.ProductionStartDate, param.Common.LanguageId, db);
                    defaultLocation = null;
                    if (specInfo != null)
                    {
                        defaultLocation = specInfo.DefaultLocation;
                    }
                }

                // フォーミュラの数量の計算
                decimal formulaQty = 0;
                if ((stdQty ?? 0) != 0)
                {
                    // 標準生産量がNullでも0でもない場合(分母なので0除算回避)
                    formulaQty = param.Header.PlanedQty * ((recipeFormula.Qty ?? 0) / (decimal)stdQty);
                }
                // ロット番号
                string formulaLotNo = constValues.DefaultLotNo;
                if (new List<string> { constValues.ItemTypeMain, constValues.ItemTypeSub, constValues.ItemTypeMiddle }.IndexOf(
                    ComSTDUtil.ConvertDecimalToString(recipeFormula.LineType)) >= 0)
                {
                    formulaLotNo = param.LotNo;
                }

                // 製造指図フォーミュラ登録
                ParamRegistDirectionFormula paramFormula = new ParamRegistDirectionFormula(param, recipeFormula,
                    new DirectionFormula(recipeFormula.StepNo, defaultLocation, formulaLotNo, formulaQty));
                if (!RegistDirectionFormula(APConsts.DIRECTION.FORMULA.RESULT_DIVISION.FINISH, paramFormula, db, now))
                {
                    // 「製造指図フォーミュラ（仕上）登録処理失敗 品目=仕様=指図数=」
                    string errMsg = ComSTDUtil.GetPropertiesMessage(
                        new string[] { APResources.ID.M10036, APResources.ID.M10040, param.Header.ItemCd, param.Header.SpecificationCd, param.Header.PlanedQty.ToString() }, param.Common.LanguageId, null, db);
                    return new ResultInfo(true, errMsg);
                }
            }

            return new ResultInfo();
        }

        /// <summary>
        /// 仕上げか判定する
        /// </summary>
        /// <param name="division">区分</param>
        /// <returns>true:正常　false:エラー</returns>
        private static bool isFinish(int division)
        {
            return division == APConsts.DIRECTION.FORMULA.RESULT_DIVISION.FINISH;
        }

        /// <summary>
        /// 製造指図フォーミュラの登録情報を作成
        /// </summary>
        /// <param name="paramForumula">製造指図フォーミュラ登録処理の引数</param>
        /// <param name="resultDivision">実績区分</param>
        /// <param name="now">システム日付</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>登録・更新に使用する情報</returns>
        private static ComDao.DirectionFormulaEntity getDirectionFormulaCondition(ParamRegistDirectionFormula paramForumula, int resultDivision, DateTime now, ComDB db)
        {
            // 品目名称と仕様名称を取得
            ResultGetItemInfo itemInfo = APComUtil.GetItemInfoByActiveDate<ResultGetItemInfo>(paramForumula.Recipe.ItemCd, paramForumula.Common.ProductionStartDate, paramForumula.Common.LanguageId, db);

            var formula = new ComDao.DirectionFormulaEntity();
            formula.DirectionDivision = paramForumula.Common.DirectionDivision;
            formula.DirectionNo = paramForumula.Common.DirectionNo;
            formula.StepNo = paramForumula.Formula.StepNo;
            formula.ResultDivision = resultDivision;
            formula.SeqNo = paramForumula.Recipe.SeqNo;
            formula.Seq = paramForumula.Recipe.SubStep;
            formula.LineType = isFinish(resultDivision) ? paramForumula.Recipe.LineType : PARTS_LINE_TYPE;
            formula.ItemCd = paramForumula.Recipe.ItemCd;
            formula.ItemActiveDate = itemInfo.ActiveDate;
            formula.SpecificationCd = paramForumula.Recipe.SpecificationCd;
            formula.LocationCd = paramForumula.Formula.LocationCd;
            // ロット番号(初期値は部品の場合)
            string lotNo = paramForumula.Formula.LotNo;
            if (isFinish(resultDivision) && !formula.DirectionDivision.Equals(APConsts.DIRECTION.COMMON.DIRECTION_DIVISION.OUT))
            {
                // 仕上の場合、ロット番号は、製造終了予定日(yyyyMMdd) + 連番
                lotNo = string.Format("{0:yyyyMMdd}", paramForumula.Common.ProductionEndDate) + APComUtil.GetNumber(db, SeqLotSeries);
            }
            formula.LotNo = lotNo;
            // サブロット番号は設定しないのでデフォルトロット番号を設定
            string defaultLotNo = APComUtil.GetDefaultLotNo(db);
            formula.SubLotNo1 = defaultLotNo;
            formula.SubLotNo2 = defaultLotNo;
            if (isFinish(resultDivision))
            {
                // 仕上の場合、シーケンスNOを設定
                formula.FinishNo = formula.SeqNo;
            }
            else
            {
                // 部品の場合、処方フォーミュラから設定
                formula.FinishNo = paramForumula.Recipe.FinishNo;
            }
            // 計上予定日時　部品なら製造開始予定日時、仕上なら製造終了予定日時
            formula.ScheduledAccountingDate = resultDivision == APConsts.DIRECTION.FORMULA.RESULT_DIVISION.PARTS ? paramForumula.Common.ProductionStartDate : paramForumula.Common.ProductionEndDate;
            formula.AccountingDate = null;
            formula.Qty = paramForumula.Formula.Qty;
            formula.ResultQty = null;
            formula.PartInputDivision = paramForumula.Recipe.PartInputDivision;
            formula.Notes = paramForumula.Recipe.Notes;
            formula.Remark = null;
            formula.InputDate = now;
            formula.InputUserId = paramForumula.Common.InputUserId;
            formula.UpdateDate = now;
            formula.UpdateUserId = paramForumula.Common.InputUserId;

            return formula;
        }
        #endregion

        #region 製造指図登録処理で使用
        /// <summary>
        /// 製造指図登録処理で使用するDBから取得した定数を管理するクラス
        /// </summary>
        private class RegistDirectionConst
        {
            /// <summary>Gets 品目タイプ・原材料</summary>
            /// <value>品目タイプ・原材料</value>
            public string ItemTypeMat { get; }
            /// <summary>Gets 品目タイプ・製品</summary>
            /// <value>品目タイプ・製品</value>
            public string ItemTypeMain { get; }
            /// <summary>Gets 品目タイプ・副生品</summary>
            /// <value>品目タイプ・副製品</value>
            public string ItemTypeSub { get; }
            /// <summary>Gets 品目タイプ・中間品</summary>
            /// <value>品目タイプ・中間品</value>
            public string ItemTypeMiddle { get; }
            /// <summary>Gets 品目タイプ・廃棄物、回収品</summary>
            /// <value>品目タイプ・廃棄物、回収品</value>
            public string ItemTypeWaste { get; }

            /// <summary>Gets 受払区分・製造投入</summary>
            /// <value>受払区分・製造投入</value>
            public string IoDivDirectOut { get; }
            /// <summary>Gets 受払区分・製造出来高</summary>
            /// <value>受払区分・製造出来高</value>
            public string IoDivDirectIn { get; }

            /// <summary>Gets デフォルトロット番号</summary>
            /// <value>デフォルトロット番号</value>
            public string DefaultLotNo { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="db">DB操作クラス</param>
            public RegistDirectionConst(ComDB db)
            {
                // 品目タイプ
                // 原材料
                this.ItemTypeMat = APFunc.FncGetNameCd("CLTP", "MATERIAL", db);
                // 製品
                this.ItemTypeMain = APFunc.FncGetNameCd("CLTP", "MAIN", db);
                // 副生品
                this.ItemTypeSub = APFunc.FncGetNameCd("CLTP", "SUB", db);
                // 中間品
                this.ItemTypeMiddle = APFunc.FncGetNameCd("CLTP", "MIDDLE", db);
                // 廃棄物、回収品
                this.ItemTypeWaste = APFunc.FncGetNameCd("CLTP", "WASTE", db);

                // 受払区分
                // 製造投入
                this.IoDivDirectOut = APFunc.FncGetNameCd("IODV", "DIROUT", db);
                // 製造出来高
                this.IoDivDirectIn = APFunc.FncGetNameCd("IODV", "DIRIN", db);

                // デフォルトロット番号
                this.DefaultLotNo = APComUtil.GetDefaultLotNo(db);
            }
        }

        /// <summary>
        /// 生産量の計算とチェック
        /// </summary>
        /// <param name="unitQty">単位生産量</param>
        /// <param name="planedQty">予定生産量</param>
        /// <param name="minQty">最小生産量</param>
        /// <param name="maxQty">最大生産量</param>
        /// <param name="isError">計算した生産量がエラーの場合、trueを返す</param>
        /// <returns>計算した生産量</returns>
        private static decimal calcPlanedQty(decimal? unitQty, decimal planedQty, decimal? minQty, decimal? maxQty, out bool isError)
        {
            // エラー有無初期値
            isError = false;
            // 計算した生産量初期値
            decimal returnQty = 0;

            // 計算
            if ((unitQty ?? 0) != 0)
            {
                // 単位生産量がNullでも0でもない場合(分母なので0除算回避)
                // 単位の計算 全生産数 ＝｛(仮生産数/単位生産量)小数点以下繰り上げ｝*単位生産量
                returnQty = Math.Ceiling(planedQty / (decimal)unitQty) * (decimal)unitQty;
            }

            // 範囲チェック
            if (!((minQty ?? decimal.MinValue) <= returnQty
                && returnQty <= (maxQty ?? decimal.MaxValue)))
            {
                // 最小と最大の範囲に無い場合、エラー
                // 最小と最大がNullの場合はDecimal型の最小最大値で、必ず範囲内になるようにする
                isError = true;
            }

            return returnQty;
        }

        #endregion

        #region 全機能で使用
        /// <summary>
        /// 製造指図関連プルーフテーブル登録
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="sqlName">プルーフ登録SQLファイル名</param>
        /// <param name="proofStatus">登録するプルーフテーブルのステータス</param>
        /// <param name="prikeyValue">登録するプルーフテーブルの主キーの値</param>
        /// <param name="userId">登録ユーザID</param>
        /// <param name="db">DB接続</param>
        /// <returns>エラーの場合false</returns>
        /// <remarks>主キーがヘッダ、プロシージャ、フォーミュラで親子関係にあることを利用して共通で行う</remarks>
        public static bool RegistDirectionProof<T>(string sqlName, decimal proofStatus, T prikeyValue, string userId, ComDB db)
        {
            // プルーフ登録
            return APComUtil.CreateProof<T>(db, prikeyValue, sqlName, userId, proofStatus);
        }

        /// <summary>
        /// 製造指図に登録する工程番号は、処方より取得した番号を加工して取得する
        /// </summary>
        /// <param name="recipeStepNo">処方より取得した工程番号</param>
        /// <returns>製造指図に登録する工程番号</returns>
        public static int GetDirectionStepNo(int recipeStepNo)
        {
            // 100倍して1を加算
            return (recipeStepNo * 100) + 1;
        }
        #endregion

        #region 各処理で使用する引数の子要素

        /// <summary>
        /// 各製造指図の処理で共通して使用する項目の値
        /// </summary>
        public class CommonDirection
        {
            /// <summary>Gets or sets 指図区分</summary>
            /// <value>指図区分</value>
            public int DirectionDivision { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string DirectionNo { get; set; }
            /// <summary>Gets or sets 登録ユーザID</summary>
            /// <value>登録ユーザID</value>
            public string InputUserId { get; set; }
            /// <summary>Gets or sets 言語ID</summary>
            /// <value>言語ID</value>
            /// <remarks>クラスの外がいい？</remarks>
            public string LanguageId { get; set; }
            /// <summary>Gets or sets 開始予定日時</summary>
            /// <value>開始予定日時</value>
            public DateTime ProductionStartDate { get; set; }
            /// <summary>Gets or sets 終了予定日時</summary>
            /// <value>終了予定日時</value>
            public DateTime ProductionEndDate { get; set; }
        }

        /// <summary>
        /// ヘッダーで使用する引数の値
        /// </summary>
        public class HeaderDirection
        {
            /// <summary>Gets or sets 受注番号</summary>
            /// <value>受注番号</value>
            public string OrderNo { get; set; }
            /// <summary>Gets or sets 受注行番号</summary>
            /// <value>受注行番号</value>
            public int? OrderRowNo { get; set; }
            /// <summary>Gets or sets 生産ライン</summary>
            /// <value>生産ライン</value>
            public string ProductionLine { get; set; }
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets レシピコード</summary>
            /// <value>レシピコード</value>
            public string RecipeCd { get; set; }
            /// <summary>Gets or sets レシピバージョン</summary>
            /// <value>レシピバージョン</value>
            public long? RecipeVersion { get; set; }
            /// <summary>Gets or sets 予定生産量</summary>
            /// <value>予定生産量</value>
            public decimal PlanedQty { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 指図ステータス</summary>
            /// <value>指図ステータス</value>
            public int DirectionStatus { get; set; }
            /// <summary>Gets or sets 承認ステータス</summary>
            /// <value>承認ステータス</value>
            public int ApprovalStatus { get; set; }
            /// <summary>Gets or sets 仕入先コード</summary>
            /// <value>仕入先コード</value>
            public string VenderCd { get; set; }
            /// <summary>Gets or sets リファレンス番号</summary>
            /// <value>リファレンス番号</value>
            public string ReferenceNo { get; set; }

            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
        }

        /// <summary>
        /// フォーミュラで使用する項目
        /// </summary>
        public class DirectionFormula
        {
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal Qty { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="stepNo">工程番号</param>
            /// <param name="locationCd">ロケーションコード</param>
            /// <param name="lotNo">ロット番号</param>
            /// <param name="qty">数量</param>
            public DirectionFormula(int stepNo, string locationCd, string lotNo, decimal qty)
            {
                this.StepNo = stepNo;
                this.LocationCd = locationCd;
                this.LotNo = LotNo;
                this.Qty = qty;
            }
        }
        #endregion

        #region 各Public処理の引数クラス
        /// <summary>
        /// 製造指図登録処理に使用する引数
        /// </summary>
        /// <remarks>RegistDirectionの引数</remarks>
        public class ParamRegistDirection
        {
            /// <summary>Gets or sets ヘッダ、プロシージャ、フォーミュラで使用</summary>
            /// <value>ヘッダ、プロシージャ、フォーミュラで使用</value>
            public CommonDirection Common { get; set; }
            /// <summary>Gets or sets ヘッダで使用</summary>
            /// <value>ヘッダで使用</value>
            public HeaderDirection Header { get; set; }

            /// <summary>Gets or sets ロケーションコード</summary>
            /// <value>ロケーションコード</value>
            public string LocationCd { get; set; }
            /// <summary>Gets or sets ロット番号</summary>
            /// <value>ロット番号</value>
            public string LotNo { get; set; }

            /// <summary>
            /// クラスのディープコピーを作成する
            /// </summary>
            /// <returns>製造指図登録処理に使用する引数</returns>
            public ParamRegistDirection DeepCopy()
            {
                // シリアライズして後にデシリアライズすることで参照型も含めてディープコピーできる
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, this);
                    ms.Position = 0;
                    return (ParamRegistDirection)bf.Deserialize(ms);
                }
            }
        }

        /// <summary>
        /// RegistDirectionHeaderの引数
        /// </summary>
        public class ParamRegistDirectionHeader
        {
            /// <summary>Gets or sets ヘッダ、プロシージャ、フォーミュラで使用</summary>
            /// <value>ヘッダ、プロシージャ、フォーミュラで使用</value>
            public CommonDirection Common { get; set; }
            /// <summary>Gets or sets ヘッダで使用</summary>
            /// <value>ヘッダで使用</value>
            public HeaderDirection Header { get; set; }
            /// <summary>Gets or sets 処方ヘッダより取得した項目</summary>
            /// <value>処方ヘッダより取得した項目</value>
            public RecipeHeaderCommon Recipe { get; set; }

            /// <summary>
            /// コンストラクタ　共通の引数と処方ヘッダより作成
            /// </summary>
            /// <param name="param">共通の引数</param>
            /// <param name="recipe">処方ヘッダ</param>
            public ParamRegistDirectionHeader(ParamRegistDirection param, ResultGetRecipeHeader recipe)
            {
                this.Common = param.Common;
                this.Header = param.Header;

                this.Recipe = recipe.Common;
            }
        }

        /// <summary>
        /// RegistDirectionProcedureの引数
        /// </summary>
        public class ParamRegistDirectionProcedure
        {
            /// <summary>Gets or sets ヘッダ、プロシージャ、フォーミュラで使用</summary>
            /// <value>ヘッダ、プロシージャ、フォーミュラで使用</value>
            public CommonDirection Common { get; set; }
            /// <summary>Gets or sets 処方プロシージャより取得した項目</summary>
            /// <value>処方プロシージャより取得した項目</value>
            public ResultGetRecipeProcedure Recipe { get; set; }

            /// <summary>
            /// コンストラクタ　共通の引数と処方プロシージャより作成
            /// </summary>
            /// <param name="param">パラメータ</param>
            /// <param name="recipe">レシピ</param>
            public ParamRegistDirectionProcedure(ParamRegistDirection param, ResultGetRecipeProcedure recipe)
            {
                this.Common = param.Common;
                this.Recipe = recipe;
            }
        }

        /// <summary>
        /// RegistDirectionFormulaの引数
        /// </summary>
        public class ParamRegistDirectionFormula
        {
            /// <summary>Gets or sets ヘッダ、プロシージャ、フォーミュラで使用</summary>
            /// <value>ヘッダ、プロシージャ、フォーミュラで使用</value>
            public CommonDirection Common { get; set; }
            /// <summary>Gets or sets フォーミュラで使用</summary>
            /// <value>フォーミュラで使用</value>
            public DirectionFormula Formula { get; set; }
            /// <summary>Gets or sets 処方フォーミュラより取得した項目</summary>
            /// <value>処方フォーミュラより取得した項目</value>
            public ResultGetRecipeFormula Recipe { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="param">パラメータ</param>
            /// <param name="recipe">レシピ</param>
            /// <param name="formula">フォーミュラのデータクラス</param>
            public ParamRegistDirectionFormula(ParamRegistDirection param, ResultGetRecipeFormula recipe, DirectionFormula formula)
            {
                this.Common = param.Common;
                this.Recipe = recipe;
                this.Formula = formula;
            }
        }
        #endregion

        #region SQL実行結果

        /// <summary>
        /// GetRecipeHeaderの結果
        /// </summary>
        public class ResultGetRecipeHeader
        {
            /// <summary>Gets or sets 最小生産量</summary>
            /// <value>最小生産量</value>
            public decimal? MinQty { get; set; }
            /// <summary>Gets or sets 最大生産量</summary>
            /// <value>最大生産量</value>
            public decimal? MaxQty { get; set; }
            /// <summary>Gets or sets 標準生産量</summary>
            /// <value>標準生産量</value>
            public decimal? StdQty { get; set; }
            /// <summary>Gets or sets 単位生産量</summary>
            /// <value>単位生産量</value>
            public decimal? UnitQty { get; set; }
            /// <summary>Gets or sets ヘッダで使用</summary>
            /// <value>ヘッダで使用</value>
            public RecipeHeaderCommon Common { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="recipeHeader">レシピヘッダ情報</param>
            public ResultGetRecipeHeader(ComDao.RecipeHeaderEntity recipeHeader)
            {
                this.Common = new RecipeHeaderCommon();
                this.Common.RecipeDescription = recipeHeader.RecipeDescription;
                this.Common.RecipeMemo = recipeHeader.RecipeMemo;
                this.Common.Yield = recipeHeader.Yield;
                this.Common.ApprovalId = recipeHeader.ApprovalId;
                this.Common.ApprovalDate = recipeHeader.ApprovalDate;
                this.MinQty = recipeHeader.MinQty;
                this.MaxQty = recipeHeader.MaxQty;
                this.StdQty = recipeHeader.StdQty;
                this.UnitQty = recipeHeader.UnitQty;
            }
        }

        /// <summary>
        /// 処方ヘッダより取得した項目のうち、ヘッダ登録処理に必要な項目
        /// </summary>
        public class RecipeHeaderCommon
        {
            /// <summary>Gets or sets レシピ注釈</summary>
            /// <value>レシピ注釈</value>
            public string RecipeDescription { get; set; }
            /// <summary>Gets or sets レシピ備考</summary>
            /// <value>レシピ備考</value>
            public string RecipeMemo { get; set; }
            /// <summary>Gets or sets 出来高|配合量(合計)に対する標準生産量または収率(%)またはロス率(%)</summary>
            /// <value>出来高|配合量(合計)に対する標準生産量または収率(%)またはロス率(%)</value>
            public decimal? Yield { get; set; }
            /// <summary>Gets or sets 承認者ID(原処方)</summary>
            /// <value>承認者ID(原処方)</value>
            public string ApprovalId { get; set; }
            /// <summary>Gets or sets 承認日時(BOM)</summary>
            /// <value>承認日時(BOM)</value>
            public DateTime? ApprovalDate { get; set; }
        }

        /// <summary>
        /// GetRecipeProcedureの結果
        /// </summary>
        public class ResultGetRecipeProcedure
        {
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets 表示順</summary>
            /// <value>表示順</value>
            public int? Seq { get; set; }
            /// <summary>Gets or sets 工程コード</summary>
            /// <value>工程コード</value>
            public string OperationCd { get; set; }
            /// <summary>Gets or sets 工程条件自由入力</summary>
            /// <value>工程条件自由入力</value>
            public string Condition { get; set; }
            /// <summary>Gets or sets 備考</summary>
            /// <value>備考</value>
            public string Remark { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
        }

        /// <summary>
        /// GetRecipeFormulaの結果
        /// </summary>
        public class ResultGetRecipeFormula
        {
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }
            /// <summary>Gets or sets サブステップNO</summary>
            /// <value>サブステップNO</value>
            public int? SubStep { get; set; }
            /// <summary>Gets or sets 品目タイプ|各種名称マスタ</summary>
            /// <value>品目タイプ|各種名称マスタ</value>
            public int? LineType { get; set; }
            /// <summary>Gets or sets 品目コード</summary>
            /// <value>品目コード</value>
            public string ItemCd { get; set; }
            /// <summary>Gets or sets 仕様コード</summary>
            /// <value>仕様コード</value>
            public string SpecificationCd { get; set; }
            /// <summary>Gets or sets 数量</summary>
            /// <value>数量</value>
            public decimal? Qty { get; set; }
            /// <summary>Gets or sets 条件</summary>
            /// <value>条件</value>
            public string Condition { get; set; }
            /// <summary>Gets or sets 注釈</summary>
            /// <value>注釈</value>
            public string Notes { get; set; }
            /// <summary>Gets or sets 仕上NO</summary>
            /// <value>仕上NO</value>
            public int? FinishNo { get; set; }
            /// <summary>Gets or sets 部品入力区分</summary>
            /// <value>部品入力区分</value>
            public int? PartInputDivision { get; set; }
        }

        /// <summary>
        /// GetItemInfoの結果
        /// </summary>
        public class ResultGetItemInfo
        {
            /// <summary>Gets or sets 品目名称</summary>
            /// <value>品目名称</value>
            public string ItemName { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ActiveDate { get; set; }
        }

        /// <summary>
        /// GetSpecificationInfoの結果
        /// </summary>
        public class ResultGetSpecificationInfo
        {
            /// <summary>Gets or sets 仕様名称</summary>
            /// <value>仕様名称</value>
            public string SpecificationName { get; set; }
            /// <summary>Gets or sets 有効開始日</summary>
            /// <value>有効開始日</value>
            public DateTime ActiveDate { get; set; }
            /// <summary>Gets or sets 検査フラグ</summary>
            /// <value>検査フラグ</value>
            public int InspectionFlg { get; set; }
        }

        #endregion

        #region SQL実行パラメータ
        /// <summary>
        /// 製造指図ヘッダテーブルの主キーの値を管理するクラス
        /// </summary>
        public class PkDirectionHeader
        {
            /// <summary>Gets or sets 指図区分</summary>
            /// <value>指図区分</value>
            public int DirectionDivision { get; set; }
            /// <summary>Gets or sets 指図番号</summary>
            /// <value>指図番号</value>
            public string DirectionNo { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="division">指図区分</param>
            /// <param name="no">指図番号</param>
            public PkDirectionHeader(int division, string no)
            {
                this.DirectionDivision = division;
                this.DirectionNo = no;
            }
        }

        /// <summary>
        /// 製造指図プロシージャテーブルの主キーの値を管理するクラス
        /// </summary>
        /// <remarks>ヘッダテーブルの子なので継承することでポリモーフィズムを利用して処理をまとめている</remarks>
        public class PkDirectionProcedure : PkDirectionHeader
        {
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int StepNo { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="division">指図区分</param>
            /// <param name="no">指図番号</param>
            /// <param name="stepNo">工程番号</param>
            public PkDirectionProcedure(int division, string no, int stepNo) : base(division, no)
            {
                this.StepNo = stepNo;
            }
        }

        /// <summary>
        /// 製造指図フォーミュラテーブルの主キーの値を管理するクラス
        /// </summary>
        /// <remarks>プロシージャの子</remarks>
        public class PkDirectionFormula : PkDirectionProcedure
        {
            /// <summary>Gets or sets 実績区分</summary>
            /// <value>実績区分</value>
            public int ResultDivision { get; set; }
            /// <summary>Gets or sets シーケンスNO</summary>
            /// <value>シーケンスNO</value>
            public int SeqNo { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="division">指図区分</param>
            /// <param name="no">指図番号</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="resultDivision">実績区分</param>
            /// <param name="seqno">シーケンスNO</param>
            public PkDirectionFormula(int division, string no, int stepNo, int resultDivision, int seqno) : base(division, no, stepNo)
            {
                this.ResultDivision = resultDivision;
                this.SeqNo = seqno;
            }
        }

        /// <summary>
        /// 処方フォーミュラテーブルの検索キーの値を管理するクラス
        /// </summary>
        public class SelectKeyRecipeFormula
        {
            /// <summary>Gets or sets レシピインデックス</summary>
            /// <value>レシピインデックス</value>
            public long RecipeId { get; set; }
            /// <summary>Gets or sets 工程番号</summary>
            /// <value>工程番号</value>
            public int? StepNo { get; set; }
            /// <summary>Gets or sets 実績区分</summary>
            /// <value>実績区分</value>
            public int ResultDivision { get; set; }
            /// <summary>Gets or sets 品目タイプリスト</summary>
            /// <value>品目タイプリスト</value>
            public List<string> LineTypeList { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="recipeId">レシピインデックス</param>
            /// <param name="stepNo">工程番号</param>
            /// <param name="resultDivison">実績区分</param>
            /// <param name="lineTypeList">品目タイプリスト</param>
            public SelectKeyRecipeFormula(long recipeId, int? stepNo, int resultDivison, List<string> lineTypeList)
            {
                this.RecipeId = recipeId;
                this.StepNo = stepNo;
                this.ResultDivision = resultDivison;
                this.LineTypeList = lineTypeList;
            }
        }
        #endregion

    }
}
