using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Globalization;
using System.Reflection;
using CSELibrary.Com.IEnumerableEx;
using CSELibrary.Com.Models;
using CSELibrary.Com.ModelRefrection;

namespace CSELibrary.Com.Methods
{
    /// <summary>
    /// 拡張クラス
    /// </summary>
    public static class CseCommonMethod
    {
        const string abcLowerList = "abcdefghijklmnopqrstuvwxyz";
        const string abcUpperList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string kanaList = "アイウエオカキクケコサシスセソタチツテトナニヌネノマミムメモハヒフヘホラリルレロヤユヨワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポァィゥェォャュョ";
        const string hiraList = "あいうえおかきくけこさしすせそたちつてとなにぬねのまみむめもはひふへほらりるれろやゆよわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽぁぃぅぇぉゃゅょ";
        const string fullWidthNumberList = "０１２３４５６７８９";
        const string halfWidthNumberList = "0123456789";

        /// <summary>
        /// ページネーションを利用可能
        /// </summary>
        /// <typeparam name="T">全てのモデルクラス</typeparam>
        /// <param name="source">元データ</param>
        /// <param name="paginationFilterDto">ページャー</param>
        /// <returns>ページネーション後のモデル配列</returns>
        public static IEnumerable<T> PageNation<T>(this IEnumerable<T> source, PaginationFilterDto paginationFilterDto)
        {
            paginationFilterDto.count = source.Count();
            var res = source.Skip(paginationFilterDto.createStartIndex()).Take(paginationFilterDto.rowsPerPage);
            return res;
        }

        /// <summary>
        /// 全角と半角のスペースを取り除く
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns>スペースを除いた文字列</returns>
        public static string RemoveSpace(this string data) {
            if (data == null) return "";
            return data.Replace(" ", "").Replace("　", "");
        }

        /// <summary>
        /// 全角と半角のスペースを取り除く
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns>スペースを除いた文字列</returns>
        public static string FullWidthNumberToHalfWidthNumber(this string data)
        {
            string res = "";
            foreach (var item in data)
            {
                var targetChar = item;
                if (fullWidthNumberList.Contains(item))
                {
                    var newItem = halfWidthNumberList[fullWidthNumberList.IndexOf(item)];
                    var resStr = targetChar.ToString().Replace(item, newItem);
                    res += resStr;
                }
                else {
                    res += item;
                }
                
            }
            return res;
        }

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
        /// 含まれるかどうかを評価する(null可能)
        /// </summary>
        public static bool ContainsEx(this string self, string target)
        {
            if (string.IsNullOrEmpty(self)) return false;

            return self.Contains(target);
            ;
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
        /// スネークケースに変換する。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CamelToSnake(string str, bool isUpper = false)
        {
            var charList = new NList<char>();
            foreach (var item in str)
            {
                charList.Add(item);
            }
            int index = 1;
            var target = new NList<char>();
            for (int i = 0; i < charList.Count; i++)
            {
                var item = charList[i];
                var nextItem = (charList.Count > i + 1)? charList[i + 1] : new char();
                target.Add(item);
                if (!charList.IsLast(item) && abcUpperList.Contains(nextItem))
                {
                    target.Insert(index, '_');
                    index++;
                }
                index++;
            }
            var res = new string(target.ToArray());

            return isUpper ? res.ToUpper()
                : res.ToLower()
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

        public static T CopyToFields<T>(object src, T dest)
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

        public static T CopyToFields<T>(Type srcType, Type destType)
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

        public static T CopyToProperties<T>(object src, T dest)
        {
            if (src == null || dest == null) return dest;
            var srcProperties = src.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var destProperties = dest.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var properties = srcProperties.Join(destProperties, p => new { p.Name, p.PropertyType }, p => new { p.Name, p.PropertyType }, (p1, p2) => new { p1, p2 });
            foreach (var property in properties)
                property.p2.SetValue(dest, property.p1.GetValue(src));
            return dest;
        }

        public static T CopyToProperties<T>(Type srcType, Type destType)
        {
            var srcProperties = srcType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var destProperties = destType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var properties = srcProperties.Join(destProperties, p => new { p.Name, p.PropertyType }, p => new { p.Name, p.PropertyType }, (p1, p2) => new { p1, p2 });
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

        /// <summary>
        /// QRコードを作成してそのバイナリを返す。
        /// </summary>
        /// <param name="code">対象の値</param>
        /// <param name="savePath">一時保存パス</param>
        /// <returns></returns>
        public static byte[] QRcodeCreate(string code, string savePath, bool withDelete = false)
        {
            DotNetBarcode QR = new DotNetBarcode();
            QR.Type = DotNetBarcode.Types.QRCode;               //QRコードを指定
            QR.SaveFileType = DotNetBarcode.SaveFileTypes.Jpeg; //JPEGタイプで保存

            //QRコードを保存します
            //第一引数:QRコード、第二引数:保存パス、第三引数:横サイズ、第四引数:縦サイズ
            QR.Save(code, savePath, 775, 206);

            using (FileStream fileStream = new FileStream(savePath, FileMode.Open))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                //ファイルを読み込むバイト型配列を作成する
                byte[] bs = new byte[fileStream.Length];
                //ファイルの内容をすべて読み込む
                fileStream.Read(bs, 0, bs.Length);
                //閉じる
                fileStream.Close();
                if (withDelete) File.Delete(savePath);
                return bs;
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


    /// <summary>
    /// Indexつきループ
    /// </summary>
    public static partial class TupleEnumerable
    {
        public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            IEnumerable<(T item, int index)> impl()
            {
                var i = 0;
                foreach (var item in source)
                {
                    yield return (item, i);
                    ++i;
                }
            }

            return impl();
        }
    }

    /// <summary>
    /// あらゆる階層オブジェクトをDictionary化
    /// </summary>
    public static partial class DictionaryEnumerable
    {
        public static List<Dictionary<string, object>> ToDIctionaryList<T>(this IEnumerable<T> source) where T : ModelReflection
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var response = new List<Dictionary<string, object>>();
            foreach (var model in source)
            {
                var dic = model.ToDictionaryProperty();
                response.Add(dic);
            }
            return response;
        }
    }
}
