using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NEETLibrary.Tiba.Com.Methods;

namespace NEETLibrary.Tiba.Com.SqlConnection
{
    public class SQLCreater
    {
        public static string MasterAllGetSQL(string databaseName, string table) {
            var result = $@"SELECT * FROM {databaseName}.{table}";
            return result;
        }

        public static string MasterAllGetSQL(string databaseName, string table, string join)
        {
            var result = $@"SELECT * FROM {databaseName}.{table} {join}";
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
            result += $@" {where};";
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

        /// <summary>
        /// Where文の作成(Update文に利用)
        /// </summary>
        /// <param name="dic">データ</param>
        /// <param name="databaseName">DB名称</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns></returns>
        public static string CreateWhereSQLByDictionary(Dictionary<string, object> dic, string databaseName, string tableName)
        {
            var values = " WHERE ";
            if (dic.Count <= 0) return "";
            foreach (var item in dic)
            {
                //最初の時
                if (item.Key == dic.First().Key && item.Value == dic.First().Value)
                {
                    values += $@" {NeetCommonMethod.CamelToSnake(item.Key)} = '{item.Value}' ";
                }
                else
                {
                    values += $@" AND '{NeetCommonMethod.CamelToSnake(item.Key)} = '{item.Value}'";
                }
            }
            var result = values;
            return result;
        }

        /// <summary>
        /// Join文の作成(Select文に利用)
        /// </summary>
        /// <param name="dic">データ</param>
        /// <param name="databaseName">DB名称</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns></returns>
        public static string CreateJoinSQLByDictionary(Dictionary<string, object> baseDic, string databaseName, string baseTableName,string targetTableName,bool isLeft = false)
        {
            var values = isLeft ? $@" LEFT JOIN {databaseName}.{targetTableName} ON " : $@" INNER JOIN {databaseName}.{targetTableName} ON ";
            if (baseDic.Count <= 0) return "";
            foreach (var item in baseDic)
            {
                //最初の時
                if (item.Key == baseDic.First().Key && item.Value == baseDic.First().Value)
                {
                    values += $@" {databaseName}.{baseTableName}.{NeetCommonMethod.CamelToSnake(item.Key)} = {databaseName}.{targetTableName}.{NeetCommonMethod.CamelToSnake(item.Key)}";
                }
                else
                {
                    values += $@" AND {databaseName}.{baseTableName}.{NeetCommonMethod.CamelToSnake(item.Key)} = {databaseName}.{targetTableName}.{NeetCommonMethod.CamelToSnake(item.Key)} ";
                }
            }
            values += ";";
            var result = values;
            return result;
        }
    }
}
