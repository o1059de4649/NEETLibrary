﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using CSELibrary.Com.ModelRefrection;
using CSELibrary.Com.SqlConnection;
using CSELibrary.Com.Methods;
using CSELibrary.Com.Attribute;
using System.Linq;

namespace CSELibrary.Com.Models
{

    public class BaseModel : ModelReflection
    {
        public BaseModel() { }
        /// <summary>
        /// DB名称を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <returns>DB名</returns>
        static public string GetDBName<T>(T dest) {
            var atList = dest.GetType().GetCustomAttributes(typeof(DatabaseNameAttribute), false).ToList();
            if (atList.Count <= 0)
            {
                throw new Exception("正規のモデルを利用してください。");
            }
            var modelAt = atList.FirstOrDefault();
            DatabaseNameAttribute dbnameAt = modelAt as DatabaseNameAttribute;

            var databaseName = dbnameAt.DatabaseName;
            return databaseName;
        }
        /// <summary>
        /// 汎用更新メソッド（追加・更新を自動判定）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <param name="databaseName"></param>
        public virtual void Register<T>(T dest)
        {
            //存在チェック
            var isInsert = !IsExist(dest);
            var databaseName = GetDBName(dest);
            var tableName = CseCommonMethod.CamelToSnake(dest.GetType().Name);
            var isRemoveAutoIncrement = true;
            var dic = ToDictionaryProperty(false, !isRemoveAutoIncrement);
            var pkdic = ToDictionaryProperty(true, isRemoveAutoIncrement);
            var where = SQLCreater.CreateWhereSQLByDictionary(pkdic, databaseName, tableName);
            var sql = isInsert ? SQLCreater.CreateInsertSQLByDictionary(dic, databaseName, tableName)
                : SQLCreater.CreateUpdateSQLByDictionary(dic, databaseName, tableName, where)
                ;
            NameValueCollection Values = new NameValueCollection();
            Values["sql"] = sql;
            Handler.URL = Handler.InsertAndUpdateURL;
            Handler.DoPost(Values);
        }

        /// <summary>
        /// 汎用削除メソッド（削除）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <param name="databaseName"></param>
        public virtual void Delete<T>(T dest)
        {

        }

        public virtual bool IsExist<T>(T dest) {
            var databaseName = GetDBName(dest);
            var isRemoveAutoIncrement = true;
            var tableName = CseCommonMethod.CamelToSnake(dest.GetType().Name);
            var pkdic = ToDictionaryProperty(true, !isRemoveAutoIncrement);
            //SQL生成
            var where = SQLCreater.CreateWhereSQLByDictionary(pkdic, databaseName, tableName);
            var select = SQLCreater.MasterAllGetSQL(databaseName, tableName);
            var sb = new StringBuilder();
            sb.AppendLine(select);
            sb.AppendLine(where);

            NameValueCollection Values = new NameValueCollection();
            Values["sql"] = sb.ToString();
            Handler.URL = Handler.SelectURL;
            var jsonStr = Handler.DoPost(Values);
            var res = Handler.ConvertDeserialize(jsonStr);
            //存在する
            return (res.Count > 0);
        }

        static public List<T> GetFindAll<T>(T dest){
            var databaseName = GetDBName(dest);
            var tableName = CseCommonMethod.CamelToSnake(dest.GetType().Name);
            var select = SQLCreater.MasterAllGetSQL(databaseName, tableName);
            
            NameValueCollection Values = new NameValueCollection();
            Values["sql"] = select;
            Handler.URL = Handler.SelectURL;
            var jsonStr = Handler.DoPost(Values);
            var res = Handler.ConvertDeserialize(jsonStr);
            var list = res.Select(x => DictionaryToClass<T>(x)).ToList();
            var result = list;
            return result;
        }

        public List<(T,K)> Join<T,K>(T baseModel, K targetModel)
        {
            var databaseName = GetDBName(baseModel);
            var baseTableName = CseCommonMethod.CamelToSnake(baseModel.GetType().Name);
            var targetTableName = CseCommonMethod.CamelToSnake(targetModel.GetType().Name);
            var pkdic = ToDictionaryProperty(true, false);
            var join = SQLCreater.CreateJoinSQLByDictionary(pkdic, databaseName, baseTableName, targetTableName);
            var select = SQLCreater.MasterAllGetSQL(databaseName, baseTableName,join);

            NameValueCollection Values = new NameValueCollection();
            Values["sql"] = select;
            Handler.URL = Handler.SelectURL;
            var jsonStr = Handler.DoPost(Values);
            var res = Handler.ConvertDeserialize(jsonStr);
            var list = res.Select(x => DictionaryToClass<T>(x)).ToList();
            var result = list;
            return null;
        }
    }
}
