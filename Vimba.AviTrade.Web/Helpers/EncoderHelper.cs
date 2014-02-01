using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class EncoderHelper
    {
        //From -> To
        static Dictionary<string, string> charapterMap = new Dictionary<string, string>
        {
            { "#", "Sharp" },
            { "Ñ", "N" },
            { "ñ", "n" },
            { "á", "a" },
            { "é", "e" },
            { "í", "i" },
            { "ó", "o" },
            { "ú", "u" },
            { "Á", "A" },
            { "É", "E" },
            { "Í", "I" },
            { "Ó", "O" },
            { "Ú", "U" },
            { "ü", "u" },
            { "Ü", "U" },
            { " ", "-" },
            { ":", "" },
            { ".", "dot-"},
            { "[", ""},
            { "]", ""},
            { "{", ""},
            { "}", ""},
            { "/", ""},
            { "\\", ""},
            { ";", ""}
        };

        private static string Encode(string text)
        {
            var final = (from actualChar in text
                         let str = actualChar.ToString()
                         let isValid = !charapterMap.ContainsKey(str)
                         select isValid ? actualChar.ToString() : charapterMap[str])
                            .Aggregate(string.Empty, (accum, actual) => accum + actual);
            return final;
        }
    }
}