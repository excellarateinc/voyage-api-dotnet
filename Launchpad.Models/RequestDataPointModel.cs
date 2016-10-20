using System;

namespace Launchpad.Models
{
    public class RequestDataPointModel
    {
        public string Method { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return $"{RequestDateTime}: {Method} -> {Path}";
        }
    }
}
