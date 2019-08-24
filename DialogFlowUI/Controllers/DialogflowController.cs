using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Grpc.Auth;
using Grpc.Core;
using System;
using System.Configuration;
using System.Globalization;
using System.Web.Http;

namespace DialogFlowUI
{
    [RoutePrefix("api/dialogflow")]
    public class DialogflowController : BaseController
    {

        /// <summary>
        /// 
        /// </summary>
        public static string Language { get { return "en-US"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string WelcomeMessage
        {
            get
            {
                return @"Hi {0} <br/>I am Joy. I can assist you in HR related queries.</br>Some things you can ask me: Profile, Leave balance, Open task, Attendance, Salary Slip.";
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public static string WelcomeExceptionMessage { get { return "Sorry. I could not connect the HR database. You may ask your question a little later."; } }

        ///// <summary>
        ///// InitiateConversation
        ///// (Ignore.Action)
        ///// </summary>
        ///// <returns></returns>
        //[Route("")]
        //[HttpGet]
        //public IHttpActionResult InitiateConversation()
        //{
        //    IEnumerable<string> headerValues;
        //    var authFilter = string.Empty;
        //    if (Request.Headers.TryGetValues("Authorization", out headerValues))
        //    {
        //        authFilter = headerValues.FirstOrDefault();
        //    }
        //    //create sessionId
        //    var joyGuid = Guid.NewGuid();
        //    var clientSessions = CreateDialogFlowClientSessions();
        //    var model = new BaseEntityModel { DomainCode = CurrentUser.DomainCode, LogOnUserId = CurrentUser.UserId };
        //    var conversationContext = new ConversationContext
        //    {
        //        ContextGuid = joyGuid,
        //        ContextText = InitConversation(clientSessions, authFilter, CurrentUser.UserId.ToString(CultureInfo.CurrentCulture), joyGuid, model)
        //    };
        //    return Ok(conversationContext);
        //}

        ///// <summary>
        ///// RequestToDialogFlow (Ignore.Action)
        ///// </summary>
        ///// <remarks>
        /////     { 
        /////     "joyGuid": "00000000-0000-0000-0000-000000000000", 
        /////     "joyText": "string" 
        /////     } 
        ///// </remarks>
        ///// <param name="conversion"></param>
        ///// <returns>string</returns>
        //[Route("")]
        //[HttpPost]
        //public IHttpActionResult RequestToDialogFlow([FromBody] JoyConversation conversion)
        //{
        //    if (conversion == null)
        //    {
        //        throw new ArgumentNullException(nameof(conversion));
        //    }
        //    IEnumerable<string> headerValues;
        //    var authFilter = string.Empty;
        //    var clientSessions = CreateDialogFlowClientSessions();
        //    if (Request.Headers.TryGetValues(ConstDialogFlow.DialogFlowAuthorizationHeader, out headerValues))
        //    {
        //        authFilter = headerValues.FirstOrDefault();
        //    }
        //    var baseEntityModel = new BaseEntityModel { DomainCode = CurrentUser.DomainCode, LogOnUserId = CurrentUser.UserId };
        //    var result = GetAgentResponse(clientSessions, conversion.JoyText, CurrentUser.UserId.ToString(CultureInfo.CurrentCulture), authFilter, conversion.JoyGuid, baseEntityModel);
        //    return Ok(result);
        //}



        /// <summary>
        /// GetAgentResponse
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="joyText"></param>
        /// <param name="joyUserId"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        private string GetAgentResponse(SessionsClient clientSessions, string joyText, string joyUserId, string authFilter, Guid joyGuid, BaseEntityModel model)
        {
            string result = string.Empty;

            var response = clientSessions.DetectIntent(
                    session: new SessionName(ConfigurationManager.AppSettings["DialogFlowAgentId"], joyGuid.ToString("N", CultureInfo.CurrentCulture)),
                    queryInput: new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = joyText,
                            LanguageCode = Language
                        }
                    }
                );

            var queryResult = response.QueryResult;
            result = queryResult.FulfillmentText;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyUserId"></param>
        /// <param name="joyGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        private string ReInitConversation(SessionsClient clientSessions, string authFilter, string joyUserId, Guid joyGuid, BaseEntityModel model)
        {
            return Conversation(clientSessions, authFilter, joyUserId, joyGuid, true, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyUserId"></param>
        /// <param name="joyGuid"></param>
        /// <param name="isReInit"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        private string Conversation(SessionsClient clientSessions, string authFilter, string joyUserId, Guid joyGuid, bool isReInit, BaseEntityModel model)
        {
            EventInput evnt = new EventInput
            {
                Name = isReInit ? "reinit_conversation_event" : "custom_welcome_event",
                LanguageCode = Language
            };
            evnt.Parameters = new Google.Protobuf.WellKnownTypes.Struct();

            Google.Protobuf.WellKnownTypes.Value v1 = new Google.Protobuf.WellKnownTypes.Value();
            v1.StringValue = joyUserId;
            Google.Protobuf.WellKnownTypes.Value v2 = new Google.Protobuf.WellKnownTypes.Value();
            v2.StringValue = authFilter;

            evnt.Parameters.Fields.Add("user_name", v1);
            evnt.Parameters.Fields.Add("token", v2);

            var response = clientSessions.DetectIntent(
                session: new SessionName(ConfigurationManager.AppSettings["DialogFlowAgentId"], joyGuid.ToString("N", CultureInfo.CurrentCulture)),
                queryInput: new QueryInput
                {
                    Event = evnt
                }
            );

            var queryResult = response.QueryResult;
            return string.IsNullOrEmpty(queryResult.FulfillmentText) ? WelcomeExceptionMessage
                                                                      : string.Format(CultureInfo.CurrentCulture, WelcomeMessage, "Raju Singh");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyUserId"></param>
        /// <param name="joyGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [NonAction]
        private string InitConversation(SessionsClient clientSessions, string authFilter, string joyUserId, Guid joyGuid, BaseEntityModel model)
        {
            return Conversation(clientSessions, authFilter, joyUserId, joyGuid, false, model);
        }



        [NonAction]
        private SessionsClient CreateDialogFlowClientSessions()
        {
            var dialogFlowConfigurationFilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/hrone-wxhfau-221616b3f7be.json");
            GoogleCredential googleCredential = GoogleCredential.FromFile(dialogFlowConfigurationFilePath);
            Channel channel = new Channel(SessionsClient.DefaultEndpoint.Host,
                                          googleCredential.ToChannelCredentials());
            return SessionsClient.Create(channel);
        }
    }
}
