using GrammarCheck.Models;
using GrammarCheck.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GrammarCheck.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        /// <summary>
        /// User Login
        /// </summary>
        [Route("api/login")]
        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserLoginDto _UserLoginDto)
        {
            UserLoginResponse _ResponseObj = new UserLoginResponse();
            try
            {
                if (!string.IsNullOrEmpty(_UserLoginDto.username) && !string.IsNullOrEmpty(_UserLoginDto.password))
                {
                    if (_UserLoginDto.username == "kelvin" && _UserLoginDto.password == "kelvin123")
                    {
                        string userId = "123456";
                        _ResponseObj.status = true;
                        _ResponseObj.message = Messages.successful_login;
                        _ResponseObj.token = JWTService.CreateJwtToken(userId);
                        return Request.CreateResponse<UserLoginResponse>(HttpStatusCode.OK, _ResponseObj);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Messages.incorrect_credentials);
                    }
                }
                if (string.IsNullOrEmpty(_UserLoginDto.username))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Messages.username_required);
                }
                if (string.IsNullOrEmpty(_UserLoginDto.password))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Messages.password_required);
                }
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Messages.try_again);
            }
            catch (Exception Ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex.Message);
            }
        }
    }
}
