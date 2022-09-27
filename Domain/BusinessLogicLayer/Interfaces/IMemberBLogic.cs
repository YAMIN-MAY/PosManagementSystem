using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTestSolution.Domain.Models;

namespace NetTestSolution.Domain.BusinessLogicLayer.Interfaces
{
    public interface IMemberBLogic
    {
        Task<ApiResponseWithDataModel<MemberRegistrationResponseModel>> MemberRegistration(MemberRegistrationRequestModel requestModel);
        Task<ApiResponseWithDataModel<MemberLoginResponseModel>> MemberUserLogin(MemberLoginRequestModel requestModel);
        Task<ApiResponseModel> MemberUserLogout(LogoutRequestModel requestModel);
        Task<APIResponseWithDataModel> GetPurchaseHistory(UserRequestModel requestModel);
        Task<APIResponseWithDataModel> GetTotalPointByMemberId(GetTotalPointByMemberId requestModel);
        Task<ApiResponseModel> ExchangePointByMemberId(ExchangePointByMemberIdRequestModel requestModel);

    }
}
