using System;
using System.Collections.Generic;
using System.Linq;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interface.Repository;

namespace ChatApp.Infra.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Context.Context _db;

        public UserRepository()
        {
            _db = new Context.Context();
        }

        public User GetById(string id)
        {
            return _db.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users.ToList();
        }
        public void DisableLock(string id)
        {
            _db.Users.Find(id).LockoutEnabled = false;
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}