using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Imprinno.Models.Enums;
using Imprinno.Core.Attributes;
using Imprinno.Core.Extensions;

namespace Accounts.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {

        // [HasPermission(PermEnums.ReadPermission)]
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var permissions = ((PermEnums[])Enum.GetValues(
                typeof(PermEnums))).Select(c => c.GetPermission()).ToList();

                return StatusCode((int)HttpStatusCode.OK, permissions);
            }
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); }
        }
    }
}
