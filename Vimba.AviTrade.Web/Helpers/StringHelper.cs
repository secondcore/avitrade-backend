using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class StringHelper
    {
        private static Regex regStrip = new Regex(@"(<[^>]*>)|(\r)|(\n)");

        public static string Strip(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            return regStrip.Replace(text, string.Empty).Trim();
        }

        public static string NewLinesToHtml(this string text)
        {
            string result = "<p>" + text
                .Replace(Environment.NewLine + Environment.NewLine, "</p><p>")
                .Replace(Environment.NewLine, "<br />")
                .Replace("</p><p>", "</p>" + Environment.NewLine + "<p>") + "</p>";
            return result;
        }

        //The functions that follow I have found this url: http://weblogs.asp.net/jgalloway/archive/2005/09/27/426087.aspx
        /// <summary>
        /// Parses a camel cased or pascal cased string and returns a new
        /// string with spaces between the words in the string.
        /// </summary>
        /// <example>
        /// The string "PascalCasing" will return an array with two
        /// elements, "Pascal" and "Casing".
        /// </example>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SplitUpperCaseToString(this string source)
        {
            return string.Join(" ", SplitUpperCase(source));
        }

        /// <summary>
        /// Parses a camel cased or pascal cased string and returns an array
        /// of the words within the string.
        /// </summary>
        /// <example>
        /// The string "PascalCasing" will return an array with two
        /// elements, "Pascal" and "Casing".
        /// </example>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string[] SplitUpperCase(string source)
        {
            if (source == null)
                return new string[] { }; //Return empty array.

            if (source.Length == 0)
                return new string[] { "" };

            StringCollection words = new StringCollection();
            int wordStartIndex = 0;

            char[] letters = source.ToCharArray();
            // Skip the first letter. we don't care what case it is.
            for (int i = 1; i < letters.Length; i++)
            {
                if (char.IsUpper(letters[i]))
                {
                    //Grab everything before the current index.
                    words.Add(new String(letters, wordStartIndex, i - wordStartIndex));
                    wordStartIndex = i;
                }
            }

            //We need to have the last word.
            words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex));

            //Copy to a string array.
            string[] wordArray = new string[words.Count];
            words.CopyTo(wordArray, 0);
            return wordArray;
        }
    }
}