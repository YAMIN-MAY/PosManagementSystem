using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetTestSolution.Domain.Models
{
    public class CouponCMSRequestResponseModel
    {
    }

    public class UserRequestModel 
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string SessionID { get; set; }
    } 

    public class CreateUpdateCouponRequestModel : UserRequestModel
    {
        public string ID { get; set; }

        [Required]
        public string CouponName { get; set; }

        [Required]
        public string CouponCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public string DiscountAmount { get; set; }

        [Required]
        public string AvailableQuantity { get; set; }
        public string QRCodeURL{ get; set; }
    }

    public class GenerateQrRequestModel
    {
        public string QrId { get; set; }
        public string CouponId { get; set; }

        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
