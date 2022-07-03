using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using CSELibrary.Com.ModelRefrection;
using CSELibrary.Com.SqlConnection;
using CSELibrary.Com.Methods;
using CSELibrary.Com.Attribute;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Cse.Models.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSELibrary.Com.Models
{

    public class EfwBaseModel : BaseModel
    {
        public EfwBaseModel() {
        
        }

        /// <summary>
        /// Table名称を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <returns>DB名</returns>
        static public string GetTableName<T>(T dest)
        {
            var atList = dest.GetType().GetCustomAttributes(typeof(Table), false).ToList();
            if (atList.Count <= 0)
            {
                throw new Exception("正規のモデルを利用してください。");
            }
            var modelAt = atList.FirstOrDefault();
            Table tbnameAt = modelAt as Table;

            var tableName = tbnameAt.Name;
            return tableName;
        }

    }
}
