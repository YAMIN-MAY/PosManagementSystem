using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTestSolution.Domain.BusinessLogicLayer.Interfaces;
using NetTestSolution.Domain.Models;
using Newtonsoft.Json;

namespace NetTestSolution.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Api/[controller]")]
    public class MemberController : ApiControllerBase
    {
        private readonly IMemberBLogic _memberBLogic;

        public MemberController(IMemberBLogic memberBLogic)
        {
            _memberBLogic = memberBLogic;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("MemberRegistration")]
        public async Task<IActionResult> MemberRegistration([FromBody] MemberRegistrationRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _memberBLogic.MemberRegistration(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(resp);

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return BadRequest();
            }
            finally
            {
            }
        }

        [HttpPost]
        [Route("MemberUserLogin")]
        public async Task<IActionResult> MemberUserLogin([FromBody] MemberLoginRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _memberBLogic.MemberUserLogin(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(resp);

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return BadRequest();
            }
            finally
            {
            }
        }

        [HttpPost]
        [Route("MemberUserLogout")]
        public async Task<IActionResult> MemberUserLogout([FromBody] LogoutRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _memberBLogic.MemberUserLogout(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(resp);

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return BadRequest();
            }
            finally
            {
            }
        }

        [HttpPost]
        [Route("GetPurchaseHistory")]
        public async Task<IActionResult> GetPurchaseHistory([FromBody] UserRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _memberBLogic.GetPurchaseHistory(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(resp);

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return BadRequest();
            }
            finally
            {
            }
        }

        [HttpPost]
        [Route("GetTotalPointByMemberId")]
        public async Task<IActionResult> GetTotalPointByMemberId([FromBody] GetTotalPointByMemberId requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _memberBLogic.GetTotalPointByMemberId(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(resp);

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return BadRequest();
            }
            finally
            {
            }
        }

        [HttpPost]
        [Route("ExchangePointByMemberId")]
        public async Task<IActionResult> ExchangePointByMemberId([FromBody] ExchangePointByMemberIdRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _memberBLogic.ExchangePointByMemberId(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(resp);

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return BadRequest();
            }
            finally
            {
            }
        }
    }
}
