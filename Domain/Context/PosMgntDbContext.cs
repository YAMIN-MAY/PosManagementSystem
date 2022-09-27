using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetTestSolution.Domain.Context
{
    public class PosMgntDbContext : DbContext
    {
        public PosMgntDbContext(DbContextOptions<PosMgntDbContext> options) : base(options)
        {

        }

        public DbSet<UsersTblModel> usersTblModel { get; set; }
        public DbSet<LoginLogTblModel> loginlogTblModel { get; set; }
        public DbSet<CouponTblModel> couponTblModel { get; set; }
        public DbSet<MembersTblModel> membersTblModel { get; set; }
        public DbSet<CouponHistoryTableModel> couponHistoryTableModel { get; set; }
        public DbSet<ExchangePointHistoryTableModel> exchangePointHistoryTblModel { get; set; }
        public DbSet<OrderDetailTblModel> orderDetailTblModel { get; set; }
        public DbSet<OrderTblModel> orderTblModel { get; set; }
        public DbSet<UserRefreshTokens> UserRefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersTblModel>().ToTable("users");
            modelBuilder.Entity<LoginLogTblModel>().ToTable("loginlog");
            modelBuilder.Entity<CouponTblModel>().ToTable("coupon");
            modelBuilder.Entity<MembersTblModel>().ToTable("member");
            modelBuilder.Entity<CouponHistoryTableModel>().ToTable("couponhistory");
            modelBuilder.Entity<ExchangePointHistoryTableModel>().ToTable("exchangepointhistory");
            modelBuilder.Entity<OrderDetailTblModel>().ToTable("orderdetail");
            modelBuilder.Entity<OrderTblModel>().ToTable("purchaseorder");
            base.OnModelCreating(modelBuilder);
        }
    }
}
