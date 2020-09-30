using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NEETLibrary.NEETLibrary.Tiba.Com.CSVConvert
{
    static public class CSVConvertMethods
    {
        /// <summary>
        /// CSVを二次元配列に変える。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="encode">エンコード</param>
        /// <returns></returns>
        public static List<List<string>> CSVToLists(Stream stream, string encode = "UTF-8")
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(encode));
                return CommonReadStreamToList(reader);
            }
            catch (IOException e)
            {
                // ファイルを読み込めない場合エラーメッセージを表示
                Console.WriteLine("ファイルを読み込めませんでした");
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// CSVを二次元配列に変える。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="encode">エンコード</param>
        /// <returns></returns>
        public static List<List<string>> CSVToLists(string path, string encode = "UTF-8")
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader reader = new StreamReader(path, Encoding.GetEncoding(encode));
                return CommonReadStreamToList(reader);
            }
            catch (IOException e)
            {
                // ファイルを読み込めない場合エラーメッセージを表示
                Console.WriteLine("ファイルを読み込めませんでした");
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// CSVを二次元配列に変える。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="encode">エンコード</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> CSVToDic(Stream stream, string encode = "UTF-8")
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(encode));
                return CommonReadStreamToDictionary(reader);
            }
            catch (IOException e)
            {
                // ファイルを読み込めない場合エラーメッセージを表示
                Console.WriteLine("ファイルを読み込めませんでした");
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// CSVを二次元配列に変える。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="encode">エンコード</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> CSVToDic(string path, string encode = "UTF-8")
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader reader = new StreamReader(path, Encoding.GetEncoding(encode));
                return CommonReadStreamToDictionary(reader);
            }
            catch (IOException e)
            {
                // ファイルを読み込めない場合エラーメッセージを表示
                Console.WriteLine("ファイルを読み込めませんでした");
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// CSVを二次元配列に変える。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="encode">エンコード</param>
        /// <returns></returns>
        private static List<List<string>> CommonReadStreamToList(StreamReader reader)
        {
            // 読み込みたいテキストを開く
            var result = new List<List<string>>();
            using (reader)
            {
                // テキストファイルをString型で読み込みコンソールに表示
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    var list = line.Split(',').ToList();
                    result.Add(list);
                }
            }
            return result;
        }



        /// <summary>
        /// CSVを二次元配列に変える。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="encode">エンコード</param>
        /// <returns></returns>
        private static List<Dictionary<string, string>> CommonReadStreamToDictionary(StreamReader reader)
        {
            // 読み込みたいテキストを開く
            var result = new List<Dictionary<string, string>>();
            using (reader)
            {
                var columsList = reader.ReadLine().Split(',').ToList();
                // テキストファイルをString型で読み込みコンソールに表示
                while (reader.Peek() >= 0)
                {
                    //値をループ
                    var dic = new Dictionary<string, string>();
                    var lineList = reader.ReadLine().Split(',').ToList();
                    for (int i = 0; i < lineList.Count; i++)
                    {
                        //キーをループ
                        dic.Add(columsList[i], lineList[i]);
                    }
                    result.Add(dic);
                }
            }
            return result;
        }
    }
}
