///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　共通ﾃﾞｰﾀのDB操作クラス
/// 説明　　　：　共通ﾃﾞｰﾀのDB操作処理を実装します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System;
using System.Collections.Generic;
using System.Linq;

using CommonWebTemplate.Models.Common;
using System.Reflection;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using CommonWebTemplate.CommonDefinitions;
using Microsoft.EntityFrameworkCore;

using Model = CommonWebTemplate.Models.Common;
using Microsoft.Extensions.Caching.Memory;

namespace CommonWebTemplate.CommonUtil
{
    public class CommonDataUtil
    {
        #region === 定数 ===
        /// <summary>
        /// コード＋翻訳 テキストボックス幅初期値
        /// </summary>
        public static int CodeTransTextDefaultWidth = 100;
        /// <summary>
        /// 業務ﾛｼﾞｯｸﾌﾟﾛｼｰｼﾞｬ処理区分
        /// </summary>
        /// <remarks>業務ﾛｼﾞｯｸﾌﾟﾛｼｰｼﾞｬの処理区分</remarks>
        public enum ProcKbn
        {
            Init = 0,   //初期化処理
            InitBack,   //再表示処理
            Check,      //チェック処理
            Exec,       //処理実行
        }
        public string GetProcKbnStr(ProcKbn procKbn)
        {
            string[] vals = { "Init", "InitBack", "Check", "" };
            return vals[(int)procKbn];
        }

        /// <summary>
        /// 画面コントロール定義情報取得用SQL文字列
        /// </summary>
        public string SqlGetFormControlDefineInfo =
            @"select 
                fcd.location_structure_id AS LOCATION_LAYER_ID,
                fd.program_id AS PGMID,
                fd.form_no AS FORMNO,
                fd.control_group_id AS CTRLID,
                fcd.define_type AS DEFINETYPE,
                fcd.control_no AS ITEMNO,
                fcd.control_type AS CELLTYPE,
                fcd.control_id AS ITEMID,
                fcd.display_division AS DISPKBN,
                fcd.row_no AS ROWNO,
                fcd.column_no AS COLNO,
                fcd.row_span AS ROWSPAN,
                fcd.column_span AS COLSPAN,
                fcd.header_row_span AS HEADER_ROWSPAN,
                fcd.header_column_span AS HEADER_COLSPAN,
                fcd.position AS POSITION,
                fcd.column_width AS COLWIDTH,
                fcd.from_to_division AS FROMTOKBN,
                fcd.control_count AS ITEM_CNT,
                fcd.initial_value AS INITVAL,
                fcd.required_division AS NULLCHKKBN,
                fcd.text_auto_complete_division AS TXT_AUTOCOMPKBN,
                fcd.button_control_id AS BTN_CTRLID,
                fcd.button_action_division AS BTN_ACTIONKBN,
                fcd.button_authority_division AS BTN_AUTHCONTROLKBN,
                fcd.button_after_execution_division AS BTN_AFTEREXECKBN,
                fcd.button_message AS BTN_MESSAGE,
                fcd.dat_transition_pattern AS DAT_TRANSITION_PATTERN,
                fcd.dat_transition_action_division AS DAT_TRANSITION_ACTION_DIVISION,
                fcd.relation_id AS RELATIONID,
                fcd.relation_parameters AS RELATIONPARAM,
                fcd.option_information AS OPTIONINFO,
                fcd.unchangeable_division AS UNCHANGEABLEKBN,
                fcd.column_fixed_division AS COLFIXKBN,
                fcd.filter_use_division AS FILTERUSEKBN,
                ISNULL(fcd.sort_division, 0) AS SORT_DIVISION,
                fcd.detailed_search_division AS DETAILED_SEARCH_DIVISION,
                ISNULL(fcd.detailed_search_control_type, fcd.control_type) AS DETAILED_SEARCH_CELLTYPE,
                fcd.control_customize_flg AS ITEM_CUSTOMIZE_FLG,
                fcd.css_name AS CSSNAME,
                fcd.expansion_key_name AS EXP_KEY_NAME,
                fcd.expansion_table_name AS EXP_TABLE_NAME,
                fcd.expansion_column_name AS EXP_COL_NAME,
                fcd.expansion_parameters_name AS EXP_PARAM_NAME,
                fcd.expansion_alias_name AS EXP_ALIAS_NAME,
                fcd.expansion_like_pattern AS EXP_LIKE_PATTERN,
                fcd.expansion_in_clause_division AS EXP_IN_CLAUSE_KBN,
                fcd.expansion_lock_type AS EXP_LOCK_TYPE,
                fcd.delete_flg AS DELFLG,
                fcd.control_id AS ITEMNAME,
                cd.minimum_value AS MINVAL,
                cd.maximum_value AS MAXVAL,
                CASE WHEN cd.format_translation_id IS NULL THEN null ELSE CONVERT(nvarchar, cd.format_translation_id) END AS FORMAT,
                ISNULL(cd.maximum_length, 0) AS MAXLENGTH,
                CASE WHEN cd.text_placeholder_translation_id IS NULL THEN null ELSE CONVERT(nvarchar, cd.text_placeholder_translation_id) END AS TXT_PLACEHOLDER,
                CASE WHEN cd.tooltip_translation_id IS NULL THEN null ELSE CONVERT(nvarchar, cd.tooltip_translation_id) END AS TOOLTIP,";

        public string SqlGetFormControlDefineInfo2 = @"
                ISNULL(uc.display_order, fcd.column_no) AS DISPLAY_ORDER,
                ISNULL(uc.display_flg, CONVERT(BIT, 1)) AS DISPLAY_FLG
                ,fd.group_no as GrpNo
                ,cd.column_name as ColOrgName
                ,cd2.column_name as DetailSearchColOrgName
                ,ISNULL(cd2.data_type, cd.data_type) as DetailSearchDataType
                ,cd.data_type as DataType
                ,fcd.expansion_lock_table_name as LockTblName

