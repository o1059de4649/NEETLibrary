using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Globalization;
using System.Reflection;

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
        /// スネークケースをアッパーキャメル(パスカル)ケースに変換します
        /// 例) quoted_printable_encode → QuotedPrintableEncode
        /// </summary>
        public static string SnakeToUpperCamel(this string self)
        {
            if (string.IsNullOrEmpty(self)) return self;

            return self
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2)
            ;
        }

        /// <summary>
        /// スネークケースをローワーキャメル(キャメル)ケースに変換します
        /// 例) quoted_printable_encode → quotedPrintableEncode
        /// </summary>
        public static string SnakeToLowerCamel(this string self)
        {
            if (string.IsNullOrEmpty(self)) return self;

            return self
                .SnakeToUpperCamel()
                .Insert(0, char.ToLowerInvariant(self[0]).ToString()).Remove(1, 1)
            ;
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
        /// 落ちないlong型
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
        /// 落ちないfloat型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static float? ToFloat(this string data)
        {
            //nullを返す
            if (float.TryParse(data, out float result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 落ちないdouble型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static double? ToDouble(this string data)
        {
            //nullを返す
            if (double.TryParse(data, out double result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 落ちないInt型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static int ToIntValue(this string data)
        {
            //0を返す
            if (int.TryParse(data, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 落ちないlong型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static long ToLongValue(this string data)
        {
            //0を返す
            if (long.TryParse(data, out long result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 落ちないfloat型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static float ToFloatValue(this string data)
        {
            //0を返す
            if (float.TryParse(data, out float result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 落ちないdouble型
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns></returns>
        public static double? ToDoubleValue(this string data)
        {
            //0を返す
            if (double.TryParse(data, out double result))
            {
                return result;
            }
            else
            {
                return 0;
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

        public static T CopyTo<T>(object src, T dest)
        {
            if (src == null || dest == null) return dest;
            var srcProperties = src.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var destProperties = dest.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var properties = srcProperties.Join(destProperties, p => new { p.Name, p.FieldType }, p => new { p.Name, p.FieldType }, (p1, p2) => new { p1, p2 });
            foreach (var property in properties)
                property.p2.SetValue(dest, property.p1.GetValue(src));
            return dest;
        }

        public static T CopyTo<T>(Type srcType, Type destType)
        {
            var srcProperties = srcType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var destProperties = destType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var properties = srcProperties.Join(destProperties, p => new { p.Name, p.FieldType }, p => new { p.Name, p.FieldType }, (p1, p2) => new { p1, p2 });
            var dest = Activator.CreateInstance(destType);
            var src = Activator.CreateInstance(srcType);
            foreach (var property in properties)
                property.p2.SetValue(dest, property.p1.GetValue(src));
            return (T)dest;
        }

        public static Dictionary<string, object> ToDictionaryField(object obj)
        {
            var type = obj.GetType();
            var dic = new Dictionary<string, object>();
            //メンバを取得する
            var members = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            foreach (FieldInfo item in members)
            {
                dic.Add(item.Name, item.GetValue(obj));
            }
            return dic;
        }

        public static Dictionary<string, object> ToDictionaryProperty(object obj)
        {
            var type = obj.GetType();
            var dic = new Dictionary<string, object>();
            //メンバを取得する
            var members = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            foreach (PropertyInfo item in members)
            {
                dic.Add(item.Name, item.GetValue(obj));
            }
            return dic;
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
