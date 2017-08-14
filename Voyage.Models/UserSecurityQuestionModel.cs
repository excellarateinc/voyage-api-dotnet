using Embarr.WebAPI.AntiXss;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voyage.Models.Entities
{
    public class UserSecurityQuestionModel
    {
        [AntiXss]
        public string Id { get; set; }

        [AntiXss]
        public string UserId { get; set; }

        [AntiXss]
        public string QuestionId { get; set; }

        [AntiXss]
        public string Answer { get; set; }

        [AntiXss]
        public string CreatedBy { get; set; }

        [AntiXss]
        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