            FROM cm_form_define fd
            LEFT JOIN cm_form_control_define fcd
            ON  fd.program_id = fcd.program_id
            AND fd.form_no = fcd.form_no
            AND fd.control_group_id = fcd.control_group_id
            LEFT JOIN cm_control_define cd
            ON fcd.control_id = cd.control_id
            AND fcd.control_type = cd.control_type
            LEFT JOIN cm_control_unused cu
            ON fcd.location_structure_id = cu.location_structure_id
            AND fcd.control_id = cu.control_id
            AND fcd.control_type = cu.control_type
            LEFT JOIN (SELECT * FROM cm_control_user_customize WHERE user_id = {2}) uc
            ON fcd.program_id = uc.program_id
            AND fcd.form_no = uc.form_no
            AND fcd.control_group_id = uc.control_group_id
            AND fcd.control_no = uc.control_no
            LEFT JOIN cm_control_define cd2
            ON fcd.control_id = cd2.control_id
            AND fcd.detailed_search_control_type = cd2.control_type

            WHERE
                fd.program_id = {0}
            AND (fcd.location_structure_id = 0 OR fcd.location_structure_id = {1})
            AND fcd.delete_flg != 1
            AND cu.control_id IS NULL
            AND (uc.data_division = {3} OR uc.data_division IS NULL)";

        public string SqlGetFormControlDefineInfo2Com = @"
                fcd.column_no AS DISPLAY_ORDER,
                CONVERT(BIT, 1) AS DISPLAY_FLG
            ,fd.group_no as GrpNo
            ,cd.column_name as ColOrgName
            ,cd2.column_name as DetailSearchColOrgName
            ,ISNULL(cd2.data_type, cd.data_type) as DetailSearchDataType
            ,cd.data_type as DataType
            ,fcd.expansion_lock_table_name as LockTblName

            FROM cm_form_define fd
            LEFT JOIN cm_form_control_define fcd
            ON fd.common_form_no = fcd.form_no
            LEFT JOIN cm_control_define cd
            ON fcd.control_id = cd.control_id
            AND fcd.control_type = cd.control_type
            LEFT JOIN cm_control_unused cu
            ON fcd.location_structure_id = cu.location_structure_id
            AND fcd.control_id = cu.control_id
            AND fcd.control_type = cu.control_type
            LEFT JOIN cm_control_define cd2
            ON fcd.control_id = cd2.control_id
            AND fcd.detailed_search_control_type = cd2.control_type

            WHERE
                fd.program_id = {0}
            AND (fd.common_form_no IS NOT NULL AND fd.common_form_no != 0)
            AND (fcd.location_structure_id = 0 OR fcd.location_structure_id = {1})
            AND fcd.control_group_id = 'CommonCtrl'
            AND fcd.delete_flg != 1
                AND cu.control_id IS NULL";

        #endregion

        #region === メンバ変数 ===
        protected CommonLogger logger = CommonLogger.GetInstance();
        protected static CommonMemoryData comMemoryData;
        #endregion

        #region === プロパティ ===
        /// <summary>
        /// DbContext
        /// </summary>
        private CommonDataEntities _context;
        #endregion

        #region === ｺﾝｽﾄﾗｸﾀ ===
        public CommonDataUtil(CommonDataEntities context)
        {
            this._context = context;
            comMemoryData = CommonMemoryData.GetInstance();
        }
        #endregion

        #region === Dispose ===
        /// <summary>
        /// クラスオブジェクト破棄時処理
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        #endregion

        #region === public処理 ===
        /// <summary>
        /// DbContext:ﾄﾗﾝｻﾞｸｼｮﾝ発行
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction DbBeginTrans()
        {
            return _context.Database.BeginTransaction();
        }

