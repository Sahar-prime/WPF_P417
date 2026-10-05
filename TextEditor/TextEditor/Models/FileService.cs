using System.IO;

namespace TextEditor.Models
{
    public class FileService
    {
        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }
        public void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
