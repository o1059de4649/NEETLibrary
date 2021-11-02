using NEETLibrary.Tiba.Com.Attribute;
using NEETLibrary.Tiba.Com.ModelRefrection;
using NEETLibrary.Tiba.Com.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NEETLibrary.TestFolder
{
    public class TestModel : BaseModel
    {
       [PrimaryProperty]
       public string testVal { get; set; } = "テスト内容1";
       [PrimaryProperty]
       public string testName { get; set; } = "テスト内容2";
       public string testCd { get; set; } = "テスト内容3";
       public string testMode { get; set; } = "テスト内容5";
    }

    [DatabaseName(DatabaseName = "FortniteDB")]
    public class TPlayer : BaseModel
    {
        [AutoIncrement]
        public long playerId { get; set; } = 1;
        public string playerName { get; set; } = "テスト内容2";
        public string profile { get; set; } = "テスト内容3";
        public string imagePath { get; set; } = "テスト内容5";
        public string twitterId { get; set; } = "テスト内容5";
    }
}