        /// <summary>
        /// メニュー定義：ﾕｰｻﾞｰ機能権限からメニュー情報を生成します。
        /// </summary>
        /// <param name="userAuth">ﾕｰｻﾞｰ機能権限</param>
        public IList<CommonConductMst> GetUserMenuInfo(IList<string> conductIds)
        {
            //ﾕｰｻﾞｰ機能権限で絞込みした機能ﾏｽﾀﾘｽﾄを取得する
            // ※AsEnumerable()でLinq to Entities ⇒ Linq to Objectsとしないと
            // ※EF Coreではjoin句が通らない
            List<COM_CONDUCT_MST> menues = (
                    from conductMst in _context.COM_CONDUCT_MST.AsEnumerable()
                    join userAuth in conductIds on conductMst.CONDUCTID equals userAuth
                    where
                        conductMst.MENUDISP == CONDUCT_MST_CONSTANTS.MENUDISP.Disp &&
                        conductMst.DELFLG == false
                    orderby conductMst.PTN, conductMst.MENUORDER
                    select conductMst
                ).Distinct().ToList();

            //使用される共通機能ﾏｽﾀﾘｽﾄを取得する
            IList<String> cmList = (
                from menu in menues
                where menu.CM_CONDUCTID != null
                select menu.CM_CONDUCTID).Distinct().ToList();
            IList<String> cmConducts = new List<String>();
            foreach (var cm in cmList)
            {
                if (cm.IndexOf('|') >= 0)
                {
                    foreach (var c in cm.Split('|'))
                    {
                        cmConducts.Add(c);
                    }
                }
                else
                {
                    cmConducts.Add(cm);
                }
            }
            cmConducts = cmConducts.Distinct().ToList();

            // ※AsEnumerable()でLinq to Entities ⇒ Linq to Objectsとしないと
            // ※EF Coreではjoin句が通らない
            IList<COM_CONDUCT_MST> cmMsts = (
                    from conductMst in _context.COM_CONDUCT_MST.AsEnumerable()
                    join cm in cmConducts on conductMst.CONDUCTID equals cm
                    where
                        conductMst.DELFLG == false
                    orderby conductMst.PTN, conductMst.MENUORDER
                    select conductMst
                ).Distinct().ToList();

            //ｻﾌﾞ機能ｸﾞﾙｰﾌﾟの機能ｸﾞﾙｰﾌﾟIdﾘｽﾄ
            IList<string> groupIds = (
                from menue in menues
                select menue.CONDUCTGRP).Distinct().ToList();

            //ｻﾌﾞ機能ｸﾞﾙｰﾌﾟのﾏｽﾀﾘｽﾄを取得する
            IList<COM_CONDUCT_MST> groups = new List<COM_CONDUCT_MST>();
            foreach (string groupId in groupIds)
            {
                IList<COM_CONDUCT_MST> groupW = _context.COM_CONDUCT_MST
                .Where(x =>
                    x.CONDUCTID == groupId &&
                    x.MENUDISP == CONDUCT_MST_CONSTANTS.MENUDISP.Disp &&
                    x.DELFLG == false
                ).ToList();
                if (groupW.Count > 0)
                {
                    groups.Add(groupW[0]);
                }
            }
            //Top機能ｸﾞﾙｰﾌﾟの機能ｸﾞﾙｰﾌﾟIdﾘｽﾄ
            IList<string> topGroupIds = (
                from groupW in groups
                select groupW.CONDUCTGRP).Distinct().ToList();

            //Top機能ｸﾞﾙｰﾌﾟのﾏｽﾀﾘｽﾄを取得する
            IList<COM_CONDUCT_MST> topGroups = new List<COM_CONDUCT_MST>();
            foreach (string groupId in topGroupIds)
            {
                IList<COM_CONDUCT_MST> groupW = _context.COM_CONDUCT_MST
                .Where(x =>
                    x.CONDUCTID == groupId &&
                    x.MENUDISP == CONDUCT_MST_CONSTANTS.MENUDISP.Disp &&
                    x.DELFLG == false
                ).ToList();
                if (groupW.Count > 0)
                {
                    topGroups.Add(groupW[0]);
                }
            }

            //top機能ｸﾞﾙｰﾌﾟ単位にｻﾌﾞ機能ｸﾞﾙｰﾌﾟﾘｽﾄを保持する
            IList<CommonConductMst> comTopGroups = new List<CommonConductMst>();
            foreach (COM_CONDUCT_MST topGroup in topGroups.AsQueryable().OrderBy(x => x.PTN).ThenBy(x => x.MENUORDER).ToList())
            {
                //top機能ｸﾞﾙｰﾌﾟ階層のｻﾌﾞ機能ｸﾞﾙｰﾌﾟﾘｽﾄを生成
                CommonConductMst comTopGroup = new CommonConductMst(topGroup);

                //top機能ｸﾞﾙｰﾌﾟ内のｻﾌﾞ機能ｸﾞﾙｰﾌﾟで絞込
                IList<COM_CONDUCT_MST> subGroups = groups
                    .Where(x => x.CONDUCTGRP == topGroup.CONDUCTID)
                    .OrderBy(x => x.PTN)
                    .ThenBy(x => x.MENUORDER)
                    .ToList();
                //子階層に追加
                comTopGroup.CHILDCONDUCTMST = new List<CommonConductMst>();
                foreach (COM_CONDUCT_MST subGroup in subGroups)
                {
                    //ｻﾌﾞ機能ｸﾞﾙｰﾌﾟ階層の機能ﾏｽﾀﾘｽﾄを生成
                    CommonConductMst comGroup = new CommonConductMst(subGroup);

                    //ｻﾌﾞ機能ｸﾞﾙｰﾌﾟ内のﾒﾆｭｰで絞込

                    //※ソート順
                    // - ①ﾊﾟﾀｰﾝ＝10,11(入力／ﾊﾞｯﾁ)の中でMENUORDER順
                    List<COM_CONDUCT_MST> groupMenues = menues
                        .Where(x =>
                            x.CONDUCTGRP == subGroup.CONDUCTID && x.CONDUCTID != subGroup.CONDUCTID &&
                            (x.PTN == CONDUCT_MST_CONSTANTS.PTN.Input || x.PTN == CONDUCT_MST_CONSTANTS.PTN.Bat))
                        .OrderBy(x => x.MENUORDER)
                        .ToList();
                    // - ②ﾊﾟﾀｰﾝ＝20(帳票)の中でMENUORDER順
                    // - ③ﾊﾟﾀｰﾝ＝30(ﾏｽﾀ)の中でMENUORDER順
                    IList<COM_CONDUCT_MST> groupMenuesW = menues
                        .Where(x =>
                            x.CONDUCTGRP == subGroup.CONDUCTID &&
                            (x.PTN == CONDUCT_MST_CONSTANTS.PTN.Report || x.PTN == CONDUCT_MST_CONSTANTS.PTN.Master))
                        .OrderBy(x => x.PTN)
                        .ThenBy(x => x.MENUORDER)
                        .ToList();
                    groupMenues.AddRange(groupMenuesW);

                    //子階層に追加
                    comGroup.CHILDCONDUCTMST = new List<CommonConductMst>();
                    foreach (COM_CONDUCT_MST groupMenue in groupMenues)
                    {
                        comGroup.CHILDCONDUCTMST.Add(new CommonConductMst(groupMenue));
                    }

                    comTopGroup.CHILDCONDUCTMST.Add(comGroup);
                }
                //共通機能
                comTopGroup.CM_CONDUCTMSTS = new List<CommonConductMst>();
                foreach (COM_CONDUCT_MST cm in cmMsts)
                {
                    comTopGroup.CM_CONDUCTMSTS.Add(new CommonConductMst(cm));
                }

                comTopGroups.Add(comTopGroup);
            }

            return comTopGroups;
        }

