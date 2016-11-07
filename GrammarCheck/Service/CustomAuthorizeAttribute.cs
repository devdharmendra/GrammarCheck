using GrammarCheck.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GrammarCheck.Service
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public AuthorizationDto _AuthorizationObj = new AuthorizationDto();
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                _AuthorizationObj.status = false;
                _AuthorizationObj.message = Messages.not_authorize;
                actionContext.Response = actionContext.Request.CreateResponse<AuthorizationDto>(HttpStatusCode.Unauthorized, _AuthorizationObj);
            }

            if (SkipAuthorization(actionContext)) return;

            if (!IsAuthorized(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }

        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            _AuthorizationObj.status = false;
            _AuthorizationObj.message = Messages.not_authorize;
            actionContext.Response = actionContext.Request.CreateResponse<AuthorizationDto>(HttpStatusCode.Unauthorized, _AuthorizationObj);
        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return JWTService.AuthenticateUser(actionContext);
        }
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}