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
using Dao = BusinessLogic_MS0020.BusinessLogicDataClass_MS0020;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

/// <summary>
/// 機種別仕様マスタメンテ
/// </summary>
namespace BusinessLogic_MS0020
{
    /// <summary>
    /// 機種別仕様マスタメンテ
    /// </summary>
    public partial class BusinessLogic_MS0020 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class Sql
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Master\MachineSpec";
            /// <summary>機種別仕様一覧画面</summary>
            public static class SpecList
            {
                /// <summary>機種別仕様一覧の検索SQL</summary>
                public const string GetList = "GetMachineSpecList";
                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecList";
            }
            /// <summary>機種別仕様登録画面</summary>
            public static class SpecRegist
            {
                /// <summary>仕様の言語の一覧と登録された翻訳を取得するSQL</summary>
                public const string GetSpecTranslationList = "GetSpecTranslationList";
                /// <summary>仕様の情報を取得するSQL</summary>
                public const string GetSpecInfos = "GetSpecInfo";

                // 翻訳の更新はマスタ共通を使用

                /// <summary>INSERT：仕様項目マスタ</summary>
                public const string InsertMsSpec = "InsertMsSpec";
                /// <summary>UPDATE：仕様項目マスタ</summary>
                public const string UpdateMsSpec = "UpdateMsSpec";

                /// <summary>INSERT：機種別仕様関連付けマスタ</summary>
                public const string InsertMsMachineSpecRelation = "InsertMsMachineSpecRelation";
                /// <summary>DELETE：機種別仕様関連付けマスタ</summary>
                public const string DeleteMsMachineSpecRelation = "DeleteMsMachineSpecRelation";

                /// <summary>入力チェック：仕様項目マスタで重複した登録が無いかを判定するSQL</summary>
                public const string GetCountDuplicateTranslation = "GetCountDuplicateTranslation";

                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecRegist";
            }
            /// <summary>仕様項目選択値一覧画面</summary>
            public static class SpecItemList
            {
                /// <summary>画面情報取得SQL</summary>
                public const string GetData = "GetSpecSelectItemList";

                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecItemList";
            }
            /// <summary>選択肢登録画面</summary>
            public static class SpecItemRegist
            {
                /// <summary>画面情報取得SQL</summary>
                public const string GetData = "GetSpecItemTranslationList";

