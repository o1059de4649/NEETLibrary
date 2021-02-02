using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NEETLibrary.Tiba.Com.ModelRefrection
{
    class ModelRefrection
    {
        public Dictionary<string,object> ToDictionary()
        {
            var type = this.GetType();
            var dic = new Dictionary<string,object>();
            //メンバを取得する
            var members = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            foreach (FieldInfo item in members)
            {
                dic.Add(item.Name, item.GetValue(this));
            }
            return dic;
        }
    }
}
