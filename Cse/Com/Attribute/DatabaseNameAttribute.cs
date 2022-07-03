using System;
using System.Collections.Generic;
using System.Text;

namespace CSELibrary.Com.Attribute
{
    /// <summary>
    /// プライマリーキーに付与
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class DatabaseNameAttribute : System.Attribute
    {
        public string DatabaseName { get; set; }
        public DatabaseNameAttribute(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public DatabaseNameAttribute()
        {

        }
    }

}
