using System;
using System.Linq;
using System.Windows;

namespace RecipeBook.Utilities
{
    public static class Common
    {
        /// <summary>
        /// Using common English language rules, this method makes a noun plural
        /// </summary>
        /// <param name="s">singular noun</param>
        /// <returns>plural noun</returns>
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
        /// <summary>
        /// Creates or calls a window of type T
        /// </summary>
        /// <typeparam name="T">Derived Window</typeparam>
        /// <param name="col">Window collection</param>
        /// <returns></returns>
        public static Window NewWindow<T>() where T : Window
        {
            try
            {
                if (Application.Current.Windows.OfType<T>().Count() == 0)
                {
                    return (T)Activator.CreateInstance(typeof(T));
                }
                else
                {
                    //this does not seem to work right now. It grabs the right window but it does not show it
                    return Application.Current.Windows.OfType<T>().First();
                }
            }
            catch (Exception)
            {
                return new Window();
                throw;
            }
        }
    }
}
