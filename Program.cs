using System;
using NEETLibrary.NEETLibrary.Tiba.Com.CSVConvert;
using NEETLibrary.Tiba.Com.Methods;

namespace NEETLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SubStringテスト--------------");
            string stringTest = "ABCABCABC";
            stringTest = stringTest.SubStringEx(0,100);
            Console.WriteLine(stringTest);
            stringTest = stringTest.SubStringEx(0, 9);
            Console.WriteLine(stringTest);
            stringTest = stringTest.SubStringEx(0, 5);
            Console.WriteLine(stringTest);

            Console.WriteLine("カナ→かな テスト--------------");
            stringTest = "アイウエオ";
            stringTest = stringTest.KanaToHira();
            Console.WriteLine(stringTest);
            stringTest = "あイウエお";
            stringTest = stringTest.KanaToHira();
            Console.WriteLine(stringTest);

            Console.WriteLine("かな→カナ テスト--------------");
            stringTest = "あいうえお";
            stringTest = stringTest.HiraToKana();
            Console.WriteLine(stringTest);
            stringTest = "アいうえオ";
            stringTest = stringTest.HiraToKana();
            Console.WriteLine(stringTest);

            Console.WriteLine("IntParseEx テスト--------------");
            stringTest = "0";
            var result = stringTest.ToInt();
            Console.WriteLine(result);
            stringTest = "00010";
                result = stringTest.ToInt();
            Console.WriteLine(result);


            Console.WriteLine("CSV読み込み テスト--------------");
            var path = $@"D:\git\NEETLibrary\NEETLibrary\TestFolder\testCSV.txt";
            var csvResult = CSVConvertMethods.CSVToLists(path, "Shift_JIS");
            foreach (var item in csvResult)
            {
                foreach (var _item in item)
                {
                    Console.WriteLine("data"+item.IndexOf(_item)+":"+_item);
                }
            }

            Console.WriteLine("Dic読み込み テスト--------------");
            path = $@"D:\git\NEETLibrary\NEETLibrary\TestFolder\testDic.txt";
            var dicResult = CSVConvertMethods.CSVToDic(path, "Shift_JIS");
            foreach (var item in dicResult)
            {
                Console.WriteLine(item["name"] + ":のステータス");
                foreach (var _item in item)
                {
                    Console.WriteLine(_item.Key+":"+_item.Value);
                }
            }

            Console.WriteLine("DateTime テスト--------------");
            stringTest = "20200901";
            var dateResult = stringTest.ToDateTime("yyyyMMdd");
            Console.WriteLine(dateResult);
        }
    }
}
