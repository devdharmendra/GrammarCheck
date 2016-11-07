using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GrammarCheck.Models
{
    public class Response
    {
        public string ResultText { get; set; }
        public int ChangedCount { get; set; }
    }

    public class Suggestions
    {
        public string Text { get; set; }
        public List<string> SuggestedText { get; set; }
    }

}