using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TmqDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_HM0003
{
    /// <summary>
    /// 使用するデータクラスを定義
    /// </summary>
    public class BusinessLogicDataClass_HM0003
    {
        /// <summary>
        /// 検索条件のデータクラス
        /// </summary>
        public class searchCondition : TmqDao.HmHistoryManagementEntity
        {
            /// <summary>Gets or sets 表示制御用フラグ（承認依頼の場合true、否認の場合false）</summary>
            /// <value>表示制御用フラグ（承認依頼の場合true、否認の場合false）</value>
            public bool RequestFlg { get; set; }
        }
    }
}
