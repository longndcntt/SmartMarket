using Plugin.Connectivity;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace SmartMarket.Services.HttpService
{
    public class HttpRequest : IHttpRequest
    {
        #region Properties

        public int TimeoutSeconds = 100;

        public TimeSpan Timeout
        {
            get => BaseClient.Timeout;
            set => BaseClient.Timeout = value;
        }

        private Serializer _crossJsonSerializer;
        public Serializer CrossJsonSerializer
            => LazyInitializer.EnsureInitialized(ref _crossJsonSerializer, () => new Serializer());

        public HttpClient BaseClient
        {
            get;
            private set;
        }

        public Uri BaseAddress
        {
            get => BaseClient.BaseAddress;
            set => BaseClient.BaseAddress = value;
        }

        public long MaxResponseContentBufferSize
        {
            get => BaseClient.MaxResponseContentBufferSize;
            set => BaseClient.MaxResponseContentBufferSize = value;
        }

        public HttpRequestHeaders DefaultRequestHeaders => BaseClient.DefaultRequestHeaders;

        public HttpContentHeaders DefaultContentHeaders { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        private bool DidCancellationTokenSourceDispose;

        #endregion

        #region Constructor

        public HttpRequest()
        {
            BaseClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(TimeoutSeconds) };
            Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
        }

        #endregion

        #region Origin method

        private async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
              //  await MessagePopup.Instance.Show(TranslateExtension.Get("NetworkError"));
                return null;
            }

            return await BaseClient.GetAsync(requestUri, completionOption, cancellationToken);
        }

        private async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content,
            CancellationToken cancellationToken)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
              //  await MessagePopup.Instance.Show(TranslateExtension.Get("NetworkError"));
                return null;
            }

            return await BaseClient.PostAsync(requestUri, content, cancellationToken);
        }

        private async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
            //    await MessagePopup.Instance.Show(TranslateExtension.Get("NetworkError"));
                return null;
            }

            return await BaseClient.DeleteAsync(requestUri, cancellationToken);
        }

        private async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content,
            CancellationToken cancellationToken)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
             //   await MessagePopup.Instance.Show(TranslateExtension.Get("NetworkError"));
                return null;
            }

            return await BaseClient.PutAsync(requestUri, content, cancellationToken);
        }

        #endregion

        #region Get Task Async

        /// <summary>
        /// get with call back is a task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="processorSuccess"></param>
        /// <param name="processorError"></param>
        /// <returns></returns>
        public async Task<T> GetTaskAsync<T>(string requestUri, Func<T, Task> processorSuccess = null,
            Func<Task> processorError = null)
            where T : class
        {
#if DEBUG
            Debug.WriteLine($"Get method with url: {requestUri}");
#endif
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
                return null;
            }

            try
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                var responseMessage = await GetAsync(requestUri,
                    HttpCompletionOption.ResponseContentRead, CancellationTokenSource.Token);

                if (responseMessage == null)
                {
                    if (processorError != null) await processorError();
                    return null;
                }
                else
                {
                    if (await IsTokenExpired(responseMessage))
                    {
                        await TokenExpiredExecute();
                        return null;
                    }

                    responseMessage = responseMessage.EnsureSuccessStatusCode();
                    using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                        if (result == null)
                        {
                            if (processorError != null) await processorError();
                            return null;
                        }
                        else
                        {
                            if (processorSuccess != null)
                                await processorSuccess(result);
                            return result;
                        }
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == CancellationTokenSource.Token)
                {
                    if (processorError != null) await processorError();
                    var token = CancellationTokenSource.Token;
#if DEBUG
                    Debug.WriteLine(
                        $"A real cancellation, triggered by the caller, token is: {token}, message: {ex.Message}, url: {requestUri}");
#endif
                }
                else
                {
                    if (processorError != null) await processorError();
#if DEBUG
                    Debug.WriteLine($"A web request timeout, message: {ex.Message}, url: {requestUri}");
#endif
                }
                return null;
            }
            catch (UnauthorizedAccessException unAuEx)
            {
                //await MessagePopup.Instance.Show("Invalid API call");
                return null;
            }
            catch (Exception exception)
            {
                if (processorError != null) await processorError();
#if DEBUG
                Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                return null;
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        /// <summary>
        /// get with call back is a task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="processorSuccess"></param>
        /// <param name="processorError"></param>
        /// <returns></returns>
        public async Task<T> GetObjectTaskAsync<T>(string requestUri, Func<T, Task> processorSuccess = null,
            Func<Task> processorError = null)
            where T : class
        {
#if DEBUG
            Debug.WriteLine($"Get method with url: {requestUri}");
#endif
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
                return null;
            }

            try
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                var responseMessage = await GetAsync(requestUri,
                    HttpCompletionOption.ResponseContentRead, CancellationTokenSource.Token);

                if (responseMessage == null)
                {
                    if (processorError != null) await processorError();
                    return null;
                }
                else
                {
                    if (await IsTokenExpired(responseMessage))
                    {
                        await TokenExpiredExecute();
                        return null;
                    }

                    responseMessage = responseMessage.EnsureSuccessStatusCode();
                    var response = await responseMessage.Content.ReadAsStringAsync();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(response);
                    var result = this.CrossJsonSerializer.Deserialize<T>(doc.InnerXml);

                    if (result == null)
                    {
                        if (processorError != null) await processorError();
                        return null;
                    }
                    else
                    {
                        if (processorSuccess != null)
                            await processorSuccess(result);
                        return result;
                    }

                    /*using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var result = this.CrossJsonSerializer.Deserialize<T>(jsonStream, requestUri);

                        if (result == null)
                        {
                            if (processorError != null) await processorError();
                            return null;
                        }
                        else
                        {
                            if (processorSuccess != null)
                                await processorSuccess(result);
                            return result;
                        }
                    }*/
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == CancellationTokenSource.Token)
                {
                    if (processorError != null) await processorError();
                    var token = CancellationTokenSource.Token;
#if DEBUG
                    Debug.WriteLine(
                        $"A real cancellation, triggered by the caller, token is: {token}, message: {ex.Message}, url: {requestUri}");
#endif
                }
                else
                {
                    if (processorError != null) await processorError();
#if DEBUG
                    Debug.WriteLine($"A web request timeout, message: {ex.Message}, url: {requestUri}");
#endif
                }
                return null;
            }
            catch (UnauthorizedAccessException unAuEx)
            {
                //await MessagePopup.Instance.Show("Invalid API call");
                return null;
            }
            catch (Exception exception)
            {
                if (processorError != null) await processorError();
#if DEBUG
                Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                return null;
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        /// <summary>
        /// get with callback object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="processorSuccess"></param>
        /// <param name="processorError"></param>
        /// <param name="callbackObject"></param>
        /// <returns></returns>
        public async Task GetTaskAsyncCallback<T>(string requestUri, Func<T, object, Task> processorSuccess,
            Func<Task> processorError = null, object callbackObject = null) where T : class
        {
#if DEBUG
            Debug.WriteLine($"Get method with url: {requestUri}");
#endif
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                try
                {
                    InitTokenSource();
                    CancellationTokenSource.CancelAfter(this.Timeout);
                    var responseMessage = await GetAsync(requestUri, HttpCompletionOption.ResponseContentRead,
                        CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        responseMessage = responseMessage.EnsureSuccessStatusCode();
                        using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                            if (result == null)
                            {
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                await processorSuccess(result, callbackObject);
                            }
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (ex.CancellationToken == CancellationTokenSource.Token)
                    {
                        if (processorError != null) await processorError();
                        var token = CancellationTokenSource.Token;
#if DEBUG
                        Debug.WriteLine(
                            $"A real cancellation, triggered by the caller, token is: {token}, message: {ex.Message}, url: {requestUri}");
#endif
                    }
                    else
                    {
                        if (processorError != null) await processorError();
#if DEBUG
                        Debug.WriteLine($"A web request timeout, message: {ex.Message}, url: {requestUri}");
#endif
                    }
                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
                    if (processorError != null) await processorError();
#if DEBUG
                    Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        #endregion

        #region Post task Async

        public async Task PostTaskAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, Task> processorSuccess, Func<Task> processorError = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var stringContent = keyvalues.KeyValuePairToStringContent(requestUri);
                    var responseMessage = await PostAsync(requestUri, stringContent, CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        responseMessage = responseMessage.EnsureSuccessStatusCode();
                        using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                            if (result == null)
                            {
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                if (processorSuccess != null) await processorSuccess(result);
                            }
                        }
                    }
                }
                catch (TaskCanceledException taskCanceledException)
                {
#if DEBUG
                    Debug.WriteLine(
                        taskCanceledException.CancellationToken == CancellationTokenSource.Token
                            ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                            : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        public async Task PostTaskAsync<T>(string requestUri, HttpContent content, Func<T, Task> processorSuccess,
            Func<Task> processorError = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var responseMessage = await PostAsync(requestUri, content, CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        responseMessage = responseMessage.EnsureSuccessStatusCode();
                        using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                            if (result == null)
                            {
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                if (processorSuccess != null) await processorSuccess(result);
                            }
                        }
                    }
                }
                catch (TaskCanceledException taskCanceledException)
                {
#if DEBUG
                    Debug.WriteLine(
                        taskCanceledException.CancellationToken == CancellationTokenSource.Token
                            ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                            : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();

                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        public async Task<T> PostTaskAsync<T>(string requestUri, HttpContent content) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }

            InitTokenSource();
            CancellationTokenSource.CancelAfter(this.Timeout);
            try
            {
                var responseMessage = await PostAsync(requestUri, content, CancellationTokenSource.Token);
                if (responseMessage == null)
                {
                    return null;
                }
                else
                {
                    responseMessage = responseMessage.EnsureSuccessStatusCode();
                    if (await IsTokenExpired(responseMessage))
                    {
                        await TokenExpiredExecute();
                        return null;
                    }

                    using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                        return result;
                    }
                }
            }
            catch (TaskCanceledException taskCanceledException)
            {
#if DEBUG
                Debug.WriteLine(
                    taskCanceledException.CancellationToken == CancellationTokenSource.Token
                        ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, " +
                          $"message: {taskCanceledException.Message}, url: {requestUri}"
                        : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                return null;
            }
            catch (UnauthorizedAccessException unAuEx)
            {
                //await MessagePopup.Instance.Show("Invalid API call");
                return null;
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                return null;
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        public async Task PostTaskAsync(string requestUri, HttpContent content, Func<object, Task> processorSuccess,
            Func<Task> processorError = null, object callbackObject = null, HttpMessageHandler httpMesssageHandler = null)
        {
            //BaseClient = httpMesssageHandler == null ? new HttpClient() { Timeout = TimeSpan.FromSeconds(TimeoutSeconds) } :
            //    new HttpClient(httpMesssageHandler) { Timeout = TimeSpan.FromSeconds(TimeoutSeconds) };

            InitTokenSource();
            CancellationTokenSource.CancelAfter(this.Timeout);

            try
            {
                var responseMessage = await PostAsync(requestUri, content, CancellationTokenSource.Token);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    await processorSuccess(callbackObject);
                }
                else
                {
                    if (processorError != null)
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                        }

                        await processorError();
                    }
                }

            }
            catch (TaskCanceledException taskCanceledException)
            {
#if DEBUG
                Debug.WriteLine(
                    taskCanceledException.CancellationToken == CancellationTokenSource.Token
                        ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, " +
                          $"message: {taskCanceledException.Message}, url: {requestUri}"
                        : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                if (processorError != null) await processorError();
            }
            catch (UnauthorizedAccessException unAuEx)
            {
               // await MessagePopup.Instance.Show("Invalid API call");
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                if (processorError != null) await processorError();
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        public async Task<T> PostImageTaskAsync<T>(string requestUri, HttpContent content) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }

            InitTokenSource();
            try
            {
                var responseMessage = await PostAsync(requestUri, content, CancellationTokenSource.Token);
                if (responseMessage == null)
                {
                    return null;
                }
                else
                {
                    responseMessage = responseMessage.EnsureSuccessStatusCode();

                    if (await IsTokenExpired(responseMessage))
                    {
                        await TokenExpiredExecute();
                        return null;
                    }

                    using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                        return result;
                    }
                }
            }
            catch (TaskCanceledException taskCanceledException)
            {
#if DEBUG
                Debug.WriteLine(
                    taskCanceledException.CancellationToken == CancellationTokenSource.Token
                        ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, " +
                          $"message: {taskCanceledException.Message}, url: {requestUri}"
                        : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                return null;
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                return null;
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        public async Task PostTaskAsyncCallback<T>(string requestUri,
            IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, object, Task> processorSuccess, Func<Task> processorError = null,
            object callbackObject = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
                return;
            }

            InitTokenSource();
            CancellationTokenSource.CancelAfter(this.Timeout);
            try
            {
                var stringContent = keyvalues.KeyValuePairToStringContent(requestUri);
                var responseMessage = await PostAsync(requestUri, stringContent, CancellationTokenSource.Token);
                if (responseMessage == null)
                {
                    if (processorError != null) await processorError();
                }
                else
                {
                    responseMessage = responseMessage.EnsureSuccessStatusCode();
                    using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var seObj = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);
                        if (seObj == null)
                        {
#if DEBUG
                            Debug.WriteLine($"================Can't parse json to object: {nameof(T)}");
#endif
                            if (processorError != null) await processorError();
                        }
                        else
                        {
                            await processorSuccess(seObj, callbackObject);
                        }
                    }
                }
            }
            catch (TaskCanceledException taskCanceledException)
            {
#if DEBUG
                Debug.WriteLine(
                    taskCanceledException.CancellationToken == CancellationTokenSource.Token
                        ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                        : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                if (processorError != null) await processorError();

            }
            catch (UnauthorizedAccessException unAuEx)
            {
                //await MessagePopup.Instance.Show("Invalid API call");
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine(
                    $"Your request will be terminal with error: {exception.Message}, url: {requestUri}");
#endif
                if (processorError != null) await processorError();
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        public async Task PostTaskAsyncCallback<T>(string requestUri, HttpContent content,
            Func<T, object, Task> processorSuccess, Func<Task> processorError = null,
            object callbackObject = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var responseMessage = await PostAsync(requestUri, content, CancellationTokenSource.Token);
                    responseMessage = responseMessage.EnsureSuccessStatusCode();
                    using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var seObj = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);
                        if (seObj == null)
                        {
#if DEBUG
                            Debug.WriteLine($"================Can't parse json to object: {nameof(T)}");
#endif
                            if (processorError != null) await processorError();
                        }
                        else
                        {
                            await processorSuccess(seObj, callbackObject);
                        }
                    }
                }
                catch (TaskCanceledException taskCanceledException)
                {
#if DEBUG
                    Debug.WriteLine(
                        taskCanceledException.CancellationToken == CancellationTokenSource.Token
                            ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                            : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine(
                        $"Your request will be terminal with error: {exception.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        #endregion

        #region Delete Task Async

        /// <summary>
        /// delele task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="processorSuccess"></param>
        /// <param name="processorError"></param>
        /// <returns></returns>
        public async Task<T> DeleteTaskAsync<T>(string requestUri, Func<T, Task> processorSuccess = null,
            Func<Task> processorError = null) where T : class
        {
#if DEBUG
            Debug.WriteLine($"Delete method with url: {requestUri}");
#endif
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null)
                {
                    await processorError();
                }
                return null;
            }

            InitTokenSource();
            CancellationTokenSource.CancelAfter(this.Timeout);
            try
            {
                var responseMessage = await DeleteAsync(requestUri, CancellationTokenSource.Token);
                if (responseMessage == null)
                {
                    if (processorError != null)
                    {
                        await processorError();
                    }
                    return null;
                }
                else
                {
                    if (await IsTokenExpired(responseMessage))
                    {
                        await TokenExpiredExecute();
                        return null;
                    }

                    responseMessage = responseMessage.EnsureSuccessStatusCode();
                    using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                        if (result == null)
                        {
                            if (processorError != null)
                            {
                                await processorError();
                            }
                            return null;
                        }
                        else
                        {
                            if (processorSuccess != null)
                            {
                                await processorSuccess(result);
                            }
                            return result;
                        }
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == CancellationTokenSource.Token)
                {
                    if (processorError != null) await processorError();
                    var token = CancellationTokenSource.Token;
#if DEBUG
                    Debug.WriteLine(
                        $"A real cancellation, triggered by the caller, token is: {token}, message: {ex.Message}, url: {requestUri}");
#endif
                }
                else
                {
                    if (processorError != null) await processorError();
#if DEBUG
                    Debug.WriteLine($"A web request timeout, message: {ex.Message}, url: {requestUri}");
#endif
                }
                return null;
            }
            catch (UnauthorizedAccessException unAuEx)
            {
                //await MessagePopup.Instance.Show("Invalid API call");
                return null;
            }
            catch (Exception exception)
            {
                if (processorError != null) await processorError();
#if DEBUG
                Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                return null;
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        /// <summary>
        /// delete task with call back
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="processorSuccess"></param>
        /// <param name="processorError"></param>
        /// <param name="callbackObject"></param>
        /// <returns></returns>
        public async Task DeleteTaskAsyncCallback<T>(string requestUri, Func<T, object, Task> processorSuccess,
            Func<Task> processorError = null, object callbackObject = null) where T : class
        {
#if DEBUG
            Debug.WriteLine($"Delete method with url: {requestUri}");
#endif
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var responseMessage = await DeleteAsync(requestUri, CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        responseMessage = responseMessage.EnsureSuccessStatusCode();
                        using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                            if (result == null)
                            {
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                await processorSuccess(result, callbackObject);
                            }
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (ex.CancellationToken == CancellationTokenSource.Token)
                    {
                        if (processorError != null) await processorError();
                        var token = CancellationTokenSource.Token;
#if DEBUG
                        Debug.WriteLine(
                            $"A real cancellation, triggered by the caller, token is: {token}, message: {ex.Message}, url: {requestUri}");
#endif
                    }
                    else
                    {
                        if (processorError != null) await processorError();
#if DEBUG
                        Debug.WriteLine($"A web request timeout, message: {ex.Message}, url: {requestUri}");
#endif
                    }
                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
                    if (processorError != null) await processorError();
#if DEBUG
                    Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        #endregion

        #region Put Task Async

        public async Task<T> PutTaskAsync<T>(string requestUri, HttpContent content = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return null;
            }

            InitTokenSource();
            CancellationTokenSource.CancelAfter(this.Timeout);
            try
            {
                var responseMessage = await PutAsync(requestUri, content, CancellationTokenSource.Token);
                if (responseMessage == null)
                {
                    return null;
                }
                else
                {
                    responseMessage = responseMessage.EnsureSuccessStatusCode();

                    if (await IsTokenExpired(responseMessage))
                    {
                        await TokenExpiredExecute();
                        return null;
                    }

                    using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                        return result;
                    }
                }
            }
            catch (TaskCanceledException taskCanceledException)
            {
#if DEBUG
                Debug.WriteLine(
                    taskCanceledException.CancellationToken == CancellationTokenSource.Token
                        ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, " +
                          $"message: {taskCanceledException.Message}, url: {requestUri}"
                        : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                return null;
            }
            catch (UnauthorizedAccessException unAuEx)
            {
                //await MessagePopup.Instance.Show("Invalid API call");
                return null;
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                return null;
            }
            finally
            {
                DisposeTokenSource();
            }
        }

        public async Task PutTaskAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, Task> processorSuccess, Func<Task> processorError = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var stringContent = keyvalues.KeyValuePairToStringContent(requestUri);
                    var responseMessage = await PutAsync(requestUri, stringContent, CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        responseMessage = responseMessage.EnsureSuccessStatusCode();
                        using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                            if (result == null)
                            {
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                await processorSuccess(result);
                            }
                        }
                    }
                }
                catch (TaskCanceledException taskCanceledException)
                {
#if DEBUG
                    Debug.WriteLine(
                        taskCanceledException.CancellationToken == CancellationTokenSource.Token
                            ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                            : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();

                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        public async Task PutTaskAsync<T>(string requestUri, HttpContent content, Func<T, Task> processorSuccess,
            Func<Task> processorError = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var responseMessage = await PutAsync(requestUri, content, CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        responseMessage = responseMessage.EnsureSuccessStatusCode();
                        using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            var result = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);

                            if (result == null)
                            {
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                await processorSuccess(result);
                            }
                        }
                    }
                }
                catch (TaskCanceledException taskCanceledException)
                {
#if DEBUG
                    Debug.WriteLine(
                        taskCanceledException.CancellationToken == CancellationTokenSource.Token
                            ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                            : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine($"Server die with error: {exception.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        public async Task PutTaskAsyncCallback<T>(string requestUri,
            IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, object, Task> processorSuccess, Func<Task> processorError = null,
            object callbackObject = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var stringContent = keyvalues.KeyValuePairToStringContent(requestUri);
                    var responseMessage = await PutAsync(requestUri, stringContent, CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        using (responseMessage = responseMessage.EnsureSuccessStatusCode())
                        {
                            var jsonStream = await responseMessage.Content.ReadAsStreamAsync();
                            var seObj = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);
                            if (seObj == null)
                            {
#if DEBUG
                                Debug.WriteLine($"================Can't parse json to object: {nameof(T)}");
#endif
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                await processorSuccess(seObj, callbackObject);
                            }
                        }
                    }
                }
                catch (TaskCanceledException taskCanceledException)
                {
#if DEBUG
                    Debug.WriteLine(
                        taskCanceledException.CancellationToken == CancellationTokenSource.Token
                            ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                            : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine(
                        $"Your request will be terminal with error: {exception.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        public async Task PutTaskAsyncCallback<T>(string requestUri, HttpContent content,
            Func<T, object, Task> processorSuccess, Func<Task> processorError = null,
            object callbackObject = null) where T : class
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (processorError != null) await processorError();
            }
            else
            {
                InitTokenSource();
                CancellationTokenSource.CancelAfter(this.Timeout);
                try
                {
                    var responseMessage = await PutAsync(requestUri, content, CancellationTokenSource.Token);
                    if (responseMessage == null)
                    {
                        if (processorError != null) await processorError();
                    }
                    else
                    {
                        if (await IsTokenExpired(responseMessage))
                        {
                            await TokenExpiredExecute();
                            return;
                        }

                        responseMessage = responseMessage.EnsureSuccessStatusCode();
                        using (var jsonStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            var seObj = this.CrossJsonSerializer.DeserializeFromJsonStream<T>(jsonStream, requestUri);
                            if (seObj == null)
                            {
#if DEBUG
                                Debug.WriteLine($"================Can't parse json to object: {nameof(T)}");
#endif
                                if (processorError != null) await processorError();
                            }
                            else
                            {
                                await processorSuccess(seObj, callbackObject);
                            }
                        }
                    }
                }
                catch (TaskCanceledException taskCanceledException)
                {
#if DEBUG
                    Debug.WriteLine(
                        taskCanceledException.CancellationToken == CancellationTokenSource.Token
                            ? $"A real cancellation, triggered by the caller, token is: {CancellationTokenSource.Token}, message: {taskCanceledException.Message}, url: {requestUri}"
                            : $"A web request timeout, message: {taskCanceledException.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                catch (UnauthorizedAccessException unAuEx)
                {
                    //await MessagePopup.Instance.Show("Invalid API call");
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine(
                        $"Your request will be terminal with error: {exception.Message}, url: {requestUri}");
#endif
                    if (processorError != null) await processorError();
                }
                finally
                {
                    DisposeTokenSource();
                }
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose of everything
        /// </summary>
        public static void Dispose()
        {

        }

        #endregion

        #region Token Expired

        private async Task<bool> IsTokenExpired(object response)
        {
            try
            {
                if (((HttpResponseMessage)response).StatusCode == HttpStatusCode.Unauthorized)
                {
                    try
                    {
                        var result = await ((HttpResponseMessage)response).Content.ReadAsStreamAsync();
                        var a = (HttpResponseMessage)response;
                        var b = a.EnsureSuccessStatusCode();
                        //var res = CrossJsonSerializer.DeserializeFromJsonStream<CrossPayJObjectResponse>(result, "");

                        //if (res.ErrorCode == Define.ErrorCodeTokenExpiration)
                        return true;
                    }
                    catch (Exception)
                    {
                        var result = await ((HttpResponseMessage)response).Content.ReadAsStreamAsync();
                        //var res = CrossJsonSerializer.DeserializeFromJsonStream<CrossPayJArrayResponse>(result, "");

                        //if (res.ErrorCode == Define.ErrorCodeTokenExpiration)
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#endif
            }
            return false;
        }

        private async Task TokenExpiredExecute()
        {
//            try
//            {
//                var vm = PageManager.GetCurrentPageBaseViewModel();

//                var pageViewModel = vm.ToString();

//                vm.IsTokenExpire = true;

//                await MessagePopup.Instance.Show(TranslateExtension.Get("YourSessionIsExpired"),
//                        closeButtonText: TranslateExtension.Get("OK"),
//                        closeCommand: new Command(async () =>
//                        {
//                            await vm.LogOutAsync(pageViewModel: pageViewModel);
//                        }));
//            }
//            catch (Exception e)
//            {
//#if DEBUG
//                Debug.WriteLine(e);
//#endif
//            }
        }

        #endregion

        #region Work with CancellationTokenSource

        public void CancelTokenSource()
        {
            if (CancellationTokenSource != null)
            {
                try
                {
                    CancellationTokenSource.Cancel();
                }
                catch (Exception ex)
                { }
            }
        }

        private void InitTokenSource()
        {
            CancellationTokenSource = new CancellationTokenSource();
            DidCancellationTokenSourceDispose = false;
        }

        private void DisposeTokenSource()
        {
            if (!DidCancellationTokenSourceDispose)
            {
                try
                {
                    CancellationTokenSource.Dispose();
                    DidCancellationTokenSourceDispose = true;
                    CancellationTokenSource = new CancellationTokenSource();
                }
                catch (Exception ex)
                { }
            }
        }

        #endregion
    }
}
