using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetTestSolution.Domain.Models
{
    public class PostMgntRequestResponseModel
    {
    }

    public class PosInquiryRequestModel
    {
        public string CouponQRInfo { get; set; }
        public string MenberQRInfo { get; set; }
        public string ReceiptNumber { get; set; }
        public List<ItemInfo> Itemlist { get; set; }
    }

    public class ItemInfo
    {
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Qty { get; set; }
        public string TotalPrice { get; set; }
    }

    public class CalculateMemberEarningPointRequestModel
    {
        public string MenberId { get; set; }
        public string MemberCode { get; set; }
        public List<ItemInfo> Itemlist { get; set; }
    }
}
