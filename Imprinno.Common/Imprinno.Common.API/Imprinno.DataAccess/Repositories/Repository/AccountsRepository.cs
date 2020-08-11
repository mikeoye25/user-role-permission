using Imprinno.DataAccess.Repositories.Interface;
using Imprinno.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imprinno.DataAccess.Repositories.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly EntitiesDbContext _ctx;

        public AccountsRepository(EntitiesDbContext ctx)
        {
            _ctx = ctx;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _ctx.Users.Include(u => u.Role)
                           .OrderByDescending(p => p.CreatedOn)
                           .ToList();
            }
            catch
            {
                return null;
            }
        }

        public User GetUserById(Guid id)
        {
            try
            {
                return GetAllUsers().FirstOrDefault(u => u.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                return GetAllUsers().FirstOrDefault(u => u.Username == username);
            }
            catch
            {
                return null;
            }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                return GetAllUsers().FirstOrDefault(u => u.Email == email);
            }
            catch
            {
                return null;
            }
        }

        public User GetUserByPhone(string phone)
        {
            try
            {
                return GetAllUsers().FirstOrDefault(u => u.Phone == phone);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Role> GetAllRoles()
        {
            try
            {
                return _ctx.Roles
                           .OrderByDescending(p => p.CreatedOn)
                           .ToList();
            }
            catch
            {
                return null;
            }
        }

        public Role GetRoleById(Guid id)
        {
            try
            {
                return GetAllRoles().FirstOrDefault(u => u.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public Role GetRoleByName(string name)
        {
            try
            {
                return GetAllRoles().FirstOrDefault(u => u.Name == name);
            }
            catch
            {
                return null;
            }
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
