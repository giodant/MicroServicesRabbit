using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRabbit.Authentication.Application.Models;
using MicroServicesRabbit.Domain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("SiteCorsPolicy")]
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RequestToken([FromBody] CreateSessionRequest loginRequest)
        {
            if (loginRequest == null) return BadRequest($"No entity specified {typeof(CreateSessionRequest)}");
            CreateSessionRequestValidator validator = new CreateSessionRequestValidator();
            var result = validator.Validate(loginRequest);

            if (!result.IsValid)
            {
                return BadRequest(new { message = result.ToString() });
            }

            ResponseModel<CreateSessionRequest> loginResult = new ResponseModel<CreateSessionRequest>();
            loginResult = _webApiService.Authenticate(loginRequest);
            if (loginResult == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            if (loginResult.CreateSessionTypeResult == CreateSessionTypeResult.Success)
            {
                //Set Session on EventLogAppService
                IocManager.Instance.Resolve<EventLogInterceptor>().SetSession(loginResult.UserSession);
                return Ok(loginResult.UserSession);
            }
            else
                return BadRequest(loginResult.CreateSessionTypeResult.ToString());
        }
    }
}
