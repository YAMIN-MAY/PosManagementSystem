using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetTestSolution.Domain.Context
{
    public class TableContextModel
    {
    }

    public class UsersTblModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginLogTblModel
    {
        [Key]
        public int ID { get; set; }
        public DateTime? LoginDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string SessionId { get; set; }
        public string DeviceId { get; set; }
        public DateTime? LogoutDate { get; set; }
        public int CreatedUserID { get; set; }
    }

    public class MembersTblModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string MobileNo{ get; set; }
        public string MemberType { get; set; }
        public string Email { get; set; }
        public int TotalPoints { get; set; }
        public decimal PurchasedAmount { get; set; }
        public string MemberCode { get; set; }
        public string QRCodeURL { get; set; }
    }

    public class CouponTblModel
    {
        [Key]
        public int ID { get; set; }
        public string CouponName { get; set; }
        public string CouponCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public int AvailableQuantity { get; set; }
        public string QRCodeURL { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }

    public class CouponHistoryTableModel
    {
        [Key]
        public int ID { get; set; }
        public string CouponID { get; set; }
        public string CouponName{ get; set; }
        public string CouponCode { get; set; }
        public string MemberCode { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime UsedDate { get; set; }

    }

    public class ExchangePointHistoryTableModel
    {
        [Key]
        public int ID { get; set; }
        public string MemberId { get; set; }
        public string MemberCode { get; set; }
        public int ExchangePoint { get; set; }
        public decimal ExchangeCouponAmount { get; set; }
        public int RemainingPoint { get; set; }

    }

    public class OrderTblModel
    {
        [Key]
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public decimal TotalAmt { get; set; }
        public int OrderUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
        public virtual ICollection<OrderDetailTblModel> OrderDetail { get; set; }
    }

    public class OrderDetailTblModel
    {
        [Key]
        public int ID { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public virtual OrderTblModel Order { get; set; }
    }

    public class UserRefreshTokens
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
