using Cse.Models.Database;
using CSELibrary.Com.CSVConvert;
using CSELibrary.Com.IEnumerableEx;
using CSELibrary.Com.Methods;
using CSELibrary.Com.Methods.Security;
using CSELibrary.Com.ModelRefrection;
using CSELibrary.Com.SqlConnection;
using CSELibrary.TestFolder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSELibrary
{
    class Program
    {
        const string connectionStringTest = "http://www.tibaneet.com/SQL/Hello.php";
        const string connectionStringSelect = "http://www.tibaneet.com/SQL/Select.php";
        const string connectionStringInsert = "http://www.tibaneet.com/SQL/InsertAndUpdate.php";
        //const string connectionStringSelect = "https://httpbin.org/post";
        //const string connectionStringInsert = "https://httpbin.org/post";
        const string DB = "RolyPolyDB";
        static void Main(string[] args)
        {
            //Console.WriteLine("ニートプログラマ千葉");

            //TestSQLInsert();
            //TestSQLUpdate();
            //TestSQLSelect();
            //TestFloatParseEx();
            //TestDoubleParseEx();
            //TestSubString();
            //TestKanaToHira();
            //TestHiraToKana();
            //TestIntParseEx();
            //TestCSVRead();
            //TestDicRead();
            //TestDateTime();
            //TestCopy();
            //TestNList();
            //TestSQLInsert();
            //TestRefrection();
            //TestRefrectionEx();
            //GetAllModels();
            //RegisterForEntitry();
            //DeleteForEntitry();
            //SecurityTest();
            //FullWidthNumberToHalfWidthNumberTest();
            PropertyNameTest();
        }

        static void FullWidthNumberToHalfWidthNumberTest()
        {
            Console.WriteLine("数値の全角半角変換テスト");
            Console.WriteLine("０１２３４５６７８９".FullWidthNumberToHalfWidthNumber());
            Console.WriteLine("０1あ２い３う4５６7８９10".FullWidthNumberToHalfWidthNumber());
            //例）郵便番号「5530014」、住所2「1-3-4〇〇マンション5F-301-」の場合、「55300141-3-4-5-301」となる
            var zip = "5530014";
            var target = $@"{zip}1-3-4〇〇マンション5F-301-";
            var targetList = Regex.Split(target.FullWidthNumberToHalfWidthNumber(), @"[^0-9]").ToList();
            for (int i = 0; i < targetList.Count; i++)
            {
                if (targetList[i] == "")
                {
                    targetList[i] = null;
                }
            }
            targetList.RemoveAll(x => x == null);
            var res = string.Join("-", targetList);
            Console.WriteLine(res);

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

        static void TestFloatParseEx()
        {
            Console.WriteLine("FloatParseEx テスト--------------");
            var stringTest = "0";
            var result = stringTest.ToFloat();
            Console.WriteLine(result);
            stringTest = "0.01あ";
            result = stringTest.ToFloatValue();
            Console.WriteLine(result);
        }

        static void TestDoubleParseEx()
        {
            Console.WriteLine("DoubleParseEx テスト--------------");
            var stringTest = "0";
            var result = stringTest.ToDouble();
            Console.WriteLine(result);
            stringTest = "0.01";
            result = stringTest.ToDouble();
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
            //Console.WriteLine("ディープコピー テスト--------------");
            //var strList = new List<string>() { "A", "B", "C" };
            //var strList2 = CopyInterFace<List<string>>.DeepCopy(strList);
            //strList2[0] = "Z";
            //Console.WriteLine(strList == strList2);
            //var character = new Character() { id = 1, name = "スライム" };
            //var character2 = CopyInterFace<Character>.DeepCopy(character);
            //var character3 = character;
            //Console.WriteLine($@"ディープコピー:{character == character2}");
            //Console.WriteLine($@"シャローコピー:{character == character3}");

        }

        static void TestNList() {
            Console.WriteLine("NList テスト--------------");
            var strNList = new NList<string>() { "A", "B", "C" };
            Console.WriteLine(strNList.GetNext(50));
            Console.WriteLine(strNList.GetPrev(2));
        }

        static void TestSQLSelect()
        {
            Handler.URL = connectionStringTest;
            var values = new NameValueCollection();
            values["sql"] = SQLCreater.MasterAllGetSQL(DB, "m_creature");
            string result = Handler.DoPost(values);
            var dic = Handler.ConvertDeserialize(result);
        }

        static void TestSQLInsert()
        {
            Handler.URL = connectionStringInsert;
            var values = new NameValueCollection();
            var dic = new Dictionary<string,object>();
            dic.Add("id","4");
            dic.Add("name","test4");
            dic.Add("passWord","test");
            values["sql"] = SQLCreater.CreateInsertSQLByDictionary(dic,"PakuPakuDB","m_player");
            string result = Handler.DoPost(values);
            //var res = Handler.ConvertDeserialize(result);
        }

        static void TestSQLUpdate()
        {
            Handler.URL = connectionStringInsert;
            var values = new NameValueCollection();
            var dic = new Dictionary<string, object>();
            dic.Add("id", "4");
            dic.Add("name", "test4update");
            dic.Add("passWord", "test");
            values["sql"] = SQLCreater.CreateUpdateSQLByDictionary(dic, "PakuPakuDB", "m_player","id=4");
            string result = Handler.DoPost(values);
            //var res = Handler.ConvertDeserialize(result);
        }

        static void TestRefrection() {
            //TestModel getModel = new TestModel();
            //SetModel setModel = new SetModel();
            ////setModel.SetFields(testModel);
            //var res = NeetCommonMethod.CopyToProperties<SetModel>(getModel.GetType(), setModel.GetType());
            //var dic = res.ToDictionaryProperty();
            //foreach (var item in dic)
            //{
            //    Console.WriteLine(item.Key + ":" + item.Value);
            //}
        }

        static void TestRefrectionEx()
        {
            //TTeam getModel = new TTeam() { 
            // playerId = 1,
            // teamId = 2,
            //};
            //getModel.Register(getModel);
            //var res = BaseModel.GetFindAll(getModel);
        }

        static void Snake() {
            var result = CseCommonMethod.CamelToSnake("id");
            Console.WriteLine(result);
        }


        static void GetAllModels()
        {
            //var model = new MTibaGundan();
            //var result = BaseModel.GetFindAll<MTibaGundan>(model);
            //Console.WriteLine(result);
        }

        private static AppDbContext CreateContext() {
            var _context = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(@"Data Source=tcp:tukimuradb.database.windows.net,1433;Initial Catalog=tukimuradev;User Id=sqldbadmin@tukimuradb;Password=f)wTAHf7CHgb")
            .Options;
            var context = new AppDbContext(_context);
            return context;
        }

        private static async void RegisterForEntitry() {
            using (var SqlAccesser = new SQLAccesser<AppDbContext>(CreateContext()))
            {
                //CONtextの生成が課題となる。
                TEST_USER model = new TEST_USER()
                {
                    ID = "1",
                    NAME = "touroku1b",
                };

                TEST_USER model2 = new TEST_USER()
                {
                    ID = "2",
                    NAME = "touroku2a",
                };
                var list = new List<TEST_USER>() {
                    model,
                    model2,
                };
                SqlAccesser.Register(list);
                await SqlAccesser.SaveAsync();
            }
        }

        private static void DeleteForEntitry()
        {
            using (var SqlAccesser = new SQLAccesser<AppDbContext>(CreateContext())) {
                List<object> list = new List<object>();
                list.Add("2");
                var model = SqlAccesser.Select<TEST_USER>(list.ToArray());
                SqlAccesser.Delete(model);
                SqlAccesser.Save();
            }
        }
        private static void SecurityTest()
        {
            int HASH_ROOP = 930;
            var passWord = "test";
            var user_id = "test";
            var pasHash = SecurityMethods.EncodeHashPassword(passWord, user_id, HASH_ROOP);
            Console.WriteLine(pasHash);
        }

        private static void SecurityTest2(List<string> passList, List<string> userList)
        {
            int HASH_ROOP = 930;
            // カウントチェック
            if (passList.Count != userList.Count) {
                Console.WriteLine("Not match List Count");
                return;
            }
            var resList = new List<string>();
            foreach ((var user, var index) in userList.Indexed())
            {
                resList.Add(SecurityMethods.EncodeHashPassword(passList[index], user, HASH_ROOP));
            }

            Console.WriteLine(string.Join(',',resList));
        }

        private static void PropertyNameTest()
        {
            Console.WriteLine("Refrection開始");
            ModelReflection.CreatePropertyNames<TestModel>().ForEach(x => {
                Console.WriteLine(x);
            });
        }
    }
}
