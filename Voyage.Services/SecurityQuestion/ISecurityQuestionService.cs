using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Models.Entities;

namespace Voyage.Services
{
    public interface ISecurityQuestionService
    {
        SecurityQuestionModel AddSecurityQuestion(SecurityQuestionModel model);

        SecurityQuestionModel GetSecurityQuestion(string id);

        void DeleteSecurityQuestion(object id);

        SecurityQuestionModel UpdateSecurityQuestion(SecurityQuestionModel model);

        SecurityQuestionModel GetAll();
    }
}
