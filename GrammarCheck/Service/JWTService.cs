using JWT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Script.Serialization;

namespace GrammarCheck.Service
{
    public class JWTService
    {
        #region JWT Authentication
        //Create JWT Token
        public static string CreateJwtToken(string userId)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"userId", userId},
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            string apikey = Convert.ToString(ConfigurationManager.AppSettings["JwtSecretKey"]);

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            return token;
        }

        //Validate JWT Token
        public static bool ValidateToken(string token, string secret, bool checkExpiration)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var payloadJson = JsonWebToken.Decode(token, secret);
            var payloadData = jsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
            object exp;
            if (payloadData != null && (checkExpiration && payloadData.TryGetValue("exp", out exp)))
            {
                var validTo = FromUnixTime(long.Parse(exp.ToString()));
                if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                {
                    throw new Exception(
                        string.Format("Token is expired. Expiration: '{0}'. Current: '{1}'", validTo, DateTime.UtcNow));
                }
            }

            return true;
        }

        //From Unix Time
        private static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static bool AuthenticateUser(HttpActionContext context)
        {
            bool IsAuthenticated = false;
            try
            {

                var secret = ConfigurationManager.AppSettings["JwtSecretKey"];
                string token = context.Request.Headers.Authorization.Parameter.ToString();
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }
                IsAuthenticated = ValidateToken(token, secret, true);
                return IsAuthenticated;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #endregion
    }
}