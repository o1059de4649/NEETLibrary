using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NEETLibrary.Tiba.Com.SqlConnection
{
    public class SQLCreater
    {
        public static string MasterAllGetSQL(string databaseName, string table) {
            var result = $@"SELECT * FROM {databaseName}.{table}";
            return result;
        }

        /// <summary>
        /// Update文の作成
        /// </summary>
        /// <param name="dic">データ</param>
        /// <param name="databaseName">DB名称</param>
        /// <param name="tableName">テーブル名</param>
        /// <param name="where">Where句</param>
        /// <returns></returns>
        public static string CreateUpdateSQLByDictionary(Dictionary<string, object> dic, string databaseName, string tableName, string where)
        {
            var result = $@"UPDATE {databaseName}.{tableName} SET ";
            foreach (var item in dic)
            {
                
                //最後の時
                if (item.Key == dic.Last().Key && item.Value == dic.Last().Value)
                {
                    result += $@" {item.Key}= '{item.Value}' ";
                }
                else
                {
                    result += $@" {item.Key}= '{item.Value}',";
                }
            }
            var whereStr = $@" WHERE {where}";
            result += $@"{whereStr};";
            return result;
        }

        /// <summary>
        /// Insert文の作成
        /// </summary>
        /// <param name="dic">データ</param>
        /// <param name="databaseName">DB名称</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns></returns>
        public static string CreateInsertSQLByDictionary(Dictionary<string, object> dic, string databaseName, string tableName)
        {
            var values = "";
            foreach (var item in dic)
            {
                //最後の時
                if (item.Key == dic.Last().Key && item.Value == dic.Last().Value)
                {
                    values += $@" '{item.Value}' ";
                }
                else {
                    values += $@" '{item.Value}' ,";
                }
            }
            var result = $@"INSERT INTO {databaseName}.{tableName} VALUES ({values});";
            return result;
        }
    }
}
