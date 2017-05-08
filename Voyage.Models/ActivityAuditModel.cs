using Embarr.WebAPI.AntiXss;
using System;

namespace Voyage.Models
{
    public class ActivityAuditModel
    {
        [AntiXss]
        public string RequestId { get; set; }

        [AntiXss]
        public string Method { get; set; }

        [AntiXss]
        public string Path { get; set; }

        [AntiXss]
        public string IpAddress { get; set; }

        [AntiXss]
        public DateTime Date { get; set; }

        [AntiXss]
        public int StatusCode { get; set; }

        [AntiXss]
        public string Error { get; set; }

        [AntiXss]
        public string UserName { get; set; }
    }
}
