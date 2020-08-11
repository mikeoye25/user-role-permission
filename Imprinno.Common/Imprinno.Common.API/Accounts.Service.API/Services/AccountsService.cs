using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Accounts.Service.API.Helpers;
using Imprinno.Core.Handlers;
using Imprinno.DataAccess.Repositories.Interface;
using Imprinno.Models.Entities;
using Imprinno.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Service.API.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly TokensSettings _tokensSettings;
        private readonly IAccountsRepository _accountsRepository;

        public AccountsService(IOptions<TokensSettings> tokensSettings, IAccountsRepository accountsRepository)
        {
            _tokensSettings = tokensSettings.Value;
            _accountsRepository = accountsRepository;
        }

        public List<User> GetAllUsers()
        {
            return _accountsRepository.GetAllUsers().ToList();
        }

        public async Task<bool> AddUserAsync(User newUser)
        {
            _accountsRepository.AddEntity(newUser);
            var result = Task.Run(() => _accountsRepository.SaveAll());
            return await result;
        }

        public User GetUserById(Guid id)
        {
            return _accountsRepository.GetUserById(id);
        }

        public User GetUserByUsername(string username)
        {
            return _accountsRepository.GetUserByUsername(username);
        }

        public User GetUserByEmail(string email)
        {
            return _accountsRepository.GetUserByEmail(email);
        }

        public User GetUserByPhone(string phone)
        {
            return _accountsRepository.GetUserByPhone(phone);
        }

        public async Task<bool> CheckPasswordAsync(string password, string passwordHash, string passwordSalt)
        {
            var result = Task.Run(() => CryptoHandler.GetHash(password, passwordSalt) == passwordHash);
            return await result;
        }

        public TokensSettings GetTokensSettings()
        {
            return _tokensSettings;
        }

        public List<Role> GetAllRoles()
        {
            return _accountsRepository.GetAllRoles().ToList();
        }

        public async Task<bool> AddRoleAsync(Role newRole)
        {
            _accountsRepository.AddEntity(newRole);
            var result = Task.Run(() => _accountsRepository.SaveAll());
            return await result;
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            var role = Task.Run(() => _accountsRepository.GetRoleById(id));
            return await role;
        }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            var role = Task.Run(() => _accountsRepository.GetRoleByName(name));
            return await role;
        }

        public async Task SeedAsync()
        {
            // Get permissions for admin role
            var assignedPermissions = new List<PermEnums> { 
                PermEnums.CreateRole,
                PermEnums.ReadRole,
                PermEnums.CreateUser,
                PermEnums.ReadUser,
                PermEnums.SendMessage,
                PermEnums.ReadMessage
            };

            var permissions = assignedPermissions.Select(c => ((int)c).ToString()).ToList();

            // Seed the Admin Role
            var name = "Administrators";
            var role = await GetRoleByNameAsync(name);
            if (role == null)
            {
                role = new Role
                {
                    Description = "Admin Role",
                    Name = name,
                    Permissions = string.Join(",", permissions)
                };
                await AddRoleAsync(role);
            }

            // Seed the Admin User
            var email = "admin@inspirecoders.com";
            var user = GetUserByEmail(email);
            if (user == null)
            {
                var salt = CryptoHandler.GenerateSalt();
                user = new User
                {
                    LastName = "Site",
                    FirstName = "Admin",
                    Email = "admin@inspirecoders.com",
                    Username = "admin",
                    PasswordSalt = salt,
                    PasswordHash = CryptoHandler.GetHash("P@ssw0rd!", salt),
                    RoleId = role.Id,
                    // RegistrationIP = new NetworkHandler().GetIPAddress(HttpContext),
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    IsDisabled = false
                };

                var result = await AddUserAsync(user);
                if (!result)
                {
                    throw new InvalidOperationException("Could not create user in Seeding");
                }
            }
        }
    }
}
