using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Data.Repositories.UserSecurityQuestion;
using Voyage.Models.Entities;

namespace Voyage.Services.SecurityQuestion
{
    public class UserSecurityQuestionService : IUserSecurityQuestionService
    {
        private readonly IUserSecurityQuestionRepository _userSecurityQuestionRepository;
        private readonly IMapper _mapper;

        public UserSecurityQuestionService(IUserSecurityQuestionRepository userSecurityQuestionRepository, IMapper mapper)
        {
            _userSecurityQuestionRepository = userSecurityQuestionRepository;
            _mapper = mapper;
        }

        public UserSecurityQuestionModel AddSecurityQuestion(UserSecurityQuestionModel model)
        {
            var userSecurityQuestionEntity = _mapper.Map<Models.Entities.UserSecurityQuestion>(model);
            return _mapper.Map<UserSecurityQuestionModel>(_userSecurityQuestionRepository.Add(userSecurityQuestionEntity));
        }

        public UserSecurityQuestionModel GetSecurityQuestion(string id)
        {
            return _mapper.Map<UserSecurityQuestionModel>(_userSecurityQuestionRepository.Get(id));
        }

        public void DeleteSecurityQuestion(object id)
        {
            _userSecurityQuestionRepository.Delete(id);
        }

        public UserSecurityQuestionModel UpdateSecurityQuestion(UserSecurityQuestionModel model)
        {
            var userSecurityQuestionEntity = _mapper.Map<Models.Entities.UserSecurityQuestion>(model);
            return _mapper.Map<UserSecurityQuestionModel>(_userSecurityQuestionRepository.Update(userSecurityQuestionEntity));
        }

        public UserSecurityQuestionModel GetAll()
        {
            return _mapper.Map<UserSecurityQuestionModel>(_userSecurityQuestionRepository.GetAll());
        }

        /// <summary>
        /// Get the Security Question and unencrypted answer by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>UserSecurityQuestionModel</returns>
        public UserSecurityQuestionModel GetSecurityQuestionByUserId(string userId)
        {
            return _mapper.Map<UserSecurityQuestionModel>(_userSecurityQuestionRepository.GetAll().FirstOrDefault(u => u.UserId == userId && u.IsDeleted == false));
        }
    }
}
