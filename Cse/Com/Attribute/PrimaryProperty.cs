using System;
using System.Collections.Generic;
using System.Text;

namespace CSELibrary.Com.Attribute
{
    /// <summary>
    /// プライマリーキーに付与
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PrimaryPropertyAttribute : System.Attribute
    {
        public PrimaryPropertyAttribute()
        {

        }
    }

    /// <summary>
    /// オートインクリメントするプロパティに付与
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AutoIncrementAttribute : System.Attribute
    {
        public AutoIncrementAttribute()
        {

        }
    }

}
