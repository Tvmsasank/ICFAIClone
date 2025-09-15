using ICFAIClone.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }

    internal string SaveChanges(string connStr)
    {
        throw new NotImplementedException();
    }
}
