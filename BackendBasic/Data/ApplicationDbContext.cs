using BackendBasic.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendBasic.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> UserDb { get; set; }
}