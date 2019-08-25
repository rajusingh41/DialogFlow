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
            //  byte[] request = await Request.Content.ReadAsByteArrayAsync();
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
                    var flyingTo= parameters.Fields["flyingTo"].ToString();
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

    }
}
