using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Models.Entities;

namespace Voyage.Services
{
    public interface IUserSecurityQuestionService
    {
        UserSecurityQuestionModel AddSecurityQuestion(UserSecurityQuestionModel model);

        UserSecurityQuestionModel GetSecurityQuestion(string id);

        void DeleteSecurityQuestion(object id);

        UserSecurityQuestionModel UpdateSecurityQuestion(UserSecurityQuestionModel model);

        UserSecurityQuestionModel GetAll();

        UserSecurityQuestionModel GetSecurityQuestionByUserId(string userId);
    }
}
