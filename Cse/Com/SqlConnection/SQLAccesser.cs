using Cse.Models.Database;
using CSELibrary.Com.Methods;
using CSELibrary.Com.ModelRefrection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CSELibrary.Com.SqlConnection
{
    public class SQLAccesser<T> : IDisposable where T : AppDbContext
    {
        /// <summary>
        /// 固定で設定する。(更新日時・登録日時の対象のカラムを設定)
        /// </summary>
        public Dictionary<string, Object> registrationDictionary { get; set; } = new Dictionary<string, object>();
        public T appDbContext;
        //接続先のURL
        public string connectionString = "";

        //コンストラクタ
        public SQLAccesser(T _dbContext)
        {
            appDbContext = _dbContext;
        }

        /// <summary>
        /// sql実行メソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <param name="databaseName"></param>
        public async void ExecuteQuery(string sql)
        {

            var res = await appDbContext.Database.ExecuteSqlRawAsync(
                $@"{sql}"
                );

        }


        /// <summary>
        /// sql実行メソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <param name="databaseName"></param>
        public T Select<T>(params object[] keys)
        {
            var type = typeof(T);
            var resObj = SelectObject(type, keys).Result;
            var res = ModelReflection.ObjectToClass<T>(resObj);
            return res;
        }
        private async Task<object> SelectObject(Type type, params object[] keys)
        {
            var res = await appDbContext.FindAsync(type, keys);
            return (res);
        }

        /// <summary>
        /// 汎用更新メソッド（追加・更新を自動判定）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <param name="databaseName"></param>
        public void Register<T>(T dest) where T : ModelReflection
        {

            //存在チェック
            var target = GetModel(dest).Result;
            var isInsert = (target == null);

            // 更新日、登録日を挿入する。
            RegisterAndUpdateData(dest, registrationDictionary, target);
            // 日付変換
            //PrepareDate(dest);
            if (isInsert)
            {
                appDbContext.Add(dest);
            }
            else {
                appDbContext.Update(dest);
            }
            
        }
        /// <summary>
        /// 更新日時などを更新
        /// </summary>
        /// <typeparam name="T">登録用オブジェクト型</typeparam>
        /// <param name="dest">対象</param>
        public void PrepareDate<T>(T dest) where T : ModelReflection
        {
            // 日付系統と普通の系統
            Type type = typeof(T);
            foreach (var propertyInfo in type.GetProperties())
            {
                // 日付型のオブジェクトはLocalに変換する。
                var targetValue = propertyInfo.GetValue(dest, null);
                if(targetValue != null && targetValue.GetType() == typeof(DateTime)) {
                    // 本当にDateTimeであり、値があるとき
                    var dateTimeValue = targetValue as DateTime?;
                    if (dateTimeValue.HasValue) {
                        propertyInfo.SetValue(dest, dateTimeValue.Value.ToLocalTime());
                    }
                }
            }
        }

        /// <summary>
        /// 更新日時などを更新
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="dest"></param>
        /// <param name="destDic"></param>
        /// <param name="isInsert">新規登録時</param>
        public void RegisterAndUpdateData<D,K>(D dest, Dictionary<string, Object> destDic, K target) where D : ModelReflection where K : ModelReflection
        {
            var isInsert = (target == null);
            var targetDestDic = new Dictionary<string, Object>();
            var dic = dest.ToDictionaryProperty(false,false,false,false);
            // 日付系統と普通の系統
            foreach (var key in destDic.Keys) {
                // 新規登録時
                if (isInsert)
                {
                    if (key.ToUpper().Contains("DAY") || key.ToUpper().Contains("DATETIME"))
                    {
                        targetDestDic.Add(key, DateTime.Now.ToLocalTime());
                    }
                    else
                    {
                        targetDestDic.Add(key, destDic[key]);
                    }
                }
                else
                {
                    // 更新時
                    if (key.ToUpper().Contains("UPDATE"))
                    {
                        if ((key.ToUpper().Contains("DAY") || key.ToUpper().Contains("DATETIME")))
                        {
                            targetDestDic.Add(key, DateTime.Now);
                        }
                        else
                        {
                            targetDestDic.Add(key, destDic[key]);
                        }
                    }
                    else if(key.ToUpper().Contains("REGIST"))
                    {
                        // 登録日時をキープ
                        if ((key.ToUpper().Contains("DAY") || key.ToUpper().Contains("DATETIME")))
                        {
                            // 登録日時を自動取得
                            var targetValue = target.ToDictionaryProperty(false, false, false, false)[key];
                            targetDestDic.Add(key, targetValue);
                        }
                        else
                        {
                            // 登録日時を自動取得
                            var targetValue = target.ToDictionaryProperty(false, false, false, false)[key];
                            targetDestDic.Add(key, targetValue);
                        }
                    }
                }

            }
            Type type = typeof(D);
            foreach (var key in targetDestDic.Keys)
            {
                var getPro = type.GetProperty(key.ToString());
                var value = targetDestDic[key];
                Debug.WriteLine($@"key:{key} value:{value}");
                getPro.SetValue(dest, value);
            }
        }

        /// <summary>
        /// 汎用更新メソッド（追加・更新を自動判定）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        /// <param name="databaseName"></param>
        public void Register <T>(List<T> dest) where T : ModelReflection
        {
            foreach (var item in dest) {
                //存在チェック
                Register(item);
            }
        }

        public async Task SaveAsync()
        {
            try {
                var res = await appDbContext.SaveChangesAsync(true);
                //var res = appDbContext.SaveChanges();
                var log = res == 0 ? "更新されませんでした" : "更新されました";
                Console.WriteLine(log);
            } catch(DbEntityValidationException ex) {
                Console.WriteLine($@"更新エラー発生:{ex}");
            }

        }

        public void Save()
        {
            try
            {
                var res = appDbContext.SaveChanges();
                //var res = appDbContext.SaveChanges();
                var log = res == 0 ? "更新されませんでした" : "更新されました";
                Console.WriteLine(log);
            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine($@"更新エラー発生:{ex}");
            }

        }

        /// <summary>
        /// 汎用削除メソッド（削除）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        public void Delete<T>(T dest) where T : ModelReflection
        {
            //存在チェック
            var isExist = IsExist(dest);
            if (isExist)
            {
                appDbContext.Entry(dest).State = EntityState.Deleted;
                appDbContext.Remove(dest);
            }
        }

        /// <summary>
        /// 汎用削除メソッド（削除）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest"></param>
        public void Delete<T>(List<T> dest) where T : ModelReflection
        {
            foreach (var item in dest)
            {
                Delete(item);
            }
        }

        /// <summary>
        /// 存在チェック
        /// </summary>
        /// <typeparam name="K">自由</typeparam>
        /// <param name="dest">対象</param>
        /// <returns>存在の真偽</returns>
        public bool IsExist<K>(K dest)
        {
            if (dest == null) return false;
            var isRemoveAutoIncrement = true;
            var modelRef = dest as ModelReflection;
            var pkdic = modelRef.ToDictionaryProperty(true, !isRemoveAutoIncrement);
            Type type = typeof(K);
            var result = IsExistEx(type, pkdic.Values.ToList().ToArray());
            return (result.Result);
        }

        private async Task<bool> IsExistEx(Type type, params object[] keys)
        {
            var res = await SelectObject(type, keys);
            if((res != null)) appDbContext.Entry(res).State = EntityState.Detached;
            return (res != null);
        }

        /// <summary>
        /// キーでモデルを取得する(存在チェック)
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        public async Task<K> GetModel<K>(K dest) where K : ModelReflection
        {
            if (dest == null) return null;
            var isRemoveAutoIncrement = true;
            var modelRef = dest;
            var pkdic = modelRef.ToDictionaryProperty(true, !isRemoveAutoIncrement, false, false);
            Type type = typeof(K);
            var res = await SelectObject(type, pkdic.Values.ToList().ToArray());
            if ((res != null)) appDbContext.Entry(res).State = EntityState.Detached;
            return (K)res;
        }


        public void DetachAllEntities()
        {
            var changedEntriesCopy = appDbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public void Dispose()
        {
            //appDbContext.DisposeAsync();
        }
    }
}
