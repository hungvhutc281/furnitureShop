using Microsoft.EntityFrameworkCore;
using ProjectApi.Model;

namespace FurnitureStore.Models
{
    public class FurnitureStoreContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Customer> Customers { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public FurnitureStoreContext(DbContextOptions<FurnitureStoreContext> options)
            : base(options)
        {
        }
       

    }
}
