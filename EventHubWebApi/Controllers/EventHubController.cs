using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace EventHubWebApi.Controllers
{
    public class EventHubController : ApiController
    {
        private static readonly string sas = "YOUR_SAS_TOKEN";
        private static readonly string serviceNamespace = "YOUR_SERVICE_NAMESPACE";
        private static readonly string hubName = "YOUR_SERVICE_NAME";

        [HttpPost]
        public IHttpActionResult sendMessage(string value)
        {
            try
            {
                var response = SendHttpMessage(value);

                if (response.IsSuccessStatusCode == true)
                {
                    return Ok("-- Mensaje enviado");
                }
                else
                {
                    return Ok("-- Comunicación fallida");
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        public HttpResponseMessage SendHttpMessage(string message)
        {

            var url = string.Format("{0}/publishers/{1}/messages", hubName, "navegador");

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(string.Format("https://{0}.servicebus.windows.net/", serviceNamespace))
            };

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", sas);

            var content = new StringContent(message, Encoding.UTF8, "application/json");

            content.Headers.Add("ContentType", "application/atom+xml;type=entry;charset=utf-8");

            var response = httpClient.PostAsync(url, content).Result;

            return response;

        }

    }
}
