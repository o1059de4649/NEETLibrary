using NEETLibrary.Tiba.Com.Attribute;
using NEETLibrary.Tiba.Com.ModelRefrection;
using NEETLibrary.Tiba.Com.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NEETLibrary.TestFolder
{
    public class SetModel : ModelReflection
    {
        public string test1 { get; set; } = "";
        public string test2 { get; set; } = "";
        public string test3 { get; set; } = "";
        public string test4 { get; set; } = "";
    }

    [DatabaseName(DatabaseName = "FortniteDB")]
    public class MTibaGundan : BaseModel
    {

        [AutoIncrement]
        public long tibaPlayerId { get; set; }


        public string playerName { get; set; }


        public string subTitle { get; set; }


        public string description { get; set; }


        public string imagePath { get; set; }


        //[DatabaseName(DatabaseName = "FortniteDB")]
        //public class TTeam : BaseModel
        //{

        //    [PrimaryProperty]
        //    public long teamId { get; set; }

        //    [PrimaryProperty]
        //    public long playerId { get; set; }

        //}

    }
}