                /// <summary>SQL格納先サブディレクトリ名</summary>
                public const string SubDir = Sql.SubDir + @"\SpecItemRegist";
            }
        }

        /// <summary>
        /// 画面定義の定数クラス
        /// </summary>
        private static class FormInfo
        {
            /// <summary>機種別仕様一覧画面</summary>
            public static class SpecList
            {
                /// <summary>フォームNo</summary>
                public const short No = 1;

                /// <summary>検索条件</summary>
                public const string Condition = "BODY_000_00_LST_0";
                /// <summary>検索結果一覧</summary>
                public const string List = "BODY_020_00_LST_0";
                /// <summary>非表示項目</summary>
                public const string Hide = "BODY_050_00_LST_0";
            }
            /// <summary>機種別仕様登録画面</summary>
            public static class SpecRegist
            {
                /// <summary>フォームNo</summary>
                public const short No = 2;

                /// <summary>ヘッダ</summary>
                public const string Header = "BODY_000_00_LST_1";
                /// <summary>翻訳リスト</summary>
                public const string TranslateList = "BODY_010_00_LST_1";
                /// <summary>仕様内容</summary>
                public const string SpecInfo = "BODY_020_00_LST_1";
                /// <summary>機種別仕様関連付けマスタ(排他チェック用)</summary>
                public const string RelationInfos = "BODY_040_00_LST_1";
            }
            /// <summary>仕様項目選択値一覧画面</summary>
            public static class SpecItemList
            {
                /// <summary>フォームNo</summary>
                public const short No = 3;

                /// <summary>ヘッダ</summary>
                public const string Header = "BODY_000_00_LST_2";
                /// <summary>一覧</summary>
                public const string List = "BODY_010_00_LST_2";
            }
            /// <summary>選択肢登録画面</summary>
            public static class SpecItemRegist
            {
                /// <summary>フォームNo</summary>
                public const short No = 4;

                /// <summary>登録情報</summary>
                public const string Info = "BODY_010_00_LST_3";
                /// <summary>一覧</summary>
                public const string List = "BODY_000_00_LST_3";
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MS0020() : base()
        {
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int InitImpl()
        {
            this.ResultList = new();
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.FormNo)
            {
                case FormInfo.SpecList.No:   // 機種別仕様一覧画面
                    if (compareId.IsBack() || compareId.IsRegist())
                    {
                        // 戻る場合、再検索を行う
                        return InitSearch();
                    }
                    else
                    {
                        if (!initSpecList())
                        {
                            return ComConsts.RETURN_RESULT.NG;
                        }
                        break;
                    }
                    break;
                case FormInfo.SpecRegist.No:   // 機種別仕様登録画面
                    if (!initSpecRegist())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case FormInfo.SpecItemList.No: // 仕様項目選択肢一覧画面
                    if (!initSpecItemList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case FormInfo.SpecItemRegist.No: // 仕様項目選択肢一覧画面
                    if (!initSpecItemRegist())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            switch (this.FormNo)
            {
                case FormInfo.SpecList.No:     // 一覧検索
                    if (!searchSpecList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら1以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsRegist())
            {
                // 登録の場合
                // 登録処理実行
                return Regist();
            }
            else if (compareId.IsDelete())
            {
                // 削除の場合
                // 削除処理実行
                return Delete();
            }
            // この部分は到達不能なので、エラーを返す
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            switch (this.FormNo)
            {
                case FormInfo.SpecRegist.No:
                    // 機種別仕様登録画面の場合の登録処理
                    resultRegist = registSpecRegist();
                    break;
                case FormInfo.SpecItemRegist.No:
                    // 選択肢登録画面の場合の登録処理
                    resultRegist = registSpecItemRegist();
                    break;
                default:
                    // 到達不能
                    return ComConsts.RETURN_RESULT.NG;
            }
            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「登録処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            bool resultDelete = false;  // 登録処理戻り値、エラーならFalse

            switch (this.FormNo)
            {
                case FormInfo.SpecItemRegist.No:
                    // 機種別仕様登録
                    resultDelete = true;
                    // 削除処理
                    break;
                case FormInfo.SpecItemList.No:
                    // 仕様項目選択肢一覧
                    resultDelete = true;
                    // 削除処理
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// 翻訳マスタ登録処理
        /// </summary>
        /// <param name="ctrlId">翻訳一覧のコントロールID</param>
        /// <param name="isNew">新規登録の場合TRUE</param>
        /// <param name="factoryId">工場ID</param>
        /// <param name="now">システム日時</param>
        /// <param name="translationId">out INSERT/UPDATEした翻訳ID</param>
        /// <returns>エラーの場合FALSE</returns>
        private bool registTranslation(string ctrlId, bool isNew, int factoryId, DateTime now, out int translationId)
        {
            translationId = -1; // 採番した翻訳マスタのキー値

            // 翻訳の変更内容より、INSERTorUPDATEフラグと、UPDATEの場合は更新するキー値を取得
            bool isInsert = isInsertTranslation(out int updateTransId);
            // 翻訳を登録し、新規の場合は追加したキーを取得
            executeRegistTranslation(isInsert, updateTransId, out int insertedTransId);

            translationId = isInsert ? insertedTransId : updateTransId;
            return true;

            /// <summary>
            /// 翻訳マスタにINSERTするかUPDATEするか判定する
            /// </summary>
            /// <param name="updTranslationId">UPDATEするばあい、対象の翻訳ID</param>
            /// <returns>INSERTの場合TRUE</returns>
            bool isInsertTranslation(out int updTranslationId)
            {
                updTranslationId = -1; // 使用する翻訳ID
                bool isInsert = false; // INSERTの場合TRUE

                // 翻訳一覧のデータクラスを取得
                var transList = convertDicListToClassList<Dao.Common.Translation>(this.resultInfoDictionary, ctrlId);
                // 自分の言語IDを取得
                string userLanguageId = getUserLanguageId();
                // 自分の言語を取得
                var userTrans = transList.Where(x => x.LanguageId == userLanguageId).FirstOrDefault();

                if (isNew)
                {
                    // 新規登録の場合
                    // 同じ工場で日本語で同じ翻訳があるか確認
                    isInsert = isNewTranslation(out updTranslationId);
                    return isInsert;
                }
                else
                {
                    // 修正登録の場合

                    // ユーザ言語の翻訳の更新有無を確認
                    bool isUpdate = userTrans.TranslationText != userTrans.TranslationTextBk;
                    if (!isUpdate)
                    {
                        // 翻訳が変更されていない場合
                        // 翻訳ID取得
                        updTranslationId = userTrans.TranslationId ?? -1;
                        // 修正登録
                        return false;
                    }
                    // 翻訳が変更されている場合
                    // 入力された翻訳があるかどうか確認
                    isInsert = isNewTranslation(out updTranslationId);
                    if (isInsert)
                    {
                        // ない場合でも、現在の翻訳IDをUPDATEする
                        updTranslationId = userTrans.TranslationId ?? -1;
                    }
                    // 修正登録の場合は必ずUPDATE
                    return false;
                }

                bool isNewTranslation(out int updTranslationId)
                {
                    updTranslationId = -1;
                    // 同じ翻訳文字列の翻訳IDを取得
                    var registTranIds = TMQUtil.SqlExecuteClass.SelectList<int?>(Master.SqlName.GetMsTranslationInfo, Master.SqlName.SubDir, userTrans, this.db);
                    if (registTranIds == null || registTranIds.Count == 0)
                    {
                        // 同じ翻訳が存在しない場合、新規登録
                        return true;
                    }

                    updTranslationId = registTranIds.First().Value;
                    return false;

                }
            }

            /// <summary>
            /// 翻訳マスタの登録SQL実行
            /// </summary>
            /// <param name="isInsert">INSERTの場合TRUE</param>
            /// <param name="updateTransId">UPDATEの場合更新対象の翻訳ID</param>
            /// <param name="insertedTransId">out INSERTの場合採番した翻訳ID</param>
            /// <returns></returns>
            bool executeRegistTranslation(bool isInsert, int updateTransId, out int insertedTransId)
            {
                insertedTransId = -1; // 採番した翻訳マスタのキー値

                // INSERTの場合、先頭行はINSERTしたキー値を取得し、以降は取得したキーでINSERTを行う
                bool isFirstInsert = true;
                // UPDATEの場合のSQLとコメントアウト文字列
                string registSql = Master.SqlName.UpdateMsTranslationInfo; // UPDATE
                List<string> listUnComment = new List<string> { "TranslationItemDescription" };
                if (isInsert)
                {
                    // INSERTの場合のSQL
                    registSql = Master.SqlName.InsertMsTranslationInfoGetTranslationId; //INSERT
                    listUnComment = new(); // コメントアウトは無し
                }

                // 一覧の内容を取得
                var dicTranslations = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
                foreach (var rowDic in dicTranslations)
                {
                    // 繰り返し実行し、1行ずつ登録
                    ComDao.MsTranslationEntity regist = new();
                    SetExecuteConditionByDataClass(rowDic, ctrlId, regist, now, this.UserId, this.UserId);
                    regist.LocationStructureId = factoryId; // 退避した前画面から引き継いだ工場ID
                    if (!isInsert && string.IsNullOrEmpty(regist.TranslationText))
                    {
                        // 更新で入力内容が空の場合、登録しない
                        continue;
                    }
                    if (isInsert)
                    {
                        if (isFirstInsert)
                        {
                            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out insertedTransId, registSql, Master.SqlName.SubDir, regist, this.db, listUnComment: listUnComment);
                            isFirstInsert = false; // 以降はキーの採番を行わない
                            registSql = Master.SqlName.InsertMsTranslationInfo; //INSERT
                            continue;
                        }
                        else
                        {
                            regist.TranslationId = insertedTransId;
                        }
                    }
                    else
                    {
                        regist.TranslationId = updateTransId;
                    }
                    // SQL実行
                    TMQUtil.SqlExecuteClass.Regist(registSql, Master.SqlName.SubDir, regist, this.db, listUnComment: listUnComment);
                }

                return true;
            }
        }

        /// <summary>
        /// ユーザの言語を取得
        /// </summary>
        /// <returns>言語ID</returns>
        private string getUserLanguageId()
        {
            int userId = int.Parse(this.UserId);
            var userInfo = new ComDao.MsUserEntity().GetEntity(userId, this.db);
            return userInfo.LanguageId;
        }

        /// <summary>
        /// 翻訳一覧入力チェック
        /// </summary>
        /// <param name="isSpec">機種別仕様を登録する場合True、選択肢を登録する場合False</param>
        /// <param name="keyInfo">登録用キー情報</param>
        /// <param name="errorInfoDictionary">out エラー情報</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorTranslationList(bool isSpec, Dao.Common.Param keyInfo, out List<Dictionary<string, object>> errorInfoDictionary)
        {
            // 一覧のID
            string ctrlId = isSpec ? FormInfo.SpecRegist.TranslateList : FormInfo.SpecItemRegist.List;
            // アイテムの翻訳重複チェックのSQL
            string sqlIdTransDuplicate = isSpec ? Sql.SpecRegist.GetCountDuplicateTranslation : Master.SqlName.GetCountTranslation;
            // アイテムの翻訳重複チェックのSQLのフォルダ
            string sqlDirTransDuplicate = isSpec ? Sql.SpecRegist.SubDir : Master.SqlName.SubDir;
            // アイテムの翻訳重複チェックのSQLのアンコメント文字列
            List<string> listUnCommentDuplicate = isSpec ? null : new List<string> { "Extension" };

            errorInfoDictionary = new();
            // 戻り値、エラーの場合True
            bool isError = false;

            // 翻訳一覧のマッピング情報リスト
            var infoTransList = getResultMappingInfo(ctrlId);

            // 翻訳一覧の画面の内容
            var dicTranslationList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);

            // ユーザ言語を取得
            var userLanguageId = getUserLanguageId();

            // 翻訳一覧を繰り返し処理
            foreach (var dicRow in dicTranslationList)
            {
                // 行の内容をデータクラスに変換
                Dao.Common.Translation transRow = new();
                SetDataClassFromDictionary(dicRow, ctrlId, transRow);

                // 表示するエラー情報を設定
                ErrorInfo errorInfo = new ErrorInfo(dicRow);
                // 行単位のエラー有無
                bool isErrorRow = false;

                // ユーザ言語の必須チェック
                if (isErrorTransNameNull(transRow, userLanguageId))
                {
                    isErrorRow = true;
                    // ツールチップ表示項目のVAL値
                    string val = infoTransList.getValName("item_tran_name");
                    // 「入力してください。」
                    string msg = GetResMessage(ComRes.ID.ID941220009);
                    errorInfo.setError(msg, val);
                }
                // アイテム翻訳重複チェック
                if (isErrorDuplicateItemTraslation(transRow))
                {
                    isErrorRow = true;
                    // ツールチップ表示項目のVAL値
                    string val = infoTransList.getValName("item_tran_name");
                    // 「アイテム翻訳は既に登録されています。」
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID941260001, ComRes.ID.ID111010005 });
                    errorInfo.setError(errMsg, val);
                }

                if (isErrorRow)
                {
                    isError = true;
                    // エラー情報を設定
                    errorInfoDictionary.Add(errorInfo.Result);
                }
            }

            return isError;

            // ユーザ言語の翻訳必須チェック
            bool isErrorTransNameNull(Dao.Common.Translation transRow, string userLanguageId)
            {
                // 行の言語がログインユーザの言語の場合
                if (transRow.LanguageId == userLanguageId)
                {
                    // 行の名称がNullのない場合、エラー
                    bool isError = string.IsNullOrEmpty(transRow.TranslationText);
                    return isError;
                }
                // ログインユーザの言語でない場合はエラーにならない
                return false;
            }

            // アイテム翻訳重複チェック
            bool isErrorDuplicateItemTraslation(Dao.Common.Translation transRow)
            {
                if (string.IsNullOrEmpty(transRow.TranslationText))
                {
                    // アイテム翻訳が未入力の場合、チェック対象外
                    return false;
                }
                if (transRow.TranslationText == transRow.TranslationTextBk)
                {
                    // アイテム翻訳に変更がない場合、チェック対象外
                    return false;
                }
                // 同じ構成グループで同じ工場のアイテムで、同じ翻訳があるかを取得
                int cnt = 0;

                if (!isSpec)
                {
                    // 選択肢の場合、検索SQLが異なるため追加で条件に設定
                    transRow.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem;
                    transRow.SequenceNo = 1;
                    transRow.ExtensionData = keyInfo.SpecId.ToString();
                }
                if (!TMQUtil.GetCountDb(transRow, sqlIdTransDuplicate, ref cnt, db, sqlDirTransDuplicate, listUnCommentDuplicate))
                {
                    return true;
                }

                if (cnt > 0)
                {
                    // 翻訳がある場合、エラー
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 一覧に対する排他チェック
        /// </summary>
        /// <param name="ctrlId">一覧のID</param>
        /// <param name="isTranslation">翻訳の場合、IDが無ければチェック対象から外すためTRUE</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusiveList(string ctrlId, bool isTranslation = false)
        {
            var dicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
            if (isTranslation)
            {
                // 翻訳の排他チェックの場合、特定の言語は登録されていない場合があり、その場合に必ず排他チェックエラーとなるのを避けるため、チェック対象より除外する
                var mapInfo = getResultMappingInfo(ctrlId);
                string valNo = mapInfo.getValName("translation_id");
                dicList = dicList.Where(x => !string.IsNullOrEmpty(x[valNo].ToString())).ToList();
            }
            bool result = !checkExclusiveList(ctrlId, dicList);
            return result;
        }
        #endregion
    }
}