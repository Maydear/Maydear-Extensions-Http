using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace System.Net.Http
{
    /// <summary>
    /// HttpContent扩展
    /// </summary>
    public static class HttpContentExtension
    {
        /// <summary>
        /// 转换为格式json对象HttpContent，mime：text/plain
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpContent ToStringContentForTextPlain<T>(this T data) where T : class
        {
            
            return new StringContent(JsonSerializer.Serialize<T>(data), Encoding.UTF8, "text/plain");
        }

        /// <summary>
        /// 转换为格式json对象HttpContent，mime：application/json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpContent ToStringContentForJson<T>(this T data) where T : class
        {
            return new StringContent(JsonSerializer.Serialize<T>(data), Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// 转换为FormUrlEncodedContent对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpContent ToFormUrlEncodedContent<T>(this T data) where T : class
        {
            if(data is string)
            {
                return new FormUrlEncodedContent(JsonKeyValuePair.Deserialize(data.ToString()));
            }
            else
            {
                return new FormUrlEncodedContent(JsonKeyValuePair.BuidKeyValues(data));
            }
        
        }
    }
}
