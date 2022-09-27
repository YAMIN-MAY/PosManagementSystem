using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTestSolution.Domain.BusinessLogicLayer.Interfaces;
using NetTestSolution.Domain.Context;
using NetTestSolution.Domain.Models;
using NetTestSolution.Helpers;

namespace NetTestSolution.Domain.BusinessLogicLayer.Impl
{
    public class MemberBlogic : IMemberBLogic
    {
        private readonly PosMgntDbContext _dbContext;

        public MemberBlogic(PosMgntDbContext posMgntDbContext)
        {
            _dbContext = posMgntDbContext;
        }
        public async Task<ApiResponseWithDataModel<MemberRegistrationResponseModel>> MemberRegistration(MemberRegistrationRequestModel requestModel)
        {
            var responseModel = new MemberRegistrationResponseModel();

            var GeneratePassword = Helper.GeneratePassword();
            requestModel.Password = GeneratePassword;

            var member = new MembersTblModel
            {
                Name = requestModel.Name,
                MobileNo = requestModel.MobileNo,
                MemberType = requestModel.MemberType,
                Email = requestModel.Email,
                TotalPoints = 0,
                PurchasedAmount = 0,
                MemberCode = "M" + DateTime.Now.ToString("ddMMyyyy")
            };

            await _dbContext.AddAsync(member);
            await _dbContext.SaveChangesAsync();

            await _dbContext.AddAsync(new UsersTblModel
            {
                Name = requestModel.Name,
                MobileNo = requestModel.MobileNo,
                UserType = "M",
                Email = requestModel.Email,
                Password = requestModel.Password,
            });
            await _dbContext.SaveChangesAsync();

            responseModel.Password = requestModel.Password;

            #region Generate Member QR
            var reqInfo = new GenerateMemberQrRequestModel();
            reqInfo.MemberId = member.ID.ToString();
            reqInfo.MemberCode = member.MemberCode;
            reqInfo.QrId = Guid.NewGuid().ToString();
            reqInfo.Name = requestModel.Name;
            reqInfo.MobileNo = requestModel.MobileNo;
            reqInfo.Email = requestModel.Email;

            var QR = Helper.GenerateMemberQr(reqInfo);
            #endregion

            #region Update Member QR
            var memberInfo = await _dbContext.membersTblModel.Where(m => m.ID == member.ID).Select(m => m).FirstOrDefaultAsync();
            memberInfo.QRCodeURL = QR;
            await _dbContext.SaveChangesAsync();
            #endregion
            responseModel.QRCodeURL = QR;


            return new ApiResponseWithDataModel<MemberRegistrationResponseModel>
            {
                ResponseCode = "000",
                ResponseDescription = "Success",
                Data = responseModel
            };
        }

        public async Task<ApiResponseWithDataModel<MemberLoginResponseModel>> MemberUserLogin(MemberLoginRequestModel requestModel)
        {
            var responseModel = new MemberLoginResponseModel();

            #region Check User
            var user = await _dbContext.usersTblModel.Where(x => x.MobileNo == requestModel.MobileNo && x.Password == requestModel.Password).FirstOrDefaultAsync();

            if (user != null)
            {
                responseModel.UserID = user.ID.ToString();
                responseModel.Email = user.Email;
                responseModel.MobileNo = user.MobileNo;
                responseModel.UserType = user.UserType;
            }
            #endregion

            #region Get Member
            var member = await _dbContext.membersTblModel.Where(x => x.MobileNo == requestModel.MobileNo).FirstOrDefaultAsync();
            if (member != null)
            {
                responseModel.Name = member.Name;
                responseModel.MemberType = member.MemberType;
                responseModel.MemberID = member.ID.ToString();
            }
            #endregion

            #region Insert Log In Log
            var loginlog = await _dbContext.loginlogTblModel.Where(i => i.MobileNo == requestModel.MobileNo && i.Password == requestModel.Password).FirstOrDefaultAsync();
            var strSessionID = Guid.NewGuid().ToString().ToUpper();
            responseModel.SessionId = strSessionID;

            if (loginlog != null)
            {
                loginlog.SessionId = strSessionID;
                loginlog.LoginDate = DateTime.Now;
                loginlog.MobileNo = responseModel.MobileNo;
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
                    MobileNo = responseModel.MobileNo,
                    DeviceId = requestModel.DeviceId,
                    CreatedUserID = user.ID,
                    Password = requestModel.Password,
                    LogoutDate = null
                }); ;
                await _dbContext.SaveChangesAsync();
            }
            #endregion

