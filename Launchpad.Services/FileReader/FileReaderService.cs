using Launchpad.Services.Interfaces;
using System.IO;

namespace Launchpad.Services
{
    public class FileReaderService : IFileReaderService
    {
        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}