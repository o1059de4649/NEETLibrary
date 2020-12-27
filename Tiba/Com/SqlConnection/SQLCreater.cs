using System;
using System.Collections.Generic;
using System.Text;

namespace NEETLibrary.Tiba.Com.SqlConnection
{
    public class SQLCreater
    {
        public static string MasterAllGetSQL(string table) {
            var result = $@"SELECT * FROM {table}";
            return result;
        }
    }
}
