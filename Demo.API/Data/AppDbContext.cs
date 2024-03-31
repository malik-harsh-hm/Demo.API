using Demo.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Data
{
    public class AppDbContext: IdentityDbContext<UserModel>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<EmployeeModel> Employees { get; set; }
    }
}
