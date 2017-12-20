using System;
using System.Runtime.InteropServices;

namespace PaddleCheckoutSDK
{
    internal class NativeMethods
    {
        const int INTERNET_OPTION_SUPPRESS_BEHAVIOR = 81;
        const int INTERNET_SUPPRESS_COOKIE_PERSIST = 3;

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, ref int flag, int dwBufferLength);

        public static void SuppressCookiePersistence()
        {
            int flag = INTERNET_SUPPRESS_COOKIE_PERSIST;
            if (!InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SUPPRESS_BEHAVIOR, ref flag, sizeof(int)))
            {
                var ex = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                throw ex;
            }
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);
        const int URLMON_OPTION_USERAGENT = 0x10000001;

        public static void ChangeUserAgent(string Agent)
        {
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, Agent, Agent.Length, 0);
        }


    }

}