            return new ApiResponseWithDataModel<MemberLoginResponseModel>
            {
                ResponseCode = "000",
                ResponseDescription = "Success",
                Data = responseModel
            };
        }

        public async Task<ApiResponseModel> MemberUserLogout(LogoutRequestModel requestModel)
        {

            #region Check User
            var user = await _dbContext.usersTblModel.Where(x => x.ID == int.Parse(requestModel.UserId)).FirstOrDefaultAsync();
            if (user != null)
            {
                #region Insert Log In Log
                var loginlog = await _dbContext.loginlogTblModel.Where(i => i.MobileNo == requestModel.MobileNo && i.CreatedUserID == int.Parse(requestModel.UserId)).FirstOrDefaultAsync();

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

        public async Task<APIResponseWithDataModel> GetPurchaseHistory(UserRequestModel requestModel)
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

            var orderList = await (from ord in _dbContext.orderTblModel
                                   where ord.OrderUserId == int.Parse(requestModel.UserID)
                                   orderby ord.OrderDate descending
                                   select ord).ToListAsync();

            foreach (var order in orderList)
            {
                var o = order;
                var orderDetailList = await _dbContext.orderDetailTblModel.Where(x => x.OrderId == order.ID)
                                            .Select(x => new OrderDetailTblModel {
                                                OrderId = x.OrderId,
                                                ProductId = x.ProductId,
                                                Qty = x.Qty,
                                                Price = x.Price,
                                                ProductName = x.ProductName
                                            }).ToListAsync();
                order.OrderDetail = orderDetailList;
            }

            return new APIResponseWithDataModel { ResponseCode = "000", ResponseDescription = "Success", Data = orderList };
        }

        public async Task<APIResponseWithDataModel> GetTotalPointByMemberId(GetTotalPointByMemberId requestModel)
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

            int totalPoints = await _dbContext.membersTblModel.Where(m => m.ID == int.Parse(requestModel.MemberID)).Select(m => m.TotalPoints).FirstOrDefaultAsync();

            return new APIResponseWithDataModel { ResponseCode = "000", ResponseDescription = "Success", Data = totalPoints };
        }

        public async Task<ApiResponseModel> ExchangePointByMemberId(ExchangePointByMemberIdRequestModel requestModel)
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
            var memberInfo = await _dbContext.membersTblModel.Where(m => m.ID == int.Parse(requestModel.MemberID)).Select(m => m).FirstOrDefaultAsync();
            if (memberInfo != null)
            {
                if (memberInfo.TotalPoints == 0)
                {
                    return new ApiResponseModel { ResponseCode = "012", ResponseDescription = "Can't exchange point for this time." };
                }
                else if(requestModel.Points < 100)
                {
                    return new ApiResponseModel { ResponseCode = "012", ResponseDescription = "Your Point is not enough to exchange coupon." };
                }
                else if ((requestModel.Points) > memberInfo.TotalPoints)
                {
                    return new ApiResponseModel { ResponseCode = "012", ResponseDescription = "You only remain " + memberInfo.TotalPoints + " points to exchange." };
                }
                else
                {
                    var exchangePoint = new ExchangePointHistoryTableModel();
                    exchangePoint.ExchangeCouponAmount = requestModel.Points / 100;
                    exchangePoint.ExchangePoint = requestModel.Points;
                    exchangePoint.RemainingPoint = memberInfo.TotalPoints - requestModel.Points;
                    exchangePoint.MemberCode = requestModel.MemberID;
                    exchangePoint.MemberId = requestModel.MemberID;

                    await _dbContext.AddAsync(exchangePoint);
                    await _dbContext.SaveChangesAsync();

                    memberInfo.TotalPoints = exchangePoint.RemainingPoint;
                    await _dbContext.SaveChangesAsync();
                }
            }
            else
            {
                return new ApiResponseModel { ResponseCode = "012", ResponseDescription = "Not a member" };
            }

            return new ApiResponseModel { ResponseCode = "000", ResponseDescription = "Success"};
        }

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
