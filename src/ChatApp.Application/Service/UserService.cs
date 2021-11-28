using System.Collections.Generic;
using AutoMapper;
using ChatApp.Application.Interface;
using ChatApp.Application.ViewModels;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interface.Repository;

namespace ChatApp.Application.Service
{
    public class UserService : IUserService
    {
        #region Fields

        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructors

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region Methods

        public void DisableLock(string id)
        {
            _userRepository.DisableLock(id);
        }

        public IEnumerable<UserViewModel> GetAll()
        {
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(_userRepository.GetAll());
        }

        public UserViewModel GetById(string id)
        {
            return Mapper.Map<User, UserViewModel>(_userRepository.GetById(id));
        }

        #endregion

    }
}
