using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetTestSolution.Domain.Models;
using Newtonsoft.Json;

namespace NetTestSolution.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult FailApiResponse(string RespCode, string RespDesc)
        {
            ApiResponseModel model = new ApiResponseModel();
            model.ResponseCode = RespCode;
            model.ResponseDescription = RespDesc;

            var response = new StringContent(JsonConvert.SerializeObject(model));

            if (RespCode != "000")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        protected IActionResult ApiResponseWithDataModel<T>(ApiResponseWithDataModel<T> responseModel)
        {

            var response = JsonConvert.SerializeObject(responseModel);

            return Ok(response);
        }

        protected IActionResult ExceptionApiResponse(Exception ex)
        {

            ApiResponseModel model = new ApiResponseModel();
            model.ResponseCode = "012";
            model.ResponseDescription = ex.Message;

            var response = new StringContent(JsonConvert.SerializeObject(model));

            return BadRequest(response);
        }
    }
}
