using System;

namespace Launchpad.Models.EntityFramework
{
    public class ActivityAudit
    {
        public int Id { get; set; }

        public string RequestId { get; set; }

        public string Method { get; set; }
        public string Path { get; set; }

        public string IpAddress { get; set; }

        public DateTime Date { get; set; }

        public int StatusCode { get; set; }

        public string Error { get; set; }

        public string UserName { get; set; }
    }
}
