using Imprinno.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imprinno.DataAccess.Repositories.Interface
{
    public interface IAccountsRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByPhone(string phone);
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(Guid id);
        Role GetRoleByName(string name);
        bool SaveAll();
        void AddEntity(object model);
    }
}
