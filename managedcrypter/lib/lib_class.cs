using System;
using System.IO;

namespace A
{
    public class class1
    {
        public static void method1()
        {
            File.WriteAllText("C:\\Users\\Admin\\Desktop\\HelloWorld.txt", "Hello From DLL!");
        }
    }
}
