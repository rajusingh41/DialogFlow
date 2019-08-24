using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DialogFlowUI
{
    public class AppUser : ClaimsPrincipal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        public AppUser(ClaimsPrincipal principal)
       : base(principal)
        {
        }

        /// <summary>
        /// Get login user id 
        /// this use for createBy and updateBy in query parameter and other reference 
        /// </summary>
        public int UserId
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Get login user company code / domain code 
        /// </summary>
        public string DomainCode
        {
            get { return "dgn5"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return "Raju Singh"; }
        }
    }
}