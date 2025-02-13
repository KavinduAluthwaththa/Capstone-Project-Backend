using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Persistence.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
        { 
    }

    public DbSet<Farmer> Farmer { get; set; }
    public DbSet<Crop> Crop { get; set; }
    public DbSet<Disease> Disease { get; set; }
    public DbSet<Request> Request { get; set; }
    public DbSet<Shop> Shop { get; set; }
    public DbSet<Item> Item { get; set; }
    public DbSet<MarketData> MarketData { get; set; }
    public DbSet<Inspector> Inspector { get; set; }
    public DbSet<Fertilizer> Fertilizer { get; set; }
    public DbSet<Pesticide> Pesticide { get; set; }
    public DbSet<CropShop> CropShop { get; set; }
    public DbSet<GrowingCrop> CropDisease { get; set; }
    public DbSet<CropDisease> GrowingCrop { get; set; }




}
