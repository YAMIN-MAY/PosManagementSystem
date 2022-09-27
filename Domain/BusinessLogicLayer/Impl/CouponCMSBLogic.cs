using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTestSolution.Domain.BusinessLogicLayer.Interfaces;
using NetTestSolution.Domain.Context;
using NetTestSolution.Domain.Models;
using NetTestSolution.Domain.Service;
using NetTestSolution.Helpers;
using NetTestSolution.Utility;

namespace NetTestSolution.Domain.BusinessLogicLayer.Impl
{
    public class CouponCMSBLogic : ICouponCMSBLogic
    {
        private readonly PosMgntDbContext _dbContext;
        private readonly ICacheService _cacheService;
        public CouponCMSBLogic(PosMgntDbContext posDbContext, ICacheService service)
        {
            _dbContext = posDbContext;
            _cacheService = service;

        }

        public async Task<ApiResponseWithDataModel<UserInfo>> UserLogin(AuthenticateRequestModel requestModel)
        {
            var responseModel = new UserInfo();

            #region Check User
            var user = await _dbContext.usersTblModel.Where(x => x.Email == requestModel.Email && x.Password == requestModel.Password && x.UserType == "A").FirstOrDefaultAsync();

            if (user != null)
            {
                responseModel.ID = user.ID.ToString();
                responseModel.Email = user.Email;
                responseModel.MobileNo = user.MobileNo;
                responseModel.UserType = user.UserType;
                responseModel.Name = user.Name;
            }
            else
            {
                return new ApiResponseWithDataModel<UserInfo> { ResponseCode = "012", ResponseDescription = "Not a valid user" };
            }
            #endregion

            #region Insert Log In Log
            var loginlog = await _dbContext.loginlogTblModel.Where(i => i.Email == requestModel.Email && i.Password == requestModel.Password).FirstOrDefaultAsync();
            var strSessionID = Guid.NewGuid().ToString().ToUpper();
            responseModel.SessionId = strSessionID;

            if (loginlog !=null)
            {
                loginlog.SessionId = strSessionID;
                loginlog.LoginDate = DateTime.Now;
                loginlog.Email = responseModel.Email;
                loginlog.DeviceId = requestModel.DeviceId;
                loginlog.CreatedUserID = user.ID;
                loginlog.LogoutDate = null;
                loginlog.Password = requestModel.Password;

               await _dbContext.SaveChangesAsync();
            }
            else
            {
                await _dbContext.AddAsync(new LoginLogTblModel
                {
                    SessionId = strSessionID,
                    LoginDate = DateTime.Now,
                    Email = responseModel.Email,
                    DeviceId = requestModel.DeviceId,
                    CreatedUserID = user.ID,
                    LogoutDate = null,
                    Password = requestModel.Password
                });
                await _dbContext.SaveChangesAsync();
            }
            #endregion

            return new ApiResponseWithDataModel<UserInfo>
            {
                ResponseCode = "000",
                ResponseDescription = "Success",
                Data = responseModel
            };
        }

        public async Task<ApiResponseModel> UserLogout(LogoutRequestModel requestModel)
        {

            #region Check User
            var user = await _dbContext.usersTblModel.Where(x => x.ID == int.Parse(requestModel.UserId)).FirstOrDefaultAsync();

            if (user != null)
            {
                #region Insert Log In Log
                var loginlog = await _dbContext.loginlogTblModel.Where(i => i.Email == requestModel.Email && i.CreatedUserID == int.Parse(requestModel.UserId)).FirstOrDefaultAsync();

                if (loginlog != null)
                {
                    loginlog.SessionId = null;
                    loginlog.LogoutDate = DateTime.Now;
                    loginlog.CreatedUserID = user.ID;
                    await _dbContext.SaveChangesAsync();
                }
                #endregion
            }
            #endregion

            return new ApiResponseModel
            {
                ResponseCode = "000",
                ResponseDescription = "Success"
            };
        }

        public async Task<ApiResponseModel> CreateCoupon(CreateUpdateCouponRequestModel requestModel)
        {
            #region Check Session
            bool isLogin = await CheckSessionIDAsync(requestModel.SessionID, requestModel.UserID);
            if (!isLogin)
            {
                return new ApiResponseModel
                {
                    ResponseCode = "005",
                    ResponseDescription = "Your session has expired. Please login again."
                };
            }
            #endregion

            var coupon = new CouponTblModel
            {
                CouponName = requestModel.CouponName,
                StartDate = requestModel.StartDate,
                EndDate = requestModel.EndDate,
                CouponCode = requestModel.CouponCode,
                DiscountAmount = decimal.Parse(requestModel.DiscountAmount),
                AvailableQuantity = int.Parse(requestModel.AvailableQuantity),
                CreatedDate = DateTime.Now,
                CreatedBy = int.Parse(requestModel.UserID)
            };

            await _dbContext.AddAsync(coupon);
            await _dbContext.SaveChangesAsync();


            #region Generate QR
            var reqInfo = new GenerateQrRequestModel();
            reqInfo.CouponId = coupon.ID.ToString();
            reqInfo.QrId = Guid.NewGuid().ToString() + requestModel.UserID;
            reqInfo.CouponName = requestModel.CouponName;
            reqInfo.CouponCode = requestModel.CouponCode;
            reqInfo.DiscountAmount = decimal.Parse(requestModel.DiscountAmount);
            var QR = Helper.GenerateQRImage(reqInfo);
            #endregion

            #region Update Coupon QR
            var couponInfo = await _dbContext.couponTblModel.Where(coup => coup.ID == coupon.ID).Select(coup => coup).FirstOrDefaultAsync();
            if (couponInfo == null)
            {
                string ez = requestModel.ID;
                return new ApiResponseModel { ResponseCode = "014", ResponseDescription = "Data not found" };
            }

            coupon.QRCodeURL = QR;

            _cacheService.RemoveData("coupon");

            await _dbContext.SaveChangesAsync();
            #endregion

            return new ApiResponseModel 
            { 
                ResponseCode = "000", 
                ResponseDescription = "Success" 
            };
        }

