using GrammarCheck.Models;
using System.Collections.Generic;
using System.Configuration;

namespace GrammarCheck.Service
{
    public static class GrammarService
    {
        public static Response BulkChangeGrammarService(string Paragraph)
        {
            Response _ResponseObj = new Response();
            int counter = 0;
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(ConfigurationManager.AppSettings["GrammarFile"] + "grammar.txt");
            while ((line = file.ReadLine()) != null)
            {
                string prefix = line.Substring(0, line.IndexOf("|"));
                string suffix = line.Substring(line.IndexOf("|") + 1, line.Length - (prefix.Length + 1));
                if (suffix.Contains("("))
                {
                    suffix = suffix.Substring(0, suffix.IndexOf("(")).TrimEnd();
                }
                if (Paragraph.Contains(prefix))
                {
                    Paragraph = Paragraph.Replace(prefix, suffix);
                    counter++;
                }
            }
            _ResponseObj.ResultText = Paragraph;
            _ResponseObj.ChangedCount = counter;

            file.Close();
            return _ResponseObj;
        }

        public static List<Suggestions> GrammarSuggestionsService(string Paragraph)
        {
            List<Suggestions> _SuggestionsObj = new List<Suggestions>();
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(ConfigurationManager.AppSettings["GrammarFile"] + "grammar.txt");
            while ((line = file.ReadLine()) != null)
            {
                string prefix = line.Substring(0, line.IndexOf("|"));
                string suffix = line.Substring(line.IndexOf("|") + 1, line.Length - (prefix.Length + 1));
                if (suffix.Contains("("))
                {
                    suffix = suffix.Substring(0, suffix.IndexOf("(")).TrimEnd();
                }
                if (Paragraph.Contains(prefix))
                {
                    int index = _SuggestionsObj.FindIndex(item => item.Text == prefix);
                    if (index >= 0)
                    {
                        _SuggestionsObj[index].SuggestedText.Add(suffix);
                    }
                    else
                    {
                        Suggestions TempSuggestions = new Suggestions();
                        List<string> TempSuggestedText = new List<string>();
                        TempSuggestions.Text = prefix;
                        TempSuggestedText.Add(suffix);
                        TempSuggestions.SuggestedText = TempSuggestedText;
                        _SuggestionsObj.Add(TempSuggestions);
                    }
                }
            }
            file.Close();
            return _SuggestionsObj;
        }
    }
}