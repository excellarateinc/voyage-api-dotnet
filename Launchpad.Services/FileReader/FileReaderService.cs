using System.IO;

namespace Voyage.Services.FileReader
{
    public class FileReaderService : IFileReaderService
    {
        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}