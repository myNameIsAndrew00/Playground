using Service.ConfigurationAPI.Models.Response;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Service.ConfigurationAPI.Client
{
    public class ConfigurationAPIClient : IDisposable
    {
        private HttpClient client;

        private Uri baseAddress;

        /// <summary>
        /// Use this to store all long polling tasks registered for this client.
        /// </summary>
        private Dictionary<Guid, CancellationTokenSource> longPollingTasks;

        /// <summary>
        /// An event which triggers when a request is made. For long polling tasks, callback is no triggered.
        /// </summary>
        public event Action OnRequestStart;

        /// <summary>
        /// An event which triggers when a request finish. For long polling tasks, callback is no triggered.
        /// </summary>
        public event Action OnRequestEnd;


        public ConfigurationAPIClient(string address, string port, bool ignoreSsl)
        {
            if (ignoreSsl)
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;

                client = new HttpClient(handler);
            }
            else
            {
                client = new HttpClient();
            }

            baseAddress = new Uri(String.Concat(address, ':', port));

            longPollingTasks = new Dictionary<Guid, CancellationTokenSource>();
        }

        /// <summary>
        /// Initialise long polling session to server.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="frequency"></param>
        /// <returns>A guid which identifies the long polling. It can be used to stop the long polling from being executed.</returns>
        public Guid? StartLongPoll<DTOModel>(Endpoint endpoint, HttpMethod method, Action<StandardResponse<DTOModel>> callback, int frequency = 5)
        {
            /* Callback is mandatory for long polling*/
            if (callback is null)
                return null;

            var tokenSource = new CancellationTokenSource();

            var token = tokenSource.Token;

            /* Run a task which will execute a request every *frequency* seconds */
            var runningTask = Task.Run(async () =>
            {
                while (true)
                {
                    StandardResponse<DTOModel> requestResult = null;
                    try
                    {
                        if (token.IsCancellationRequested)
                            return;

                        await Task.Delay(frequency * 1000);

                        requestResult = await ExecuteRequest<DTOModel>(
                            endpoint: endpoint,
                            method: method,
                            triggerEvents: false);

                    }
                    catch
                    {
                        requestResult = null;
                    }

                    callback(requestResult);
                }
            }, token);

            Guid pollIdentifier = Guid.NewGuid();

            longPollingTasks.Add(pollIdentifier, tokenSource);

            return pollIdentifier;
        }

        /// <summary>
        /// Cancel the long polling for the given polling identifier.
        /// </summary>
        /// <param name="key"></param>
        public void CancelLongPolling(Guid key)
        {
            if (longPollingTasks.TryGetValue(key, out CancellationTokenSource cancellationSource))
            {
                cancellationSource.Cancel();

                longPollingTasks.Remove(key);
            }
        }

        /// <summary>
        /// Executes a get request to the service.
        /// </summary>
        /// <typeparam name="DTOModel"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<StandardResponse<DTOModel>> Get<DTOModel>(Endpoint endpoint) => await ExecuteRequest<DTOModel>(endpoint, HttpMethod.Get);

        /// <summary>
        /// Execute a post request to the service.
        /// </summary>
        /// <typeparam name="DTOModel"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<StandardResponse<DTOModel>> Post<DTOModel>(Endpoint endpoint) => await ExecuteRequest<DTOModel>(endpoint, HttpMethod.Post);




        public void Dispose()
        {
            // Release any long polling tasking running
            foreach (var longPollingTask in longPollingTasks)
                longPollingTask.Value.Cancel();

            client.Dispose();

            GC.SuppressFinalize(this);
        }

        #region Private

        private async Task<StandardResponse<DTOModel>> ExecuteRequest<DTOModel>(Endpoint endpoint, HttpMethod method, bool triggerEvents = true)
        {
            try
            {
                if (triggerEvents)
                    OnRequestStart?.Invoke();

                string endpointAddress = endpoint.GetEndpoint();

                HttpRequestMessage message = new HttpRequestMessage(method, new Uri(baseAddress, endpointAddress));

                var response = await client.SendAsync(message);

                string content = await response.Content.ReadAsStringAsync();

                if (triggerEvents)
                    OnRequestEnd?.Invoke();

                return JsonSerializer.Deserialize<StandardResponse<DTOModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                if (triggerEvents)
                    OnRequestEnd?.Invoke();

                return null;
            }
        }

        #endregion
    }
}