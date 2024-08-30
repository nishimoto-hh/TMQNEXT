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
using Dao = BusinessLogic_LN0001.BusinessLogicDataClass_LN0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_LN0001
{
    /// <summary>
    /// 件名別長期計画(詳細編集画面)
    /// </summary>
    public partial class BusinessLogic_LN0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 参照画面の表示種類
        /// </summary>
        private enum EditDispType
        {
            /// <summary>新規</summary>
            New,
            /// <summary>修正</summary>
            Update,
            /// <summary>複写</summary>
            Copy,
            /// <summary>登録後再表示</summary>
            Redisplay,
            /// <summary>未設定</summary>
            None = -1
        }

        /// <summary>
        /// 詳細編集画面初期化処理
        /// </summary>
        /// <param name="type">詳細編集画面の起動種類</param>
        /// <returns>エラーの場合False</returns>
        private bool initEdit(EditDispType type)
        {
            // グループより対象のコントロールIDを取得
            List<string> toCtrlIdList = getResultMappingInfoByGrpNo(ConductInfo.FormEdit.HeaderGroupNo).CtrlIdList;

            if (type == EditDispType.New)
            {
                // 新規の場合
                Dao.ListSearchResult param = new();
                param.LocationStructureId = getTreeValue(true);
                param.JobStructureId = getTreeValue(false);
                // 取得した結果に対して、地区と職種の情報を設定する
                IList<Dao.ListSearchResult> paramList = new List<Dao.ListSearchResult> { param };
                TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.ListSearchResult>(ref paramList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

                initFormByParam(param, toCtrlIdList);
                // ツリーの階層IDの値が単一の場合その値を返す処理
                int? getTreeValue(bool isLocation)
                {
                    var list = isLocation ? GetLocationTreeValues() : GetJobTreeValues();
                    if (list != null && list.Count == 1)
                    {
                        // 値が単一でもその下に紐づく階層が複数ある場合は初期表示しないので判定
                        bool result = TMQUtil.GetButtomValueFromTree(list[0], this.db, this.LanguageId, out int buttomId);
                        return result ? buttomId : null;
                    }
                    return null;
                }
            }
            else
            {
                // 更新の場合
                // 再表示かどうか
                bool isReSearch = type == EditDispType.Redisplay;
                // キー情報取得元のコントロールID、再表示でない場合、参照画面の非表示項目。再表示の場合はこの画面の非表示項目
                string ctrlId = !isReSearch ? ConductInfo.FormDetail.ControlId.Hide : ConductInfo.FormEdit.ControlId.Hide;
                // キー情報取得
                var param = getParam(ctrlId, isReSearch);
                // 初期化処理呼出
                // 参照画面の非表示項目より取得した情報で参照画面の項目に値を設定する
                // ツリー表示
                initFormByLongPlanId(param, toCtrlIdList, out bool isMaintainanceKindFactory, out int factoryId, true);
                // ★画面定義の翻訳情報取得★
                GetContorlDefineTransData(factoryId);
            }

            return true;
        }

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit(bool isInsert)
        {
            // 排他チェック(更新のみ)
            if (!isInsert && !checkExclusiveSingle(ConductInfo.FormEdit.ControlId.Hide))
            {
                return false;
            }

            // 画面情報取得
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            DateTime now = DateTime.Now;
            ComDao.LnLongPlanEntity registInfo = GetRegistInfoByGroupNo<ComDao.LnLongPlanEntity>(ConductInfo.FormEdit.HeaderGroupNo, now);
            // 職種の階層情報を取得するために、階層を持つクラスに画面の内容を反映し、取得する
            IList<Dao.ListSearchResult> registStructureInfo = new List<Dao.ListSearchResult> { GetRegistInfoByGroupNo<Dao.ListSearchResult>(ConductInfo.FormEdit.HeaderGroupNo, now) };
            // 階層情報を設定
            TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.ListSearchResult>(ref registStructureInfo, new List<StructureType> { StructureType.Location, StructureType.Job });
            // 職種と地区の値を登録するデータクラスに設定
            setLayerInfo(ref registInfo, registStructureInfo[0]);
            // 登録
            if (!registDb(registInfo, out long newLongPlanId))
            {
                return false;
            }

            // 再検索
            if (!isInsert)
            {
                // 更新時、再検索処理
                this.selectedLongPlanIdList.Add(registInfo.LongPlanId);
                return initEdit(EditDispType.Redisplay);
            }
            else
            {
                // 登録時
                this.selectedLongPlanIdList.Add(newLongPlanId);
            }

            // INSERTの場合の再検索処理
            var param = new ComDao.LnLongPlanEntity();
            param.LongPlanId = newLongPlanId;
            List<string> toCtrlIdList = getResultMappingInfoByGrpNo(ConductInfo.FormEdit.HeaderGroupNo).CtrlIdList;
            initFormByLongPlanId(param, toCtrlIdList, out bool isMaintainanceKindFactory, out int factoryId, true);
            return true;

            // 画面のツリーの階層情報を登録用データクラスにセット
            void setLayerInfo(ref ComDao.LnLongPlanEntity target, Dao.ListSearchResult source)
            {
                // 場所階層
                target.LocationStructureId = source.LocationStructureId;
                // 各階層のIDは名称のプロパティに文字列として格納される（ツリーの定義の関係）ため、数値に変換
                target.LocationDistrictStructureId = ComUtil.ConvertStringToInt(source.DistrictName);
                target.LocationFactoryStructureId = ComUtil.ConvertStringToInt(source.FactoryName);
                target.LocationPlantStructureId = ComUtil.ConvertStringToInt(source.PlantName);
                target.LocationSeriesStructureId = ComUtil.ConvertStringToInt(source.SeriesName);
                target.LocationStrokeStructureId = ComUtil.ConvertStringToInt(source.StrokeName);
                target.LocationFacilityStructureId = ComUtil.ConvertStringToInt(source.FacilityName);
                // 職種機種階層
                target.JobStructureId = source.JobStructureId;
                // 各階層のIDは名称のプロパティに文字列として格納される（ツリーの定義の関係）ため、数値に変換
                target.JobKindStructureId = ComUtil.ConvertStringToInt(source.JobName);
                target.JobLargeClassficationStructureId = ComUtil.ConvertStringToInt(source.LargeClassficationName);
                target.JobMiddleClassficationStructureId = ComUtil.ConvertStringToInt(source.MiddleClassficationName);
                target.JobSmallClassficationStructureId = ComUtil.ConvertStringToInt(source.SmallClassficationName);
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
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit:
                    // 修正
                    return EditDispType.Update;
                case LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Copy:
                    // 複写
                    return EditDispType.Copy;
                default:
                    // 到達不能
                    return EditDispType.None;
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
        /// <param name="registInfo">登録データ</param>
        /// <param name="newLongPlanId">out INSERTの場合、採番した長期計画ID</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(ComDao.LnLongPlanEntity registInfo, out long newLongPlanId)
        {
            newLongPlanId = -1;

            if (isInsertEdit(getEditType()))
            {
                // INSERT文
                return TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<long>(out newLongPlanId, SqlName.Common.InsertLongPlan, SqlName.SubDir, registInfo, db);
            }
            else
            {
                // UPDATE文
                return TMQUtil.SqlExecuteClass.Regist(SqlName.Common.UpdateLongPlan, SqlName.SubDir, registInfo, db);
            }
        }
    }
}
