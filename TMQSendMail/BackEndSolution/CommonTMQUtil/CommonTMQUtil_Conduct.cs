using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using TMQDataClass = CommonTMQUtil.TMQCommonDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using ComDataBaseClass = CommonSTDUtil.CommonDataBaseClass;
using ExData = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId;

namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// 機能の固有の処理だが、機能間で共通定義として使用する場合などに処理を共用したい場合はこちらに作成
    /// </summary>
    public static partial class CommonTMQUtil
    {


        //public class PartsInventory
        //{
        //    /// <summary>
        //    /// 予備品在庫登録クラス
        //    /// </summary>
        //    public class PartsInventoryBase
        //    {
        //        /// <summary>DB接続</summary>
        //        protected ComDB Db { get; set; }
        //        /// <summary>ユーザID</summary>
        //        protected int UserId { get; set; }
        //        /// <summary>言語ID</summary>
        //        protected string LanguageId { get; set; }
        //        /// <summary>処理種類</summary>
        //        protected Type type { get; set; }

        //        /// <summary>
        //        /// コンストラクタ
        //        /// </summary>
        //        /// <param name="db">DB接続</param>
        //        /// <param name="userId">ログインユーザID</param>
        //        /// <param name="languageId">言語ID</param>
        //        protected PartsInventoryBase(ComDB db, string userId, string languageId)
        //        {
        //            this.Db = db;
        //            this.UserId = int.Parse(userId);
        //            this.LanguageId = languageId;
        //        }

        //        /// <summary>
        //        /// SQLファイル
        //        /// </summary>
        //        protected class Sql
        //        {
        //            /// <summary>SQLファイル格納フォルダ</summary>
        //            public const string SubDir = @"Common\PartsInventory";

        //            /// <summary>ロット情報登録SQL</summary>
        //            public const string InsertLot = "InsertLot";
        //            /// <summary>ロット情報更新SQL(部門移庫)</summary>
        //            public const string UpdateLot = "UpdateLot";
        //            /// <summary>ロット情報削除SQL</summary>
        //            public const string DeleteLot = "UpdateLotDelete";
        //            /// <summary>ロット情報更新SQL(入庫入力)</summary>
        //            public const string UpdateLotFromInput = "UpdateLotFromInput";

        //            /// <summary>在庫データ登録SQL</summary>
        //            public const string InsertStock = "InsertStock";
        //            /// <summary>在庫データ更新SQL</summary>
        //            public const string UpdateStock = "UpdateStock";
        //            /// <summary>在庫データ削除SQL</summary>
        //            public const string DeleteStock = "UpdateStockDelete";

        //            /// <summary>受払履歴登録SQL</summary>
        //            public const string InsertInoutHistory = "InsertInoutHistory";
        //            /// <summary>受払履歴登録SQL(ロット情報より指定)</summary>
        //            public const string InsertInoutHistoryByLotCtrlId = "InsertInoutHistoryByLotCtrlId";
        //            /// <summary>受払履歴削除SQL</summary>
        //            public const string DeleteInoutHistory = "UpdateInoutHistoryDelete";

        //            /// <summary>在庫データを取得(同じ棚の同じロット)</summary>
        //            public const string GetLocationStockByLocation = "GetLocationStockByLocation";
        //            /// <summary>在庫データを取得(同じロット)</summary>
        //            public const string GetLocationStockByLot = "GetLocationStockByLot";

        //            /// <summary>
        //            /// 棚卸のみで使用するSQL
        //            /// </summary>
        //            /// <remarks>他の機能では使用しないテーブルがあるので、分ける</remarks>
        //            public class TakeInventory
        //            {
        //                /// <summary>SQLファイル格納フォルダ</summary>
        //                public const string SubDir = @"Common\PartsInventory\TakeInventory";

        //                /// <summary>棚差調整データ取得</summary>
        //                public const string GetDifferenceByInventoryId = "GetDifferenceByInventoryId";
        //                /// <summary>ロット情報登録SQL</summary>
        //                public const string InsertLot = "InsertLot";
        //                /// <summary>在庫データ登録SQL</summary>
        //                public const string InsertStock = "InsertStock";
        //                /// <summary>受払履歴登録SQL</summary>
        //                public const string InsertInoutHistory = "InsertInoutHistory";
        //                /// <summary>棚差調整データ更新SQL</summary>
        //                public const string UpdateInventoryDifference = "UpdateInventoryDifference";
        //                /// <summary>棚卸データ更新SQL(確定日時)</summary>
        //                public const string UpdateInventoryForConfirmDatetime = "UpdateInventoryForConfirmDatetime";
        //                /// <summary>受払履歴更新SQL(確定)</summary>
        //                public const string UpdateHistoryForConfirm = "UpdateHistoryForConfirm";
        //                /// <summary>受払履歴更新SQL(確定解除)</summary>
        //                public const string UpdateHistoryForConfirmCancel = "UpdateHistoryForConfirmCancel";
        //                /// <summary>受払履歴削除SQL</summary>
        //                public const string DeleteInoutHistory = "DeleteInoutHistory";

        //                /// <summary>棚差調整データ更新SQL</summary>
        //                public const string UpdateInventoryDifferenceCancel = "UpdateInventoryDifferenceCancel";
        //            }
        //        }

        //        /// <summary>
        //        /// DB登録情報の設定
        //        /// </summary>
        //        /// <typeparam name="T">テーブル共通アイテムを持つクラス</typeparam>
        //        /// <param name="condition">設定する登録情報</param>
        //        protected void SetRegistInfo<T>(ref T condition)
        //            where T : ComDataBaseClass.CommonTableItem
        //        {
        //            DateTime now = DateTime.Now;

        //            condition.InsertDatetime = now;
        //            condition.InsertUserId = this.UserId;
        //            condition.UpdateSerialid = 0;
        //            condition.UpdateDatetime = now;
        //            condition.UpdateUserId = this.UserId;
        //        }

        //        /// <summary>
        //        /// 在庫データテーブルの在庫数取得処理
        //        /// </summary>
        //        /// <param name="inventoryCtrlId">在庫管理id、在庫データテーブルの主キー</param>
        //        /// <returns>在庫数の値</returns>
        //        protected decimal GetStockQuantity(long inventoryCtrlId)
        //        {
        //            var stock = new ComDao.PtLocationStockEntity().GetEntity(inventoryCtrlId, this.Db);
        //            return stock.StockQuantity;
        //        }

        //        /// <summary>
        //        /// 受払履歴の作業Noのシーケンスを進め、次の値を取得
        //        /// </summary>
        //        /// <returns>進めたシーケンスの値</returns>
        //        protected long GetNextWorkNo()
        //        {
        //            long workNo = CommonTMQUtil.GetNextSequence("seq_pt_inout_history_work_no", this.Db);
        //            return workNo;
        //        }

        //        /// <summary>
        //        /// 在庫データより現在の数量を取得し、入出庫数で増減した在庫数で更新する処理
        //        /// </summary>
        //        /// <typeparam name="T">登録するデータクラス ILocationStockCalcを実装</typeparam>
        //        /// <param name="cond">登録条件</param>
        //        /// <param name="isOut">出庫の場合True</param>
        //        protected void UpdateStock<T>(T cond, bool isOut) where T : Dao.PartsInventory.ILocationStockCalc
        //        {
        //            // 在庫データ更新
        //            var preUpdValue = GetStockQuantity(cond.InventoryControlId); // 現在の値
        //            int temp = isOut ? -1 : +1; // 出庫の場合引き算、入庫の場合足し算
        //            cond.StockQuantity = preUpdValue + (temp * cond.InoutQuantity); // 「更新後数量」を「現在の値」と「出庫数量」より計算
        //            // 更新
        //            TMQUtil.SqlExecuteClass.Regist(Sql.UpdateStock, Sql.SubDir, cond, this.Db);
        //        }

        //        /// <summary>
        //        /// 受払区分取得
        //        /// </summary>
        //        /// <returns>受払区分の受入データ</returns>
        //        protected List<StructureItemEx.StructureItemExInfo> GetInoutDivision()
        //        {
        //            // 検索条件
        //            StructureItemEx.StructureItemExInfo param = new();
        //            param.StructureGroupId = (int)Const.MsStructure.GroupId.InoutDivision; // 受払区分
        //            param.Seq = 1; // 連番1
        //            var results = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.Db);
        //            results = results.Where(x => !x.DeleteFlg).ToList(); // 削除されていないもの
        //            return results;
        //        }

        //        /// <summary>
        //        /// 指定された構成IDが受払区分の受入の構成IDかチェックする
        //        /// </summary>
        //        /// <param name="structureId">チェックする構成ID</param>
        //        /// <param name="inoutDivisions">取得した受払区分のリスト</param>
        //        /// <returns>受入ならTrue</returns>
        //        protected bool IsInOfInoutDivision(long structureId, List<StructureItemEx.StructureItemExInfo> inoutDivisions)
        //        {
        //            return IsCheckOfInoutDivision(structureId, inoutDivisions, true);
        //        }

        //        /// <summary>
        //        /// 指定された構成IDが受払区分の払出の構成IDかチェックする
        //        /// </summary>
        //        /// <param name="structureId">チェックする構成ID</param>
        //        /// <param name="inoutDivisions">取得した受払区分のリスト</param>
        //        /// <returns>払出ならTrue</returns>
        //        protected bool IsOutOfInoutDivision(long structureId, List<StructureItemEx.StructureItemExInfo> inoutDivisions)
        //        {
        //            return IsCheckOfInoutDivision(structureId, inoutDivisions, false);
        //        }

        //        /// <summary>
        //        /// 指定された構成IDが受払区分の受入or払出の構成IDかチェックする
        //        /// </summary>
        //        /// <param name="structureId">チェックする構成ID</param>
        //        /// <param name="inoutDivisions">取得した受払区分のリスト</param>
        //        /// <param name="isCheckIn">受入ならTrue、払出ならFalse</param>
        //        /// <returns>指定受払区分と合致すればTrue</returns>
        //        private bool IsCheckOfInoutDivision(long structureId, List<StructureItemEx.StructureItemExInfo> inoutDivisions, bool isCheckIn)
        //        {
        //            // 受入or払出
        //            ExData.InoutDivision checkValue = isCheckIn ? ExData.InoutDivision.In : ExData.InoutDivision.Out;
        //            // 拡張項目の値と比較する値
        //            string exDataValue = ((int)checkValue).ToString();
        //            // 指定した受払区分の構成IDを取得
        //            List<long> targetIds = inoutDivisions.Where(x => x.ExData == exDataValue).Select(x => (long)x.StructureId).ToList();
        //            // 指定した構成IDが含まれるか判定(=指定した構成IDが指定した受払区分である)
        //            bool result = targetIds.Contains(structureId);
        //            return result;
        //        }

        //        /// <summary>
        //        /// 作業Noから受払履歴を取得(削除されていないもののみ)
        //        /// </summary>
        //        /// <param name="workNo">作業No</param>
        //        /// <returns>取得した受払履歴</returns>
        //        protected IList<ComDao.PtInoutHistoryEntity> GetHisotryListByWorkNo(long workNo)
        //        {
        //            // 削除対象の受払履歴の取得
        //            var historyList = ComDao.PtInoutHistoryEntity.GetListByWorkNo(workNo, this.Db);
        //            // 作業Noを指定して受払履歴が0件だったり、削除済みだったりするのはあり得ない(指定誤り)のためエラー
        //            if (historyList == null || historyList.Count == 0)
        //            {
        //                throwError("'" + workNo + "' is incorrect value for WorkNo");
        //            }
        //            historyList = historyList.Where(x => x.DeleteFlg == false).ToList(); // 過去に削除されたかもしれないので未削除のみ
        //            if (historyList == null || historyList.Count == 0)
        //            {
        //                throwError("'" + workNo + "' is incorrect value for WorkNo");
        //            }
        //            return historyList;
        //        }

        //        /// <summary>
        //        /// 例外をスロー
        //        /// </summary>
        //        /// <param name="message">メッセージ</param>
        //        protected void throwError(string message)
        //        {
        //            throw new Exception("PartsInventory Error On " + this.type.ToString() + " " + message);
        //        }
        //        /// <summary>
        //        /// 在庫更新処理の種類
        //        /// </summary>
        //        public enum Type
        //        {
        //            Input, Output, MoveLocation, MoveDepartment, TakeInventory
        //        }
        //    }


        //    /// <summary>
        //    /// 入庫入力
        //    /// </summary>
        //    public class Input : PartsInventoryBase
        //    {
        //        public Input(ComDB db, string userId, string languageId) : base(db, userId, languageId)
        //        {
        //            this.type = Type.Input;
        //        }
        //        /// <summary>
        //        /// 新規登録時の処理
        //        /// </summary>
        //        /// <param name="condition">画面の入力内容</param>
        //        /// <param name="workNo">out 採番した受払履歴の作業No</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool New(Dao.PartsInventory.Input condition, out long workNo)
        //        {
        //            workNo = -1;
        //            // 登録情報設定
        //            SetRegistInfo(ref condition);
        //            // クラスの処理で初期値を設定
        //            condition.SetRegistInfo();
        //            // ロット情報登録
        //            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long lotControlId, Sql.InsertLot, Sql.SubDir, condition, this.Db);
        //            condition.LotControlId = lotControlId;
        //            // 在庫情報登録
        //            TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long inventoryControlId, Sql.InsertStock, Sql.SubDir, condition, this.Db);
        //            condition.InventoryControlId = inventoryControlId;
        //            // 受払履歴登録
        //            // シーケンスを採番して戻り値と登録条件に設定
        //            workNo = GetNextWorkNo();
        //            condition.WorkNo = workNo;
        //            TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistory, Sql.SubDir, condition, this.Db);

        //            return true;
        //        }
        //        /// <summary>
        //        /// 修正登録時の処理
        //        /// </summary>
        //        /// <param name="condition">画面の入力内容</param>
        //        /// <param name="workNo">画面の作業No(前回登録時の値)</param>
        //        /// <param name="newWorkNo">out 採番した受払履歴の作業No</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Update(Dao.PartsInventory.Input condition, long workNo, out long newWorkNo)
        //        {
        //            newWorkNo = -1;
        //            // 登録情報設定
        //            SetRegistInfo(ref condition);
        //            // クラスの処理で初期値を設定
        //            condition.SetRegistInfo();

        //            // 受払履歴の論理削除・登録
        //            // 作業Noによる受払履歴の取得
        //            if (isErrorInoutByWorkNo(workNo, out TMQDataClass.PtInoutHistoryEntity inout))
        //            {
        //                return false;
        //            }
        //            // 受払履歴の論理削除
        //            TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
        //            // 受払履歴の登録
        //            condition.LotControlId = inout.LotControlId;
        //            condition.InventoryControlId = inout.InventoryControlId;
        //            // 作業Noを採番して戻り値と登録条件に設定
        //            newWorkNo = GetNextWorkNo();
        //            condition.WorkNo = newWorkNo;
        //            // 登録
        //            TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistory, Sql.SubDir, condition, this.Db);

        //            // ロット情報更新
        //            updateLot(condition);
        //            // 在庫データ更新
        //            updateStock(condition);

        //            return true;


        //            // ロット情報更新
        //            void updateLot(Dao.PartsInventory.Input condition)
        //            {
        //                TMQDataClass.PtLotEntity lot = new();
        //                lot = lot.GetEntity(condition.LotControlId, this.Db);
        //                if (condition.IsChangedLotInfo(lot))
        //                {
        //                    // ロット情報に関わる情報が更新された場合、ロット情報を更新
        //                    TMQUtil.SqlExecuteClass.Regist(Sql.UpdateLotFromInput, Sql.SubDir, condition, this.Db);
        //                }
        //            }

        //            // 在庫データ更新
        //            void updateStock(Dao.PartsInventory.Input condition)
        //            {
        //                TMQDataClass.PtLocationStockEntity stock = new();
        //                stock = stock.GetEntity(condition.InventoryControlId, this.Db);
        //                if ((condition.PartsLocationId != stock.PartsLocationId)
        //                    || (condition.PartsLocationDetailNo != stock.PartsLocationDetailNo)
        //                    || (condition.InoutQuantity != stock.StockQuantity))
        //                {
        //                    // 棚ID・棚枝番・数量が更新された場合、在庫データを更新
        //                    TMQUtil.SqlExecuteClass.Regist(Sql.UpdateStock, Sql.SubDir, condition, this.Db, listUnComment: new List<string> { "IsInput" });
        //                }
        //            }
        //        }

        //        /// <summary>
        //        /// 受払履歴の取得、もし取得内容に問題があればエラー
        //        /// </summary>
        //        /// <param name="workNo">取得する作業No</param>
        //        /// <param name="inout">out 取得した受払履歴のレコード</param>
        //        /// <returns></returns>
        //        private bool isErrorInoutByWorkNo(long workNo, out TMQDataClass.PtInoutHistoryEntity inout)
        //        {
        //            inout = new();
        //            var inoutList = GetHisotryListByWorkNo(workNo);
        //            // 0件だったり1件より多い場合は無い
        //            if (inoutList == null || inoutList.Count == 0)
        //            {
        //                return true;
        //            }
        //            inoutList = inoutList.Where(x => !x.DeleteFlg).ToList();
        //            if (inoutList.Count > 1)
        //            {
        //                return true;
        //            }
        //            // 必ず1件
        //            inout = inoutList[0];
        //            return false;
        //        }

        //        /// <summary>
        //        /// 取消時の処理
        //        /// </summary>
        //        /// <param name="workNo">画面の作業No(前回登録時の値)</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Cancel(long workNo)
        //        {
        //            Dao.PartsInventory.Input condition = new();
        //            condition.WorkNo = workNo;
        //            // 登録情報設定
        //            SetRegistInfo(ref condition);
        //            // クラスの処理で初期値を設定
        //            condition.SetRegistInfo();

        //            // 作業Noによる受払履歴の取得
        //            if (isErrorInoutByWorkNo(workNo, out TMQDataClass.PtInoutHistoryEntity inout))
        //            {
        //                return false;
        //            }
        //            // 受払履歴の論理削除
        //            TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
        //            // キー値の取得
        //            condition.LotControlId = inout.LotControlId;
        //            condition.InventoryControlId = inout.InventoryControlId;
        //            // ロット情報の論理削除
        //            TMQUtil.SqlExecuteClass.Regist(Sql.DeleteLot, Sql.SubDir, condition, this.Db);
        //            // 在庫情報の論理削除
        //            TMQUtil.SqlExecuteClass.Regist(Sql.DeleteStock, Sql.SubDir, condition, this.Db);
        //            return true;
        //        }
        //    }

        //    /// <summary>
        //    /// 出庫入力
        //    /// </summary>
        //    public class Output : PartsInventoryBase
        //    {
        //        public Output(ComDB db, string userId, string languageId) : base(db, userId, languageId)
        //        {
        //            this.type = Type.Output;
        //        }
        //        /// <summary>
        //        /// 新規登録時の処理
        //        /// </summary>
        //        /// <param name="conditions">画面の入力内容(複数行)</param>
        //        /// <param name="workNo">out 採番した受払履歴の作業No</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool New(List<Dao.PartsInventory.Output> conditions, out long workNo)
        //        {
        //            // 作業Noを採番
        //            workNo = GetNextWorkNo();
        //            foreach (var condition in conditions)
        //            {
        //                Dao.PartsInventory.Output cond = condition; //ループの変数はrefが不可のため他の変数にセット
        //                SetRegistInfo(ref cond);
        //                cond.SetDivisions();
        //                cond.WorkNo = workNo;
        //                // 受払履歴登録
        //                TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistoryByLotCtrlId, Sql.SubDir, cond, this.Db);

        //                // 在庫データ更新
        //                UpdateStock(cond, true);
        //            }
        //            return true;
        //        }

        //        /// <summary>
        //        /// 取消時の処理
        //        /// </summary>
        //        /// <param name="workNo">画面の作業No(前回登録時の値)</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Cancel(long workNo)
        //        {
        //            // 更新条件
        //            Dao.PartsInventory.Output condition = new();
        //            condition.WorkNo = workNo;
        //            SetRegistInfo(ref condition);

        //            // 削除対象の受払履歴の取得
        //            var historyList = GetHisotryListByWorkNo(workNo);
        //            foreach (var history in historyList)
        //            {
        //                // 在庫データの更新
        //                condition.InventoryControlId = history.InventoryControlId;
        //                condition.InoutQuantity = history.InoutQuantity;
        //                // 在庫データ更新
        //                UpdateStock(condition, false);
        //            }
        //            // 受払履歴の論理削除
        //            TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
        //            return true;
        //        }
        //    }

        //    /// <summary>
        //    /// 棚番移庫
        //    /// </summary>
        //    public class MoveLocation : PartsInventoryBase
        //    {
        //        public MoveLocation(ComDB db, string userId, string languageId) : base(db, userId, languageId)
        //        {
        //            this.type = Type.MoveLocation;
        //        }
        //        /// <summary>
        //        /// 新規登録時の処理
        //        /// </summary>
        //        /// <param name="condition">画面の入力内容</param>
        //        /// <param name="workNo">out 採番した受払履歴の作業No</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool New(Dao.PartsInventory.MoveLocation condition, out long workNo)
        //        {
        //            // 作業Noを採番
        //            workNo = GetNextWorkNo();
        //            condition.WorkNo = workNo;
        //            // 登録情報設定
        //            SetRegistInfo(ref condition);

        //            // 移庫元在庫データ更新
        //            UpdateStock(condition, true);
        //            // 移庫元受払履歴登録
        //            registInoutHistory(condition, true);

        //            // 移庫先在庫データの有無を取得
        //            bool isExistsStock = isExistsStockForTarget(condition, out long targetInvCtrlId);
        //            // 移庫先在庫データ登録
        //            if (isExistsStock)
        //            {
        //                // 移庫先在庫データがある場合は更新
        //                condition.InventoryControlId = targetInvCtrlId;
        //                UpdateStock(condition, false);
        //            }
        //            else
        //            {
        //                // 移庫先在庫データが無い場合は新規登録
        //                // 移庫元の情報を検索し、移庫先の登録に必要な情報を取得する
        //                ComDao.PtLocationStockEntity stock = new ComDao.PtLocationStockEntity().GetEntity(condition.InventoryControlId, this.Db);
        //                condition.PartsId = stock.PartsId;
        //                // 登録
        //                TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out targetInvCtrlId, Sql.InsertStock, Sql.SubDir, condition, this.Db);
        //                condition.InventoryControlId = targetInvCtrlId; // 採番したIDを次の登録条件に設定
        //            }

        //            // 移庫先受払履歴登録
        //            registInoutHistory(condition, false);

        //            return true;

        //            // 移庫先在庫データ存在確認
        //            bool isExistsStockForTarget(Dao.PartsInventory.MoveLocation condition, out long targetInvCtrlId)
        //            {
        //                // 移庫先の在庫データを取得
        //                ComDao.PtLocationStockEntity stock = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.PtLocationStockEntity>(Sql.GetLocationStockByLocation, Sql.SubDir, condition, this.Db);
        //                // 在庫データの有無
        //                bool result = stock != null;
        //                // 在庫データのキー値、無い場合は使用しないため0
        //                targetInvCtrlId = result ? stock.InventoryControlId : 0;
        //                return result;
        //            }
        //            // 受払履歴登録
        //            void registInoutHistory(Dao.PartsInventory.MoveLocation condition, bool isSource)
        //            {
        //                // 受払区分と作業区分を設定
        //                condition.SetDivisions(isSource);
        //                // 登録
        //                TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistoryByLotCtrlId, Sql.SubDir, condition, this.Db);
        //            }
        //        }
        //        /// <summary>
        //        /// 取消時の処理
        //        /// </summary>
        //        /// <param name="workNo">画面の作業No(前回登録時の値)</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Cancel(long workNo)
        //        {
        //            // 更新条件
        //            Dao.PartsInventory.MoveLocation condition = new();
        //            condition.WorkNo = workNo;
        //            SetRegistInfo(ref condition);

        //            // 受払区分取得
        //            var inoutList = GetInoutDivision();

        //            // 削除対象の受払履歴の取得
        //            var historyList = GetHisotryListByWorkNo(workNo);
        //            foreach (var history in historyList)
        //            {
        //                // 在庫データの更新
        //                // 更新情報設定
        //                condition.InventoryControlId = history.InventoryControlId;
        //                condition.InoutQuantity = history.InoutQuantity;

        //                // 受払区分の判定
        //                bool isIn = IsInOfInoutDivision(history.InoutDivisionStructureId, inoutList); // 受入
        //                bool isOut = IsOutOfInoutDivision(history.InoutDivisionStructureId, inoutList); // 払出
        //                // もし両方ともTrueまたはFalseならエラー(データ不正)
        //                if (isIn == isOut) { throwError("'" + history.InoutDivisionStructureId + "' is incorrect value for InoutDivision."); }
        //                // 在庫データ更新(取消なので、払出の場合は払出の取消のため、受入)
        //                UpdateStock(condition, !isOut);
        //            }
        //            // 受払履歴の論理削除
        //            TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);
        //            return true;
        //        }
        //        /// <summary>
        //        /// 修正登録時の処理
        //        /// </summary>
        //        /// <param name="condition">画面の入力内容</param>
        //        /// <param name="workNo">画面の作業No(前回登録時の値)</param>
        //        /// <param name="newWorkNo">out 採番した受払履歴の作業No</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Update(Dao.PartsInventory.MoveLocation condition, long workNo, out long newWorkNo)
        //        {
        //            // 取消→登録
        //            this.Cancel(workNo);
        //            this.New(condition, out newWorkNo);
        //            return true;
        //        }
        //    }

        //    /// <summary>
        //    /// 部門移庫
        //    /// </summary>
        //    public class MoveDepartment : PartsInventoryBase
        //    {
        //        public MoveDepartment(ComDB db, string userId, string languageId) : base(db, userId, languageId)
        //        {
        //            this.type = Type.MoveDepartment;
        //        }
        //        /// <summary>
        //        /// 新規登録時の処理
        //        /// </summary>
        //        /// <param name="condition">画面の入力内容</param>
        //        /// <param name="workNo">out 採番した受払履歴の作業No</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool New(Dao.PartsInventory.MoveDepartment condition, out long workNo)
        //        {
        //            workNo = GetNextWorkNo();
        //            condition.WorkNo = workNo;
        //            SetRegistInfo(ref condition);

        //            // ロット管理IDから同じ予備品、ロットの在庫データを移庫元として取得
        //            List<ComDao.PtLocationStockEntity> sourceStockList = TMQUtil.SqlExecuteClass.SelectList<ComDao.PtLocationStockEntity>(Sql.GetLocationStockByLot, Sql.SubDir, condition, this.Db);
        //            // 受払履歴を登録
        //            foreach (var sourceStock in sourceStockList)
        //            {
        //                // 移庫元
        //                condition.SetDivisions(true);
        //                condition.InventoryControlId = sourceStock.InventoryControlId; // 在庫管理ID
        //                condition.InoutQuantity = sourceStock.StockQuantity; // 入出庫数は在庫数と同じ
        //                // 登録(移庫元なのでロット管理IDより情報を取得)
        //                TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistoryByLotCtrlId, Sql.SubDir, condition, this.Db);

        //                // 移庫先
        //                condition.SetDivisions(false);
        //                // 登録(移庫先なので指定された情報を登録)
        //                TMQUtil.SqlExecuteClass.Regist(Sql.InsertInoutHistory, Sql.SubDir, condition, this.Db);
        //            }

        //            // ロット管理IDでロット情報を更新
        //            TMQUtil.SqlExecuteClass.Regist(Sql.UpdateLot, Sql.SubDir, condition, this.Db);

        //            return true;
        //        }
        //        /// <summary>
        //        /// 取消時の処理
        //        /// </summary>
        //        /// <param name="workNo">画面の作業No(前回登録時の値)</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Cancel(long workNo)
        //        {
        //            // 更新条件
        //            Dao.PartsInventory.MoveDepartment condition = new();
        //            condition.WorkNo = workNo;
        //            SetRegistInfo(ref condition);
        //            // 受払区分取得
        //            var inoutList = GetInoutDivision();

        //            // WorkNoで受払履歴を取得
        //            // 削除対象の受払履歴の取得
        //            var historyList = GetHisotryListByWorkNo(workNo);
        //            // 払出の先頭行の情報で、ロット情報を更新
        //            foreach (var history in historyList)
        //            {
        //                // 受払区分の判定
        //                bool isOut = IsOutOfInoutDivision(history.InoutDivisionStructureId, inoutList); // 払出
        //                // 払出(移庫元)の情報でロット情報を更新
        //                if (isOut)
        //                {
        //                    // ロット管理IDでロット情報を更新
        //                    // 部門、勘定科目、管理区分、管理No、管理No、入庫日
        //                    condition.DepartmentStructureId = history.DepartmentStructureId;
        //                    condition.AccountStructureId = history.AccountStructureId;
        //                    condition.ManagementDivision = history.ManagementDivision;
        //                    condition.ManagementNo = history.ManagementNo;
        //                    condition.InoutDatetime = history.InoutDatetime;
        //                    condition.UnitPrice = 0; // 移庫元が修理部門の場合は0、そうでない場合は元々の値で更新(=更新しない
        //                    // キー
        //                    condition.LotControlId = history.LotControlId;
        //                    TMQUtil.SqlExecuteClass.Regist(Sql.UpdateLot, Sql.SubDir, condition, this.Db);
        //                    break;
        //                }
        //                continue;
        //            }

        //            // 受払履歴の論理削除
        //            TMQUtil.SqlExecuteClass.Regist(Sql.DeleteInoutHistory, Sql.SubDir, condition, this.Db);

        //            return true;
        //        }
        //        /// <summary>
        //        /// 修正登録時の処理
        //        /// </summary>
        //        /// <param name="condition">画面の入力内容</param>
        //        /// <param name="workNo">画面の作業No(前回登録時の値)</param>
        //        /// <param name="newWorkNo">out 採番した受払履歴の作業No</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Update(Dao.PartsInventory.MoveDepartment condition, long workNo, out long newWorkNo)
        //        {
        //            // 取消→登録
        //            this.Cancel(workNo);
        //            this.New(condition, out newWorkNo);
        //            return true;
        //        }
        //    }

        //    /// <summary>
        //    /// 入庫入力、出庫入力、移庫入力共通チェック処理
        //    /// </summary>
        //    public class InventryGetInfo
        //    {
        //        /// <summary>Gets or sets データ件数</summary>
        //        /// <value>データ件数</value>
        //        public int Count { get; set; }
        //        /// <summary>
        //        /// SQLファイル名称
        //        /// </summary>
        //        public static class SqlName
        //        {
        //            /// <summary>SQL名：構成リスト取得</summary>
        //            public const string InventryGetCount = "Select_InventryGetCount";
        //            /// <summary>SQL格納先サブディレクトリ名</summary>
        //            public const string SubDir = "Common";

        //            /// <summary>
        //            /// 予備品ID(@PartsId)、受払履歴の更新日時(@UpdateDatetime)、受払履歴の受払日時(@InoutDatetime)より棚卸中のデータがあるかどうかチェックする処理
        //            /// </summary>
        //            /// <param name="param">検索条件：予備品ID</param>
        //            /// <param name="db">DB接続</param>
        //            /// <returns>データがあればTrue</returns>
        //            public static bool IsExistsInventryData(object condition, ComDB db)
        //            {
        //                // 移庫先の在庫データを取得
        //                InventryGetInfo info = TMQUtil.SqlExecuteClass.SelectEntity<InventryGetInfo>(SqlName.InventryGetCount, SqlName.SubDir, condition, db);
        //                if (info.Count > 0)
        //                {
        //                    return true; // データが1件以上あればTrue
        //                }
        //                return false;
        //            }
        //        }
        //    }

        //    /// <summary>
        //    /// 棚卸
        //    /// </summary>
        //    public class TakeInventory : PartsInventoryBase
        //    {
        //        public TakeInventory(ComDB db, string userId, string languageId) : base(db, userId, languageId)
        //        {
        //            this.type = Type.TakeInventory;
        //        }
        //        /// <summary>
        //        /// 棚卸確定
        //        /// </summary>
        //        /// <param name="inventoryId">棚卸ID</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool Confirm(long inventoryId)
        //        {
        //            // 棚卸IDから棚卸データを取得
        //            ComDao.PtInventoryEntity inventory = getInventoryInfo(inventoryId);
        //            // 登録に使用するデータクラス
        //            Dao.PartsInventory.TakeInventory cond = getRegistCondition(inventory);
        //            // 受払区分を取得
        //            var inoutList = GetInoutDivision();
        //            // 棚差調整データを取得
        //            IList<ComDao.PtInventoryDifferenceEntity> invDifList = getInventoryDifferenceList(inventory);
        //            if (invDifList != null)
        //            {
        //                // 棚差調整データで繰り返し、登録を行う
        //                foreach (var invDif in invDifList)
        //                {
        //                    // 棚差調整データの値を設定
        //                    cond.InventoryDifferenceId = invDif.InventoryDifferenceId;
        //                    // 受入かどうかを取得
        //                    bool isIn = IsInOfInoutDivision(invDif.InoutDivisionStructureId ?? -1, inoutList);

        //                    // ロット情報登録
        //                    cond.LotControlId = registLotAndGetKey(cond, invDif.LotControlId);
        //                    // 在庫データ登録
        //                    cond.InventoryControlId = registStockAndGetKey(cond, invDif.InventoryControlId, invDif.InoutQuantity, isIn);

        //                    // 受払履歴を登録
        //                    TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out long inoutHistoryId, Sql.TakeInventory.InsertInoutHistory, Sql.TakeInventory.SubDir, cond, this.Db);

        //                    // 棚差調整データに採番したキー値を設定
        //                    cond.InoutHistoryId = inoutHistoryId;
        //                    // 受入の場合、ロット管理IDと在庫管理IDも更新
        //                    List<string> listUnComment = new();
        //                    if (isIn) { listUnComment.Add("IsInput"); }
        //                    TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryDifference, Sql.TakeInventory.SubDir, cond, this.Db, listUnComment: listUnComment);
        //                }
        //            }
        //            // 棚卸データを更新
        //            TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryForConfirmDatetime, Sql.TakeInventory.SubDir, cond, this.Db);
        //            //// 受払履歴を更新
        //            //TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateHistoryForConfirm, Sql.TakeInventory.SubDir, cond, this.Db);

        //            return true;

        //            // 登録用データクラス取得
        //            Dao.PartsInventory.TakeInventory getRegistCondition(ComDao.PtInventoryEntity inventory)
        //            {
        //                Dao.PartsInventory.TakeInventory cond = new();
        //                SetRegistInfo(ref cond); // 更新日時などを設定
        //                cond.InventoryId = inventory.InventoryId;
        //                // 対象年月の末日を取得
        //                cond.InoutDatetime = getMaxTimeOfDate(inventory.TargetMonth);
        //                // 作業Noを採番
        //                cond.WorkNo = GetNextWorkNo();
        //                return cond;

        //                // 指定年月の最終日の23:59:59を取得する処理
        //                DateTime getMaxTimeOfDate(DateTime target)
        //                {
        //                    var lastday = ComUtil.GetDateMonthLastDay(target);
        //                    return new DateTime(lastday.Year, lastday.Month, lastday.Day, 23, 59, 59);
        //                }
        //            }

        //            // ロット情報の登録が必要なら行い、ロット管理IDを取得する
        //            // cond 登録情報
        //            // lotCtrlId 棚差と調整データのロット管理ID
        //            // return ロット管理ID(登録時は採番、そうでない場合は引数の値)
        //            long registLotAndGetKey(Dao.PartsInventory.TakeInventory cond, long? lotCtrlId)
        //            {
        //                long returnKey;
        //                if (lotCtrlId == null)
        //                {
        //                    // 登録
        //                    TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out returnKey, Sql.TakeInventory.InsertLot, Sql.TakeInventory.SubDir, cond, this.Db);
        //                }
        //                else
        //                {
        //                    // 登録不要
        //                    returnKey = lotCtrlId ?? -1;
        //                }
        //                return returnKey;
        //            }

        //            // 在庫データの登録or更新を行う処理
        //            // cond 登録情報
        //            // invCtrlId 在庫管理ID
        //            // inoutQty 棚差調整データの受払数
        //            // isIn 棚差調整データで受入の場合True
        //            // return 在庫管理ID(新規は採番、更新は引数の値)
        //            long registStockAndGetKey(Dao.PartsInventory.TakeInventory cond, long? invCtrlId, decimal? inoutQty, bool isIn)
        //            {
        //                long returnKey;
        //                if (invCtrlId == null)
        //                {
        //                    // 登録
        //                    TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out returnKey, Sql.TakeInventory.InsertStock, Sql.TakeInventory.SubDir, cond, this.Db);
        //                }
        //                else
        //                {
        //                    // 更新
        //                    returnKey = invCtrlId ?? -1;
        //                    cond.InventoryControlId = returnKey;
        //                    cond.InoutQuantity = inoutQty ?? 0;
        //                    // 在庫更新
        //                    UpdateStock(cond, !isIn);
        //                }
        //                return returnKey;
        //            }
        //        }

        //        /// <summary>
        //        /// 棚卸IDより棚卸データを取得し、取得できなかった場合エラーとする
        //        /// </summary>
        //        /// <param name="inventoryId"></param>
        //        /// <returns></returns>
        //        private ComDao.PtInventoryEntity getInventoryInfo(long inventoryId)
        //        {
        //            ComDao.PtInventoryEntity inventory = new ComDao.PtInventoryEntity().GetEntity(inventoryId, this.Db);
        //            // 取得できなかった場合エラー
        //            if (inventory == null)
        //            {
        //                throwError("'" + inventoryId + "' is incorrect value for InventoryId");
        //            }
        //            return inventory;
        //        }

        //        /// <summary>
        //        /// 棚卸IDより棚差調整データを取得
        //        /// </summary>
        //        /// <param name="inventory"></param>
        //        /// <returns></returns>
        //        private IList<ComDao.PtInventoryDifferenceEntity> getInventoryDifferenceList(ComDao.PtInventoryEntity inventory)
        //        {
        //            // 棚差調整データを取得
        //            IList<ComDao.PtInventoryDifferenceEntity> invDifList = TMQUtil.SqlExecuteClass.SelectList<ComDao.PtInventoryDifferenceEntity>(Sql.TakeInventory.GetDifferenceByInventoryId, Sql.TakeInventory.SubDir, inventory, this.Db);
        //            return invDifList;
        //        }

        //        /// <summary>
        //        /// 棚卸確定解除
        //        /// </summary>
        //        /// <param name="inventoryId">棚卸ID</param>
        //        /// <returns>エラーの場合False、ロールバックしてください</returns>
        //        public bool ConfirmCancel(long inventoryId)
        //        {
        //            ComDao.PtInventoryEntity inventory = getInventoryInfo(inventoryId);
        //            Dao.PartsInventory.TakeInventory cond = getRegistCondition(inventory);
        //            //// 受払履歴を更新
        //            //TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateHistoryForConfirmCancel, Sql.TakeInventory.SubDir, cond, this.Db);
        //            // 棚卸データを更新(受払履歴を更新時に、棚卸データの棚卸確定日時を使用するため、受払履歴更新後に行う)
        //            TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryForConfirmDatetime, Sql.TakeInventory.SubDir, cond, this.Db);

        //            // 受払区分を取得
        //            var inoutList = GetInoutDivision();

        //            // 棚差調整データを取得
        //            IList<ComDao.PtInventoryDifferenceEntity> invDifList = getInventoryDifferenceList(inventory);
        //            if (invDifList == null)
        //            {
        //                return true;
        //            }
        //            // 棚差調整データで繰り返し、登録を行う
        //            foreach (var invDif in invDifList)
        //            {
        //                // 棚差調整データの値を設定
        //                cond.InventoryDifferenceId = invDif.InventoryDifferenceId;
        //                // 受入かどうかを取得
        //                bool isIn = IsInOfInoutDivision(invDif.InoutDivisionStructureId ?? -1, inoutList);
        //                // 受払履歴
        //                cond.WorkNo = invDif.WorkNo ?? -1;
        //                TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.DeleteInoutHistory, Sql.TakeInventory.SubDir, cond, this.Db);
        //                if (isIn)
        //                {
        //                    //受入の場合、ロット情報と在庫データは物理削除
        //                    // ロット情報
        //                    new ComDao.PtLotEntity().DeleteByPrimaryKey(invDif.LotControlId ?? -1, this.Db);
        //                    // 在庫データ
        //                    new ComDao.PtLocationStockEntity().DeleteByPrimaryKey(invDif.InventoryControlId ?? -1, this.Db);
        //                }
        //                else
        //                {
        //                    // 在庫データ
        //                    cond.InventoryControlId = invDif.InventoryControlId ?? -1;
        //                    cond.InoutQuantity = invDif.InoutQuantity ?? 0;
        //                    UpdateStock(cond, isIn);
        //                }

        //                // 棚差調整データ
        //                // 受入の場合、ロット管理IDと在庫管理IDも更新
        //                List<string> listUnComment = new();
        //                if (isIn) { listUnComment.Add("IsInput"); }
        //                TMQUtil.SqlExecuteClass.Regist(Sql.TakeInventory.UpdateInventoryDifferenceCancel, Sql.TakeInventory.SubDir, cond, this.Db, listUnComment: listUnComment);
        //            }
        //            return true;

        //            // 登録用データクラス取得
        //            Dao.PartsInventory.TakeInventory getRegistCondition(ComDao.PtInventoryEntity inventory)
        //            {
        //                Dao.PartsInventory.TakeInventory cond = new();
        //                SetRegistInfo(ref cond); // 更新日時などを設定
        //                cond.InventoryId = inventory.InventoryId;
        //                // 日時はNullで更新
        //                cond.InoutDatetime = null;
        //                return cond;
        //            }
        //        }
        //    }
        //}

    }
}
