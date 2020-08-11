using Accounts.Service.API.Helpers;
using Imprinno.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Service.API.Services
{
    public interface IAccountsService
    {
        List<User> GetAllUsers();
        Task<bool> AddUserAsync(User user);
        User GetUserById(Guid id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByPhone(string phone);
        Task<bool> CheckPasswordAsync(string password, string passwordHash, string passwordSalt);
        TokensSettings GetTokensSettings();
        List<Role> GetAllRoles();
        Task<bool> AddRoleAsync(Role role);
        Task<Role> GetRoleByIdAsync(Guid id);
        Task<Role> GetRoleByNameAsync(string name);
        Task SeedAsync();
    }
}