        public async Task<ApiResponseModel> UpdateCoupon(CreateUpdateCouponRequestModel requestModel)
        {
            #region Check Session
            bool isLogin = await CheckSessionIDAsync(requestModel.SessionID, requestModel.UserID);
            if (!isLogin)
            {
                return new ApiResponseModel
                {
                    ResponseCode = "005",
                    ResponseDescription = "Your session has expired. Please login again."
                };
            }
            #endregion

            var coupon = await _dbContext.couponTblModel.Where(coup => coup.ID == int.Parse(requestModel.ID)).Select(coup => coup).FirstOrDefaultAsync();
            if (coupon == null)
            {
                string ez = requestModel.ID;
                return new ApiResponseModel { ResponseCode = "014", ResponseDescription = "Data not found" };
            }

            coupon.CouponName = requestModel.CouponName;
            coupon.StartDate = requestModel.StartDate;
            coupon.EndDate = requestModel.EndDate;
            coupon.DiscountAmount = decimal.Parse(requestModel.DiscountAmount);
            coupon.AvailableQuantity = int.Parse(requestModel.AvailableQuantity);
            coupon.UpdatedBy = int.Parse(requestModel.UserID);
            coupon.UpdatedDate = DateTime.Now;

            _cacheService.RemoveData("coupon");

            await _dbContext.SaveChangesAsync();

            return new ApiResponseModel
            {
                ResponseCode = "000",
                ResponseDescription = "Success"
            };
        }

        public async Task<APIResponseWithDataModel> GetListOfCoupon(UserRequestModel requestModel)
        {
            #region Check Session
            bool isLogin = await CheckSessionIDAsync(requestModel.SessionID, requestModel.UserID);
            if (!isLogin)
            {
                return new APIResponseWithDataModel
                {
                    ResponseCode = "005",
                    ResponseDescription = "Your session has expired. Please login again."
                };
            }
            #endregion
            var lstOfValues = await (from d in _dbContext.couponTblModel orderby d.ID descending select d).ToListAsync();

            #region Set Redis Cache
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            _cacheService.SetData("coupon", lstOfValues, expirationTime);
            #endregion

            return new APIResponseWithDataModel { ResponseCode = "000", ResponseDescription = "Success", Data = lstOfValues };
        }

        public async Task<APIResponseWithDataModel> GetCouponHistoryReport(UserRequestModel requestModel)
        {
            #region Check Session
            bool isLogin = await CheckSessionIDAsync(requestModel.SessionID, requestModel.UserID);
            if (!isLogin)
            {
                return new APIResponseWithDataModel
                {
                    ResponseCode = "005",
                    ResponseDescription = "Your session has expired. Please login again."
                };
            }
            #endregion
            var lstOfValues = await (from d in _dbContext.couponHistoryTableModel orderby d.ID descending select d).ToListAsync();
            return new APIResponseWithDataModel { ResponseCode = "000", ResponseDescription = "Success", Data = lstOfValues };
        }
        public async Task<APIResponseWithDataModel> GetExchangePointHistoryReport(UserRequestModel requestModel)
        {
            #region Check Session
            bool isLogin = await CheckSessionIDAsync(requestModel.SessionID, requestModel.UserID);
            if (!isLogin)
            {
                return new APIResponseWithDataModel
                {
                    ResponseCode = "005",
                    ResponseDescription = "Your session has expired. Please login again."
                };
            }
            #endregion
            var lstOfValues = await (from d in _dbContext.exchangePointHistoryTblModel orderby d.ID descending select d).ToListAsync();
            return new APIResponseWithDataModel { ResponseCode = "000", ResponseDescription = "Success", Data = lstOfValues };
        }

        #region Member
        public async Task<APIResponseWithDataModel> GetListOfMember(UserRequestModel requestModel)
        {
            #region Check Session
            bool isLogin = await CheckSessionIDAsync(requestModel.SessionID, requestModel.UserID);
            if (!isLogin)
            {
                return new APIResponseWithDataModel
                {
                    ResponseCode = "005",
                    ResponseDescription = "Your session has expired. Please login again."
                };
            }
            #endregion

            var lstOfValues = await (from d in _dbContext.membersTblModel orderby d.ID descending select d).ToListAsync();
            return new APIResponseWithDataModel { ResponseCode = "000", ResponseDescription = "Success", Data = lstOfValues };
        }
        #endregion
        public async Task<bool> CheckSessionIDAsync(string sessionID, string userId)
        {
            var loginlog = await _dbContext.loginlogTblModel.Where(log => log.SessionId == sessionID && log.CreatedUserID == int.Parse(userId) && log.LoginDate != null && log.LogoutDate == null).FirstOrDefaultAsync();

            if (loginlog != null)
            {
                return true;
            }

            return false;
        }

    }
}
