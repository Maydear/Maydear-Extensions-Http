using Maydear.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maydear
{
    /// <summary>
    /// Http 扩展方法
    /// </summary>
    public static class HttpClientPackageObjectExtension
    {
        /// <summary>
        /// Get方式执行Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="headers">头信息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> GetPackageAsync<T>(this HttpClient client, string requestUri, IDictionary<string, string> headers = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    if (client.DefaultRequestHeaders.Contains(item.Key))
                    {
                        client.DefaultRequestHeaders.Remove(item.Key);
                    }

                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            client.GetAsync(requestUri).ContinueWith((requestTask) =>
            {
                if (TaskHelper.HandleFaultsAndCancelation(requestTask, tcs, cancellationToken))
                {
                    return;
                }
                try
                {
                    HttpResponseMessage result = requestTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        result.Content.ReadAsStringAsync().ContinueWith((resultTask) =>
                        {
                            string text = resultTask.Result;
                            if (TaskHelper.HandleFaultsAndCancelation(resultTask, tcs, cancellationToken))
                            {
                                return;
                            }

                            try
                            {
                                IPackageObject<T> packageObject = JsonConvert.DeserializeObject<IPackageObject<T>>(text);
                                if (packageObject.StatusCode == 2000)
                                {
                                    tcs.SetResult(packageObject.Body);
                                }
                                else
                                {
                                    tcs.SetException(new Exceptions.StatusCodeException(packageObject.StatusCode, packageObject.Notification));
                                }
                            }
                            catch (Exception deserializeException)
                            {
                                tcs.SetException(deserializeException);
                            }
                        });
                    }
                    else
                    {
                        tcs.SetException(new HttpRequestException(result.ReasonPhrase));
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Delete方式执行Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="headers">头信息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> DeletePackageAsync<T>(this HttpClient client, string requestUri, IDictionary<string, string> headers = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    if (client.DefaultRequestHeaders.Contains(item.Key))
                    {
                        client.DefaultRequestHeaders.Remove(item.Key);
                    }

                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

            }
            client.DeleteAsync(requestUri).ContinueWith((requestTask) =>
            {
                if (TaskHelper.HandleFaultsAndCancelation(requestTask, tcs, cancellationToken))
                {
                    return;
                }
                try
                {
                    HttpResponseMessage result = requestTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        result.Content.ReadAsStringAsync().ContinueWith((resultTask) =>
                        {
                            string text = resultTask.Result;
                            if (TaskHelper.HandleFaultsAndCancelation(resultTask, tcs, cancellationToken))
                            {
                                return;
                            }

                            try
                            {
                                IPackageObject<T> packageObject = JsonConvert.DeserializeObject<IPackageObject<T>>(text);
                                if (packageObject.StatusCode == 2000)
                                {
                                    tcs.SetResult(packageObject.Body);
                                }
                                else
                                {
                                    tcs.SetException(new Exceptions.StatusCodeException(packageObject.StatusCode, packageObject.Notification));
                                }
                            }
                            catch (Exception deserializeException)
                            {
                                tcs.SetException(deserializeException);
                            }
                        });
                    }
                    else
                    {
                        tcs.SetException(new HttpRequestException(result.ReasonPhrase));
                    }

                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }


        /// <summary>
        /// POST方式执行Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="headers">头信息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> PostPackageAsync<T>(this HttpClient client, string requestUri, HttpContent content, IDictionary<string, string> headers = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    if (client.DefaultRequestHeaders.Contains(item.Key))
                    {
                        client.DefaultRequestHeaders.Remove(item.Key);
                    }

                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            client.PostAsync(requestUri, content).ContinueWith((requestTask) =>
            {
                if (TaskHelper.HandleFaultsAndCancelation(requestTask, tcs, cancellationToken))
                {
                    return;
                }

                try
                {
                    HttpResponseMessage result = requestTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        result.Content.ReadAsStringAsync().ContinueWith((resultTask) =>
                        {
                            string text = resultTask.Result;
                            if (TaskHelper.HandleFaultsAndCancelation(resultTask, tcs, cancellationToken))
                            {
                                return;
                            }

                            try
                            {
                                IPackageObject<T> packageObject = JsonConvert.DeserializeObject<IPackageObject<T>>(text);
                                if (packageObject.StatusCode == 2000)
                                {
                                    tcs.SetResult(packageObject.Body);
                                }
                                else
                                {
                                    tcs.SetException(new Exceptions.StatusCodeException(packageObject.StatusCode, packageObject.Notification));
                                }
                            }
                            catch (Exception deserializeException)
                            {
                                tcs.SetException(deserializeException);
                            }

                        });
                    }
                    else
                    {
                        tcs.SetException(new HttpRequestException(result.ReasonPhrase));
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Put方式执行Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="headers">头信息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> PutPackageAsync<T>(this HttpClient client, string requestUri, HttpContent content, IDictionary<string, string> headers = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    if (client.DefaultRequestHeaders.Contains(item.Key))
                    {
                        client.DefaultRequestHeaders.Remove(item.Key);
                    }

                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            client.PutAsync(requestUri, content).ContinueWith((requestTask) =>
            {
                if (TaskHelper.HandleFaultsAndCancelation(requestTask, tcs, cancellationToken))
                {
                    return;
                }

                try
                {
                    HttpResponseMessage result = requestTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        result.Content.ReadAsStringAsync().ContinueWith((resultTask) =>
                        {
                            try
                            {
                                string text = resultTask.Result;
                                if (TaskHelper.HandleFaultsAndCancelation(resultTask, tcs, cancellationToken))
                                {
                                    return;
                                }

                                IPackageObject<T> packageObject = JsonConvert.DeserializeObject<IPackageObject<T>>(text);
                                if (packageObject.StatusCode == 2000)
                                {
                                    tcs.SetResult(packageObject.Body);
                                }
                                else
                                {
                                    tcs.SetException(new Exceptions.StatusCodeException(packageObject.StatusCode, packageObject.Notification));
                                }
                            }
                            catch (Exception deserializeException)
                            {
                                tcs.SetException(deserializeException);
                            }
                        });
                    }
                    else
                    {
                        tcs.SetException(new HttpRequestException(result.ReasonPhrase));
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}
