using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GYMONE.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace GYMMVC6_Identity.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        ////public DbSet<YearwiseModel> YearwiseModel { get; set; }
        ////public DbSet<MonthwiseModel> MonthwiseModel { get; set; }
        ////public DbSet<RecepitDTO> ReciepDtos { get; set; }
        ////public DbSet<MemberRegistrationDTO> MemberRegistrationDtos { get; set; }
        ////public DbSet<PaymentDetailsDTO> PaymentDetailsDtos { get; set; }
        ////public DbSet<PlanMasterDTO> PalnMasterDtos { get; set; }
        ////public DbSet<SchemeMasterDTO> SchemeMasterDtos { get; set; }
        ////public DbSet<Users> UsersDtos{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ////builder.Entity<CartItem>().HasKey(b => b.CartItemId);

            ////// TODO: Remove when explicit values insertion removed.
            ////builder.Entity<Artist>().Property(a => a.ArtistId).ValueGeneratedNever();
            ////builder.Entity<Genre>().Property(g => g.GenreId).ValueGeneratedNever();

            //////Deleting an album fails with this relation
            ////builder.Entity<Album>().Ignore(a => a.OrderDetails);
            ////builder.Entity<OrderDetail>().Ignore(od => od.Album);

            base.OnModelCreating(builder);
        }
    }

}
