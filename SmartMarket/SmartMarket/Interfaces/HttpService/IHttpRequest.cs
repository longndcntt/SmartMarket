using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SmartMarket.Interfaces.HttpService
{
    public interface IHttpRequest
    {

        #region Properties

        HttpClient BaseClient { get; }
        Uri BaseAddress { get; set; }
        long MaxResponseContentBufferSize { get; set; }
        HttpRequestHeaders DefaultRequestHeaders { get; }
        TimeSpan Timeout { get; set; }
        CancellationTokenSource CancellationTokenSource { get; set; }

        #endregion

        #region Task

        #region Post Task

        Task PostTaskAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, Task> processorSuccess, Func<Task> processorError = null) where T : class;

        Task PostTaskAsync<T>(string requestUri, HttpContent content, Func<T, Task> processorSuccess,
            Func<Task> processorError = null) where T : class;

        Task<T> PostTaskAsync<T>(string requestUri, HttpContent content) where T : class;

        Task PostTaskAsync(string requestUri, HttpContent content, Func<object, Task> processorSuccess, Func<Task> processorError = null,
            object callbackObject = null, HttpMessageHandler httpMesssageHandler = null);

        Task<T> PostImageTaskAsync<T>(string requestUri, HttpContent content) where T : class;

        Task PostTaskAsyncCallback<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, object, Task> processorSuccess, Func<Task> processorError = null,
            object callbackObject = null) where T : class;

        Task PostTaskAsyncCallback<T>(string requestUri, HttpContent content, Func<T, object, Task> processorSuccess,
            Func<Task> processorError = null, object callbackObject = null) where T : class;

        #endregion

        #region Get Task

        Task<T> GetTaskAsync<T>(string requestUri, Func<T, Task> processorSuccess = null,
            Func<Task> processorError = null) where T : class;

        Task<T> GetObjectTaskAsync<T>(string requestUri, Func<T, Task> processorSuccess = null,
            Func<Task> processorError = null) where T : class;

        Task GetTaskAsyncCallback<T>(string requestUri, Func<T, object, Task> processorSuccess,
            Func<Task> processorError = null, object callbackObject = null) where T : class;

        #endregion

        #region Delete Task

        Task<T> DeleteTaskAsync<T>(string requestUri, Func<T, Task> processorSuccess = null,
            Func<Task> processorError = null) where T : class;

        Task DeleteTaskAsyncCallback<T>(string requestUri, Func<T, object, Task> processorSuccess,
            Func<Task> processorError = null, object callbackObject = null) where T : class;

        #endregion

        #region Put Task

        Task<T> PutTaskAsync<T>(string requestUri, HttpContent content = null) where T : class;

        Task PutTaskAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, Task> processorSuccess, Func<Task> processorError = null) where T : class;

        Task PutTaskAsync<T>(string requestUri, HttpContent content, Func<T, Task> processorSuccess,
            Func<Task> processorError = null) where T : class;

        Task PutTaskAsyncCallback<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> keyvalues,
            Func<T, object, Task> processorSuccess, Func<Task> processorError = null,
            object callbackObject = null) where T : class;

        Task PutTaskAsyncCallback<T>(string requestUri, HttpContent content, Func<T, object, Task> processorSuccess,
            Func<Task> processorError = null, object callbackObject = null) where T : class;

        #endregion

        #endregion

        void CancelTokenSource();
    }
}
