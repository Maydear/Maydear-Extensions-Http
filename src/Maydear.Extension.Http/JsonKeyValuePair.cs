using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Reflection;

namespace System.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    internal static class JsonKeyValuePair
    {
        /// <summary>
        /// 将json结构的字符串序列化为keyvalue结构
        /// </summary>
        /// <param name="jsonString">待序列化的json字符串</param>
        /// <returns>返回键值对集合</returns>
        internal static IEnumerable<KeyValuePair<string, string>> Deserialize(string jsonString)
        {
            object obj = JsonSerializer.Deserialize<object>(jsonString);
            return BuidKeyValues(obj);
        }

        /// <summary>
        /// 将object对象序列化为键值对结构
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <returns>返回键值对集合</returns>
        internal static IEnumerable<KeyValuePair<string, string>> BuidKeyValues(object obj)
        {
            if (obj == null)
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }
            List<KeyValuePair<string, string>> requestKeyValue = new List<KeyValuePair<string, string>>();

            foreach (PropertyInfo item in obj.GetType().GetProperties())
            {
                var propertyName= item.PropertyType.Name;
                var IsGenericType = item.PropertyType.IsGenericType;
                var isList = item.PropertyType.GetInterface("IEnumerable", false);
                if (IsGenericType && isList != null)
                {
                    var listVal = item.GetValue(obj) as IEnumerable<object>;
                    if (listVal == null) continue;
                    foreach (var aa in listVal)
                    {
                        var dtype = aa.GetType();
                        foreach (var bb in dtype.GetProperties())
                        {
                            var dtlName = bb.Name.ToLower();
                            var dtlType = bb.PropertyType.Name;
                            var oldValue = bb.GetValue(aa);
                            if (dtlType == typeof(decimal).Name)
                            {
                                int dit = 4;
                                if (dtlName.Contains("price") || dtlName.Contains("amount"))
                                    dit = 2;
                                bb.SetValue(aa, Math.Round(Convert.ToDecimal(oldValue), dit, MidpointRounding.AwayFromZero));
                            }
                            Console.WriteLine($"子级属性名称：{dtlName}，类型：{dtlType}，值：{oldValue}");
                        }
                    }
                }
            }


            return requestKeyValue;
        }
    }
}