        /// <summary>
        /// 画面定義：該当機能の画面定義情報を取得します。
        /// </summary>
        /// <param name="procData">業務ﾛｼﾞｯｸﾃﾞｰﾀ</param>
        /// <param name="formNo">画面NO</param>
        /// <param name="areaKbn">
        /// 　定義区分(0:条件 1:一覧 2:入力)
        /// 　※areaKbn = 9の場合は絞込みなし
        /// 　※areaKbn = 1の場合は1,2で絞込み
        /// </param>
        /// <returns>機能マスタ-画面定義-一覧項目定義</returns>
        public CommonConductMst GetFormInfo(CommonProcData procData, ref short formNo, short areaKbn,
            string ctrlId = "")
        {
            //①該当機能の機能ﾏｽﾀ情報取得
            //★インメモリ化対応 start
            //COM_CONDUCT_MST resultW = _context.COM_CONDUCT_MST
            //    .Where(x =>
            //        x.CONDUCTID == procData.ConductId &&
            //        x.DELFLG == false
            //    ).FirstOrDefault();
            // 共有メモリから取得する
            var comConductMstList = (List<COM_CONDUCT_MST>)comMemoryData.GetData(nameof(COM_CONDUCT_MST));
            COM_CONDUCT_MST resultW;
            if (comConductMstList != null)
            {
                resultW = comConductMstList
                    .Where(x =>
                        x.CONDUCTID == procData.ConductId
                    ).FirstOrDefault();
            }
            else
            {
                logger.WriteLog("CommonMemoryData.GetData():" + nameof(COM_CONDUCT_MST));

                resultW = _context.COM_CONDUCT_MST
                .Where(x =>
                    x.CONDUCTID == procData.ConductId &&
                    x.DELFLG == false
                ).FirstOrDefault();
            }
            //★インメモリ化対応 end
            if (resultW == null)
            {
                return null;
            }

            //COM_CONDUCT_MST⇒CommonConductMst詰替え
            CommonConductMst result = new CommonConductMst(resultW);

            // ②機能ﾏｽﾀに紐づく画面定義情報を取得
            // ③画面定義(一覧)に紐づく一覧項目定義情報を取得
            // ④画面定義(一覧)に紐づく一覧項目ユーザ情報を取得
            // ⑤画面定義(一覧)に紐づくコントロールグループの画面定義情報を取得
            //★インメモリ化対応 start
            //result.FORMDEFINES = GetFormDefineInfo(result.CONDUCTMST.PGMID, ref formNo, areaKbn, ctrlId, procData.LoginUserId, procData.FactoryIdList, procData.BelongingInfo);
            result.FORMDEFINES = GetFormDefineInfo(result.CONDUCTMST.PGMID, ref formNo, areaKbn, ctrlId, procData);
            //★インメモリ化対応 end

            //⑥共通機能の画面定義を取得
            if (!string.IsNullOrEmpty(result.CONDUCTMST.CM_CONDUCTID))
            {
                result.CM_CONDUCTMSTS = new List<CommonConductMst>();

                var conductIds = result.CONDUCTMST.CM_CONDUCTID.Split('|');
                foreach (var conductId in conductIds)
                {
                    // 共通機能の機能マスタ情報取得
                    //★インメモリ化対応 start
                    //var comConductMstW = _context.COM_CONDUCT_MST.Where(x =>
                    //    x.CONDUCTID == conductId &&
                    //    x.DELFLG == false
                    //).FirstOrDefault();
                    var comConductMstW = comConductMstList.Where(x =>
                        x.CONDUCTID == conductId
                    ).FirstOrDefault();
                    //★インメモリ化対応 end
                    if (comConductMstW == null) { continue; }

                    // 画面定義取得
                    CommonConductMst comConductMst = new CommonConductMst(comConductMstW);
                    //★インメモリ化対応 start
                    //comConductMst.FORMDEFINES = GetFormDefineInfo(comConductMstW.PGMID, ref formNo, areaKbn, ctrlId, procData.LoginUserId, procData.FactoryIdList, procData.BelongingInfo);
                    comConductMst.FORMDEFINES = GetFormDefineInfo(comConductMstW.PGMID, ref formNo, areaKbn, ctrlId, procData);
                    //★インメモリ化対応 end

                    result.CM_CONDUCTMSTS.Add(comConductMst);
                }
            }

            return result;
        }

