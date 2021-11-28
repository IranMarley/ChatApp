using System;
using System.Collections.Generic;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interface.Repository
{
    public interface IUserRepository
    {
        User GetById(string id);
        IEnumerable<User> GetAll();
        void DisableLock(string id);
    }
}