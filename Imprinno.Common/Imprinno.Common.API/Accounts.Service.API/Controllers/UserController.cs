using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Accounts.Service.API.Services;
using Imprinno.Core.Attributes;
using Imprinno.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public UserController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        // [HasPermission(PermEnums.ReadUser)]
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var users = _accountsService.GetAllUsers();

                return StatusCode((int)HttpStatusCode.OK, users);
            }
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); }
        }
    }
}
