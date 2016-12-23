using System.IO;

namespace Launchpad.Services.FileReader
{
    public class FileReaderService : IFileReaderService
    {
        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}