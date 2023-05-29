using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistic;

public class DataBaseContext: DbContext {
    public DataBaseContext(DbContextOptions <DataBaseContext> options): base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.UseSerialColumns();
    }
    
    public DbSet <Customer> Customers { get; set; }
    public DbSet <Address> Addresses { get; set; }
    public DbSet <DeliveryPoint> DeliveryPoints { get; set; }
}