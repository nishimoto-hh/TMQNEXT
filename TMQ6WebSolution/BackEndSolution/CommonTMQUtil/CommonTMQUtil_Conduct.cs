using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComRes = CommonSTDUtil.CommonResources;
using TMQDataClass = CommonTMQUtil.TMQCommonDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using STDDao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using ComDataBaseClass = CommonSTDUtil.CommonDataBaseClass;
using ExData = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId;
using System.Collections.Generic;
using System;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using HistoryManagementDao = CommonTMQUtil.CommonTMQUtilDataClass;
using CommonWebTemplate.Models.Common;

namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// 機能の固有の処理だが、機能間で共通定義として使用する場合などに処理を共用したい場合はこちらに作成
    /// </summary>
    public static partial class CommonTMQUtil
    {
        public class PartsInventory
        {
            /// <summary>
            /// 予備品在庫登録クラス
            /// </summary>
            public class PartsInventoryBase
            {
                /// <summary>DB接続</summary>
                protected ComDB Db { get; set; }
                /// <summary>ユーザID</summary>
                protected int UserId { get; set; }
                /// <summary>言語ID</summary>
                protected string LanguageId { get; set; }
                /// <summary>処理種類</summary>
                protected Type type { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="db">DB接続</param>
                /// <param name="userId">ログインユーザID</param>
                /// <param name="languageId">言語ID</param>
                protected PartsInventoryBase(ComDB db, string userId, string languageId)
                {
                    this.Db = db;
                    this.UserId = int.Parse(userId);
                    this.LanguageId = languageId;
                }

                /// <summary>
                /// SQLファイル
                /// </summary>
                protected class Sql
                {
                    /// <summary>SQLファイル格納フォルダ</summary>
                    public const string SubDir = @"Common\PartsInventory";

                    /// <summary>ロット情報登録SQL</summary>
                    public const string InsertLot = "InsertLot";
                    /// <summary>ロット情報更新SQL(部門移庫)</summary>
                    public const string UpdateLot = "UpdateLot";
                    /// <summary>ロット情報削除SQL</summary>
                    public const string DeleteLot = "UpdateLotDelete";
                    /// <summary>ロット情報更新SQL(入庫入力)</summary>
                    public const string UpdateLotFromInput = "UpdateLotFromInput";

                    /// <summary>在庫データ登録SQL</summary>
                    public const string InsertStock = "InsertStock";
                    /// <summary>在庫データ更新SQL</summary>
                    public const string UpdateStock = "UpdateStock";
                    /// <summary>在庫データ削除SQL</summary>
                    public const string DeleteStock = "UpdateStockDelete";

                    /// <summary>受払履歴登録SQL</summary>
                    public const string InsertInoutHistory = "InsertInoutHistory";
                    /// <summary>受払履歴登録SQL(ロット情報より指定)</summary>
                    public const string InsertInoutHistoryByLotCtrlId = "InsertInoutHistoryByLotCtrlId";
                    /// <summary>受払履歴削除SQL</summary>
                    public const string DeleteInoutHistory = "UpdateInoutHistoryDelete";

                    /// <summary>在庫データを取得(同じ棚の同じロット)</summary>
                    public const string GetLocationStockByLocation = "GetLocationStockByLocation";
                    /// <summary>在庫データを取得(同じロット)</summary>
                    public const string GetLocationStockByLot = "GetLocationStockByLot";

                    /// <summary>
                    /// 棚卸のみで使用するSQL
                    /// </summary>
                    /// <remarks>他の機能では使用しないテーブルがあるので、分ける</remarks>
                    public class TakeInventory
                    {
                        /// <summary>SQLファイル格納フォルダ</summary>
                        public const string SubDir = @"Common\PartsInventory\TakeInventory";

                        /// <summary>棚差調整データ取得</summary>
                        public const string GetDifferenceByInventoryId = "GetDifferenceByInventoryId";
                        /// <summary>ロット情報登録SQL</summary>
                        public const string InsertLot = "InsertLot";
                        /// <summary>在庫データ登録SQL</summary>
                        public const string InsertStock = "InsertStock";
                        /// <summary>受払履歴登録SQL</summary>
                        public const string InsertInoutHistory = "InsertInoutHistory";
                        /// <summary>棚差調整データ更新SQL</summary>
                        public const string UpdateInventoryDifference = "UpdateInventoryDifference";
                        /// <summary>棚卸データ更新SQL(確定日時)</summary>
                        public const string UpdateInventoryForConfirmDatetime = "UpdateInventoryForConfirmDatetime";
                        /// <summary>受払履歴更新SQL(確定)</summary>
                        public const string UpdateHistoryForConfirm = "UpdateHistoryForConfirm";
                        /// <summary>受払履歴更新SQL(確定解除)</summary>
                        public const string UpdateHistoryForConfirmCancel = "UpdateHistoryForConfirmCancel";
                        /// <summary>受払履歴削除SQL</summary>
                        public const string DeleteInoutHistory = "DeleteInoutHistory";

                        /// <summary>棚差調整データ更新SQL</summary>
                        public const string UpdateInventoryDifferenceCancel = "UpdateInventoryDifferenceCancel";
                    }
                }

                /// <summary>
                /// DB登録情報の設定
                /// </summary>
                /// <typeparam name="T">テーブル共通アイテムを持つクラス</typeparam>
                /// <param name="condition">設定する登録情報</param>
                protected void SetRegistInfo<T>(ref T condition)
                    where T : ComDataBaseClass.CommonTableItem
                {
                    DateTime now = DateTime.Now;

                    condition.InsertDatetime = now;
                    condition.InsertUserId = this.UserId;
                    condition.UpdateSerialid = 0;
                    condition.UpdateDatetime = now;
                    condition.UpdateUserId = this.UserId;
                }

                /// <summary>
                /// 在庫データテーブルの在庫数取得処理
                /// </summary>
                /// <param name="inventoryCtrlId">在庫管理id、在庫データテーブルの主キー</param>
                /// <returns>在庫数の値</returns>
                protected decimal GetStockQuantity(long inventoryCtrlId)
                {
                    var stock = new ComDao.PtLocationStockEntity().GetEntity(inventoryCtrlId, this.Db);
                    return stock.StockQuantity;
                }

                /// <summary>
                /// 受払履歴の作業Noのシーケンスを進め、次の値を取得
                /// </summary>
                /// <returns>進めたシーケンスの値</returns>
                protected long GetNextWorkNo()
                {
                    long workNo = CommonTMQUtil.GetNextSequence("seq_pt_inout_history_work_no", this.Db);
                    return workNo;
                }

                /// <summary>
                /// 在庫データより現在の数量を取得し、入出庫数で増減した在庫数で更新する処理
                /// </summary>
                /// <typeparam name="T">登録するデータクラス ILocationStockCalcを実装</typeparam>
                /// <param name="cond">登録条件</param>
                /// <param name="isOut">出庫の場合True</param>
                protected void UpdateStock<T>(T cond, bool isOut) where T : Dao.PartsInventory.ILocationStockCalc
                {
                    // 在庫データ更新
                    var preUpdValue = GetStockQuantity(cond.InventoryControlId); // 現在の値
                    int temp = isOut ? -1 : +1; // 出庫の場合引き算、入庫の場合足し算
                    cond.StockQuantity = preUpdValue + (temp * cond.InoutQuantity); // 「更新後数量」を「現在の値」と「出庫数量」より計算
                    // 更新
                    TMQUtil.SqlExecuteClass.Regist(Sql.UpdateStock, Sql.SubDir, cond, this.Db);
                }

                /// <summary>
                /// 受払区分取得
                /// </summary>
                /// <returns>受払区分の受入データ</returns>
                protected List<StructureItemEx.StructureItemExInfo> GetInoutDivision()
                {
                    // 検索条件
                    StructureItemEx.StructureItemExInfo param = new();
                    param.StructureGroupId = (int)Const.MsStructure.GroupId.InoutDivision; // 受払区分
                    param.Seq = 1; // 連番1
                    var results = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.Db);
                    results = results.Where(x => !x.DeleteFlg).ToList(); // 削除されていないもの
                    return results;
                }

                /// <summary>
                /// 指定された構成IDが受払区分の受入の構成IDかチェックする
                /// </summary>
                /// <param name="structureId">チェックする構成ID</param>
                /// <param name="inoutDivisions">取得した受払区分のリスト</param>
                /// <returns>受入ならTrue</returns>
                protected bool IsInOfInoutDivision(long structureId, List<StructureItemEx.StructureItemExInfo> inoutDivisions)
                {
                    return IsCheckOfInoutDivision(structureId, inoutDivisions, true);
                }

                /// <summary>
                /// 指定された構成IDが受払区分の払出の構成IDかチェックする
                /// </summary>
                /// <param name="structureId">チェックする構成ID</param>
                /// <param name="inoutDivisions">取得した受払区分のリスト</param>
                /// <returns>払出ならTrue</returns>
                protected bool IsOutOfInoutDivision(long structureId, List<StructureItemEx.StructureItemExInfo> inoutDivisions)
                {
                    return IsCheckOfInoutDivision(structureId, inoutDivisions, false);
                }

                /// <summary>
                /// 指定された構成IDが受払区分の受入or払出の構成IDかチェックする
                /// </summary>
                /// <param name="structureId">チェックする構成ID</param>
                /// <param name="inoutDivisions">取得した受払区分のリスト</param>
                /// <param name="isCheckIn">受入ならTrue、払出ならFalse</param>
                /// <returns>指定受払区分と合致すればTrue</returns>
                private bool IsCheckOfInoutDivision(long structureId, List<StructureItemEx.StructureItemExInfo> inoutDivisions, bool isCheckIn)
                {
                    // 受入or払出
                    ExData.InoutDivision checkValue = isCheckIn ? ExData.InoutDivision.In : ExData.InoutDivision.Out;
                    // 拡張項目の値と比較する値
                    string exDataValue = ((int)checkValue).ToString();
                    // 指定した受払区分の構成IDを取得
                    List<long> targetIds = inoutDivisions.Where(x => x.ExData == exDataValue).Select(x => (long)x.StructureId).ToList();
                    // 指定した構成IDが含まれるか判定(=指定した構成IDが指定した受払区分である)
                    bool result = targetIds.Contains(structureId);
                    return result;
                }

                /// <summary>
                /// 作業Noから受払履歴を取得(削除されていないもののみ)
                /// </summary>
                /// <param name="workNo">作業No</param>
                /// <returns>取得した受払履歴</returns>
                protected IList<ComDao.PtInoutHistoryEntity> GetHisotryListByWorkNo(long workNo)
                {
                    // 削除対象の受払履歴の取得
                    var historyList = ComDao.PtInoutHistoryEntity.GetListByWorkNo(workNo, this.Db);
                    // 作業Noを指定して受払履歴が0件だったり、削除済みだったりするのはあり得ない(指定誤り)のためエラー
                    if (historyList == null || historyList.Count == 0)
                    {
                        throwError("'" + workNo + "' is incorrect value for WorkNo");
                    }
                    historyList = historyList.Where(x => x.DeleteFlg == false).ToList(); // 過去に削除されたかもしれないので未削除のみ
                    if (historyList == null || historyList.Count == 0)
                    {
                        throwError("'" + workNo + "' is incorrect value for WorkNo");
                    }
                    return historyList;
                }

                /// <summary>
                /// 例外をスロー
                /// </summary>
                /// <param name="message">メッセージ</param>
                protected void throwError(string message)
                {
                    throw new Exception("PartsInventory Error On " + this.type.ToString() + " " + message);
                }
                /// <summary>
                /// 在庫更新処理の種類
                /// </summary>
                public enum Type
                {
                    Input, Output, MoveLocation, MoveDepartment, TakeInventory
                }
            }


            /// <summary>
            /// 入庫入力
            /// </summary>
            public class Input : PartsInventoryBase
            {
                public Input(ComDB db, string userId, string languageId) : base(db, userId, languageId)
                {
                    this.type = Type.Input;
                }
                /// <summary>
                /// 新規登録時の処理
                /// </summary>
                /// <param name="condition">画面の入力内容</param>
                /// <param name="workNo">out 採番した受払履歴の作業No</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool New(Dao.PartsInventory.Input condition, out long workNo)
                {
                    workNo = -1;
                    // 登録情報設定
                    SetRegistInfo(ref condition);
                    // クラスの処理で初期値を設定
                    condition.SetRegistInfo();
                    // ロット情報登録
                    TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long lotControlId, Sql.InsertLot, Sql.SubDir, condition, this.Db);
                    condition.LotControlId = lotControlId;
                    // 在庫情報登録
                    TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long inventoryControlId, Sql.InsertStock, Sql.SubDir, condition, this.Db);
                    condition.InventoryControlId = inventoryControlId;
                    // 受払履歴登録
                    // シーケンスを採番して戻り値と登録条件に設定
                    workNo = GetNextWorkNo();
                    condition.WorkNo = workNo;
                    TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistory, Sql.SubDir, condition, this.Db);

                    return true;
                }
                /// <summary>
                /// 修正登録時の処理
                /// </summary>
                /// <param name="condition">画面の入力内容</param>
                /// <param name="workNo">画面の作業No(前回登録時の値)</param>
                /// <param name="newWorkNo">out 採番した受払履歴の作業No</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Update(Dao.PartsInventory.Input condition, long workNo, out long newWorkNo)
                {
                    newWorkNo = -1;
                    // 登録情報設定
                    SetRegistInfo(ref condition);
                    // クラスの処理で初期値を設定
                    condition.SetRegistInfo();

                    // 受払履歴の論理削除・登録
                    // 作業Noによる受払履歴の取得
                    if (isErrorInoutByWorkNo(workNo, out TMQDataClass.PtInoutHistoryEntity inout))
                    {
                        return false;
                    }
                    // 受払履歴の論理削除
                    TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
                    // 受払履歴の登録
                    condition.LotControlId = inout.LotControlId;
                    condition.InventoryControlId = inout.InventoryControlId;
                    // 作業Noを採番して戻り値と登録条件に設定
                    newWorkNo = GetNextWorkNo();
                    condition.WorkNo = newWorkNo;
                    // 登録
                    TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistory, Sql.SubDir, condition, this.Db);

                    // ロット情報更新
                    updateLot(condition);
                    // 在庫データ更新
                    updateStock(condition);

                    return true;


                    // ロット情報更新
                    void updateLot(Dao.PartsInventory.Input condition)
                    {
                        TMQDataClass.PtLotEntity lot = new();
                        lot = lot.GetEntity(condition.LotControlId, this.Db);
                        if (condition.IsChangedLotInfo(lot))
                        {
                            // ロット情報に関わる情報が更新された場合、ロット情報を更新
                            TMQUtil.SqlExecuteClass.Regist(Sql.UpdateLotFromInput, Sql.SubDir, condition, this.Db);
                        }
                    }

                    // 在庫データ更新
                    void updateStock(Dao.PartsInventory.Input condition)
                    {
                        TMQDataClass.PtLocationStockEntity stock = new();
                        stock = stock.GetEntity(condition.InventoryControlId, this.Db);
                        if ((condition.PartsLocationId != stock.PartsLocationId)
                            || (condition.PartsLocationDetailNo != stock.PartsLocationDetailNo)
                            || (condition.InoutQuantity != stock.StockQuantity))
                        {
                            // 棚ID・棚枝番・数量が更新された場合、在庫データを更新
                            TMQUtil.SqlExecuteClass.Regist(Sql.UpdateStock, Sql.SubDir, condition, this.Db, listUnComment: new List<string> { "IsInput" });
                        }
                    }
                }

                /// <summary>
                /// 受払履歴の取得、もし取得内容に問題があればエラー
                /// </summary>
                /// <param name="workNo">取得する作業No</param>
                /// <param name="inout">out 取得した受払履歴のレコード</param>
                /// <returns></returns>
                private bool isErrorInoutByWorkNo(long workNo, out TMQDataClass.PtInoutHistoryEntity inout)
                {
                    inout = new();
                    var inoutList = GetHisotryListByWorkNo(workNo);
                    // 0件だったり1件より多い場合は無い
                    if (inoutList == null || inoutList.Count == 0)
                    {
                        return true;
                    }
                    inoutList = inoutList.Where(x => !x.DeleteFlg).ToList();
                    if (inoutList.Count > 1)
                    {
                        return true;
                    }
                    // 必ず1件
                    inout = inoutList[0];
                    return false;
                }

                /// <summary>
                /// 取消時の処理
                /// </summary>
                /// <param name="workNo">画面の作業No(前回登録時の値)</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Cancel(long workNo)
                {
                    Dao.PartsInventory.Input condition = new();
                    condition.WorkNo = workNo;
                    // 登録情報設定
                    SetRegistInfo(ref condition);
                    // クラスの処理で初期値を設定
                    condition.SetRegistInfo();

                    // 作業Noによる受払履歴の取得
                    if (isErrorInoutByWorkNo(workNo, out TMQDataClass.PtInoutHistoryEntity inout))
                    {
                        return false;
                    }
                    // 受払履歴の論理削除
                    TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
                    // キー値の取得
                    condition.LotControlId = inout.LotControlId;
                    condition.InventoryControlId = inout.InventoryControlId;
                    // ロット情報の論理削除
                    TMQUtil.SqlExecuteClass.Regist(Sql.DeleteLot, Sql.SubDir, condition, this.Db);
                    // 在庫情報の論理削除
                    TMQUtil.SqlExecuteClass.Regist(Sql.DeleteStock, Sql.SubDir, condition, this.Db);
                    return true;
                }
            }

            /// <summary>
            /// 出庫入力
            /// </summary>
            public class Output : PartsInventoryBase
            {
                public Output(ComDB db, string userId, string languageId) : base(db, userId, languageId)
                {
                    this.type = Type.Output;
                }
                /// <summary>
                /// 新規登録時の処理
                /// </summary>
                /// <param name="conditions">画面の入力内容(複数行)</param>
                /// <param name="workNo">out 採番した受払履歴の作業No</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool New(List<Dao.PartsInventory.Output> conditions, out long workNo)
                {
                    // 作業Noを採番
                    workNo = GetNextWorkNo();
                    foreach (var condition in conditions)
                    {
                        Dao.PartsInventory.Output cond = condition; //ループの変数はrefが不可のため他の変数にセット
                        SetRegistInfo(ref cond);
                        cond.SetDivisions();
                        cond.WorkNo = workNo;
                        // 受払履歴登録
                        TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistoryByLotCtrlId, Sql.SubDir, cond, this.Db);

                        // 在庫データ更新
                        UpdateStock(cond, true);
                    }
                    return true;
                }

                /// <summary>
                /// 取消時の処理
                /// </summary>
                /// <param name="workNo">画面の作業No(前回登録時の値)</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Cancel(long workNo)
                {
                    // 更新条件
                    Dao.PartsInventory.Output condition = new();
                    condition.WorkNo = workNo;
                    SetRegistInfo(ref condition);

                    // 削除対象の受払履歴の取得
                    var historyList = GetHisotryListByWorkNo(workNo);
                    foreach (var history in historyList)
                    {
                        // 在庫データの更新
                        condition.InventoryControlId = history.InventoryControlId;
                        condition.InoutQuantity = history.InoutQuantity;
                        // 在庫データ更新
                        UpdateStock(condition, false);
                    }
                    // 受払履歴の論理削除
                    TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
                    return true;
                }
            }

            /// <summary>
            /// 棚番移庫
            /// </summary>
            public class MoveLocation : PartsInventoryBase
            {
                public MoveLocation(ComDB db, string userId, string languageId) : base(db, userId, languageId)
                {
                    this.type = Type.MoveLocation;
                }
                /// <summary>
                /// 新規登録時の処理
                /// </summary>
                /// <param name="condition">画面の入力内容</param>
                /// <param name="workNo">out 採番した受払履歴の作業No</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool New(Dao.PartsInventory.MoveLocation condition, out long workNo)
                {
                    // 作業Noを採番
                    workNo = GetNextWorkNo();
                    condition.WorkNo = workNo;
                    // 登録情報設定
                    SetRegistInfo(ref condition);

                    // 移庫元在庫データ更新
                    UpdateStock(condition, true);
                    // 移庫元受払履歴登録
                    registInoutHistory(condition, true);

                    // 移庫先在庫データの有無を取得
                    bool isExistsStock = isExistsStockForTarget(condition, out long targetInvCtrlId);
                    // 移庫先在庫データ登録
                    if (isExistsStock)
                    {
                        // 移庫先在庫データがある場合は更新
                        condition.InventoryControlId = targetInvCtrlId;
                        UpdateStock(condition, false);
                    }
                    else
                    {
                        // 移庫先在庫データが無い場合は新規登録
                        // 移庫元の情報を検索し、移庫先の登録に必要な情報を取得する
                        ComDao.PtLocationStockEntity stock = new ComDao.PtLocationStockEntity().GetEntity(condition.InventoryControlId, this.Db);
                        condition.PartsId = stock.PartsId;
                        // 登録
                        TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out targetInvCtrlId, Sql.InsertStock, Sql.SubDir, condition, this.Db);
                        condition.InventoryControlId = targetInvCtrlId; // 採番したIDを次の登録条件に設定
                    }

                    // 移庫先受払履歴登録
                    registInoutHistory(condition, false);

                    return true;

                    // 移庫先在庫データ存在確認
                    bool isExistsStockForTarget(Dao.PartsInventory.MoveLocation condition, out long targetInvCtrlId)
                    {
                        // 移庫先の在庫データを取得
                        ComDao.PtLocationStockEntity stock = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.PtLocationStockEntity>(Sql.GetLocationStockByLocation, Sql.SubDir, condition, this.Db);
                        // 在庫データの有無
                        bool result = stock != null;
                        // 在庫データのキー値、無い場合は使用しないため0
                        targetInvCtrlId = result ? stock.InventoryControlId : 0;
                        return result;
                    }
                    // 受払履歴登録
                    void registInoutHistory(Dao.PartsInventory.MoveLocation condition, bool isSource)
                    {
                        // 受払区分と作業区分を設定
                        condition.SetDivisions(isSource);
                        // 登録
                        TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistoryByLotCtrlId, Sql.SubDir, condition, this.Db);
                    }
                }
                /// <summary>
                /// 取消時の処理
                /// </summary>
                /// <param name="workNo">画面の作業No(前回登録時の値)</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Cancel(long workNo)
                {
                    // 更新条件
                    Dao.PartsInventory.MoveLocation condition = new();
                    condition.WorkNo = workNo;
                    SetRegistInfo(ref condition);

                    // 受払区分取得
                    var inoutList = GetInoutDivision();

                    // 削除対象の受払履歴の取得
                    var historyList = GetHisotryListByWorkNo(workNo);
                    foreach (var history in historyList)
                    {
                        // 在庫データの更新
                        // 更新情報設定
                        condition.InventoryControlId = history.InventoryControlId;
                        condition.InoutQuantity = history.InoutQuantity;

                        // 受払区分の判定
                        bool isIn = IsInOfInoutDivision(history.InoutDivisionStructureId, inoutList); // 受入
                        bool isOut = IsOutOfInoutDivision(history.InoutDivisionStructureId, inoutList); // 払出
                        // もし両方ともTrueまたはFalseならエラー(データ不正)
                        if (isIn == isOut) { throwError("'" + history.InoutDivisionStructureId + "' is incorrect value for InoutDivision."); }
                        // 在庫データ更新(取消なので、払出の場合は払出の取消のため、受入)
                        UpdateStock(condition, !isOut);
                    }
                    // 受払履歴の論理削除
                    TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
                    return true;
                }
                /// <summary>
                /// 修正登録時の処理
                /// </summary>
                /// <param name="condition">画面の入力内容</param>
                /// <param name="workNo">画面の作業No(前回登録時の値)</param>
                /// <param name="newWorkNo">out 採番した受払履歴の作業No</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Update(Dao.PartsInventory.MoveLocation condition, long workNo, out long newWorkNo)
                {
                    // 取消→登録
                    this.Cancel(workNo);
                    this.New(condition, out newWorkNo);
                    return true;
                }
            }

            /// <summary>
            /// 部門移庫
            /// </summary>
            public class MoveDepartment : PartsInventoryBase
            {
                public MoveDepartment(ComDB db, string userId, string languageId) : base(db, userId, languageId)
                {
                    this.type = Type.MoveDepartment;
                }
                /// <summary>
                /// 新規登録時の処理
                /// </summary>
                /// <param name="condition">画面の入力内容</param>
                /// <param name="workNo">out 採番した受払履歴の作業No</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool New(Dao.PartsInventory.MoveDepartment condition, out long workNo)
                {
                    workNo = GetNextWorkNo();
                    condition.WorkNo = workNo;
                    SetRegistInfo(ref condition);

                    // ロット管理IDから同じ予備品、ロットの在庫データを移庫元として取得
                    List<ComDao.PtLocationStockEntity> sourceStockList = TMQUtil.SqlExecuteClass.SelectList<ComDao.PtLocationStockEntity>(Sql.GetLocationStockByLot, Sql.SubDir, condition, this.Db);
                    // 受払履歴を登録
                    foreach (var sourceStock in sourceStockList)
                    {
                        // 移庫元
                        condition.SetDivisions(true);
                        condition.InventoryControlId = sourceStock.InventoryControlId; // 在庫管理ID
                        condition.InoutQuantity = sourceStock.StockQuantity; // 入出庫数は在庫数と同じ
                        // 登録(移庫元なのでロット管理IDより情報を取得)
                        TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistoryByLotCtrlId, Sql.SubDir, condition, this.Db);

                        // 移庫先
                        condition.SetDivisions(false);
                        // 登録(移庫先なので指定された情報を登録)
                        TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistory, Sql.SubDir, condition, this.Db);
                    }

                    // ロット管理IDでロット情報を更新
                    TMQUtil.SqlExecuteClass.Regist(Sql.UpdateLot, Sql.SubDir, condition, this.Db);

                    return true;
                }
                /// <summary>
                /// 取消時の処理
                /// </summary>
                /// <param name="workNo">画面の作業No(前回登録時の値)</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Cancel(long workNo)
                {
                    // 更新条件
                    Dao.PartsInventory.MoveDepartment condition = new();
                    condition.WorkNo = workNo;
                    SetRegistInfo(ref condition);
                    // 受払区分取得
                    var inoutList = GetInoutDivision();

                    // WorkNoで受払履歴を取得
                    // 削除対象の受払履歴の取得
                    var historyList = GetHisotryListByWorkNo(workNo);
                    // 払出の先頭行の情報で、ロット情報を更新
                    foreach (var history in historyList)
                    {
                        // 受払区分の判定
                        bool isOut = IsOutOfInoutDivision(history.InoutDivisionStructureId, inoutList); // 払出
                        // 払出(移庫元)の情報でロット情報を更新
                        if (isOut)
                        {
                            // ロット管理IDでロット情報を更新
                            // 部門、勘定科目、管理区分、管理No、管理No、入庫日
                            condition.DepartmentStructureId = history.DepartmentStructureId;
                            condition.AccountStructureId = history.AccountStructureId;
                            condition.ManagementDivision = history.ManagementDivision;
                            condition.ManagementNo = history.ManagementNo;
                            condition.InoutDatetime = history.InoutDatetime;
                            condition.UnitPrice = 0; // 移庫元が修理部門の場合は0、そうでない場合は元々の値で更新(=更新しない
                            // キー
                            condition.LotControlId = history.LotControlId;
                            TMQUtil.SqlExecuteClass.Regist(Sql.UpdateLot, Sql.SubDir, condition, this.Db);
                            break;
                        }
                        continue;
                    }

                    // 受払履歴の論理削除
                    TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);

                    return true;
                }
                /// <summary>
                /// 修正登録時の処理
                /// </summary>
                /// <param name="condition">画面の入力内容</param>
                /// <param name="workNo">画面の作業No(前回登録時の値)</param>
                /// <param name="newWorkNo">out 採番した受払履歴の作業No</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Update(Dao.PartsInventory.MoveDepartment condition, long workNo, out long newWorkNo)
                {
                    // 取消→登録
                    this.Cancel(workNo);
                    this.New(condition, out newWorkNo);
                    return true;
                }
            }

            /// <summary>
            /// 入庫入力、出庫入力、移庫入力共通チェック処理
            /// </summary>
            public class InventryGetInfo
            {
                /// <summary>Gets or sets データ件数</summary>
                /// <value>データ件数</value>
                public int Count { get; set; }
                /// <summary>
                /// SQLファイル名称
                /// </summary>
                public static class SqlName
                {
                    /// <summary>SQL名：構成リスト取得</summary>
                    public const string InventryGetCount = "Select_InventryGetCount";
                    /// <summary>SQL格納先サブディレクトリ名</summary>
                    public const string SubDir = "Common";

                    /// <summary>
                    /// 予備品ID(@PartsId)、受払履歴の更新日時(@UpdateDatetime)、受払履歴の受払日時(@InoutDatetime)より棚卸中のデータがあるかどうかチェックする処理
                    /// </summary>
                    /// <param name="param">検索条件：予備品ID</param>
                    /// <param name="db">DB接続</param>
                    /// <returns>データがあればTrue</returns>
                    public static bool IsExistsInventryData(object condition, ComDB db)
                    {
                        // 移庫先の在庫データを取得
                        InventryGetInfo info = TMQUtil.SqlExecuteClass.SelectEntity<InventryGetInfo>(SqlName.InventryGetCount, SqlName.SubDir, condition, db);
                        if (info.Count > 0)
                        {
                            return true; // データが1件以上あればTrue
                        }
                        return false;
                    }
                }
            }

            /// <summary>
            /// 棚卸
            /// </summary>
            public class TakeInventory : PartsInventoryBase
            {
                public TakeInventory(ComDB db, string userId, string languageId) : base(db, userId, languageId)
                {
                    this.type = Type.TakeInventory;
                }
                /// <summary>
                /// 棚卸確定
                /// </summary>
                /// <param name="inventoryId">棚卸ID</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool Confirm(long inventoryId)
                {
                    // 棚卸IDから棚卸データを取得
                    ComDao.PtInventoryEntity inventory = getInventoryInfo(inventoryId);
                    // 登録に使用するデータクラス
                    Dao.PartsInventory.TakeInventory cond = getRegistCondition(inventory);
                    // 受払区分を取得
                    var inoutList = GetInoutDivision();
                    // 棚差調整データを取得
                    IList<ComDao.PtInventoryDifferenceEntity> invDifList = getInventoryDifferenceList(inventory);
                    if (invDifList != null)
                    {
                        // 棚差調整データで繰り返し、登録を行う
                        foreach (var invDif in invDifList)
                        {
                            // 棚差調整データの値を設定
                            cond.InventoryDifferenceId = invDif.InventoryDifferenceId;
                            // 受入かどうかを取得
                            bool isIn = IsInOfInoutDivision(invDif.InoutDivisionStructureId ?? -1, inoutList);

                            // ロット情報登録
                            cond.LotControlId = registLotAndGetKey(cond, invDif.LotControlId);
                            // 在庫データ登録
                            cond.InventoryControlId = registStockAndGetKey(cond, invDif.InventoryControlId, invDif.InoutQuantity, isIn);

                            // 受払履歴を登録
                            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long inoutHistoryId, Sql.TakeInventory.InsertInoutHistory, Sql.TakeInventory.SubDir, cond, this.Db);

                            // 棚差調整データに採番したキー値を設定
                            cond.InoutHistoryId = inoutHistoryId;
                            // 受入の場合、ロット管理IDと在庫管理IDも更新
                            List<string> listUnComment = new();
                            if (isIn) { listUnComment.Add("IsInput"); }
                            TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryDifference, Sql.TakeInventory.SubDir, cond, this.Db, listUnComment: listUnComment);
                        }
                    }
                    // 棚卸データを更新
                    TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryForConfirmDatetime, Sql.TakeInventory.SubDir, cond, this.Db);

                    return true;

                    // 登録用データクラス取得
                    Dao.PartsInventory.TakeInventory getRegistCondition(ComDao.PtInventoryEntity inventory)
                    {
                        Dao.PartsInventory.TakeInventory cond = new();
                        SetRegistInfo(ref cond); // 更新日時などを設定
                        cond.InventoryId = inventory.InventoryId;
                        // 対象年月の末日を取得
                        cond.InoutDatetime = getMaxTimeOfDate(inventory.TargetMonth);
                        // 作業Noを採番
                        cond.WorkNo = GetNextWorkNo();
                        return cond;

                        // 指定年月の最終日の23:59:59を取得する処理
                        DateTime getMaxTimeOfDate(DateTime target)
                        {
                            var lastday = ComUtil.GetDateMonthLastDay(target);
                            return new DateTime(lastday.Year, lastday.Month, lastday.Day, 23, 59, 59);
                        }
                    }

                    // ロット情報の登録が必要なら行い、ロット管理IDを取得する
                    // cond 登録情報
                    // lotCtrlId 棚差と調整データのロット管理ID
                    // return ロット管理ID(登録時は採番、そうでない場合は引数の値)
                    long registLotAndGetKey(Dao.PartsInventory.TakeInventory cond, long? lotCtrlId)
                    {
                        long returnKey;
                        if (lotCtrlId == null)
                        {
                            // 登録
                            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out returnKey, Sql.TakeInventory.InsertLot, Sql.TakeInventory.SubDir, cond, this.Db);
                        }
                        else
                        {
                            // 登録不要
                            returnKey = lotCtrlId ?? -1;
                        }
                        return returnKey;
                    }

                    // 在庫データの登録or更新を行う処理
                    // cond 登録情報
                    // invCtrlId 在庫管理ID
                    // inoutQty 棚差調整データの受払数
                    // isIn 棚差調整データで受入の場合True
                    // return 在庫管理ID(新規は採番、更新は引数の値)
                    long registStockAndGetKey(Dao.PartsInventory.TakeInventory cond, long? invCtrlId, decimal? inoutQty, bool isIn)
                    {
                        long returnKey;
                        if (invCtrlId == null)
                        {
                            // 登録
                            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out returnKey, Sql.TakeInventory.InsertStock, Sql.TakeInventory.SubDir, cond, this.Db);
                        }
                        else
                        {
                            // 更新
                            returnKey = invCtrlId ?? -1;
                            cond.InventoryControlId = returnKey;
                            cond.InoutQuantity = inoutQty ?? 0;
                            // 在庫更新
                            UpdateStock(cond, !isIn);
                        }
                        return returnKey;
                    }
                }

                /// <summary>
                /// 棚卸IDより棚卸データを取得し、取得できなかった場合エラーとする
                /// </summary>
                /// <param name="inventoryId"></param>
                /// <returns></returns>
                private ComDao.PtInventoryEntity getInventoryInfo(long inventoryId)
                {
                    ComDao.PtInventoryEntity inventory = new ComDao.PtInventoryEntity().GetEntity(inventoryId, this.Db);
                    // 取得できなかった場合エラー
                    if (inventory == null)
                    {
                        throwError("'" + inventoryId + "' is incorrect value for InventoryId");
                    }
                    return inventory;
                }

                /// <summary>
                /// 棚卸IDより棚差調整データを取得
                /// </summary>
                /// <param name="inventory"></param>
                /// <returns></returns>
                private IList<ComDao.PtInventoryDifferenceEntity> getInventoryDifferenceList(ComDao.PtInventoryEntity inventory)
                {
                    // 棚差調整データを取得
                    IList<ComDao.PtInventoryDifferenceEntity> invDifList = TMQUtil.SqlExecuteClass.SelectList<ComDao.PtInventoryDifferenceEntity>(Sql.TakeInventory.GetDifferenceByInventoryId, Sql.TakeInventory.SubDir, inventory, this.Db);
                    return invDifList;
                }

                /// <summary>
                /// 棚卸確定解除
                /// </summary>
                /// <param name="inventoryId">棚卸ID</param>
                /// <returns>エラーの場合False、ロールバックしてください</returns>
                public bool ConfirmCancel(long inventoryId)
                {
                    ComDao.PtInventoryEntity inventory = getInventoryInfo(inventoryId);
                    Dao.PartsInventory.TakeInventory cond = getRegistCondition(inventory);
                    // 棚卸データを更新
                    TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryForConfirmDatetime, Sql.TakeInventory.SubDir, cond, this.Db);

                    // 受払区分を取得
                    var inoutList = GetInoutDivision();

                    // 棚差調整データを取得
                    IList<ComDao.PtInventoryDifferenceEntity> invDifList = getInventoryDifferenceList(inventory);
                    if (invDifList == null)
                    {
                        return true;
                    }
                    // 棚差調整データで繰り返し、登録を行う
                    foreach (var invDif in invDifList)
                    {
                        // 棚差調整データの値を設定
                        cond.InventoryDifferenceId = invDif.InventoryDifferenceId;
                        // 受入かどうかを取得
                        bool isIn = IsInOfInoutDivision(invDif.InoutDivisionStructureId ?? -1, inoutList);
                        // 受払履歴
                        cond.WorkNo = invDif.WorkNo ?? -1;
                        TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.DeleteInoutHistory, Sql.TakeInventory.SubDir, cond, this.Db);
                        if (isIn)
                        {
                            //受入の場合、ロット情報と在庫データは物理削除
                            // ロット情報
                            new ComDao.PtLotEntity().DeleteByPrimaryKey(invDif.LotControlId ?? -1, this.Db);
                            // 在庫データ
                            new ComDao.PtLocationStockEntity().DeleteByPrimaryKey(invDif.InventoryControlId ?? -1, this.Db);
                        }
                        else
                        {
                            // 在庫データ
                            cond.InventoryControlId = invDif.InventoryControlId ?? -1;
                            cond.InoutQuantity = invDif.InoutQuantity ?? 0;
                            UpdateStock(cond, isIn);
                        }

                        // 棚差調整データ
                        // 受入の場合、ロット管理IDと在庫管理IDも更新
                        List<string> listUnComment = new();
                        if (isIn) { listUnComment.Add("IsInput"); }
                        TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryDifferenceCancel, Sql.TakeInventory.SubDir, cond, this.Db, listUnComment: listUnComment);
                    }
                    return true;

                    // 登録用データクラス取得
                    Dao.PartsInventory.TakeInventory getRegistCondition(ComDao.PtInventoryEntity inventory)
                    {
                        Dao.PartsInventory.TakeInventory cond = new();
                        SetRegistInfo(ref cond); // 更新日時などを設定
                        cond.InventoryId = inventory.InventoryId;
                        // 日時はNullで更新
                        cond.InoutDatetime = null;
                        return cond;
                    }
                }
            }
        }

        /// <summary>
        /// 変更管理 共通メソッド
        /// </summary>
        public class HistoryManagement
        {
            /// <summary>DB接続</summary>
            protected ComDB Db { get; set; }
            /// <summary>ユーザID</summary>
            protected int UserId { get; set; }
            /// <summary>言語ID</summary>
            protected string LanguageId { get; set; }
            /// <summary>現在日時</summary>
            protected DateTime Now { get; set; }
            /// <summary>
            /// 申請機能ID(1：機器台帳、2：長期計画)
            /// </summary>
            protected int ApplicationConductId { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="db">DB接続</param>
            /// <param name="userId">ログインユーザID</param>
            /// <param name="languageId">言語ID</param>
            public HistoryManagement(ComDB db, string userId, string languageId, DateTime now, TMQConst.MsStructure.StructureId.ApplicationConduct applicationConductId)
            {
                this.Db = db;
                this.UserId = int.Parse(userId);
                this.LanguageId = languageId;
                this.Now = now;
                this.ApplicationConductId = (int)applicationConductId;
            }

            /// <summary>
            /// SQLファイル
            /// </summary>
            protected class Sql
            {
                /// <summary>SQLファイル格納フォルダ</summary>
                public const string SubDir = @"Common\HistoryManagement";
                /// <summary>申請状況更新SQL</summary>
                public const string UpdateApplicationStatus = "UpdateApplicationStatus";
                /// <summary>ログインユーザの権限の拡張項目を取得するSQL</summary>
                public const string GetAuthLevel = "GetAuthLevel";
                /// <summary>変更管理IDより、場所階層の拡張項目4(工場の承認ユーザーID)を取得するSQL</summary>
                public const string GetApprovalUserId = "GetApprovalUserId";
                /// <summary>変更管理IDより、申請状況の拡張区分取得するSQL</summary>
                public const string GetApplicationStatus = "GetApplicationStatus";
                /// <summary>変更管理テーブルにデータを新規登録するSQL</summary>
                public const string InsertHistoryManagement = "InsertHistoryManagement";
                /// <summary>変更管理詳細テーブルにデータを新規登録するSQL</summary>
                public const string InsertHistoryManagementDetail = "InsertHistoryManagementDetail";
                /// <summary>変更管理IDより、変更管理詳細IDを取得するSQL</summary>
                public const string GetHistoryManagementDetailIdByHistoryManagementId = "GetHistoryManagementDetailIdByHistoryManagementId";
                /// <summary>キーID(機番ID または 長計件名ID)より、指定された申請状況データの件数を取得するSQL</summary>
                public const string GetApplicationStatusCntByKeyId = "GetApplicationStatusCntByKeyId";
                /// <summary>場所階層IDより、工場IDを取得するSQL</summary>
                public const string GetFactoryIdByLocationStructureId = "GetFactoryIdByLocationStructureId";
                /// <summary>キーID(機番ID または 長計件名ID)より、申請状況が「承認済み」以外の変更管理IDを取得するSQL</summary>
                public const string GetHistoryManagementIdByKeyId = "GetHistoryManagementIdByKeyId";
            }

            /// <summary>
            /// 変更があった項目を取得
            /// </summary>
            /// <typeparam name="T">共通の検索結果データクラス IHistoryManagementCommonを実装</typeparam>
            /// <param name="results">検索結果</param>
            public static void setValueChangedItem<T>(IList<T> results)
                where T : HistoryManagementDao.IHistoryManagementCommon, new()
            {
                // 変更のあった項目を取得して非表示列に追加(変更が無ければ空)
                foreach (T result in results)
                {
                    // 末尾の文字が「|」でなければ追加
                    if (!string.IsNullOrEmpty(result.ValueChanged) && result.ValueChanged[result.ValueChanged.Length - 1].ToString() != "|")
                    {
                        result.ValueChanged += "|";
                    }

                    // 下記項目名(District、Factory...)はJavaScriptの背景色設定処理で使用するため、JavaScriptで定義する項目名と統一させる)
                    // 場所階層、職種階層以外の変更のあった項目は各SQLで取得する
                    // 変更のあった項目は、[項目名 + 背景色設定値]とし「|」区切りで1つの文字列とする
                    result.ValueChanged += getColumnName(result.DistrictId, result.OldDistrictId, "District");                     // 地区
                    result.ValueChanged += getColumnName(result.FactoryId, result.OldFactoryId, "Factory");                        // 工場
                    result.ValueChanged += getColumnName(result.PlantId, result.OldPlantId, "Plant");                              // プラント
                    result.ValueChanged += getColumnName(result.SeriesId, result.OldSeriesId, "Series");                           // 系列
                    result.ValueChanged += getColumnName(result.StrokeId, result.OldStrokeId, "Stroke");                           // 工程
                    result.ValueChanged += getColumnName(result.FacilityId, result.OldFacilityId, "Facility");                     // 設備
                    result.ValueChanged += getColumnName(result.JobStructureId, result.OldJobStructureId, "Job");                  // 職種
                    result.ValueChanged += getColumnName(result.LargeClassficationId, result.OldLargeClassficationId, "Large");    // 機種大分類
                    result.ValueChanged += getColumnName(result.MiddleClassficationId, result.OldMiddleClassficationId, "Middle"); // 機種中分類
                    result.ValueChanged += getColumnName(result.SmallClassficationId, result.OldSmallClassficationId, "Small");    // 機種小分類

                    // 末尾の文字が「|」ならば削除
                    if (!string.IsNullOrEmpty(result.ValueChanged) && result.ValueChanged[result.ValueChanged.Length - 1].ToString() == "|")
                    {
                        result.ValueChanged = result.ValueChanged.Remove(result.ValueChanged.Length - 1);
                    }
                }
            }

            /// <summary>
            /// トランザクションデータと変更管理データの差異に応じて背景色を設定
            /// </summary>
            /// <param name="newId">変更後の値</param>
            /// <param name="oldId">変更前の値</param>
            /// <param name="itemName">項目名</param>
            /// <returns>項目名+_+背景色設定値</returns>
            private static string getColumnName(int? newId, int? oldId, string itemName)
            {
                // 申請区分
                int applicationDivision = 0;

                // 値の変更パターンに応じて申請区分を設定
                if (newId != null && oldId == null)
                {
                    // 値が追加された場合
                    applicationDivision = (int)TMQConst.MsStructure.StructureId.ApplicationDivision.New;
                }
                else if (newId == null && oldId != null)
                {
                    // 値が削除された場合
                    applicationDivision = (int)TMQConst.MsStructure.StructureId.ApplicationDivision.Delete;
                }
                else if (newId != oldId)
                {
                    // 値が変更された場合
                    applicationDivision = (int)TMQConst.MsStructure.StructureId.ApplicationDivision.Update;
                }
                else
                {
                    // 変更が無い場合
                    return string.Empty;
                }

                return itemName + "_" + applicationDivision.ToString() + "|";
            }

            /// <summary>
            /// 変更管理テーブルの申請状況更新処理
            /// </summary>
            /// <param name="condition">登録条件</param>
            /// <param name="registStatusCode">登録する申請状況の拡張アイテム(10：申請データ作成中、20：承認依頼中、30：差戻中、40：承認済)</param>
            /// <returns>エラーの場合はFalse</returns>
            public bool UpdateApplicationStatus(ComDao.HmHistoryManagementEntity condition, TMQConst.MsStructure.StructureId.ApplicationStatus registStatusCode)
            {
                // 申請状況IDを取得
                condition.ApplicationStatusId = getApplicationStatus(registStatusCode);

                // 登録情報を設定
                SetHistoryManagementInfo();

                // データクラスの中で値がNullでないものをSQLの条件に含めるので、メンバ名を取得
                List<string> listUnComment = ComUtil.GetNotNullNameByClass<ComDao.HmHistoryManagementEntity>(condition);

                // 工場IDはNullにならないので0以下の場合はコメントにするためリストから除外
                if(condition.FactoryId <= 0)
                {
                    listUnComment.Remove("FactoryId");
                }

                // 申請状況を更新
                if (!TMQUtil.SqlExecuteClass.Regist(Sql.UpdateApplicationStatus, Sql.SubDir, condition, this.Db, string.Empty, listUnComment))
                {
                    return false;
                }

                return true;

                // 登録情報を設定
                void SetHistoryManagementInfo()
                {
                    // 変更する申請状況に応じて値を設定
                    switch (registStatusCode)
                    {
                        case TMQConst.MsStructure.StructureId.ApplicationStatus.Request: // 承認依頼中
                            condition.ApplicationDate = this.Now; // 申請日
                            break;

                        case TMQConst.MsStructure.StructureId.ApplicationStatus.Approved: // 承認済み
                            condition.ApprovalUserId = this.UserId; // 承認者ID
                            condition.ApprovalDate = this.Now;      // 承認日
                            break;

                        default:
                            condition.ApprovalUserId = null;   // 承認者ID
                            break;
                    }

                    condition.UpdateDatetime = this.Now;  // 更新日
                    condition.UpdateUserId = this.UserId; // 更新者ID
                }
            }

            /// <summary>
            /// 変更管理テーブル新規登録処理
            /// </summary>
            /// <param name="applicationDivision">申請区分</param>
            /// <param name="keyId">申請データキーID</param>
            /// <param name="factoryId">申請データ工場ID</param>
            /// <param name="isNewData">新規・複写の場合はTrue、変更の場合はFalse</param>
            /// <returns>エラーの場合は(False, -1)</returns>

            public (bool returnFlag, long historyManagementId) InsertHistoryManagement(TMQConst.MsStructure.StructureId.ApplicationDivision applicationDivision, long keyId, int factoryId)
            {
                // 登録情報を作成
                ComDao.HmHistoryManagementEntity registInfo = new();

                // 申請状況IDを取得(申請データ作成中)
                registInfo.ApplicationStatusId = getApplicationStatus(TMQConst.MsStructure.StructureId.ApplicationStatus.Making);

                // 申請区分IDを取得
                registInfo.ApplicationDivisionId = getApplicationDivision(applicationDivision);

                registInfo.KeyId = keyId;                                    // 申請データキーID
                registInfo.FactoryId = factoryId;                            // 申請データ工場ID
                registInfo.ApplicationConductId = this.ApplicationConductId; // 申請機能ID
                registInfo.ApplicationUserId = this.UserId;                  // 申請者ID
                registInfo.InsertDatetime = this.Now;                        // 登録日時
                registInfo.InsertUserId = this.UserId;                       // 登録ユーザー
                registInfo.UpdateDatetime = this.Now;                        // 更新日時
                registInfo.UpdateUserId = this.UserId;                       // 更新ユーザー

                // SQL文の取得
                if (!TMQUtil.GetFixedSqlStatement(Sql.SubDir, Sql.InsertHistoryManagement, out string sql))
                {
                    return (false, -1);
                }

                long returnId = this.Db.RegistAndGetKeyValue<long>(sql, out bool isError, registInfo);
                return (!isError, returnId);
            }

            /// <summary>
            /// 変更管理テーブル更新処理(アンコメント項目指定)
            /// </summary>
            /// <param name="condition">更新条件</param>
            /// <param name="listUnComment">アンコメントする項目リスト</param>
            /// <returns></returns>
            public bool UpdateHistoryManagement(ComDao.HmHistoryManagementEntity condition, List<string> listUnComment)
            {
                // SQL実行
                return TMQUtil.SqlExecuteClass.Regist(Sql.UpdateApplicationStatus, Sql.SubDir, condition, this.Db, string.Empty, listUnComment);
            }

            /// <summary>
            /// 申請状況の拡張項目より、該当アイテムの構成IDを取得
            /// </summary>
            /// <param name="registStatusCode">申請状況の拡張項目</param>
            /// <returns>構成ID</returns>
            public int getApplicationStatus(TMQConst.MsStructure.StructureId.ApplicationStatus applicationStatus)
            {
                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = (int)TMQConst.MsStructure.GroupId.ApplicationStatus;
                //連番
                param.Seq = 1;
                // 拡張データ
                param.ExData = ((int)(TMQConst.MsStructure.StructureId.ApplicationStatus)Enum.ToObject(typeof(TMQConst.MsStructure.StructureId.ApplicationStatus), applicationStatus)).ToString();

                // 申請状況(構成ID)取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> applicationStatusList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.Db, TMQConst.CommonFactoryId, true);

                return applicationStatusList[0].StructureId;
            }


            /// <summary>
            /// 申請区分の拡張項目より、該当アイテムの構成IDを取得
            /// </summary>
            /// <param name="registStatusCode">申請状況の拡張項目</param>
            /// <returns>構成ID</returns>
            public int getApplicationDivision(TMQConst.MsStructure.StructureId.ApplicationDivision applicationDivision)
            {
                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = (int)TMQConst.MsStructure.GroupId.ApplicationDivision;
                //連番
                param.Seq = 1;
                // 拡張データ
                param.ExData = ((int)(TMQConst.MsStructure.StructureId.ApplicationDivision)Enum.ToObject(typeof(TMQConst.MsStructure.StructureId.ApplicationDivision), applicationDivision)).ToString(); ;

                // 申請区分(構成ID)取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> applicationDivisionList = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.Db);

                return applicationDivisionList[0].StructureId;
            }

            /// <summary>
            /// 変更管理テーブルの申請状況更新処理前の入力チェック
            /// </summary>
            /// <param name="condition">検索条件</param>
            /// <param name="isApproval">承認の場合はTrue、否認の場合はFalse</param>
            /// <param name="errMsg">エラーの場合のエラーメッセージ</param>
            /// <returns>エラーの場合はTrue</returns>
            public bool isErrorBeforeUpdateApplicationStatus(List<ComDao.HmHistoryManagementEntity> condition, bool isApproval, out string[] errMsg)
            {
                errMsg = null;

                // 変更管理データが「20：承認依頼中」かどうかを判定
                if (!isRequestDataByHistoryManagementId(condition, TMQConst.MsStructure.StructureId.ApplicationStatus.Request))
                {
                    // 承認依頼中でない変更管理が選択されています。
                    errMsg = new string[] { ComRes.ID.ID141190002, ComRes.ID.ID131120042, ComRes.ID.ID111290002 };
                    return true;
                }

                // 以下①、②を判定
                // ①ログインユーザーがシステム管理者かどうか
                // ②変更管理データに紐付く場所階層IDの拡張項目4(承認ユーザーID)がログインユーザかどうか
                if (!isSystemAdministrator() && !isCertifiedFactory(condition))
                {
                    // 選択された変更管理を(承認・否認)する権限がありません。
                    errMsg = new string[] { ComRes.ID.ID141140003, ComRes.ID.ID111290002, isApproval ? ComRes.ID.ID111120228 : ComRes.ID.ID111270036 };
                    return true;
                }

                return false;
            }

            /// <summary>
            /// 変更管理IDより、変更管理データが引数で指定された申請状況と一致するかどうか
            /// </summary>
            /// <param name="condition">検索条件</param>
            /// <param name="targetApplicationStatus">比較対象の申請状況</param>
            /// <returns>指定された申請状況と一致しない場合はFalse</returns>
            public bool isRequestDataByHistoryManagementId(List<ComDao.HmHistoryManagementEntity> condition, TMQConst.MsStructure.StructureId.ApplicationStatus targetApplicationStatus)
            {
                List<string> applicationStatus = getApplicationStatusByHistoryManagementIdList(condition);

                // 引数で指定された申請状況でない場合はエラー
                if (applicationStatus == null || applicationStatus.Exists(x => x != ((int)targetApplicationStatus).ToString()))
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 変更管理IDより、申請状況の拡張項目を取得する
            /// </summary>
            /// <param name="condition">検索条件</param>
            /// <returns>申請状況の拡張項目</returns>
            public string getApplicationStatusByHistoryManagementId(ComDao.HmHistoryManagementEntity condition)
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.SubDir, Sql.GetApplicationStatus, out string sql);

                // SQL実行
                TMQUtil.StructureItemEx.StructureItemExInfo statusInfo = this.Db.GetEntityByDataClass<TMQUtil.StructureItemEx.StructureItemExInfo>(sql, new { HistoryManagementIdList = condition.HistoryManagementId.ToString() });

                if (statusInfo == null || statusInfo.ExData == null)
                {
                    // 取得できない場合は空文字を返す
                    return string.Empty;
                }

                return statusInfo.ExData;
            }

            /// <summary>
            /// 変更管理IDより、申請状況の拡張項目を取得する(複数)
            /// </summary>
            /// <param name="condition">検索条件リスト</param>
            /// <returns>申請状況の拡張項目</returns>
            public List<string> getApplicationStatusByHistoryManagementIdList(List<ComDao.HmHistoryManagementEntity> condition)
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.SubDir, Sql.GetApplicationStatus, out string sql);

                //変更管理IDをカンマ区切り文字列で取得
                List<long> historyManagementIdList = condition.Select(x => x.HistoryManagementId).ToList();
                string ids = string.Join(',', historyManagementIdList);

                // SQL実行
                IList<TMQUtil.StructureItemEx.StructureItemExInfo> statusInfo = this.Db.GetList<TMQUtil.StructureItemEx.StructureItemExInfo>(sql, new { HistoryManagementIdList = ids });

                if (statusInfo == null || statusInfo.Count == 0)
                {
                    // 取得できない場合はnullを返す
                    return null;
                }

                return statusInfo.Select(x => x.ExData).ToList();
            }

            /// <summary>
            /// ログインユーザーがシステム管理者かどうか
            /// </summary>
            /// <param name="condition">検索条件</param>
            /// <returns>システム管理者でない場合はFalse</returns>
            public bool isSystemAdministrator()
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.SubDir, Sql.GetAuthLevel, out string sql);

                // SQL実行
                TMQUtil.StructureItemEx.StructureItemExInfo authInfo = this.Db.GetEntityByDataClass<TMQUtil.StructureItemEx.StructureItemExInfo>(sql, new { ApprovalUserId = this.UserId });

                if (authInfo == null || authInfo.ExData == null || authInfo.ExData != ((int)TMQConst.MsStructure.StructureId.AuthLevel.SystemAdministrator).ToString())
                {
                    // 取得できないまたはシステム管理者でない場合はエラー
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 変更管理IDより、場所階層の拡張項目4(工場の承認ユーザーID)がログインユーザーかどうか
            /// </summary>
            /// <param name="condition">検索条件</param>
            /// <returns>拡張項目がログインユーザーでない場合はFalse</returns>
            public bool isCertifiedFactory(List<ComDao.HmHistoryManagementEntity> condition)
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.SubDir, Sql.GetApprovalUserId, out string sql);

                //変更管理IDをカンマ区切り文字列で取得
                List<long> historyManagementIdList = condition.Select(x => x.HistoryManagementId).ToList();
                string ids = string.Join(',', historyManagementIdList);
                // SQL実行
                IList<TMQUtil.StructureItemEx.StructureItemExInfo> certifiedInfo = this.Db.GetList<TMQUtil.StructureItemEx.StructureItemExInfo>(sql, new { ApplicationConductId = this.ApplicationConductId, HistoryManagementIdList = ids });

                if (certifiedInfo == null || certifiedInfo.Count == 0 || certifiedInfo.ToList().Exists(x => x.ExData != this.UserId.ToString()))
                {
                    // 取得できないまたは工場の承認ユーザーでない場合はエラー
                    return false;
                }

                return true;
            }

            /// <summary>
            /// ボタン非表示制御フラグ取得(詳細画面のボタン非表示処理に使用)
            /// </summary>
            /// <typeparam name="T">共通の検索結果データクラス IHistoryManagementCommonを実装</typeparam>
            /// <param name="result">画面に設定する検索結果</param>
            /// <param name="historyManagementId">変更管理ID</param>
            /// <param name="processMode">処理モード</param>

            public void GetFlgHideButton<T>(ref T result, long? historyManagementId, TMQConst.MsStructure.StructureId.ProcessMode processMode)
                 where T : HistoryManagementDao.IHistoryManagementCommon, new()
            {
                // 変更管理IDに紐付くデータを取得
                ComDao.HmHistoryManagementEntity historyCondition = new();
                historyCondition = historyCondition.GetEntity(historyManagementId ?? -1, this.Db);

                // 申請の申請者IDがログインユーザまたはシステム管理者かどうか
                result.IsCertified = isCertified();

                // 変更管理IDが紐付く情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDかどうか
                result.IsCertifiedFactory = isCertifiedFactory();

                bool isCertified()
                {
                    bool isSystemAdministrator = this.isSystemAdministrator();

                    // トランザクションモードの場合はシステム管理者かどうかを返す
                    if (processMode == TMQConst.MsStructure.StructureId.ProcessMode.transaction)
                    {
                        return isSystemAdministrator;
                    }

                    // 申請の申請者IDがログインユーザまたはシステム管理者の場合True
                    if (historyCondition.ApplicationUserId == this.UserId || isSystemAdministrator)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                bool isCertifiedFactory()
                {
                    // 処理モードを判定
                    if (processMode == TMQConst.MsStructure.StructureId.ProcessMode.transaction)
                    {
                        return false;
                    }

                    // 変更管理IDが紐付く情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDかどうか
                    return this.isCertifiedFactory(new List<ComDao.HmHistoryManagementEntity>() { historyCondition });
                }
            }

            /// <summary>
            /// 変更管理IDより、変更管理テーブルのレコードを削除
            /// </summary>
            /// <param name="historyManagementId">変更管理ID</param>
            /// <returns>実行結果、変更管理詳細IDリスト</returns>
            public bool DeleteHistoryManagement(long historyManagementId)
            {
                // 変更管理テーブル 削除処理
                ComDao.HmHistoryManagementEntity historyManagement = new();
                if (!historyManagement.DeleteByPrimaryKey(historyManagementId, this.Db))
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 変更管理対象の工場を取得
            /// </summary>
            /// <returns>変更管理対象の工場のリスト</returns>
            public List<StructureItemEx.StructureItemExInfo> GetApprovalUserList()
            {
                // 拡張項目4の値がセットされた工場のリストを取得
                // 検索条件
                StructureItemEx.StructureItemExInfo param = new();
                param.StructureGroupId = (int)Const.MsStructure.GroupId.Location; // 場所階層
                param.Seq = 4; // 拡張4
                param.LanguageId = this.LanguageId;
                List<StructureItemEx.StructureItemExInfo> locationList = StructureItemEx.GetStructureItemExData(param, this.Db);
                if (locationList == null || locationList.Count == 0)
                {
                    // 無い場合終了
                    return locationList;
                }
                // 有る場合は階層番号が工場であり、拡張項目がNullでないものを絞り込み
                var factoryList = locationList.Where(x => x.StructureLayerNo == (int)TMQConst.MsStructure.StructureLayerNo.Location.Factory && !string.IsNullOrEmpty(x.ExData)).ToList();

                return factoryList;
            }

            /// <summary>
            /// 変更管理対象の工場IDを取得
            /// </summary>
            /// <returns>変更管理対象の工場IDのリスト</returns>
            public List<int> GetHistoryManagementFactoryIdList()
            {
                // 変更管理対象の工場リスト
                var historyManagementFactoryList = GetApprovalUserList();
                if (historyManagementFactoryList == null || historyManagementFactoryList.Count == 0)
                {
                    return new List<int>();
                }
                var factoryIdList = historyManagementFactoryList.Select(x => x.StructureId).Distinct().ToList();
                return factoryIdList;
            }

            /// <summary>
            /// ユーザが承認権限を持っている工場IDを取得
            /// </summary>
            /// <returns>ログインユーザが承認権限を持っている工場IDのリスト</returns>
            public List<int> GetUserApprovalFactoryList()
            {
                // 拡張項目4(承認者)が設定された工場の構成マスタのリストを取得
                List<StructureItemEx.StructureItemExInfo> factoryList = GetApprovalUserList();
                if (factoryList == null || factoryList.Count == 0)
                {
                    // 存在しない場合、0件のリストを返す
                    return new List<int>();
                }

                // システム管理者権限かどうか判定
                bool isSystemAdministrator = this.isSystemAdministrator();
                if (isSystemAdministrator)
                {
                    // システム管理者権限なら全ての承認者が設定された工場の構成ID
                    return factoryList.Select(x => x.StructureId).ToList();
                }

                // ログインユーザが承認者に設定されている工場を絞り込み
                var factoryIdList = factoryList.Where(x => x.ExData == this.UserId.ToString()).Select(x => x.StructureId).ToList();
                return factoryIdList;
            }
            /// <summary>
            /// ユーザが承認権限を持っているか判定
            /// </summary>
            /// <param name="factoryId">省略可能 工場IDを指定する場合は設定、無ければいずれかの工場</param>
            /// <returns>持っている場合はTrue</returns>
            public bool IsApprovalUser(int factoryId = -1)
            {
                // ユーザが承認権限を持っている工場のリストを取得
                var factoryIdList = GetUserApprovalFactoryList();
                if (factoryIdList == null | factoryIdList.Count == 0)
                {
                    // 無ければ権限なし
                    return false;
                }

                if (factoryId != -1)
                {
                    // 工場IDが指定されている場合、工場で絞り込み
                    return factoryIdList.Any(x => x == factoryId);
                }
                else
                {
                    // 指定されていない場合はリストの有無で判定
                    return factoryIdList.Count > 0;
                }
            }

            /// <summary>
            /// ユーザの所属工場に変更管理対象の工場が含まれているか判定
            /// </summary>
            /// <param name="isNoHistoryManagementFactory">out 変更管理を行わない工場が含まれている場合TRUE</param>
            /// <returns>含まれている場合はTrue</returns>
            public bool IsHistoryManagementFactoryUserBelong(out bool isNoHistoryManagementFactory)
            {
                // 変更管理を行わない工場が含まれている場合TRUE
                isNoHistoryManagementFactory = true;
                // 変更管理対象の工場リスト
                var historyManagementFactoryList = GetApprovalUserList();
                if (historyManagementFactoryList == null || historyManagementFactoryList.Count == 0)
                {
                    // 変更管理対象の工場リストがない場合、ユーザの変更管理対象の工場はなし
                    return false;
                    // 変更管理対象の工場がないなら、変更管理を行わない工場のみなので問題なし
                }

                // ユーザの権限がシステム管理者かどうか判定
                bool isSystemAdministrator = this.isSystemAdministrator();
                if (isSystemAdministrator)
                {
                    // 変更管理対象の工場があり、ユーザがシステム管理者なら、ユーザの所属工場に変更管理対象の工場があるとする
                    return true;
                    // システム管理者なら変更管理を行わない工場も持つから全工場が変更管理を行わない限り問題なし
                }

                // ユーザの所属権限リスト
                var userFactoryIdList = TMQUtil.GetUserBelongFactoryIdList(this.UserId, this.Db);
                if (userFactoryIdList == null || userFactoryIdList.Count == 0)
                {
                    // ユーザの所属権限がない場合、終了(発生しないはず)
                    return false;
                }
                // 重複を排除
                userFactoryIdList = userFactoryIdList.Distinct().ToList();
                // 変更管理対象の工場リストから工場IDのみを抽出
                var historyFactoryIdList = historyManagementFactoryList.Select(x => x.StructureId).Distinct().ToList();

                // 変更管理対象の工場が含まれるかどうかを判定するフラグ
                bool isExistsHistoryManagementFactory = false;
                // 変更管理対象外の工場が含まれるかどうかを判定するフラグを初期化
                isNoHistoryManagementFactory = false;
                // 両者のリストで一致する工場があればユーザの所属工場に変更管理対象の工場があるとする
                foreach (var userFactoryId in userFactoryIdList)
                {
                    if (historyFactoryIdList.Contains(userFactoryId))
                    {
                        // 変更管理対象の工場あり
                        isExistsHistoryManagementFactory= true;
                    }
                    else
                    {
                        // 変更管理対象外の工場あり
                        isNoHistoryManagementFactory = true;
                    }
                }
                // 上記で設定したフラグを返す
                return isExistsHistoryManagementFactory;
            }

            /// <summary>
            /// 工場IDより承認ユーザを取得
            /// </summary>
            /// <param name="factoryId">申請データ工場ID</param>
            /// <returns>承認者ID</returns>
            public int GetApprovalUser(int factoryId)
            {
                // 拡張項目4(承認者)が設定された工場の構成マスタのリストを取得
                List<StructureItemEx.StructureItemExInfo> factoryList = GetApprovalUserList();
                if (factoryList == null || factoryList.Count == 0)
                {
                    // 存在しない場合
                    return -1;
                }

                // 指定した工場に設定されている拡張項目4(承認者)を取得
                int userId = factoryList.Where(x => x.StructureId == factoryId).Select(x => int.Parse(x.ExData)).FirstOrDefault();
                return userId;
            }

            /// <summary>
            /// 指定された工場IDが変更管理対象の工場かどうか判定
            /// </summary>
            /// <param name="factoryId">判定する工場ID</param>
            /// <returns>変更管理対象の工場の場合TRUE</returns>
            public bool IsHistoryManagementFactory(int factoryId)
            {
                // 変更管理対象の工場リスト
                var historyManagementFactoryList = GetApprovalUserList();
                if (historyManagementFactoryList == null || historyManagementFactoryList.Count == 0)
                {
                    // 変更管理対象の工場リストがない場合、ユーザの変更管理対象の工場はなし
                    return false;
                }
                var factoryIdList = historyManagementFactoryList.Select(x => x.StructureId).Distinct().ToList();
                return factoryIdList.Contains(factoryId);
            }

            /// <summary>
            /// キーIDより、指定された申請状況のデータ件数を取得
            /// </summary>
            /// <param name="keyId">キーID(機番ID または 長計件名ID)</param>
            /// <param name="applicationStatus">申請状況</param>
            /// <param name="includeStatus">指定された申請状況の件数を取得する場合はTrue、指定された申請状況以外の件数を取得する場合はFalse</param>
            /// <returns></returns>
            public int getApplicationStatusCntByKeyId(long keyId, TMQConst.MsStructure.StructureId.ApplicationStatus applicationStatus, bool includeStatus)
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.SubDir, Sql.GetApplicationStatusCntByKeyId, out string sql, new List<string>() { includeStatus ? "Include" : "Except" });

                // 件数を取得 検索条件に以下を指定
                // ①キーID
                // ②申請状況
                // ③「指定された申請状況の件数を取得する」か「指定された申請状況以外の件数を取得するかどうか
                return this.Db.GetCount(sql, new { KeyId = keyId, ApplicationConductId = this.ApplicationConductId, AppliCationStatus = ((int)applicationStatus).ToString() });

            }

            /// <summary>
            /// 場所階層IDより、工場IDを取得する
            /// </summary>
            /// <param name="locationStructureId">場所階層ID</param>
            /// <returns></returns>
            public int getFactoryId(int locationStructureId)
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(Sql.SubDir, Sql.GetFactoryIdByLocationStructureId, out string sql);

                // SQL実行
                IList<ComDao.HmHistoryManagementEntity> results = this.Db.GetListByDataClass<ComDao.HmHistoryManagementEntity>(sql, new { LocationStructureId = locationStructureId });
                if (results == null || results.Count == 0)
                {
                    // 取得できない場合は引数の場所階層IDを返す
                    return locationStructureId;
                }

                // 取得した工場IDを返す
                return results[0].FactoryId;

            }

            /// <summary>
            /// 構成IDより、工場IDを取得する
            /// </summary>
            /// <param name="structureId">構成ID</param>
            /// <returns></returns>
            public int getFactoryIdByStructureId(int structureId)
            {
                // SQL実行
                var result =  STDDao.VStructureItemEntity.GetEntityById(structureId, this.Db);
                if (result == null)
                {
                    // 取得できない場合-1を返す
                    return -1;
                }

                // 取得した工場IDを返す
                return result.FactoryId.Value;

            }

            /// <summary>
            /// キーIDより申請状況が「承認済み」以外の変更管理IDを取得する
            /// </summary>
            /// <param name="keyId">キーID(機番ID または 長計件名ID)</param>
            /// <returns>変更管理ID(取得できない場合NULL)</returns>
            public long? getHistoryManagementIdByKeyId(long keyId)
            {
                return SqlExecuteClass.SelectEntity<long?>(Sql.GetHistoryManagementIdByKeyId, Sql.SubDir, new { KeyId = keyId, ApplicationConductId = this.ApplicationConductId}, this.Db);
            }

            /// <summary>
            /// トップ画面から遷移した場合、「自身の申請のみを表示」のチェック状態を遷移元リンクにしたがう
            /// </summary>
            /// <param name="searchConditionDictionary">ref 画面の検索条件</param>
            /// <param name="isFromTop">out トップ画面フラグ トップ画面から遷移していない場合はFalse</param>
            /// <param name="isDispOnlyMySubject">out チェック状態 トップ画面から遷移した場合、この値を使用する 申請件数からならTrue、承認件数からならFalse</param>
            public static void IsDispOnlyMySubjectFromTop(ref List<Dictionary<string, object>> searchConditionDictionary, out bool isFromTop, out bool isDispOnlyMySubject)
            {
                // グローバルリストのキー、1なら申請中件数、2なら承認待ち件数
                const string CM00001_GlobalKeyHistory = "CM00001_HistoryParam";
                isFromTop = false;
                isDispOnlyMySubject = false;

                var targetInfo = searchConditionDictionary.Where(x => x.ContainsKey(CM00001_GlobalKeyHistory));
                if (!targetInfo.Any())
                {
                    // 画面の検索条件にトップ画面から遷移時にセットされるキーが無い場合、終了
                    return;
                }

                // 以下、トップ画面から遷移した場合の処理
                isFromTop = true;
                var targetDic = targetInfo.First();
                // 値を取得
                string paramValue = targetDic[CM00001_GlobalKeyHistory].ToString();
                switch (paramValue)
                {
                    case "1":
                        // 申請件数より遷移の場合、チェックして検索
                        isDispOnlyMySubject = true;
                        break;
                    case "2":
                        // 承認件数より遷移の場合、チェックしないで検索
                        isDispOnlyMySubject = false;
                        break;
                    default:
                        // それ以外の場合、到達不能
                        // トップ画面から遷移していないこととする
                        isFromTop = false;
                        break;
                }
                // グローバルリストから削除
                searchConditionDictionary.Remove(targetDic);
            }
        }
    }
}
