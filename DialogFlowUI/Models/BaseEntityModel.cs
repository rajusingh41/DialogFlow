using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DialogFlowUI
{
    public class BaseEntityModel
    {
        public string DomainCode { get; set; }
        public int LogOnUserId { get; set; }

        public string UserName { get; set; }
    }
}