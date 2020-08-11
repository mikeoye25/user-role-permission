using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Accounts.Service.API.Services;
using Imprinno.Core.Attributes;
using Imprinno.Models.Entities;
using Imprinno.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public RoleController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HasPermission(PermEnums.ReadRole)]
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var roles = _accountsService.GetAllRoles();

                return StatusCode((int)HttpStatusCode.OK, roles);
            }
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); }
        }

        [HttpPost("")]
        public ActionResult PostAsync([FromBody] Role role)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(role.Name))
                { return StatusCode((int)HttpStatusCode.PreconditionFailed, "Please enter a valid name"); }

                if (_accountsService.GetRoleByNameAsync(role.Name) != null)
                { return StatusCode((int)HttpStatusCode.Conflict, string.Format("{0} already exists", role.Name)); }

                role.UpdatedOn = DateTime.Now;
                role.CreatedOn = DateTime.Now;

                _accountsService.AddRoleAsync(role);

                return StatusCode((int)HttpStatusCode.Created, "Role created successfully !!!");
            }

            //catch (Exception ex) { return this.custom_error(ex); } // TODO
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); }
        }

        [HttpGet("{name}")]
        public ActionResult<Role> Get(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, string.Format("Please enter a valid name", name));
                }

                var role = _accountsService.GetRoleByNameAsync(name);

                if (role == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, string.Format("{0}  does not exist", name));
                }

                return StatusCode((int)HttpStatusCode.OK, role);
            }
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); }
        }
    }
}
