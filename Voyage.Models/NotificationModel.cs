using System;
using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    public class NotificationModel
    {
        [AntiXss]
        public int Id { get; set; }

        public string Text { get; set; }

        public string AssignedToUserId { get; set; }

        public bool IsRead { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
