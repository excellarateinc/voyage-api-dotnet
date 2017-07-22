using System;
using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }

        [AntiXss]
        public string Subject { get; set; }

        [AntiXss]
        public string Description { get; set; }

        [AntiXss]
        public string AssignedToUserId { get; set; }

        public bool IsRead { get; set; }

        [AntiXss]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