        /// <summary>
        /// 画面定義情報の取得
        /// </summary>
        /// <param name="pgmId">プログラムID</param>
        /// <param name="formNo">画面番号</param>
        /// <param name="areaKbn"></param>
        /// 　定義区分(0:条件 1:一覧 2:入力)
        /// 　※areaKbn = 9の場合は絞込みなし
        /// 　※areaKbn = 1の場合は1,2で絞込み
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="belongingInfo">所属情報</param>
        /// <returns></returns>
        //★インメモリ化対応 start
        //public List<CommonFormDefine> GetFormDefineInfo(string pgmId, ref short formNo, short areaKbn, string ctrlId, string userId, List<int> facrotyIdList, BelongingInfo belongingInfo)
        public List<CommonFormDefine> GetFormDefineInfo(string pgmId, ref short formNo, short areaKbn, string ctrlId, CommonProcData procData)
        //★インメモリ化対応 end
        {
            List<CommonFormDefine> resultList = new List<CommonFormDefine>();

            //②機能ﾏｽﾀに紐づく画面定義情報を取得
            //★インメモリ化対応 start
            //IQueryable<COM_FORM_DEFINE> formDefines = _context.COM_FORM_DEFINE
            //    .Where(y =>
            //        y.PGMID == pgmId &&
            //        y.DELFLG == false);
            // 共有メモリから取得
            var defines = (List<COM_FORM_DEFINE>)comMemoryData.GetData(nameof(COM_FORM_DEFINE));
            List<COM_FORM_DEFINE> formDefines;
            if (defines != null)
            {
                formDefines = defines.Where(y => y.PGMID == pgmId).ToList();
            }
            else
            {
                logger.WriteLog("CommonMemoryData.GetData():" + nameof(COM_FORM_DEFINE));
                formDefines = _context.COM_FORM_DEFINE.Where(y =>
                   y.PGMID == pgmId &&
                   y.DELFLG == false).ToList();
            }
            //★インメモリ化対応 end

            //※定義区分で絞込み
            switch (areaKbn)
            {
                case FORM_DEFINE_CONSTANTS.AREAKBN.Detail:
                    //明細情報⇒定義区分:1,2で絞込み
                    formDefines = formDefines
                        .Where(y =>
                            (y.AREAKBN == FORM_DEFINE_CONSTANTS.AREAKBN.List ||
                                y.AREAKBN == FORM_DEFINE_CONSTANTS.AREAKBN.Input)).ToList();
                    if (string.IsNullOrEmpty(ctrlId))
                    {
                        //一覧のCtrlIdの絞込みがない場合、一覧は単票表示パターンのみとする
                        //2：単票入力表示
                        //3：入力画面ポップアップ表示(更新列あり)
                        formDefines = formDefines
                            .Where(y =>
                                (y.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.ControlGroup ||
                                    y.DAT_TRANSPTN == FORM_DEFINE_CONSTANTS.DAT_TRANSPTN.Edit ||
                                    y.DAT_TRANSPTN == FORM_DEFINE_CONSTANTS.DAT_TRANSPTN.Reference)).ToList();
                    }
                    else
                    {
                        //一覧をCtrlIdで絞込み
                        formDefines = formDefines
                            .Where(y =>
                                (y.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.ControlGroup ||
                                    y.CTRLID == ctrlId)).ToList();
                    }
                    break;
                case FORM_DEFINE_CONSTANTS.AREAKBN.None:
                    //絞込みなし
                    break;
                default:
                    //指定された定義区分で絞込み
                    formDefines = formDefines
                        .Where(y =>
                            y.AREAKBN == areaKbn).ToList();
                    break;

            }

            IList<COM_FORM_DEFINE> formDefineList = formDefines
                    .OrderBy(y => y.FORMNO)
                    .ThenBy(y => y.AREAKBN)
                    .ThenBy(y => y.CTRLTYPE)
                    .ThenBy(y => y.DISPORDER)
                    .ToList();
            //IList<COM_FORM_DEFINE>⇒IList<CommonFormDefine>詰替え
            resultList = new List<CommonFormDefine>();
            foreach (COM_FORM_DEFINE formDefine in formDefineList)
            {
                resultList.Add(new CommonFormDefine(formDefine));
            }

            //TMQカスタマイズ start====================================
            //③画面定義(一覧)に紐づく一覧項目定義情報を取得
            // 対象工場
            int locationLayerId = procData.BelongingInfo.DutyFactoryId;  // デフォルトは本務工場
            if(procData.FactoryIdList != null && procData.FactoryIdList.Count > 0)
            {
                // 工場指定有りの場合
                if (!procData.FactoryIdList.Contains(locationLayerId))
                {
                    // 本務工場以外の選択の場合、先頭工場を設定
                    locationLayerId = procData.FactoryIdList[0];
                }
            }

            // 直接SQLを発行してデータを取得する
            //★インメモリ化対応 start
            // 通常レイアウト
            //var listItemList = _context.COM_LISTITEM_DEFINE.FromSqlRaw(
            //    SqlGetFormControlDefineInfo + SqlGetFormControlDefineInfo2, pgmId, locationLayerId, userId, 1).ToList();
            //// 共通レイアウト(カスタマイズ情報の結合無し)
            //var listItemComList = _context.COM_LISTITEM_DEFINE.FromSqlRaw(
            //    SqlGetFormControlDefineInfo + SqlGetFormControlDefineInfo2Com, pgmId, locationLayerId).ToList();
            // 通常レイアウト
            var listItemList = getComListItemDefineList(pgmId, locationLayerId, procData, false);
            if (listItemList == null)
            {
                listItemList = _context.COM_LISTITEM_DEFINE.FromSqlRaw(
                SqlGetFormControlDefineInfo + SqlGetFormControlDefineInfo2, pgmId, locationLayerId, procData.LoginUserId, 1).ToList();
            }
            // 共通レイアウト
            var listItemComList = getComListItemDefineList(pgmId, locationLayerId, procData, true);
            if (listItemComList == null)
            {
                listItemComList = _context.COM_LISTITEM_DEFINE.FromSqlRaw(
                    SqlGetFormControlDefineInfo + SqlGetFormControlDefineInfo2Com, pgmId, locationLayerId).ToList();
            }
            //★インメモリ化対応 end

            foreach (CommonFormDefine fd in resultList.ToList())
            {
                if (fd.FORMDEFINE.COMM_FORMNO == null || fd.FORMDEFINE.COMM_FORMNO == 0) {
                    //共通レイアウト定義を使用しない場合
                    fd.LISTITEMDEFINES = listItemList.Where(x => x.FORMNO == fd.FORMDEFINE.FORMNO && x.CTRLID == fd.FORMDEFINE.CTRLID)
                        .OrderBy(x => x.DEFINETYPE)
                        .ThenByDescending(x => x.COLFIXKBN)
                        .ThenBy(x => x.ROWNO)
                        .ThenBy(x => x.DISPLAY_ORDER)
                        .ThenBy(x => x.COLNO)
                        .ThenBy(x => x.ITEMNO)
                        .ToList();
                }
                else
                {
                    //共通レイアウト定義を使用する場合
                    fd.LISTITEMDEFINES =listItemComList.Where(x => x.FORMNO == fd.FORMDEFINE.FORMNO && x.CTRLID == fd.FORMDEFINE.CTRLID)
                        .OrderBy(x => x.DEFINETYPE)
                        .ThenByDescending(x => x.COLFIXKBN)
                        .ThenBy(x => x.ROWNO)
                        .ThenBy(x => x.DISPLAY_ORDER)
                        .ThenBy(x => x.COLNO)
                        .ThenBy(x => x.ITEMNO)
                        .ToList();
                }
            }

            //TMQカスタマイズ end====================================

            //④画面定義(一覧)に紐づく一覧項目ユーザ情報を取得
            foreach (CommonFormDefine fd in resultList.Where(y =>
                                                y.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn1 ||
                                                y.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn2 ||
                                                y.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn3).ToList())
            {

                //★インメモリ化対応 start
                // COM_LISTITEM_USERはTMQでは未使用
                //fd.LISTITEMUSERS = _context.COM_LISTITEM_USER
                //    .Where(z =>
                //        z.USERID == userId &&
                //        z.PGMID == fd.FORMDEFINE.PGMID &&
                //        z.FORMNO == fd.FORMDEFINE.FORMNO &&
                //        z.CTRLID == fd.FORMDEFINE.CTRLID &&
                //        z.DEFINETYPE == 1)
                //    .OrderBy(z => z.ITEMNO)
                //    .ToList();
                fd.LISTITEMUSERS = new List<COM_LISTITEM_USER>();
                //★インメモリ化対応 end
            }

            //⑤画面定義(一覧)に紐づくコントロールグループの画面定義情報を取得
            foreach (CommonFormDefine fd in resultList.Where(y =>
                                                y.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn1 ||
                                                y.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn2 ||
                                                y.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.IchiranPtn3).ToList())
            {

                fd.CTR_FORMDEFINES = resultList
                    .Where(z =>
                        z.FORMDEFINE.CTRLTYPE == FORM_DEFINE_CONSTANTS.CTRLTYPE.ControlGroup &&
                        z.FORMDEFINE.PGMID == fd.FORMDEFINE.PGMID &&
                        z.FORMDEFINE.FORMNO == fd.FORMDEFINE.FORMNO &&
                        z.FORMDEFINE.CTR_RELATIONCTRLID == fd.FORMDEFINE.CTRLID)
                    .OrderBy(z => z.FORMDEFINE.DISPORDER)
                    .ToList();

                // 該当するコントロールグループの画面定義を画面定義リストから削除
                // (一覧に紐づくコントロールグループの画面定義は一覧の画面定義の方へ持つ)
                foreach (var ctrDefine in fd.CTR_FORMDEFINES)
                {
                    resultList.Remove(ctrDefine);
                }
            }
            return resultList;
        }

