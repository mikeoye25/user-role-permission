using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Accounts.Service.API.Services;
using Accounts.Service.API.ViewModels;
using Imprinno.Core.Handlers;
using Imprinno.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult> SignupAsync([FromBody] SignupViewModel model)
        {
            try
            {
                if (HasValidationError(model, out List<string> errors)) { return StatusCode((int)HttpStatusCode.PreconditionFailed, string.Join(" | ", errors)); }

                if(_accountsService.GetUserByUsername(model.Username) != null)
                { return StatusCode((int)HttpStatusCode.Conflict, string.Format("{0} is taken, please choose a different username", model.Username)); }

                if (_accountsService.GetUserByEmail(model.Email) != null)
                { return StatusCode((int)HttpStatusCode.Conflict, string.Format("{0} is associated with an account", model.Email)); }

                if (_accountsService.GetUserByPhone(model.Phone) != null && !string.IsNullOrWhiteSpace(model.Phone))
                { return StatusCode((int)HttpStatusCode.Conflict, string.Format("{0} is associated with an account", model.Phone)); }

                var salt = CryptoHandler.GenerateSalt();

                if (string.IsNullOrWhiteSpace(model.Role)) model.Role = "Administrators";
                var role = await _accountsService.GetRoleByNameAsync(model.Role);
                if (role == null)
                { return StatusCode((int)HttpStatusCode.BadRequest, string.Format("{0} does not exist", model.Role)); }

                var user = new User { 
                    Username = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    PasswordSalt = salt,
                    PasswordHash = CryptoHandler.GetHash(model.Password, salt),
                    RoleId = role.Id,
                    RegistrationIP = new NetworkHandler().GetIPAddress(HttpContext),
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    IsDisabled = false
                };

                var result = await _accountsService.AddUserAsync(user);

                if(result) return StatusCode((int)HttpStatusCode.Created, "Account created successfully !!!");
                return StatusCode((int)HttpStatusCode.OK, "Account creation failed !!!");
            }

            //catch (Exception ex) { return this.custom_error(ex); } // TODO
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // var user = await _userManager.FindByNameAsync(model.Username);
                var user = _accountsService.GetUserByUsername(model.Username);

                if (user != null)
                {
                    // var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    var result = await _accountsService.CheckPasswordAsync(model.Password, user.PasswordHash, user.PasswordSalt);

                    if (result)
                    {
                        var tokenSettings = _accountsService.GetTokensSettings();
                        // authentication successful so generate jwt token
                        var token = CryptoHandler.GenerateJwtToken(user.Username, user.Role.Permissions, tokenSettings.Key);

                        var results = new
                        {
                            token,
                            message = "Account created successfully !!!"
                        };

                        // attach user to context on successful jwt validation
                        // context.Items["User"] = user;

                        return StatusCode((int)HttpStatusCode.Created, results);

                        // return Created("", token);
                    }
                }
            }

            return BadRequest();
        }

        private bool HasValidationError(SignupViewModel model, out List<string> errors)
        {
            errors = new List<string>();
            if (!ValidPassword(model.Password)) { errors.Add("Password contains forbidden characters"); }
            if (!ValidEmail(model.Email)) { errors.Add("Please enter a valid email"); }

            return errors.Count > 0 ? true : false;
        }

        private bool ValidPassword(string password)
        {
            var forbidden = new string[] { "|", "=", " ", "#", "&", "%", "?", "+", "-", "(", ")", "`", @"\" };
            return !forbidden.Any(e => password.Contains(e));
        }

        private bool ValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);
            }

            catch { return false; }
        }
    }
}
