///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　認証クラス
/// 説明　　　：　各種認証の共通処理を実装します。
/// 
/// 履歴　　　：　2018.05.14 河村純子　新規作成
///</summary>

using System.Collections.Generic;

using CommonWebTemplate.Models.Common;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CommonWebTemplate.CommonUtil
{
    public class AuthenticationUtil
    {
        #region === 定数定義 ===
        #endregion

        #region === メンバ変数 ===
        /// <summary>
        /// DbContext
        /// </summary>
        private CommonDataEntities _context;
        #endregion

        #region === コンストラクタ ===
        public AuthenticationUtil(CommonDataEntities context)
        {
            this._context = context;
        }
        #endregion

        #region === Public処理 ===
        /// <summary>
        /// ﾛｸﾞｲﾝ認証処理
        /// </summary>
        /// <param name="loginData">
        /// ﾛｸﾞｲﾝ情報
        /// 　ﾕｰｻﾞｰID
        /// 　ﾛｸﾞｲﾝﾊﾟｽﾜｰﾄﾞ
        /// </param>
        /// <param name="userInfo">
        /// ﾕｰｻﾞｰ管理情報
        /// 　ﾕｰｻﾞｰID
        /// 　ﾕｰｻﾞｰ表示名
        /// 　ﾒﾆｭｰ権限
        /// 　GUID
        /// </param>
        /// <returns>true: 認証OK</returns>
        /// <remarks>
        /// siteMinderからのﾛｸﾞｲﾝの場合、ﾊﾟｽﾜｰﾄﾞ認証済み
        /// </remarks>
        public CommonProcReturn LoginAuthentication(CommonUserMst loginData, bool isPasswordCheck, CommonProcData procData, out bool isCheckOK, 
            ref UserInfoDef userInfo, bool isNewLogin)
        {
            //初期化
            isCheckOK = false;
            userInfo = null;

            //★EFのおまじない。。
            //using (var context = new CommonDataEntities())
            //{
            //    int cnt = (
            //        from userMst in context.COM_USER_MST
            //        select userMst
            //    ).Count();
            //}

            //※siteMinderからのﾛｸﾞｲﾝ以外の場合、ﾊﾟｽﾜｰﾄﾞﾁｪｯｸも行う
            string loginPassword = "";
            if (isPasswordCheck)
            {
                //※ﾊﾟｽﾜｰﾄﾞを設定してﾁｪｯｸを行う
                loginPassword = loginData.LoginPassword;
            }

            //ﾛｸﾞｲﾝ認証
            //ﾕｰｻﾞｰ情報を取得
            //ﾕｰｻﾞｰが権限保持する機能IDﾘｽﾄを取得
            //
            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：LoginCheck
            //
            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            UserInfoDef loginUserInfo;
            List<string> retConductIds = null;
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_LoginCheck(loginData.LoginUser, loginPassword, isPasswordCheck, isNewLogin,
                out isCheckOK, out loginUserInfo, out retConductIds);
            if (returnInfo.IsProcEnd())
            {
                //処理に失敗
                return returnInfo;
            }
            if (false == isCheckOK)
            {
                //ﾕｰｻﾞｰ認証：NG
                return returnInfo;
            }

            if(userInfo != null)
            {
                // ログイン済みの場合、以降の処理をスキップ
                return returnInfo;
            }

            //Sessionに管理するﾕｰｻﾞｰ情報を生成する
            userInfo = loginUserInfo;

            //業務ﾛｼﾞｯｸ処理用のﾊﾟﾗﾒｰﾀにもｾｯﾄ
            procData.LoginUserId = userInfo.UserId;
            procData.LanguageId = userInfo.LanguageId;
            procData.AuthorityLevelId = userInfo.AuthorityLevelId;
            procData.BelongingInfo = userInfo.BelongingInfo;

            //ﾕｰｻﾞｰ権限機能ﾏｽﾀﾘｽﾄ：※ﾕｰｻﾞｰ権限のある機能IDﾘｽﾄから、機能ﾏｽﾀ情報ﾘｽﾄをここで取得して保持しておく
            BusinessLogicUtil blogic = new BusinessLogicUtil();

            IList<CommonConductMst> resultsMenu = null;
            CommonProcReturn returnInfoW2 = getUserMenuInfoTranslated(retConductIds, procData, ref blogic, out resultsMenu);
            if (returnInfoW2.IsProcEnd())
            {
                return returnInfo;
            }
            blogic.MargeReturnInfo(ref returnInfo, returnInfoW2);  //処理結果をﾏｰｼﾞ

            if (resultsMenu == null)
            {
                userInfo.UserAuthConducts = new List<CommonConductMst>();   //初期化
            }
            else
            {
                userInfo.UserAuthConducts = resultsMenu;
            }

            return returnInfo;
        }

        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <param name="userInfo">
        /// ﾕｰｻﾞｰ管理情報
        /// 　ﾕｰｻﾞｰID
        /// 　ﾕｰｻﾞｰ表示名
        /// 　ﾒﾆｭｰ権限
        /// 　GUID
        /// </param>
        /// <returns>true: 認証OK</returns>
        public CommonProcReturn Logout(UserInfoDef userInfo)
        {
            //ログアウト処理
            //一時テーブルデータのクリア
            //セッション管理情報へログアウト日時を登録
            //
            // - 業務ロジックコール
            // - DLL名：BusinessLogic_Common.dll
            // - 関数：ExecuteBusinessLogic(dynamic inParam, out dynamic outParam);
            // - 起動処理名：Logout
            //
            CommonProcData procData = new CommonProcData();
            procData.LoginUserId = userInfo.UserId;
            procData.GUID = userInfo.GUID;

            BusinessLogicIO logicIO = new BusinessLogicIO(procData);
            CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_Logout();

            return returnInfo;
        }
        #endregion

        #region === Private処理 ===
        /// <summary>
        /// ﾒﾆｭｰ定義：ﾕｰｻﾞｰ機能権限からﾒﾆｭｰ情報を生成し
        /// 翻訳して返却します。
        /// </summary>
        /// <param name="conductIds">ﾕｰｻﾞｰ機能権限</param>
        public CommonProcReturn getUserMenuInfoTranslated(IList<string> conductIds, CommonProcData procData, ref BusinessLogicUtil blogic, out IList<CommonConductMst> resultsMenu)
        {
            //初期化
            resultsMenu = null;

            //ﾕｰｻﾞｰ権限機能ﾏｽﾀﾘｽﾄ
            CommonDataUtil commonDataUtil = new CommonDataUtil(this._context);
            resultsMenu = commonDataUtil.GetUserMenuInfo(conductIds);

            //翻訳を取得してﾒﾆｭｰ情報に反映
            // + ﾘｿｰｽIDの取り出し
            List<string> resourceIds = new List<string>();
            foreach (var conduct in resultsMenu)
            {
                // ++ 機能ﾏｽﾀ - ﾒﾆｭｰ表示名
                resourceIds.Add(conduct.CONDUCTMST.RYAKU);
                resourceIds.Add(conduct.CONDUCTMST.NAME);
                //// ++ 機能ﾏｽﾀ - 変更履歴
                //resourceIds.Add(conduct.CONDUCTMST.CHANGELOG);

                if (conduct.CHILDCONDUCTMST == null || conduct.CHILDCONDUCTMST.Count <= 0)
                {
                    //※子階層なし
                    continue;
                }

                //++ 子階層のﾒﾆｭｰ情報を取得
                getResourceIdsForChildMenu(conduct.CHILDCONDUCTMST, ref resourceIds);
            }
            // ++ 重複を除去
            resourceIds = resourceIds.Distinct().ToList();

            // + ﾘｿｰｽ翻訳を取得
            //※業務ﾛｼﾞｯｸｺｰﾙ
            IDictionary<string, string> resources = null;
            CommonProcReturn returnInfo = blogic.GetResourceName(procData, resourceIds, out resources);
            if (returnInfo.IsProcEnd())
            {
                return returnInfo;
            }

            // + ﾘｿｰｽ翻訳をﾒﾆｭｰ情報に反映
            foreach (var conduct in resultsMenu)
            {
                // ++ 機能ﾏｽﾀ - ﾒﾆｭｰ表示名
                conduct.CONDUCTMST.RYAKU = blogic.ConvertResourceName(conduct.CONDUCTMST.RYAKU, resources);
                conduct.CONDUCTMST.NAME = blogic.ConvertResourceName(conduct.CONDUCTMST.NAME, resources);
                //// ++ 機能ﾏｽﾀ - 変更履歴
                //conduct.CONDUCTMST.CHANGELOG = blogic.ConvertResourceName(conduct.CONDUCTMST.CHANGELOG, resources);

                if (conduct.CHILDCONDUCTMST == null || conduct.CHILDCONDUCTMST.Count <= 0)
                {
                    //※子階層なし
                    continue;
                }

                //++ 子階層のﾒﾆｭｰ情報を翻訳
                convertResourceNamesForChildMenu(ref blogic, conduct.CHILDCONDUCTMST, resources);
            }

            //翻訳済みの機能ﾏｽﾀﾘｽﾄを返却
            return returnInfo;
        }

        /// <summary>
        /// 子階層ﾒﾆｭｰの翻訳IDを取得する
        /// </summary>
        /// <param name="childConductMsts">子階層の機能ﾏｽﾀﾘｽﾄ</param>
        /// <param name="resourceIds">(IO)翻訳IDﾘｽﾄ</param>
        public void getResourceIdsForChildMenu(IList<CommonConductMst> childConductMsts, ref List<string> resourceIds)
        {
            // ++ 機能ﾏｽﾀ - 子階層のﾒﾆｭｰ表示名
            resourceIds.AddRange(childConductMsts.Select(x => x.CONDUCTMST.RYAKU).Distinct().ToList());
            //// ++ 機能ﾏｽﾀ - 子階層の変更履歴
            //resourceIds.AddRange(conductMsts.Select(x => x.CONDUCTMST.CHANGELOG).Distinct().ToList());

            foreach (var child in childConductMsts)
            {
                if (child.CHILDCONDUCTMST == null || child.CHILDCONDUCTMST.Count <= 0)
                {
                    //※子階層なし
                    continue;
                }

                //子階層のﾒﾆｭｰ情報を取得
                getResourceIdsForChildMenu(child.CHILDCONDUCTMST, ref resourceIds);
            }
        }

        /// <summary>
        /// 子階層ﾒﾆｭｰの翻訳IDを翻訳名に変換して返却
        /// </summary>
        /// <param name="blogic"></param>
        /// <param name="childConductMsts">子階層の機能ﾏｽﾀﾘｽﾄ</param>
        /// <param name="resources">ﾘｿｰｽ翻訳情報＞KEY：翻訳Id、VALUE：翻訳名</param>
        public void convertResourceNamesForChildMenu(ref BusinessLogicUtil blogic, IList<CommonConductMst> childConductMsts, IDictionary<string, string> resources)
        {

            foreach (var child in childConductMsts)
            {
                // ++ 機能ﾏｽﾀ - 子階層のﾒﾆｭｰ表示名
                child.CONDUCTMST.RYAKU = blogic.ConvertResourceName(child.CONDUCTMST.RYAKU, resources);
                // ++ 機能ﾏｽﾀ - 子階層の変更履歴
                child.CONDUCTMST.CHANGELOG = blogic.ConvertResourceName(child.CONDUCTMST.CHANGELOG, resources);
            }

            foreach (var child in childConductMsts)
            {
                if (child.CHILDCONDUCTMST == null || child.CHILDCONDUCTMST.Count <= 0)
                {
                    //※子階層なし
                    continue;
                }

                //子階層のﾒﾆｭｰ情報を取得
                convertResourceNamesForChildMenu(ref blogic, child.CHILDCONDUCTMST, resources);
            }
        }
        #endregion

    }
}