using System;

namespace Launchpad.Models
{
    public class StatusModel
    {
        public Enum.StatusCode Code { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }
    }
}
