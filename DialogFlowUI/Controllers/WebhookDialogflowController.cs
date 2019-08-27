using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DialogFlowUI
{
    [RoutePrefix("api/webhookDialogflow")]
    public class WebhookDialogflowController : BaseController
    {
        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// https://medium.com/@lifei.8886196/integration-of-google-dialogflow-webhook-fullfillment-and-net-core-web-api-using-c-2a2edcba01f2
        /// </remarks>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult GetWebhookResponse()
        {
           
            WebhookRequest request;
            using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                request = jsonParser.Parse<WebhookRequest>(reader);
            }

            var actionType = request.QueryResult.Action;
            var parameters = request.QueryResult.Parameters;
            var response = new WebhookResponse();
            switch (actionType)
            {
                case "input.welcome":
                    response.FulfillmentText = $"Hi {CurrentUser.UserName}, I am trip palnner agent how can help you.";
                    return Ok(response);
                case "input.flight":
                    var flightDate = parameters.Fields["date"].ToString();
                    var flyingFrom = parameters.Fields["flyingFrom"].ToString();
                    var flyingTo = parameters.Fields["flyingTo"].ToString();
                    response.FulfillmentText = $"Congrax your flight from {flyingFrom} to {flyingTo} booked for {flightDate}";
                    return Ok(response);
                case "input.hotal":
                    response.FulfillmentText = "your hotal has been booked";
                    return Ok(response);
                default:
                    response.FulfillmentText = "Sorry ask somting else";
                    return Ok(response);
            }
        }


         /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [Route("Test")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetResponse()
        {
            WebhookRequest request;
            using (var stream = await Request.Content.ReadAsStreamAsync())
            {
                using (var reader = new StreamReader(stream))
                {
                    request = jsonParser.Parse<WebhookRequest>(reader);
                }
            }

            var actionType = request.QueryResult.Action;
            var parameters = request.QueryResult.Parameters;
            var webhookResponse = new WebhookResponse();
            var _response = Request.CreateResponse();
            switch (actionType)
            {
                case "input.welcome":
                    webhookResponse.FulfillmentText = $"Hi {CurrentUser.UserName}, I am trip palnner agent how can help you.";
                    return httpResponceMessage(webhookResponse, _response);
                case "input.flight":
                    var flightDate = parameters.Fields["date"].ToString();
                    var flyingFrom = parameters.Fields["flyingFrom"].ToString();
                    var flyingTo = parameters.Fields["flyingTo"].ToString();
                    webhookResponse.FulfillmentText = $"Congrax your flight from {flyingFrom} to {flyingTo} booked for {flightDate}";
                    return httpResponceMessage(webhookResponse, _response);
                case "input.hotal":
                    webhookResponse.FulfillmentText = "your hotal has been booked";
                    _response.Content = new StringContent(webhookResponse.ToString());
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return _response;
                default:
                    webhookResponse.FulfillmentText = "Sorry ask somting else";
                    _response.Content = new StringContent(webhookResponse.ToString());
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return _response;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webhookResponse"></param>
        /// <param name="_response"></param>
        /// <returns></returns>

        private static HttpResponseMessage httpResponceMessage(WebhookResponse webhookResponse, HttpResponseMessage _response)
        {
            _response.Content = new StringContent(webhookResponse.ToString());
            _response.StatusCode = HttpStatusCode.OK;
            _response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return _response;
        }



    }
}
