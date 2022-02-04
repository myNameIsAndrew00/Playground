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
        /// An event which triggers when a request is made.
        /// </summary>
        public event Action OnRequestStart;

        /// <summary>
        /// An event which triggers when a request finish.
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
            client.Dispose();

            GC.SuppressFinalize(this);
        }

        #region Private

        private async Task<StandardResponse<DTOModel>> ExecuteRequest<DTOModel>(Endpoint endpoint, HttpMethod method)
        {
            try
            {
                OnRequestStart?.Invoke();

                string endpointAddress = endpoint.GetEndpoint();

                HttpRequestMessage message = new HttpRequestMessage(method, new Uri(baseAddress, endpointAddress));

                var response = await client.SendAsync(message);

                string content = await response.Content.ReadAsStringAsync();

                OnRequestEnd?.Invoke();

                return JsonSerializer.Deserialize<StandardResponse<DTOModel>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}