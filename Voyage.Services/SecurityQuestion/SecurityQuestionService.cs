using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Data.Repositories.SecurityQuestion;
using Voyage.Models.Entities;

namespace Voyage.Services.SecurityQuestion
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly ISecurityQuestionRepository _securityQuestionRepository;
        private readonly IMapper _mapper;

        public SecurityQuestionService(ISecurityQuestionRepository securityQuestionRepository, IMapper mapper)
        {
            _securityQuestionRepository = securityQuestionRepository;
            _mapper = mapper;
        }

        public SecurityQuestionModel AddSecurityQuestion(SecurityQuestionModel model)
        {
            var securityQuestionEntity = _mapper.Map<Models.Entities.SecurityQuestion>(model);
            return _mapper.Map<SecurityQuestionModel>(_securityQuestionRepository.Add(securityQuestionEntity));
        }

        public SecurityQuestionModel GetSecurityQuestion(string id)
        {
            return _mapper.Map<SecurityQuestionModel>(_securityQuestionRepository.Get(id));
        }

        public void DeleteSecurityQuestion(object id)
        {
            _securityQuestionRepository.Delete(id);
        }

        public SecurityQuestionModel UpdateSecurityQuestion(SecurityQuestionModel model)
        {
            var securityQuestionEntity = _mapper.Map<Models.Entities.SecurityQuestion>(model);
            return _mapper.Map<SecurityQuestionModel>(_securityQuestionRepository.Update(securityQuestionEntity));
        }

        public SecurityQuestionModel GetAll()
        {
            return _mapper.Map<SecurityQuestionModel>(_securityQuestionRepository.GetAll());
        }
    }
}
