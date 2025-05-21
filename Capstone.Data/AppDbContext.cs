using System.Collections.Generic;
using Capstone.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Persistence.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<user> Farmers { get; set; }
    public DbSet<Crop> Crops { get; set; }
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Fertilizer> Fertilizers { get; set; }
    public DbSet<Pesticide> Pesticides { get; set; }
    public DbSet<CropShop> CropShops { get; set; }
    public DbSet<GrowingCrop> GrowingCrops { get; set; }
    public DbSet<CropDisease> CropDiseases  { get; set; }


}
