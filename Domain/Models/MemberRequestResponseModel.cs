using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetTestSolution.Domain.Models
{
    public class MemberRequestResponseModel
    {
    }

    public class MemberRegistrationRequestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string MemberType { get; set; }
        public string QRCodeURL { get; set; }
        public string Password { get; set; }
        public string MemberCode { get; set; }
    }

    public class MemberRegistrationResponseModel
    {
        public string QRCodeURL { get; set; }
        public string Password { get; set; }
    }

    public class GenerateMemberQrRequestModel
    {
        public string MemberId { get; set; }
        public string MemberCode { get; set; }
        public string QrId { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }

    public class MemberLoginRequestModel
    {
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string Password { get; set; }
        public string DeviceId { get; set; }
    }

    public class MemberLoginResponseModel
    {
        public string UserID { get; set; }
        public string MemberID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string UserType { get; set; }
        public string MemberType { get; set; }
        public string SessionId { get; set; }
    }

    public class GetTotalPointByMemberId : UserRequestModel
    {
        [Required]
        public string MemberID { get; set; }
    }

    public class ExchangePointByMemberIdRequestModel : UserRequestModel
    {
        [Required]
        public string MemberID { get; set; }
        [Required]
        public int Points { get; set; }
    }

    public class OrderDetail
    {
        public int ID { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
