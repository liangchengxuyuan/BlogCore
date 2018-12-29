using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyp.BlogCore.Common.Redis
{
    public class RedisCache
    {
        //获取一个redis连接
        private static IDatabase db = RedisManager.Instance.GetDatabase();

        /// <summary>
        /// 判断一个键是否存在,通用
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return db.KeyExists(key);
        }
        #region string操作

        /// <summary>
        /// 获取一个键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringKey(string key)
        {
            return db.StringGet(key);
        }
        /// <summary>
        /// 设置一个键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetStringKey(string key, string value)
        {
            db.StringSet(key, value);
        }
        /// <summary>
        /// 设置一个键，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">过期时间</param>
        public static void SetStringKey(string key, string value, TimeSpan time)
        {
            db.StringSet(key, value, time);
        }
        /// <summary>
        /// 删除一个键
        /// </summary>
        /// <param name="key"></param>
        public static void DelStringKey(string key)
        {
            db.KeyDelete(key);
        }
        /// <summary>
        /// 使用lua脚本模糊查询key后批量删除
        /// </summary>
        /// <param name="pattern"></param>
        public static void DeleteKeys(string pattern)
        {
            db.ScriptEvaluate(LuaScript.Prepare(
                  //Redis的keys模糊查询：
                  " local ks = redis.call('KEYS', @keypattern) " + //local ks为定义一个局部变量，其中用于存储获取到的keys
                  " for i=1,#ks,5000 do " +    //#ks为ks集合的个数, 语句的意思： for(int i = 1; i <= ks.Count; i+=5000)
                  "     redis.call('del', unpack(ks, i, math.min(i+4999, #ks))) " + //Lua集合索引值从1为起始，unpack为解包，获取ks集合中的数据，每次5000，然后执行删除
                  " end " +
                 " return true "),
                 new { keypattern = pattern + "*" });

        }
        #endregion

        #region hash操作

        /// <summary>
        /// 保存一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="list">数据集合</param>
        /// <param name="getModelId"></param>
        public static void HashSet<T>(string key, List<T> list, Func<T, string> getModelId)
        {
            var t = TimeSpan.FromSeconds(10);
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonConvert.SerializeObject(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }

            db.HashSet(key, listHashEntry.ToArray());
            db.KeyExpire(key, TimeSpan.FromSeconds(10));

        }
        /// <summary>
        /// 保存一个集合,并设置过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="list">数据集合</param>
        /// <param name="getModelId"></param>
        public static void HashSet<T>(string key, List<T> list, Func<T, string> getModelId, TimeSpan time)
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonConvert.SerializeObject(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }
            db.HashSet(key, listHashEntry.ToArray());
            db.KeyExpire(key, time);
        }

        /// <summary>
        /// 获取Hash中的单个key的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="hasFildValue">RedisValue</param>
        /// <returns></returns>
        public static T GetHashKey<T>(string key, string hasFildValue)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(hasFildValue))
            {
                RedisValue value = db.HashGet(key, hasFildValue);
                if (!value.IsNullOrEmpty)
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获取hash中的多个key的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="listhashFields">RedisValue value</param>
        /// <returns></returns>
        public static List<T> GetHashKey<T>(string key, List<RedisValue> listhashFields)
        {
            List<T> result = new List<T>();
            if (!string.IsNullOrWhiteSpace(key) && listhashFields.Count > 0)
            {
                RedisValue[] value = db.HashGet(key, listhashFields.ToArray());
                foreach (var item in value)
                {
                    if (!item.IsNullOrEmpty)
                    {
                        result.Add(JsonConvert.DeserializeObject<T>(item));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> GetHashAll<T>(string key)
        {
            List<T> result = new List<T>();
            RedisValue[] arr = db.HashKeys(key);
            foreach (var item in arr)
            {
                if (!item.IsNullOrEmpty)
                {
                    result.Add(JsonConvert.DeserializeObject<T>(item));
                }
            }
            return result;
        }

        /// <summary>
        /// 获取hashkey所有的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> HashGetAll<T>(string key)
        {
            List<T> result = new List<T>();
            HashEntry[] arr = db.HashGetAll(key);
            
            foreach (var item in arr)
            {
                if (!item.Value.IsNullOrEmpty)
                {
                    result.Add(JsonConvert.DeserializeObject<T>(item.Value));
                }
            }
            return result;
        }

        /// <summary>
        /// 删除hasekey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool DeleteHash(RedisKey key, RedisValue hashField)
        {
            return db.HashDelete(key, hashField);
        }

        /// <summary>
        /// 删除所有的值
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteAll(string key)
        {
            db.KeyDelete(key);
        }

        /// <summary>
        /// 是否存在hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool HashExists(string key, string field)
        {
            return db.HashExists(key, field);
        }
        #endregion

        #region List操作
        /// <summary>
        /// 设置List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        public static void SetList<T>(string key, List<T> value, TimeSpan time)
        {
            foreach (var single in value)
            {
                var s = JsonConvert.SerializeObject(single);
                db.ListRightPush(key, s);
            }
            db.KeyExpire(key, time);
        }
        /// <summary>
        /// 获取List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string key)
        {
            var vList = db.ListRange(key);
            List<T> result = new List<T>();
            foreach (var item in vList)
            {
                var model = JsonConvert.DeserializeObject<T>(item);
                result.Add(model);
            }
            return result;
        }
        /// <summary>
        /// 移除指定项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="listField"></param>
        /// <returns></returns>
        public static long RemoveList<T>(RedisKey key, RedisValue listField)
        {
            return db.ListRemove(key, listField);
        }
        #endregion
    }
}