        /// <summary>
        /// 業務ﾛｼﾞｯｸ実行処理
        /// </summary>
        /// <param name="procData">
        /// 業務ﾛｼﾞｯｸ用ﾃﾞｰﾀ(※必須項目:(*))
        /// 　ConductId:機能ID(*)
        /// 　PgmId:ﾌﾟﾛｸﾞﾗﾑID(*)
        /// 　FormNo:画面NO(*)
        /// 　CtrlId:ｺﾝﾄﾛｰﾙID
        /// 　ConditionData(val1～100):条件ﾃﾞｰﾀ
        /// 　ListData(val1～200):明細ﾃﾞｰﾀ一覧
        /// </param>
        /// <param name="areaKbn">画面ﾃﾞｰﾀの定義区分(0:条件,1:明細)</param>
        /// <returns>実行結果</returns>
        /// <remarks>
        /// ①画面.入力値を中間ﾃｰﾌﾞﾙに保存
        /// </remarks>
        public CommonProcReturn SaveItemUsertbl(CommonProcData procData)
        {

            //DBのｼｽﾃﾑ日付を取得する
            int YMD = 0;
            int HMIS = 0;
            GetDBDate(out YMD, out HMIS);    //YYYYMMDDHH24MISS

            //※Delete⇒Insert

            //①共通＿一覧項目ユーザ定義マスタを一括削除
            IList<COM_LISTITEM_USER> dataList = _context.COM_LISTITEM_USER
                .Where(x =>
                    x.USERID == procData.LoginUserId &&
                    x.PGMID == procData.ConductId &&
                    x.CTRLID == procData.CtrlId &&
                    x.DEFINETYPE == 1
                ).ToList();

            if (dataList != null && dataList.Count > 0)
            {
                //削除ﾃﾞｰﾀ追加
                _context.COM_LISTITEM_USER.RemoveRange(dataList);
            }

            //②非表示項目を共通＿一覧項目ユーザ定義マスタに保存
            COM_LISTITEM_USER conditionData = null;
            if (procData.ConditionData != null && procData.ConditionData.Count > 0)
            {
                var condition = procData.ConditionData[0];
                var itemNo = condition["itemNo"].ToString();
                string[] itemNoList = itemNo.Split(',');

                foreach (var data in itemNoList)
                {
                    if (data != null && data.Length > 0)
                    {
                        //登録データ
                        conditionData = new COM_LISTITEM_USER();

                        conditionData.USERID = procData.LoginUserId;
                        conditionData.PGMID = procData.ConductId;
                        conditionData.CTRLID = procData.CtrlId;
                        conditionData.DEFINETYPE = 1;    //(固定)
                        conditionData.ITEMNO = short.Parse(data);
                        conditionData.UPDID = procData.LoginUserId;

                        //更新日時
                        conditionData.UPDYMD = YMD;
                        conditionData.UPDHMIS = HMIS;

                        //登録ﾃﾞｰﾀ追加
                        _context.COM_LISTITEM_USER.Add(conditionData);
                    }
                }

            }

            ////DB保存
            _context.SaveChanges();

            //正常ｽﾃｰﾀｽを返す
            return new CommonProcReturn();
        }

        /// <summary>
        /// ｼｽﾃﾑ日時を取得する
        /// </summary>
        /// <returns>DBのｼｽﾃﾑ日付</returns>
        public void GetDBDate(out int YMD, out int HMIS)
        {
            //戻り値を初期化
            YMD = 0;
            HMIS = 0;
            
            string sysDateStr = DateTime.Now.ToString("yyyyMMddHHmmss");

            string dateStr = string.Empty;  //YYYYMMDD
            string timeStr = string.Empty;  //HH24MISS

            if (!string.IsNullOrEmpty(sysDateStr))
            {
                if (sysDateStr.Length >= 8)
                {
                    dateStr = sysDateStr.Substring(0, 8);
                }
                if (sysDateStr.Length >= 14)
                {
                    timeStr = sysDateStr.Substring(8, 6);
                }
            }
            int.TryParse(dateStr, out YMD);
            int.TryParse(timeStr, out HMIS);

        }
        #endregion

