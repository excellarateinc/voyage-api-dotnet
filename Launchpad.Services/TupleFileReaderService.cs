using Launchpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Launchpad.Services
{
    public class TupleFileReaderService : ITupleFileReaderService
    {
        public string[] ReadAllLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}