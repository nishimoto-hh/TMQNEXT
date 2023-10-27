using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonWebTemplate.CommonDefinitions
{
    /// <summary>
    /// 共通処理INパラメータクラス
    /// </summary>
    public class CommonProcParamIn
    {
        /// <summary>端末IPアドレス</summary>
        public string TerminalNo { get; set; }
        /// <summary>機能ID</summary>
        public string ConductId { get; set; }
        /// <summary>プログラムID</summary>
        public string PgmId { get; set; }
        /// <summary>Form番号</summary>
        public short FormNo { get; set; }
        /// <summary>登録者ID</summary>
        public string UserId { get; set; }
        /// <summary>ログインID</summary>
        public string LoginId { get; set; }
        /// <summary>ユーザー権限レベルID</summary>
        public int AuthorityLevelId { get; set; }
        /// <summary>ユーザー所属情報</summary>
        public BelongingInfo BelongingInfo { get; set; }
        /// <summary>ユーザー本務工場ID</summary>
        public string FactoryId { get; set; }
        /// <summary>コントロールID</summary>
        public string CtrlId { get; set; }
        /// <summary>アクション区分</summary>
        public short ActionKbn { get; set; }
        /// <summary>画面遷移アクション区分</summary>
        public short TransActionDiv { get; set; }
        /// <summary>言語ID</summary>
        public string LanguageId { get; set; }
        /// <summary>ルートパス</summary>
        public string RootPath { get; set; }
        /// <summary>入力ファイル情報</summary>
        public Microsoft.AspNetCore.Http.IFormFile[] InputStream { get; set; }
        /// <summary>実行条件</summary>
        public List<Dictionary<string, object>> ConditionList { get; set; }
        /// <summary>ページ情報</summary>
        public List<PageInfo> PageInfoList { get; set; }
        /// <summary>実行結果</summary>
        public List<Dictionary<string, object>> ResultList { get; set; }
        /// <summary>プッシュ通知結果</summary>
        public string PushTarget { get; set; }
        /// <summary>グローバルリスト</summary>
        public Dictionary<string, object> Individual { get; set; }
        /// <summary>ボタン情報</summary>
        public List<Dictionary<string, object>> ButtonStatusList { get; set; }
        /// <summary>GUID</summary>
        public string GUID { get; set; }
        /// <summary>ブラウザタブ識別番号</summary>
        public string BrowserTabNo { get; set; }
        /// <summary>ステータス</summary>
        public int Status { get; set; }
        /// <summary>新規ログインかどうか</summary>
        public bool IsNewLogin { get; set; }

        /// <summary>場所階層構成IDリスト</summary>
        public List<int> ConditionSheetLocationList { get; set; } = null;
        /// <summary>職種機種構成IDリスト</summary>
        public List<int> ConditionSheetJobList { get; set; } = null;
        /// <summary>検索条件項目名リスト</summary>
        public List<string> ConditionSheetNameList { get; set; } = null;
        /// <summary>検索条件設定値リスト</summary>
        public List<string> ConditionSheetValueList { get; set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonProcParamIn()
        {
            this.TerminalNo = string.Empty;
            this.BelongingInfo = new BelongingInfo();
            this.FactoryId = string.Empty;
            this.ConductId = string.Empty;
            this.PgmId = string.Empty;
            this.FormNo = -1;
            this.UserId = string.Empty;
            this.LoginId = string.Empty;
            this.CtrlId = string.Empty;
            this.ActionKbn = short.MinValue;    // ※-1, -2はページング/ソートで使用するため、MinValueを初期値とする
            this.LanguageId = string.Empty;
            this.RootPath = string.Empty;
            this.InputStream = null;
            this.ConditionList = new List<Dictionary<string, object>>();
            this.PageInfoList = new List<PageInfo>();
            this.ResultList = new List<Dictionary<string, object>>();
            this.Individual = new Dictionary<string, object>();
            this.ButtonStatusList = new List<Dictionary<string, object>>();
            this.GUID = string.Empty;
            this.BrowserTabNo = string.Empty;
            this.Status = Models.Common.CommonProcReturn.ProcStatus.Valid;
        }
    }

    /// <summary>
    /// 共通処理OUTパラメータクラス
    /// </summary>
    public class CommonProcParamOut
    {
        /// <summary>ステータス</summary>
        public int Status { get; set; }
        /// <summary>メッセージコード</summary>
        public string MsgId { get; set; }
        /// <summary>ログ問合せ番号</summary>
        public string LogNo { get; set; }
        /// <summary>出力ファイル情報(Stream)</summary>
        public Stream OutputStream { get; set; }
        /// <summary>出力ファイル種類(1:Excel/2:CSV/3:PDF)</summary>
        public string FileType { get; set; }
        /// <summary>出力ファイル名</summary>
        public string FileName { get; set; }
        /// <summary>実行結果</summary>
        public List<Dictionary<string, object>> ResultList { get; set; }
        /// <summary>プッシュ通知結果(JSON文字列)</summary>
        public string JsonPushTarget { get; set; }
        /// <summary>グローバルリスト</summary>
        public Dictionary<string, object> Individual { get; set; }
        /// <summary>ボタン情報</summary>
        public List<Dictionary<string, object>> ButtonStatusList { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonProcParamOut()
        {
            this.Status = Models.Common.CommonProcReturn.ProcStatus.Valid;
            this.MsgId = string.Empty;
            this.LogNo = string.Empty;
            this.OutputStream = null;
            this.FileType = string.Empty;
            this.FileName = string.Empty;
            this.ResultList = new List<Dictionary<string, object>>();
            this.Individual = new Dictionary<string, object>();
            this.ButtonStatusList = new List<Dictionary<string, object>>();
        }
    }

    /// <summary>
    /// ページ情報
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PageInfo()
        {
            this.CtrlId = string.Empty;
            this.CtrlType = 101;
            this.PageNo = 1;
            this.MaxCnt = 0;
            this.PageSize = 0;
            this.SortValNo = 0;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="def">デフォルト値</param>
        public PageInfo(Dictionary<string, object> def) : this()
        {
            if (def.ContainsKey("CTRLID"))
            {
                this.CtrlId = def["CTRLID"].ToString();
            }
            if (def.ContainsKey("CTRLTYPE") && !CommonUtil.IsNullOrEmpty(def["CTRLTYPE"]))
            {
                this.CtrlType = Convert.ToInt32(def["CTRLTYPE"].ToString());
            }
            if (def.ContainsKey("VAL1") && !CommonUtil.IsNullOrEmpty(def["VAL1"]))
            {
                this.PageNo = Convert.ToInt32(def["VAL1"].ToString());
            }
            if (def.ContainsKey("VAL2") && !CommonUtil.IsNullOrEmpty(def["VAL2"]))
            {
                this.MaxCnt = Convert.ToInt32(def["VAL2"].ToString());
            }
            if (def.ContainsKey("VAL3") && !CommonUtil.IsNullOrEmpty(def["VAL3"]))
            {
                this.PageSize = Convert.ToInt32(def["VAL3"].ToString());
            }
            if (def.ContainsKey("VAL4") && !CommonUtil.IsNullOrEmpty(def["VAL4"]))
            {
                this.SortValNo = Convert.ToInt32(def["VAL4"].ToString());
            }
        }

        /// <summary>コントロールID</summary>
        public string CtrlId { get; set; }
        /// <summary>画面コントロール種類</summary>
        public int CtrlType { get; set; }
        /// <summary>ページ番号</summary>
        public int PageNo { get; set; }
        /// <summary>MAX件数</summary>
        public int MaxCnt { get; set; }
        /// <summary>1ページ当たりの件数</summary>
        public int PageSize { get; set; }
        /// <summary>ソート対象項目番号</summary>
        public int SortValNo { get; set; }
        /// <summary>ソート対象カラム名</summary>
        public string SortColName
        {
            get
            {
                return this.SortValNo > 0 ?
                    string.Format("VAL{0}", this.SortValNo) : string.Empty;
            }
        }
        /// <summary>オフセット</summary>
        public int Offset
        {
            get
            {
                return (PageSize > 0 && PageNo > 0) ? PageSize * (PageNo - 1) : 0;
            }
        }
        /// <summary>開始行番号(一時テーブル)</summary>
        public int StartRowNo
        {
            get
            {
                return (PageSize > 0 && PageNo > 0) ? (PageSize * (PageNo - 1) + 1) : 0;
            }
        }
        /// <summary>終了行番号(一時テーブル)</summary>
        public int EndRowNo
        {
            get
            {
                return (PageSize > 0 && PageNo > 0) ? (PageSize * PageNo) : 0;
            }
        }

        /// <summary>
        /// ページ情報リストの取得
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<PageInfo> GetPageInfoList(List<Dictionary<string, object>> list)
        {
            var pageInfoList = new List<PageInfo>();

            foreach (var item in list)
            {
                PageInfo info = new PageInfo(item);
                pageInfoList.Add(info);
            }

            return pageInfoList;
        }

        /// <summary>
        /// ページ情報の取得
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ctrlId"></param>
        /// <returns></returns>
        public static PageInfo GetPageInfo(List<Dictionary<string, object>> list, string ctrlId)
        {
            var item = list.Where(x =>
                ctrlId.Equals(x["CTRLID"].ToString())).FirstOrDefault();

            PageInfo info = new PageInfo(item);

            return info;
        }
    }

    /// <summary>
    /// 所属情報
    /// </summary>
    public class BelongingInfo
    {
        /// <summary>本務工場情報</summary>
        public StructureInfo DutyFactoryInfo { get; set; }

        /// <summary>本務工場ID</summary>
        public int DutyFactoryId { 
            get
            {
                if (this.DutyFactoryInfo != null)
                {
                    return this.DutyFactoryInfo.StructureId;
                }
                else
                {
                    return STRUCTURE_CONSTANTS.CommonFactoryId;
                }
            } 
        }

        /// <summary>場所階層情報リスト</summary>
        public List<StructureInfo> LocationInfoList { get; set; }

        /// <summary>職種情報リスト</summary>
        public List<StructureInfo> JobInfoList { get; set; }

        /// <summary>所属工場IDリスト</summary>
        public List<int> BelongingFactoryIdList { get; set; }

        /// <summary>所属職種IDリスト</summary>
        public List<int> BelongingJobIdList { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BelongingInfo()
        {
            this.LocationInfoList = new List<StructureInfo>();
            this.JobInfoList = new List<StructureInfo>();
            this.BelongingFactoryIdList = new List<int>();
            this.BelongingJobIdList = new List<int>();
        }

        /// <summary>
        /// 所属情報リストの設定
        /// </summary>
        /// <param name="list"></param>
        public void SetBelongingList(List<StructureInfo> list, bool isAdministrator = false)
        {
            if(list != null && list.Count > 0)
            {
                if (list.Count > 0)
                {
                    // 場所階層情報リスト
                    this.LocationInfoList = list.Where(x => x.GroupId == STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location).ToList();
                    // 職種情報リスト
                    this.JobInfoList = list.Where(x => x.GroupId == STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Job).ToList();

                    // 本務工場情報
                    this.DutyFactoryInfo = this.LocationInfoList.Where(x => x.LayerNo == STRUCTURE_CONSTANTS.LAYER_NO.Factory && x.DutyFlg).FirstOrDefault();

                    // 所属工場IDリスト
                    setBelongingFactoryIdList(isAdministrator);

                    // 所属職種IDリスト
                    this.BelongingJobIdList = this.JobInfoList.Select(x => x.StructureId).ToList();
                }
            }
        }

        /// <summary>
        /// 場所階層情報リストの設定
        /// </summary>
        /// <param name="list"></param>
        public void SetLocationInfoList(List<StructureInfo> list, bool isAdministrator)
        {
            this.LocationInfoList.Clear();
            if (list != null && list.Count > 0)
            {
                if (this.DutyFactoryInfo == null)
                {
                    // 本務工場が設定されていない場合
                    if (!isAdministrator)
                    {
                        // システム管理者以外の場合、先頭を本務工場に設定
                        this.DutyFactoryInfo = list[0];
                    }
                    this.LocationInfoList.AddRange(list);
                }
                else
                {
                    this.LocationInfoList.Add(this.DutyFactoryInfo);
                    this.LocationInfoList.AddRange(list.Where(x => x.StructureId != this.DutyFactoryInfo.StructureId));
                }
            }
            // 所属工場IDリスト
            setBelongingFactoryIdList(isAdministrator);
        }

        /// <summary>
        /// 職種情報リストの設定
        /// </summary>
        /// <param name="list"></param>
        public void SetJobInfoList(List<StructureInfo> list)
        {
            this.JobInfoList.Clear();
            this.BelongingJobIdList.Clear();

            if (list != null && list.Count > 0)
            {
                // 職種情報リスト
                this.JobInfoList.AddRange(list);
                // 所属職種IDリスト
                this.BelongingJobIdList = this.JobInfoList.Select(x => x.StructureId).ToList();
            }
        }

        /// <summary>
        /// 所属工場IDリストの設定
        /// </summary>
        private void setBelongingFactoryIdList(bool isAdministrator)
        {
            this.BelongingFactoryIdList.Clear();

            if (!isAdministrator && this.DutyFactoryInfo != null)
            {
                // 本務工場
                this.BelongingFactoryIdList.Add(this.DutyFactoryInfo.StructureId);
            }
            // 兼務工場
            //this.BelongingFactoryIdList.AddRange(this.LocationInfoList.Where(x => x.LayerNo == STRUCTURE_CONSTANTS.LAYER_NO.Factory && 
            //    (!x.DutyFlg || x.StructureId != this.DutyFactoryId))
            //    .Select(x => x.StructureId).ToList());
            this.BelongingFactoryIdList.AddRange(this.LocationInfoList.Where(x => (!x.DutyFlg || x.FactoryId != this.DutyFactoryId))
               .Select(x => x.FactoryId).Distinct().ToList());
        }
    }

    /// <summary>
    /// 構成マスタ
    /// </summary>
    public class StructureInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StructureInfo()
        {
        }

        /// <value>構成ID</value>
        public int StructureId { get; set; }

        /// <value>工場ID</value>
        public int FactoryId { get; set; }

        /// <value>構成グループID</value>
        public int GroupId { get; set; }

        /// <value>階層番号</value>
        public int LayerNo { get; set; }

        /// <value>本務フラグ</value>
        public bool DutyFlg { get; set; }
    }

}
