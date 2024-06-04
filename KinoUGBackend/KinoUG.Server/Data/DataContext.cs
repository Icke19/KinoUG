using KinoUG.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KinoUG.Server.Data
{
    public class DataContext:IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        
       public DbSet<User> Users { get; set; } 
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet <Seat> Seats { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTickets)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            
            modelBuilder.Entity<Hall>()
                .HasMany(m => m.Seats)
                .WithOne(t => t.Hall)
                .HasForeignKey(t => t.HallId);
           
           modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.Tickets)
                .WithOne(t => t.Schedule)
                .HasForeignKey(t => t.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Movie>() 
                .HasMany(m => m.Schedules)
                .WithOne(s => s.Movie)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
                
                


        }

        
    }
}
