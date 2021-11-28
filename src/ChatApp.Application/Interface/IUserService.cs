using ChatApp.Application.ViewModels;
using System.Collections.Generic;

namespace ChatApp.Application.Interface
{
    public interface IUserService
    {
        UserViewModel GetById(string id);
        IEnumerable<UserViewModel> GetAll();
        void DisableLock(string id);
    }
}
