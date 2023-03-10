using FlightFinderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightFinderApi.Data
{
    public class FlightFinderDbContext : DbContext
    {
        public FlightFinderDbContext(DbContextOptions options) : base(options)
        {
        }
          public DbSet<Itinerary> Itineraries { get; set; }
          public DbSet<Price> Prices { get; set; }
          public DbSet<Root> Roots { get; set; }
          public DbSet<User> Users { get; set; }
          public DbSet<Booking> Bookings { get; set; }

    }
}
