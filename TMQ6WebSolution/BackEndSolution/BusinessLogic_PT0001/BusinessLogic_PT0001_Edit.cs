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
using Dao = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// 詳細編集画面
    /// </summary>
    public partial class BusinessLogic_PT0001 : CommonBusinessLogicBase
    {

        /// <summary>
        /// 詳細編集画面の表示種類
        /// </summary>
        private enum EditDispType
        {
            /// <summary>新規</summary>
            New,
            /// <summary>修正</summary>
            Update,
            /// <summary>複写</summary>
            Copy,
        }

        /// <summary>
        /// 拡張データに持っている取得条件
        /// </summary>
        private static class condPartsType
        {
            /// <summary>
            /// 使用区分
            /// </summary>
            public static class useDivition
            {
                /// <summary>構成グループID</summary>
                public const short StructureGroupId = 1970;
                /// <summary>連番</summary>
                public const short Seq = 1;
                /// <summary>拡張データ</summary>
                public const string ExData = "1";
            }
        }

        /// <summary>
        /// 画面の起動種類を呼出元ボタンの画面遷移アクション区分より判定
        /// </summary>
        /// <returns>画面の起動種類、新規or修正or複写</returns>
        private EditDispType getEditType()
        {
            switch (this.TransActionDiv)
            {
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.New:
                    // 新規
                    return EditDispType.New;
                    break;
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit:
                    // 修正
                    return EditDispType.Update;
                    break;
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Copy:
                    // 複写
                    return EditDispType.Copy;
                    break;
                default:
                    // 到達不能
                    throw new Exception();
            }
        }

        /// <summary>
        /// 詳細編集画面初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchEdit()
        {

            // 棚番の結合文字列を検索
            if (!setJoinStr(TMQUtil.GetUserFactoryId(this.UserId, this.db)))
            {
                return false;
            }

            // クリックされたボタンを判定
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            // 起動パターンが「新規」の場合は初期検索を行わない
            if (compareId.IsNew())
            {
                Dao.searchResult result = new();

                // 使用区分の初期値をセット
                result.UseSegmentStructureId = getInitialValue(condPartsType.useDivition.StructureGroupId, condPartsType.useDivition.Seq, condPartsType.useDivition.ExData);

                // ページ情報取得
                var pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.PartsInfo, this.pageInfoList);

                // 一覧に値をセット
                bool retVal = SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, new List<Dao.searchResult>() { result }, 1);

                // ページ情報取得
                pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.PurchaseInfo, this.pageInfoList);

                // 一覧に値をセット
                retVal = SetSearchResultsByDataClass<Dao.searchResult>(pageInfo, new List<Dao.searchResult>() { result }, 1);

                return true;
            }

            // 検索条件を作成
            Dao.detailSearchCondition condition = getDetailSearchCondition(ConductInfo.FormDetail.ControlId.PartsInfo);
            if (!searchPartsInfo(condition, ConductInfo.FormEdit.GroupNo))
            {
                return false;
            }

            this.Status = CommonProcReturn.ProcStatus.Valid;
            return true;

            bool setJoinStr(int factoryId)
            {
                Dao.editJoinString condition = new();
                condition.JoinString = TMQUtil.GetJoinStrOfPartsLocation(factoryId, this.LanguageId, this.db);

                IList<Dao.editJoinString> result = new List<Dao.editJoinString> { condition };

                // 画面項目に値を設定
                if (!SetFormByDataClass(ConductInfo.FormEdit.ControlId.JoinString, result))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            // 画面情報取得
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            DateTime now = DateTime.Now;
            Dao.editResult registInfo = getRegistInfo<Dao.editResult>(ConductInfo.FormEdit.GroupNo, now);

            // 枝番がNULLの場合は空文字にする
            registInfo.PartsLocationDetailNo = ConvertNullToStringEmpty(registInfo.PartsLocationDetailNo);

            // 階層情報の取得
            IList<Dao.editResult> registStructureInfo = new List<Dao.editResult> { registInfo };
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.editResult>(ref registStructureInfo, new List<StructureType> { StructureType.SpareLocation });
            // 場所階層の値を設定
            setLayerInfo(ref registInfo);
            var pattern = getEditType();

            // 排他チェック(更新のみ)
            if (pattern == EditDispType.Update && !checkExclusiveSingle(ConductInfo.FormEdit.ControlId.PartsInfo))
            {
                return false;
            }

            // 入力チェック
            if (isErrorRegist((int)pattern, registInfo))
            {
                return false;
            }

            // 標準棚番まで入力されている場合は標準棚番を登録する
            if (!string.IsNullOrEmpty(registInfo.LocationStructureId.ToString()))
            {
                registInfo.PartsLocationId = long.Parse(registInfo.LocationStructureId.ToString());
                registInfo.LocationRackStructureId = registInfo.PartsLocationId;
            }

            // 発注点・発注量がNULLの場合は「0」で登録する
            if (registInfo.LeadTimeExceptUnit == null)
            {
                registInfo.LeadTimeExceptUnit = 0;
            }
            if (registInfo.OrderQuantityExceptUnit == null)
            {
                registInfo.OrderQuantityExceptUnit = 0;
            }

            // 登録処理
            if (registDb(registInfo, out long partsId) == false)
            {
                return false;
            }

            // 再検索処理を実装する
            //登録した予備品IDを設定（詳細画面を表示するための情報設定）
            var pageInfo = GetPageInfo(ConductInfo.FormEdit.ControlId.PartsInfo, this.pageInfoList);
            ComDao.PtPartsEntity info = new ComDao.PtPartsEntity();
            if (pattern == EditDispType.Update)
            {
                info.PartsId = registInfo.PartsId;
            }
            else
            {
                info.PartsId = partsId;
            }

            SetSearchResultsByDataClass<ComDao.PtPartsEntity>(pageInfo, new List<ComDao.PtPartsEntity> { info }, 1);

            return true;

            // 画面のツリーの階層情報を登録用にセット
            void setLayerInfo(ref Dao.editResult regist)
            {
                // 各階層のIDは名称のプロパティに文字列として格納される（ツリーの定義の関係）ため、数値に変換
                regist.LocationDistrictStructureId = ComUtil.ConvertStringToInt(regist.DistrictName);
                regist.LocationFactoryStructureId = ComUtil.ConvertStringToInt(regist.FactoryName);
                regist.LocationWarehouseStructureId = ComUtil.ConvertStringToInt(regist.WarehouseName);
            }
        }

        /// <summary>
        /// INSERTかUPDATEかを取得
        /// </summary>
        /// <param name="type">この画面の起動種類</param>
        /// <returns>INSERTならTRUE</returns>
        private bool isInsertEdit(EditDispType type)
        {
            return type == EditDispType.New || type == EditDispType.Copy;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(Dao.editResult registInfo, out long newPartsId)
        {
            // ページ遷移用の予備品ID
            newPartsId = -1;

            if (isInsertEdit(getEditType()))
            {
                // INSERT文
                return TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out newPartsId, SqlName.Edit.InsertPartsInfo, SqlName.SubDir, registInfo, db);
            }
            else
            {
                // UPDATE文
                return TMQUtil.SqlExecuteClass.Regist(SqlName.Edit.UpdatePartsInfo, SqlName.SubDir, registInfo, db);
            }
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="pattern">画面起動パターン</param>
        /// <param name="resultInfo">登録情報</param>
        /// <returns>エラーの場合Ture</returns>
        private bool isErrorRegist(int pattern, Dao.editResult resultInfo)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 単一の項目の場合のイメージ
            if (checkErrorRegist())
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool checkErrorRegist()
            {
                bool isUpdateNumChanged = pattern == (int)EditDispType.Update && resultInfo.PartsNo != resultInfo.PartsNoBefore;
                // 起動パターンが新規、複写、修正の予備品Noが変更された時
                if (pattern == (int)EditDispType.New || pattern == (int)EditDispType.Copy || isUpdateNumChanged)
                {
                    // 予備品Noの重複チェック
                    checkPartsNo();
                }

                // 倉庫と棚の関連チェック
                checkPartsLocation();

                // 枝番の半角英数字チェック
                checkPartsLocationDetailNo();

                // エラー情報格納クラス
                var targetPurchaseInfo = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.FormEdit.ControlId.PurchaseInfo);
                ErrorInfo errortargetPurchaseInfo = new ErrorInfo(targetPurchaseInfo);
                // エラー情報を画面に設定するためのマッピング情報リスト
                var infoPurchase = getResultMappingInfo(ConductInfo.FormEdit.ControlId.PurchaseInfo);

                string errMsg = string.Empty; // エラーメッセージ
                string val = string.Empty;    // エラー項目の項目番号

                // 発注点が10桁より多い場合エラー
                if (resultInfo.LeadTimeExceptUnit > 9999999999.99m)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111260019, ComRes.ID.ID911140003, "10" }); // 「発注点は整数部10桁以下で入力してください。」
                    val = infoPurchase.getValName("LeadTime");
                    errortargetPurchaseInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errortargetPurchaseInfo.Result); // エラー情報を追加
                }

                // 発注量が10桁より多い場合エラー
                if (resultInfo.OrderQuantityExceptUnit > 9999999999.99m)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111260020, ComRes.ID.ID911140003, "10" }); // 「発注量は整数部10桁以下で入力してください。」
                    val = infoPurchase.getValName("OrderQuantity");
                    errortargetPurchaseInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errortargetPurchaseInfo.Result); // エラー情報を追加
                }

                // 標準単価が10桁より多い場合エラー
                if (resultInfo.UnitPriceExceptUnit > 9999999999.99m)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111270009, ComRes.ID.ID911140003, "10" }); // 「標準単価は整数部10桁以下で入力してください。」
                    val = infoPurchase.getValName("DefaultPrice");
                    errortargetPurchaseInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errortargetPurchaseInfo.Result); // エラー情報を追加
                }

                // エラー情報が存在する場合はエラー
                if (errorInfoDictionary.Count > 0)
                {
                    return true;
                }

                return false;

                // 予備品Noの重複チェック
                void checkPartsNo()
                {
                    // 予備品Noの重複チェックをするSQLを取得
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.GetPartsNoCount, out string outSql);

                    // 条件を指定する
                    ComDao.PtPartsEntity param = new();
                    param.PartsNo = resultInfo.PartsNo;    //予備品No

                    // 件数を取得
                    int cnt = db.GetEntityByDataClass<int>(outSql, param);

                    //　エラーが無ければここで終了
                    if (cnt == 0)
                    {
                        return;
                    }

                    // 予備品Noに重複するものがあった場合
                    string ctrlId = ConductInfo.FormEdit.ControlId.PartsInfo;
                    // エラー情報を画面に設定するためのマッピング情報リスト
                    var info = getResultMappingInfo(ctrlId);
                    // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                    // 単一の内容を取得
                    var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                    // エラー情報格納クラス
                    var errorInfo = new ErrorInfo(targetDic);
                    string val = info.getValName("PartsNo"); // エラーをセットする予備品Noの項目番号

                    // 指定された予備品Noがすでに登録されています。
                    string errMsg = GetResMessage(new string[] { ComRes.ID.ID141120003, ComRes.ID.ID111380022 });
                    errorInfo.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                }

                // 倉庫と棚の関連チェック
                void checkPartsLocation()
                {
                    // 棚が指定されていない場合は何もしない
                    if (resultInfo.LocationStructureId == null || string.IsNullOrEmpty(resultInfo.LocationId))
                    {
                        return;
                    }

                    // 棚IDより倉庫IDを取得するSQLを取得
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.GetParentIdByLocationId, out string outSql);

                    // 条件を指定する
                    Dao.editResult param = new();
                    param.LocationStructureId = (int)resultInfo.LocationStructureId; // 棚ID

                    // 倉庫IDを取得
                    int wareHouseId = db.GetEntityByDataClass<int>(outSql, param);

                    // 倉庫IDが取得できなかったまたは、画面で選択されている倉庫IDと取得した倉庫IDが異なる(倉庫と棚に関連がない場合)場合
                    if (wareHouseId == null || int.Parse(resultInfo.WarehouseName) != wareHouseId)
                    {
                        string ctrlId = ConductInfo.FormEdit.ControlId.DefaultSafeKeepLocation;
                        // エラー情報を画面に設定するためのマッピング情報リスト
                        var info = getResultMappingInfo(ctrlId);
                        // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                        // 単一の内容を取得
                        var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                        // エラー情報格納クラス
                        var errorInfo = new ErrorInfo(targetDic);
                        string val = info.getValName("LocationId"); // エラーをセットする棚IDの項目番号

                        // 予備品倉庫に紐付く棚を選択してください。
                        string errMsg = GetResMessage(new string[] { ComRes.ID.ID141380001 });
                        errorInfo.setError(errMsg, val); // エラー情報をセット
                        errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    }
                }

                // 枝番の半角英数字チェック
                void checkPartsLocationDetailNo()
                {
                    if (!string.IsNullOrEmpty(resultInfo.PartsLocationDetailNo))
                    {
                        var enc = Encoding.GetEncoding("Shift_JIS");
                        if (enc.GetByteCount(resultInfo.PartsLocationDetailNo) != resultInfo.PartsLocationDetailNo.Length || !ComUtil.IsAlphaNumeric(resultInfo.PartsLocationDetailNo))
                        {
                            string ctrlId = ConductInfo.FormEdit.ControlId.DetailNo;
                            // エラー情報を画面に設定するためのマッピング情報リスト
                            var info = getResultMappingInfo(ctrlId);
                            // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                            // 単一の内容を取得
                            var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                            // エラー情報格納クラス
                            var errorInfo = new ErrorInfo(targetDic);
                            string val = info.getValName("PartsLocationDetailNo"); // エラーをセットする枝番の項目番号

                            // 半角英数字で入力してください。
                            string errMsg = GetResMessage(new string[] { ComRes.ID.ID141260002 });
                            errorInfo.setError(errMsg, val); // エラー情報をセット
                            errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初期値を取得する
        /// </summary>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="seq">連番</param>
        /// <param name="extentionData">取得する初期値の拡張項目の値</param>
        /// <returns>取得した初期値</returns>
        private int getInitialValue(int structureGroupId, int seq, string extentionData)
        {
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();

            //構成グループID
            param.StructureGroupId = structureGroupId;

            //連番
            param.Seq = seq;

            // 機能レベル取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> partsTypeList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            foreach (var partsType in partsTypeList)
            {
                if (partsType.ExData == extentionData)
                {
                    return partsType.StructureId;
                }
            }
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(short groupNo, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

            T resultInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);
                // 登録データの設定
                if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }
            return resultInfo;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(Dao.editResult registInfo)
        {
            // TODO:registInfoは登録するテーブルの型、データクラス作成後に変更
            string sqlName;

            // 画面遷移アクション区分に応じてINSERT/UPDATEを分岐していますが、ボタンによって処理が明らかならば必要ありません。
            // 同じボタンでINSERT/UPDATEを切り替える場合は、画面にキー値を保持しているかで判定してください。
            if (this.TransActionDiv == LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit)
            {
                // 修正ボタンの場合
                // 更新SQL文の取得
                sqlName = SqlName.Edit.UpdatePartsInfo;
            }
            else
            {
                // 新規・複写ボタンの場合
                // TODO:シーケンスを採番する処理

                // 新規登録SQL文の取得
                sqlName = SqlName.Edit.InsertPartsInfo;
            }
            // 登録SQL実行
            bool result = TMQUtil.SqlExecuteClass.Regist(sqlName, SqlName.SubDir, registInfo, this.db);
            return result;
        }
    }
}