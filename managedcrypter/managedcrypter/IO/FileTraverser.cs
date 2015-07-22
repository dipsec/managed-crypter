using System.Collections.Generic;
using System.IO;

namespace managedcrypter.IO
{
    public class FileTraverser
    {
        public string Parent;
        public Dictionary<string, string> Children;

        public FileTraverser()
        {
            Parent = string.Empty;
            Children = new Dictionary<string, string>();
        }

        public FileTraverser(string _Parent)
        {
            Parent = _Parent;
            Children = new Dictionary<string, string>();

            if (!Directory.Exists(Parent))
                Directory.CreateDirectory(Parent);
        }

        public void AddChild(string childName)
        {
            Children.Add(childName, string.Concat(Parent, "\\", childName));
        }

        public void RemoveChild(string childName)
        {
            Children.Remove(childName);
        }

        public void Write(string childName, byte[] childData)
        {
            if (File.Exists(Children[childName]))
                File.Delete(Children[childName]);

            File.WriteAllBytes(Children[childName], childData);
        }

        public void Clear()
        {
            Children.Clear();
            Directory.Delete(Parent, true);
            Parent = string.Empty;
        }
    }
}
