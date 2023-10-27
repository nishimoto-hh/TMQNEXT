using System;
using System.Collections.Generic;
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


// 一つのファイルに書くと長くなって対象の処理を探すのが大変になりそうなので分割テスト(partial)
// 将来的には適当な処理単位で分割したい。その際はファイル名も相応しい内容に変更
namespace CommonTMQUtil
{
    /// <summary>
    /// TMQ用共通ユーティリティクラス
    /// </summary>
    public static partial class CommonTMQUtil
    {
        /// <summary>
        /// 構成マスタの指定グループIDよりアイテムの表示順で並び変えた拡張項目を取得(工場共通の項目と表示順)
        /// </summary>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="db">DB接続</param>
        /// <param name="isGetDeleted">削除を含む場合True、省略時はFalse(含まない)</param>
        /// <returns>アイテムの表示順で並び変えた拡張項目のリスト</returns>
        public static List<T> GetStructureCommonExDataOrdererdItemOrder<T>(Const.MsStructure.GroupId structureGroupId, ComDB db, bool isGetDeleted = false)
        {
            // 削除済みを含む場合2未満、含まない場合は1未満
            int deleteFlg = isGetDeleted ? 2 : 1;
            var param = new { StructureGroupId = (int)structureGroupId, DeleteFlg = deleteFlg };
            return SqlExecuteClass.SelectList<T>(SqlName.GetStructureExtDataOrdered, SqlName.SubDir, param, db);
        }

        /// <summary>
        /// ファイルダウンロード　ダウンロードファイル情報取得処理
        /// </summary>
        /// <param name="searchConditionDictionary">検索条件ディクショナリ　thisをそのまま渡す</param>
        /// <param name="db">DB接続　thisをそのまま渡す</param>
        /// <param name="isError">out エラーの場合True</param>
        /// <returns>取得した添付情報テーブルの内容</returns>
        public static ComDao.AttachmentEntity GetFileDownloadInfo(List<Dictionary<string, object>> searchConditionDictionary, ComDB db, out bool isError)
        {
            isError = false;

            // リンクからのファイルダウンロードなら検索条件にファイル情報が含まれるため、取得
            var keyName = "FileDownloadInfo";
            var dicFileDownloadInfo = searchConditionDictionary.Where(x => x.ContainsKey(keyName)).FirstOrDefault();
            if (dicFileDownloadInfo == null || !dicFileDownloadInfo.ContainsKey(keyName))
            {
                // 取得できない場合はリンクからのダウンロードでないため終了
                isError = true;
                return null;
            }

            // 検索条件より取得したファイル情報を変換
            string fileDownloadInfo = dicFileDownloadInfo[keyName].ToString();
            Dao.FileDownloadInfo info = new(fileDownloadInfo);
            // 添付情報を取得し、取得したファイル情報と比較
            ComDao.AttachmentEntity attachment = new ComDao.AttachmentEntity().GetEntity(info.AttachmentId, db);
            if (!isMatchParam(info, attachment))
            {
                // 比較結果が異なる場合、クライアントで改変されたリクエストなので終了
                isError = true;
                return null;
            }

            return attachment;

            // パラメータのダウンロード情報とテーブルの添付情報の内容が同一か判定
            // クライアントで書き換えたHTMLで実行した場合、この情報が一致しない
            static bool isMatchParam(Dao.FileDownloadInfo info, ComDao.AttachmentEntity att)
            {
                // 機能タイプIDが一致するか判定
                bool isEqualFuncId = info.FunctionTypeId == att.FunctionTypeId;
                // キーIDが一致するか判定
                bool isEqualKeyId = info.KeyId == att.KeyId;
                // どちらも同じ場合のみTrue
                return isEqualFuncId || isEqualKeyId;
            }
        }

        /// <summary>
        /// ランダムな名前を生成する処理
        /// </summary>
        /// <param name="length">名前の長さ</param>
        /// <returns>生成した名前</returns>
        public static string GetRandomName(int length)
        {
            Random random = new(DateTime.Now.Second); // ランダム生成元(Seedを変えるために現在の秒)
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // 生成対象文字

            // 生成対象文字からlength数分ランダムに取得
            string result = new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        /// <summary>
        /// 指定したシーケンスを進め、値を取得
        /// </summary>
        /// <param name="sequenceName">シーケンスの名前</param>
        /// <param name="db">DB接続</param>
        /// <returns>進めたシーケンスの値</returns>
        public static long GetNextSequence(string sequenceName, ComDB db)
        {
            // シーケンスを指定して取得
            // SQLをそのまま実行するとシーケンス名が文字列として与えられるので置換
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetNextSequence, out string sql);
            sql = sql.Replace("@SequenceName", sequenceName);
            long value = db.GetEntity<long>(sql);
            return value;
        }
    }
}
