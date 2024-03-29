﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using CSELibrary.Com.Attribute;
using CSELibrary.Com.Methods;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace CSELibrary.Com.ModelRefrection
{
    public class ModelReflection
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ModelReflection(){

        }
        /// <summary>
        /// DictionaryToClass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static T DictionaryToClass<T>(Dictionary<object, object> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                var getPro = type.GetProperty(kv.Key.ToString());
                var typeName = getPro.PropertyType.FullName;
                if (typeName.Contains("Int"))
                {
                    getPro.SetValue(obj, kv.Value.ToString().ToIntValue());
                }
                else if(typeName.Contains("Bool"))
                {
                    var b = (kv.Value.ToString() == "true" || kv.Value.ToString() == "True");
                    getPro.SetValue(obj, b);
                }
                else
                {
                    getPro.SetValue(obj, kv.Value.ToString());
                }

            }
            return (T)obj;
        }

        /// <summary>
        /// ObjectToClass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ObjectToClass<T>(object obj)
        {
            if (null == obj) return default(T);
            var dictionary = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(prop => prop.Name as object, prop => (string)prop.GetValue(obj, null) as object);
            var res = DictionaryToClass<T>(dictionary);
            return res;
        }

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

        public Dictionary<string,object> ToDictionaryField()
        {
            var type = GetType();
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

        public Dictionary<string, object> ToDictionaryProperty(bool isPrimary = false, bool isRemoveAutoIncrement = false, bool isSnake = true, bool isUpper = false)
        {
            var type = GetType();
            var dic = new Dictionary<string, object>();
            //メンバを取得する
            var members = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            foreach (PropertyInfo item in members)
            {
                //属性探知(プライマリーキー探索)
                var primaryAttributes = item.GetCustomAttributes(typeof(KeyAttribute)).ToList();
                var autoIncremenAttributes = item.GetCustomAttributes(typeof(AutoIncrementAttribute)).ToList();
                var isPrimaryPropertyInfo = primaryAttributes.Count > 0 || autoIncremenAttributes.Count > 0;
                var isAutoIncrementPropertyInfo = primaryAttributes.Count > 0;
                var itemName = isSnake ? CseCommonMethod.CamelToSnake(item.Name, isUpper) : item.Name;
                //プライマリーキー探索有効
                if (!isPrimary)
                {
                    dic.Add(itemName, item.GetValue(this));
                }
                else {
                    //プライマリーキー抽出
                    if (isPrimaryPropertyInfo) {
                        //オートインクリメント削除探索有効
                        if (!isRemoveAutoIncrement)
                        {
                            dic.Add(itemName, item.GetValue(this));
                        }
                        else {
                            //オートインクリメント削除
                            if (!isAutoIncrementPropertyInfo) dic.Add(itemName, item.GetValue(this));
                        }
                        
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="dicObj"></param>
        public void SetFields(ModelReflection dicObj) {
            var dic = dicObj.ToDictionaryField();
            //メンバを取得する
            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
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

            var dic = dicObj.ToDictionaryProperty();

            //メンバを取得する
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
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

        /// <summary>
        /// プロパティ名称の作成
        /// </summary>
        /// <returns></returns>
        public static List<string> CreatePropertyNames<T> (){ 
            var list = new List<string>();
            foreach (var prop in typeof(T).GetProperties()) { 
                list.Add(prop.Name);
            }
            return list;
        }
    }
}