        #region === private処理 ===
        /// <summary>
        /// 画面項目定義を取得
        /// </summary>
        /// <param name="pgmId">プログラムID</param>
        /// <param name="locationLayerId">場所階層ID</param>
        /// <param name="procData">処理データ</param>
        /// <param name="isCommonLayout">true:共通レイアウト/false:通常レイアウト</param>
        /// <returns></returns>
        private List<COM_LISTITEM_DEFINE> getComListItemDefineList(string pgmId, int locationLayerId, CommonProcData procData, bool isCommonLayout)
        {
            var keyName = nameof(COM_LISTITEM_DEFINE) + (isCommonLayout ? "_Com" : "");
            var itemList = (List<COM_LISTITEM_DEFINE>)comMemoryData.GetData(keyName);
            if (itemList == null)
            {
                logger.WriteLog("CommonMemoryData.GetData():" + keyName);
                return null;
            }
            var listItemList = itemList.Where(x =>
                x.PGMID == pgmId &&
                (x.LOCATION_LAYER_ID == 0 || x.LOCATION_LAYER_ID == locationLayerId)).ToList();

            if (!isCommonLayout)
            {
                // 通常レイアウトの場合、ユーザカスタマイズ情報を結合
                var userCustomiseList = procData.CustomizeList.Where(x => x.PGMID == pgmId).ToList();
                listItemList = listItemList.GroupJoin(userCustomiseList,
                    item => new { item.PGMID, item.FORMNO, item.CTRLID, item.ITEMNO },
                    custom => new { custom.PGMID, custom.FORMNO, custom.CTRLID, custom.ITEMNO },
                    (item, custom) => new
                    {
                        item.PGMID,
                        item.FORMNO,
                        item.CTRLID,
                        item.ITEMNO,
                        item.DEFINETYPE,
                        item.CELLTYPE,
                        item.ITEMID,
                        item.DISPKBN,
                        item.ROWNO,
                        item.COLNO,
                        item.ROWSPAN,
                        item.COLSPAN,
                        item.HEADER_ROWSPAN,
                        item.HEADER_COLSPAN,
                        item.POSITION,
                        item.COLWIDTH,
                        item.FROMTOKBN,
                        item.ITEM_CNT,
                        item.INITVAL,
                        item.NULLCHKKBN,
                        item.TXT_AUTOCOMPKBN,
                        item.BTN_CTRLID,
                        item.BTN_ACTIONKBN,
                        item.BTN_AUTHCONTROLKBN,
                        item.BTN_AFTEREXECKBN,
                        item.BTN_MESSAGE,
                        item.DAT_TRANSITION_PATTERN,
                        item.DAT_TRANSITION_ACTION_DIVISION,
                        item.RELATIONID,
                        item.RELATIONPARAM,
                        item.OPTIONINFO,
                        item.UNCHANGEABLEKBN,
                        item.COLFIXKBN,
                        item.FILTERUSEKBN,
                        item.SORT_DIVISION,
                        item.DETAILED_SEARCH_DIVISION,
                        item.DETAILED_SEARCH_CELLTYPE,
                        item.ITEM_CUSTOMIZE_FLG,
                        item.CSSNAME,
                        item.EXP_KEY_NAME,
                        item.EXP_TABLE_NAME,
                        item.EXP_COL_NAME,
                        item.EXP_PARAM_NAME,
                        item.EXP_ALIAS_NAME,
                        item.EXP_LIKE_PATTERN,
                        item.EXP_IN_CLAUSE_KBN,
                        item.EXP_LOCK_TYPE,
                        item.ITEMNAME,
                        item.MINVAL,
                        item.MAXVAL,
                        item.FORMAT,
                        item.MAXLENGTH,
                        item.TXT_PLACEHOLDER,
                        item.TOOLTIP,
                        Customize = custom.DefaultIfEmpty()
                    }).SelectMany(x => x.Customize, (x, y) => new COM_LISTITEM_DEFINE
                    {
                        PGMID = x.PGMID,
                        FORMNO = x.FORMNO,
                        CTRLID = x.CTRLID,
                        ITEMNO = x.ITEMNO,
                        DEFINETYPE = x.DEFINETYPE,
                        CELLTYPE = x.CELLTYPE,
                        ITEMID = x.ITEMID,
                        DISPKBN = x.DISPKBN,
                        ROWNO = x.ROWNO,
                        COLNO = x.COLNO,
                        ROWSPAN = x.ROWSPAN,
                        COLSPAN = x.COLSPAN,
                        HEADER_ROWSPAN = x.HEADER_ROWSPAN,
                        HEADER_COLSPAN = x.HEADER_COLSPAN,
                        POSITION = x.POSITION,
                        COLWIDTH = x.COLWIDTH,
                        FROMTOKBN = x.FROMTOKBN,
                        ITEM_CNT = x.ITEM_CNT,
                        INITVAL = x.INITVAL,
                        NULLCHKKBN = x.NULLCHKKBN,
                        TXT_AUTOCOMPKBN = x.TXT_AUTOCOMPKBN,
                        BTN_CTRLID = x.BTN_CTRLID,
                        BTN_ACTIONKBN = x.BTN_ACTIONKBN,
                        BTN_AUTHCONTROLKBN = x.BTN_AUTHCONTROLKBN,
                        BTN_AFTEREXECKBN = x.BTN_AFTEREXECKBN,
                        BTN_MESSAGE = x.BTN_MESSAGE,
                        DAT_TRANSITION_PATTERN = x.DAT_TRANSITION_PATTERN,
                        DAT_TRANSITION_ACTION_DIVISION = x.DAT_TRANSITION_ACTION_DIVISION,
                        RELATIONID = x.RELATIONID,
                        RELATIONPARAM = x.RELATIONPARAM,
                        OPTIONINFO = x.OPTIONINFO,
                        UNCHANGEABLEKBN = x.UNCHANGEABLEKBN,
                        COLFIXKBN = x.COLFIXKBN,
                        FILTERUSEKBN = x.FILTERUSEKBN,
                        SORT_DIVISION = x.SORT_DIVISION,
                        DETAILED_SEARCH_DIVISION = x.DETAILED_SEARCH_DIVISION,
                        DETAILED_SEARCH_CELLTYPE = x.DETAILED_SEARCH_CELLTYPE,
                        ITEM_CUSTOMIZE_FLG = x.ITEM_CUSTOMIZE_FLG,
                        CSSNAME = x.CSSNAME,
                        EXP_KEY_NAME = x.EXP_KEY_NAME,
                        EXP_TABLE_NAME = x.EXP_TABLE_NAME,
                        EXP_COL_NAME = x.EXP_COL_NAME,
                        EXP_PARAM_NAME = x.EXP_PARAM_NAME,
                        EXP_ALIAS_NAME = x.EXP_ALIAS_NAME,
                        EXP_LIKE_PATTERN = x.EXP_LIKE_PATTERN,
                        EXP_IN_CLAUSE_KBN = x.EXP_IN_CLAUSE_KBN,
                        EXP_LOCK_TYPE = x.EXP_LOCK_TYPE,
                        ITEMNAME = x.ITEMNAME,
                        MINVAL = x.MINVAL,
                        MAXVAL = x.MAXVAL,
                        FORMAT = x.FORMAT,
                        MAXLENGTH = x.MAXLENGTH,
                        TXT_PLACEHOLDER = x.TXT_PLACEHOLDER,
                        TOOLTIP = x.TOOLTIP,
                        DISPLAY_FLG = y != null ? y.DISPLAY_FLG : true,
                        DISPLAY_ORDER = y != null ? y.DISPLAY_ORDER : x.COLNO
                    }).ToList();
            }
            return listItemList;
        }
        #endregion

