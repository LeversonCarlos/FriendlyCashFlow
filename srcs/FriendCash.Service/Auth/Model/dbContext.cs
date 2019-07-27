#region Using
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Auth.Model
{
   public class dbContext : IdentityDbContext<bindUser>
   {

      #region New
      public dbContext() : base("DefaultConnection", throwIfV1Schema: false)
      {
         Configuration.ProxyCreationEnabled = false;
         Configuration.LazyLoadingEnabled = false;
         Database.SetInitializer<dbContext>(null);
      }
      #endregion

      #region Entities
      public DbSet<bindClient> Clients { get; set; }
      public DbSet<bindToken> Tokens { get; set; }
      public DbSet<bindSignature> Signatures { get; set; }
      #endregion

      #region Create
      public static dbContext Create()
      {
         return new dbContext();
      }
      #endregion

      #region ModelCreation
      protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<bindUser>().ToTable("v5_identityUsers"); //.Property(p => p.Id).HasColumnName("UserId");
         modelBuilder.Entity<IdentityUser>().ToTable("v5_identityUsers"); //.Property(p => p.Id).HasColumnName("UserId");
         modelBuilder.Entity<IdentityUserRole>().ToTable("v5_identityUserRoles");
         modelBuilder.Entity<IdentityUserLogin>().ToTable("v5_identityLogins");
         modelBuilder.Entity<IdentityUserClaim>().ToTable("v5_identityClaims");
         modelBuilder.Entity<IdentityRole>().ToTable("v5_identityRoles");
         modelBuilder.Entity<bindClient>().ToTable("v5_identityClients");
         modelBuilder.Entity<bindToken>().ToTable("v5_identityTokens");
         modelBuilder.Entity<bindSignature>().ToTable("v5_identitySignatures");

      }
      #endregion

   }
}