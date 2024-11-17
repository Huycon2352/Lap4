using Microsoft.EntityFrameworkCore;
using NT106_P11.Models;

namespace NT106_P11.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } // Định nghĩa DbSet cho bảng Members
    }
}
