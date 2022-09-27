using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTestSolution.Domain.Models;

namespace NetTestSolution.Domain.BusinessLogicLayer.Interfaces
{
    public interface ICouponCMSBLogic
    {
        Task<ApiResponseWithDataModel<UserInfo>> UserLogin(AuthenticateRequestModel requestModel);
        Task<ApiResponseModel> UserLogout(LogoutRequestModel requestModel);
        Task<ApiResponseModel> CreateCoupon(CreateUpdateCouponRequestModel requestModel);
        Task<ApiResponseModel> UpdateCoupon(CreateUpdateCouponRequestModel requestModel);
        Task<APIResponseWithDataModel> GetListOfCoupon(UserRequestModel requestModel);
        Task<APIResponseWithDataModel> GetCouponHistoryReport(UserRequestModel requestModel);
        Task<APIResponseWithDataModel> GetExchangePointHistoryReport(UserRequestModel requestModel);
        Task<APIResponseWithDataModel> GetListOfMember(UserRequestModel requestModel);
    }
}
