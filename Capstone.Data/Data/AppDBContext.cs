using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Persistence.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
        { 
    }

    public DbSet<> entities { get; set; };
}
