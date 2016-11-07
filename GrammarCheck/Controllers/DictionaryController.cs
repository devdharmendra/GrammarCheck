using GrammarCheck.Models;
using GrammarCheck.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GrammarCheck.Controllers
{
    [CustomAuthorize]
    public class DictionaryController : ApiController
    {
        /// <summary>
        /// Convert the entire paragraph with correct grammar as specified in the grammar file
        /// </summary>
        /// <param name="_DictionaryObj"></param>
        /// <returns></returns>
        [Route("api/bulkChangeGrammar")]
        [HttpPost]
        public HttpResponseMessage BulkChangeGrammar([FromBody]DictionaryDto _DictionaryObj)
        {
            Response _ResponseObj = new Response();
            try
            {
                if (string.IsNullOrEmpty(_DictionaryObj.text))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Messages.Bad_request_grammar);
                }
                else
                {
                    _ResponseObj = GrammarService.BulkChangeGrammarService(_DictionaryObj.text);
                    return Request.CreateResponse<Response>(HttpStatusCode.OK, _ResponseObj);
                }
            }
            catch(Exception Ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex.Message);
            }
        }

        /// <summary>
        /// Parse the entire paragraph and give suggestions for the words and phrases as specified in the grammar file
        /// </summary>
        /// <param name="_DictionaryObj"></param>
        /// <returns></returns>

        [Route("api/suggestions")]
        [HttpPost]
        public HttpResponseMessage GrammarSuggestions([FromBody]DictionaryDto _DictionaryObj)
        {
            List<Suggestions> _SuggestionsObj = new List<Suggestions>();
            try
            {
                if (string.IsNullOrEmpty(_DictionaryObj.text))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Messages.Bad_request_grammar);
                }
                else
                {
                    _SuggestionsObj= GrammarService.GrammarSuggestionsService(_DictionaryObj.text);
                    return Request.CreateResponse<List<Suggestions>>(HttpStatusCode.OK, _SuggestionsObj);
                }
            }
            catch (Exception Ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex.Message);
            }
        }
    }
}
