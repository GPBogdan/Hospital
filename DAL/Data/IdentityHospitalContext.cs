using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class IdentityHospitalContext : IdentityDbContext<IdentityUser>
{
    public IdentityHospitalContext(DbContextOptions<IdentityHospitalContext> options)
        : base(options)
    {
    }

    protected IdentityHospitalContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
