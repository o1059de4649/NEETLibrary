using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace NEETLibrary.Tiba.Com.ModelRefrection
{
    public class ModelReflection
    {
        public static T CopyTo<T>(object src, T dest)
        {
            if (src == null || dest == null) return dest;
            var srcProperties = src.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var destProperties = dest.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            var properties = srcProperties.Join(destProperties, p => new { p.Name, p.FieldType }, p => new { p.Name, p.FieldType }, (p1, p2) => new { p1, p2 });
            foreach (var property in properties)
                property.p2.SetValue(dest, property.p1.GetValue(src));
            return dest;
        }

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

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="dicObj"></param>
        public void SetFields(ModelReflection dicObj) {
            var dic = dicObj.ToDictionary();
            //メンバを取得する
            var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            foreach (var item in fields)
            {
                //含まれていたら
                if (dic.ContainsKey(item.Name)) { 
                    //セット
                    var target = dic[item.Name];
                    item.SetValue(this, target);
                }
            }
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="dicObj"></param>
        public void SetProperties(ModelReflection dicObj)
        {

            var dic = dicObj.ToDictionary();

            //メンバを取得する
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            foreach (var item in properties)
            {
                //含まれていたら
                if (dic.ContainsKey(item.Name))
                {
                    //セット
                    var target = dic[item.Name];
                    item.SetValue(this, target);
                }
            }
        }

    }
}
