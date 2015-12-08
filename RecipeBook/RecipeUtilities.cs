using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;

namespace RecipeBook.Utilities
{
    /// <summary>
    /// Class copied and edited from StackOverflow user Simon The Cat (March 9, 2013)
    /// http://stackoverflow.com/questions/2064547/get-last-active-window-get-previously-active-window
    /// </summary>
    static class ProcessWatcher
    {
        public static void StartWatch()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timer.Start();
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            setLastActive();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        public static IntPtr LastHandle
        {
            get
            {
                return _previousToLastHandle;
            }
        }

        private static void setLastActive()
        {
            IntPtr currentHandle = GetForegroundWindow();
            if (currentHandle != _previousHandle)
            {
                _previousToLastHandle = _previousHandle;
                _previousHandle = currentHandle;
            }
        }

        private static Timer _timer;
        private static IntPtr _previousHandle = IntPtr.Zero;
        private static IntPtr _previousToLastHandle = IntPtr.Zero;
    }
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
