using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTestSolution.Domain.BusinessLogicLayer.Interfaces;
using NetTestSolution.Domain.Context;
using NetTestSolution.Domain.Models;
using NetTestSolution.Utility;
using Newtonsoft.Json;

namespace NetTestSolution.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Api/[controller]")]
    public class CouponCMSController : ApiControllerBase
    {
        private readonly PosMgntDbContext _dbContext;
        private readonly IJWTManagerRepository _iJWTManagerRepository;
        private readonly ICouponCMSBLogic _couponCMSBLogic;
        public CouponCMSController(IJWTManagerRepository jWTManagerRepository, PosMgntDbContext dbContext,  ICouponCMSBLogic couponCMSBLogic)
        {
            _dbContext = dbContext;
            _iJWTManagerRepository = jWTManagerRepository;
            _couponCMSBLogic = couponCMSBLogic;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AccessToken")]
        public async Task<IActionResult> AccessToken([FromBody] AuthenticateRequestModel requestModel)
        {
            var responseModel = new AuthenticateResponseModel();
            string errMessage = null;
            try
            {
                var user = await _dbContext.usersTblModel
                    .Where(user => (user.Email == requestModel.Email || user.MobileNo == requestModel.MobileNo) && user.Password == requestModel.Password)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    responseModel.UserId = user.ID.ToString();
                    var token = _iJWTManagerRepository.GenerateJWTTokens(requestModel);
                    if (token == null)
                    {
                        return Unauthorized("Invalid Attempt!");
                    }

                    responseModel.Token = token;
                    responseModel.ResponseCode = "000";
                    responseModel.ResponseDescription = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    return Unauthorized();
                }

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
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] AuthenticateRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var errMsg = string.Empty;
                if (string.IsNullOrEmpty(requestModel.Email))
                {
                    errMsg = "Email is required.";
                    return FailApiResponse("012", errMsg);
                }

                if (string.IsNullOrEmpty(requestModel.Password))
                {
                    errMsg = "Password is required.";
                    return FailApiResponse("012", errMsg);
                }
                var resp = await _couponCMSBLogic.UserLogin(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(respData);

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
        [Route("UserLogout")]
        public async Task<IActionResult> UserLogout([FromBody] LogoutRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {

                var resp = await _couponCMSBLogic.UserLogout(requestModel);
                respData = JsonConvert.SerializeObject(resp);
                return Ok(respData);

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

        #region Coupon

        [HttpPost]
        [Route("CreateCoupon")]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateUpdateCouponRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var CreateVoucher = await _couponCMSBLogic.CreateCoupon(requestModel);
                respData = JsonConvert.SerializeObject(CreateVoucher);
                return Ok(CreateVoucher);

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
        [Route("UpdateCoupon")]
        public async Task<IActionResult> UpdateCoupon([FromBody] CreateUpdateCouponRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _couponCMSBLogic.UpdateCoupon(requestModel);
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
        [Route("GetListOfCoupon")]
        public async Task<IActionResult> GetListOfCoupon([FromBody] UserRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _couponCMSBLogic.GetListOfCoupon(requestModel);
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
        [Route("GetCouponHistoryReport")]
        public async Task<IActionResult> GetCouponHistoryReport([FromBody] UserRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _couponCMSBLogic.GetCouponHistoryReport(requestModel);
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
        [Route("GetExchangePointHistoryReport")]
        public async Task<IActionResult> GetExchangePointHistoryReport([FromBody] UserRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _couponCMSBLogic.GetExchangePointHistoryReport(requestModel);
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
        [Route("GetListOfMember")]
        public async Task<IActionResult> GetListOfMember([FromBody] UserRequestModel requestModel)
        {
            string respData = null;
            string errMessage = null;
            try
            {
                var resp = await _couponCMSBLogic.GetListOfMember(requestModel);
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

        #endregion
    }
}