        #region === public static 処理 ===
        /// <summary>
        /// コード＋翻訳項目の列幅取得
        /// </summary>
        /// <param name="listItem">一覧項目定義</param>
        /// <returns>列幅</returns>
        public static int GetCodeTransColWidth(COM_LISTITEM_DEFINE listItem)
        {
            int colWidth = 0;
            var defColWidth = listItem.COLWIDTH;
            if (listItem.COLWIDTH.Contains("||"))
            {
                //ヘッダ列幅指定がある場合
                defColWidth = listItem.COLWIDTH.Split("||")[1];
            }

            // 「|」区切りの場合、「翻訳幅 | テキストボックス幅」
            var widths = defColWidth.Split('|');
            int textWidth = CodeTransTextDefaultWidth;
            int transWidth = Convert.ToInt32(widths[0]);
            if (widths.Length > 1)
            {
                textWidth = Convert.ToInt32(widths[1]);
            }
            if(listItem.TXT_AUTOCOMPKBN == LISTITEM_DEFINE_CONSTANTS.TXT_AUTOCOMPKBN.AutoCompTransOnly)
            {
                //textWidth = transWidth;
                transWidth = 0;
            }
            colWidth = textWidth + transWidth;
            string optionInfo = listItem.OPTIONINFO;
            int btnWidth = 0;   //選ボタン非表示、翻訳表示
            if (!string.IsNullOrEmpty(optionInfo))
            {
                //[0]:表示設定、[1]:子画面番号
                string[] aInfo = optionInfo.Split('|');
                if (aInfo.Length >= 1)
                {
                    //表示設定
                    if (aInfo[0].Equals("1"))
                    {
                        //1：選ボタン表示、翻訳非表示
                        btnWidth = 40;
                        transWidth = 0;
                    }
                    else if (aInfo[0].Equals("2"))
                    {
                        //2：選ボタン・翻訳両方表示
                        btnWidth = 40;
                    }
                    else if (aInfo[0].Equals("3"))
                    {
                        //3：選ボタン・翻訳両方非表示
                        btnWidth = 0;
                        transWidth = 0;
                    }
                }
            }
            colWidth = textWidth + transWidth + btnWidth + 4;
            return colWidth;
        }

        /// <summary>
        /// コード＋翻訳項目のテキストボックス幅取得
        /// </summary>
        /// <param name="listItem">一覧項目定義</param>
        /// <returns>テキストボックス幅</returns>
        public static int GetCodeTransTextBoxWidth(COM_LISTITEM_DEFINE listItem)
        {
            var colWidth = listItem.COLWIDTH;
            if (listItem.COLWIDTH.Contains("||"))
            {
                //ヘッダ列幅指定がある場合
                colWidth = listItem.COLWIDTH.Split("||")[1];
            }
            var widths = colWidth.Split('|');
            int textWidth = CodeTransTextDefaultWidth;
            if (widths.Length > 1)
            {
                textWidth = Convert.ToInt32(widths[1]);
            }
            return textWidth;
        }

        /// <summary>
        /// FromTo項目の列幅取得
        /// </summary>
        /// <param name="colWidth">一覧項目定義の列幅</param>
        /// <returns>列幅</returns>
        public static int GetFromToColWidth(int colWidth)
        {
            return colWidth * 2 + 21;
        }
        /// <summary>
        /// テキストボックス幅取得
        /// </summary>
        /// <param name="listItem">一覧項目定義</param>
        /// <returns>テキストボックス幅</returns>
        public static int GetMaxColWidth(COM_LISTITEM_DEFINE listItem)
        {
            var colWidth = listItem.COLWIDTH;
            if(listItem.COLWIDTH.Contains("||"))
            {
                //ヘッダ列幅指定がある場合
                colWidth = listItem.COLWIDTH.Split("||")[1];
            }
            var widths = colWidth.Split('|');
            var maxWidth = 0;
            foreach(var width in widths)
            {
                var tmpWidth = Convert.ToInt32(width);
                if(maxWidth < tmpWidth)
                {
                    maxWidth = tmpWidth;
                }
            }
            return maxWidth;
        }
        #endregion

    }
}