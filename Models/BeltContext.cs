using Microsoft.EntityFrameworkCore;
 
namespace BeltExam.Models
{
    public class BeltContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }       
        public DbSet<Login> Login { get; set; }
        public DbSet<Ideas> Ideas { get; set; }
        public DbSet<Likes> Likes { get; set; }
        
        
             
    }
}