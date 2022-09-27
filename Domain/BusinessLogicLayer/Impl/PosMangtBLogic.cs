using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTestSolution.Domain.BusinessLogicLayer.Interfaces;
using NetTestSolution.Domain.Context;
using NetTestSolution.Domain.Models;
using NetTestSolution.Domain.Service;
using Newtonsoft.Json;

namespace NetTestSolution.Domain.BusinessLogicLayer.Impl
{
    public class PosMangtBLogic : IPosMangtBLogic
    {
        private readonly PosMgntDbContext _dbContext;
        private readonly ICacheService _cacheService;

        public PosMangtBLogic(PosMgntDbContext posMgntDbContext, ICacheService service)
        {
            _dbContext = posMgntDbContext;
            _cacheService = service;
        }
        public async Task<ApiResponseModel> PosInquiry(PosInquiryRequestModel requestModel)
        {
            var calReqModel = new CalculateMemberEarningPointRequestModel();
            if (!string.IsNullOrEmpty(requestModel.MenberQRInfo))
            {
                var memberCode = JsonConvert.DeserializeObject<GenerateMemberQrRequestModel>(requestModel.MenberQRInfo);

                calReqModel.MenberId = memberCode.MemberId;
                calReqModel.MemberCode = memberCode.MemberCode;
                calReqModel.Itemlist = requestModel.Itemlist;
            }

            if (!string.IsNullOrEmpty(requestModel.CouponQRInfo))
            {
                var couponCode = JsonConvert.DeserializeObject<GenerateQrRequestModel>(requestModel.CouponQRInfo);

                var lstOfCoupon = await _dbContext.couponTblModel.Where(c => c.StartDate.Date <= DateTime.Now.Date && c.EndDate.Date >= DateTime.Now.Date).Select(c => c).ToListAsync();

                #region Redis Cache

                var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                _cacheService.SetData("coupon", lstOfCoupon, expirationTime);

                var cacheData = _cacheService.GetData<IEnumerable<CouponTblModel>>("coupon");
                if (cacheData != null)
                {
                    CouponTblModel filteredData = cacheData.Where(x => x.ID == int.Parse(couponCode.CouponId)).FirstOrDefault();

                    if (filteredData.AvailableQuantity != 0)
                    {
                        var couponInfo = await _dbContext.couponTblModel.Where(c => c.ID == int.Parse(couponCode.CouponId) && c.StartDate.Date <= DateTime.Now.Date && c.EndDate.Date >= DateTime.Now.Date).Select(c => c).FirstOrDefaultAsync();
                        if (couponInfo != null)
                        {
                            couponInfo.AvailableQuantity--;
                            _cacheService.RemoveData("coupon");
                            await _dbContext.SaveChangesAsync();

                            var couponHistory = new CouponHistoryTableModel();
                            couponHistory.CouponID = couponCode.CouponId;
                            couponHistory.CouponCode = couponCode.CouponCode;
                            couponHistory.CouponName = couponCode.CouponName;
                            couponHistory.MemberCode = calReqModel.MemberCode;
                            couponHistory.ReceiptNumber = requestModel.ReceiptNumber;
                            couponHistory.UsedDate = DateTime.Now;

                            await _dbContext.AddAsync(couponHistory);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }

                #endregion

            }

            var res = await CalculateMemberEarningPoint(calReqModel);

            return res;

        }

        public async Task<ApiResponseModel> CalculateMemberEarningPoint(CalculateMemberEarningPointRequestModel requestModel)
        {
            decimal totalAmount = 0;
            int earnPoint = 0;
            if (requestModel.Itemlist.Count > 0)
            {
                totalAmount = requestModel.Itemlist.Sum(i => Convert.ToDecimal(i.TotalPrice));

                if (totalAmount != 0)
                {
                    earnPoint = Convert.ToInt32( totalAmount / 10 );
                }

                var memberInfo = await _dbContext.membersTblModel.Where(m => m.ID == int.Parse(requestModel.MenberId)).Select(m => m).FirstOrDefaultAsync();
                memberInfo.PurchasedAmount = totalAmount;
                memberInfo.TotalPoints = earnPoint;

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return new ApiResponseModel
                {
                    ResponseCode = "012",
                    ResponseDescription = "No Purchase Items."
                };
            }

            return new ApiResponseModel
            {
                ResponseCode = "000",
                ResponseDescription = "Success"
            };
        }
    }
}
