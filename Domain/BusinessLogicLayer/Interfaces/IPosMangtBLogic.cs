using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTestSolution.Domain.Models;

namespace NetTestSolution.Domain.BusinessLogicLayer.Interfaces
{
    public interface IPosMangtBLogic
    {
        Task<ApiResponseModel> PosInquiry(PosInquiryRequestModel requestModel);
        Task<ApiResponseModel> CalculateMemberEarningPoint(CalculateMemberEarningPointRequestModel requestModel);
    }
}
