using System;
using System.Linq;
using System.Collections.Generic;
using NEETLibrary.TestFolder;
using NEETLibrary.Tiba.Com.CSVConvert;
using NEETLibrary.Tiba.Com.IEnumerableEx;
using NEETLibrary.Tiba.Com.Methods;

namespace NEETLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSubString();
            TestKanaToHira();
            TestHiraToKana();
            TestIntParseEx();
            TestCSVRead();
            TestDicRead();
            TestDateTime();
            TestCopy();
            TestNList();
        }

        static void TestSubString()
        {
            Console.WriteLine("SubStringテスト--------------");
            string stringTest = "ABCABCABC";
            stringTest = stringTest.SubStringEx(0, 100);
            Console.WriteLine(stringTest);
            stringTest = stringTest.SubStringEx(0, 9);
            Console.WriteLine(stringTest);
            stringTest = stringTest.SubStringEx(0, 5);
            Console.WriteLine(stringTest);
        }

        static void TestKanaToHira()
        {
            Console.WriteLine("カナ→かな テスト--------------");
            var stringTest = "アイウエオ";
            stringTest = stringTest.KanaToHira();
            Console.WriteLine(stringTest);
            stringTest = "あイウエお";
            stringTest = stringTest.KanaToHira();
            Console.WriteLine(stringTest);
        }

        static void TestHiraToKana()
        {
            Console.WriteLine("かな→カナ テスト--------------");
            var stringTest = "あいうえお";
            stringTest = stringTest.HiraToKana();
            Console.WriteLine(stringTest);
            stringTest = "アいうえオ";
            stringTest = stringTest.HiraToKana();
            Console.WriteLine(stringTest);
        }

        static void TestIntParseEx()
        {
            Console.WriteLine("IntParseEx テスト--------------");
            var stringTest = "0";
            var result = stringTest.ToInt();
            Console.WriteLine(result);
            stringTest = "00010";
            result = stringTest.ToInt();
            Console.WriteLine(result);
        }

        static void TestCSVRead()
        {
            Console.WriteLine("CSV読み込み テスト--------------");
            var path = $@"..\..\..\TestFolder\testCSV.txt";
            var csvResult = CSVConvertMethods.CSVToLists(path, "Shift_JIS");
            foreach (var item in csvResult)
            {
                foreach (var _item in item)
                {
                    Console.WriteLine("data" + item.IndexOf(_item) + ":" + _item);
                }
            }
        }

        static void TestDicRead()
        {
            Console.WriteLine("Dic読み込み テスト--------------");
            var path = $@"..\..\..\TestFolder\testDic.txt";
            var dicResult = CSVConvertMethods.CSVToDic(path, "Shift_JIS");
            foreach (var item in dicResult)
            {
                Console.WriteLine(item["name"] + ":のステータス");
                foreach (var _item in item)
                {
                    Console.WriteLine(_item.Key + ":" + _item.Value);
                }
            }
        }

        static void TestDateTime()
        {
            Console.WriteLine("DateTime テスト--------------");
            var stringTest = "20200901";
            var dateResult = stringTest.ToDateTime("yyyyMMdd");
            Console.WriteLine(dateResult);
        }

        static void TestCopy() {
            Console.WriteLine("ディープコピー テスト--------------");
            var strList = new List<string>() { "A", "B", "C" };
            var strList2 = CopyInterFace<List<string>>.DeepCopy(strList);
            strList2[0] = "Z";
            Console.WriteLine(strList == strList2);
            var character = new Character() { id = 1, name = "スライム" };
            var character2 = CopyInterFace<Character>.DeepCopy(character);
            var character3 = character;
            Console.WriteLine($@"ディープコピー:{character == character2}");
            Console.WriteLine($@"シャローコピー:{character == character3}");

        }

        static void TestNList() {
            Console.WriteLine("NList テスト--------------");
            var strNList = new NList<string>() { "A", "B", "C" };
            Console.WriteLine(strNList.GetNext(50));
            Console.WriteLine(strNList.GetPrev(2));
        }
    }
}
