using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.Utilities
{
    public static class Common
    {
        public static string Pluralize(string s)
        {
            if (s.Length < 1)
                return s;
            char ultimateLetter = s.Last();
            string penultimateLetter = s.Substring(s.Length - 2, 1);
            if (ultimateLetter == 'y' && "aeiou".IndexOf(penultimateLetter) < 0)
            {
                return s.Substring(0, s.Length - 1) + "ies";
            }
            else
                if (ultimateLetter == 's' || (ultimateLetter == 'h' && penultimateLetter == "s"))
            {
                return s + "es";
            }
            else
            {
                return s + "s";
            }
        }
    }
}
