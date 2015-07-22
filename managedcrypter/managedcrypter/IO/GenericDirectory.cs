using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace managedcrypter.IO
{
    class GenericDirectory
    {
        public GenericDirectory(string rootDirectory)
        {
            DirectoryPath = rootDirectory;
            Workspace = new FileTraverser();
            Files = new List<string>();

            SetWorkingFiles();
        }

        public string DirectoryPath { get; private set; }
        public FileTraverser Workspace { get; private set; }
        public List<string> Files { get; private set; }

        public void CreateWorkspaceDirectory()
        {
            string folderID = Guid.NewGuid().ToString();
            string writePath = Path.Combine(DirectoryPath, folderID);
            Workspace = new FileTraverser(writePath);
        }

        void SetWorkingFiles()
        {
            var csFiles = System.IO.Directory.GetFiles(DirectoryPath).Where(
              f => Path.GetExtension(f) == ".cs");
            foreach (var csFile in csFiles)
                Files.Add(csFile);
        }
    }
}
