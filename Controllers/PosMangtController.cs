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
    public class PosMangtController : ApiControllerBase
    {
        private readonly IPosMangtBLogic _posMangtBLogic;

        public PosMangtController(IPosMangtBLogic posMangtBLogic)
        {
            _posMangtBLogic = posMangtBLogic;
        }

        [HttpPost]
        [Route("PosInquiry")]
        public async Task<IActionResult> CreateCoupon([FromBody] PosInquiryRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _posMangtBLogic.PosInquiry(requestModel);
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
