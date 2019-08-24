using System.Security.Claims;
using System.Web.Http;

namespace DialogFlowUI
{
    public class BaseController : ApiController
    {

        /// <summary>
        /// this is use for pass authorization token value to all controller action
        /// </summary>
        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(User as ClaimsPrincipal);
            }
        }
    }
}
