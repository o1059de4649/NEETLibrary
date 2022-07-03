using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CSELibrary.Com.Net
{
    public static class CseNetAccessor
    {
        public static int OpenURL(string url)
        {
            var res = 0;
            //string url  

            try
            {
                Process.Start(url);
                res = 1;
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //Windowsのとき  
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //Linuxのとき  
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    //Macのとき  
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
            return res;
        }
    }
}
