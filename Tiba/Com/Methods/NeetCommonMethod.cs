using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Globalization;

namespace NEETLibrary.Tiba.Com.Methods
{
    /// <summary>
    /// 拡張クラス
    /// </summary>
    public static class NeetCommonMethod
    {
        const string kanaList = "アイウエオカキクケコサシスセソタチツテトナニヌネノマミムメモハヒフヘホラリルレロヤユヨワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポァィゥェォャュョ";
        const string hiraList = "あいうえおかきくけこさしすせそたちつてとなにぬねのまみむめもはひふへほらりるれろやゆよわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽぁぃぅぇぉゃゅょ";

        /// <summary>
        /// カウンタを気にしなくてよいSubStringメソッド
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="startIndex">開始インデックス</param>
        /// <param name="count">カウント</param>
        /// <returns></returns>
        public static string SubStringEx(this string data,int startIndex,int count)
        {
            var result = data;
            if (count <= 0) count = 0;
            if (startIndex <= 0) startIndex = 0;
            //カウンタを超えてしまうケース
            if (data.Length < startIndex + count)
            {
                result = result.Substring(startIndex, data.Length);
            }
            else {
                result = result.Substring(startIndex, count);
            }
            return result;
        }


        /// <summary>
        /// カナ→ひら
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static string KanaToHira(this string data)
        {
            var result = "";
            foreach (char item in data.ToCharArray().ToList())
            {
                result += (kanaList.Contains(item)) ? hiraList[kanaList.IndexOf(item)] : item;
            }
            return result;
        }

        /// <summary>
        /// ひら→カナ
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static string HiraToKana(this string data)
        {
            var result = "";
            foreach (char item in data.ToCharArray().ToList())
            {
                result += (hiraList.Contains(item)) ? kanaList[hiraList.IndexOf(item)] : item;
            }
            return result;
        }

        /// <summary>
        /// 落ちないInt型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static int? ToInt(this string data)
        {
            //nullを返す
            if (int.TryParse(data, out int result))
            {
                return result;
            }
            else { 
                return null;
            }
        }

        /// <summary>
        /// 落ちないInt型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static long? ToLong(this string data)
        {
            //nullを返す
            if (long.TryParse(data, out long result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 日付に変換
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="fromFormat">元データフォーマット</param>
        /// <param name="toFormat">新データフォーマット</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string data,string fromFormat) 
        {
            //日付に変換
            if (DateTime.TryParseExact(data, fromFormat, null,DateTimeStyles.None, out DateTime date)) {
                return date;
            }
            else {
                return null;
            }
        }
    }

    public static class CopyInterFace<T> {

        public static T DeepCopy(T src)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter
                  = new System.Runtime.Serialization
                        .Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, src); // シリアライズ
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream); // デシリアライズ
            }
        }
    }
}
