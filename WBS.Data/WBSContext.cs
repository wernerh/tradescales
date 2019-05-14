using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WBS.Data.Configuration;
using WBS.Entities;

namespace WBS.Data
{
    public class WBSContext : DbContext
    {
        public WBSContext() : base("WBS")
        {
            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directoryPath = $"{rootPath}\\ThinkIT\\WBS";
            AppDomain.CurrentDomain.SetData("DataDirectory", directoryPath);
            Database.SetInitializer<WBSContext>(null);
        }

        #region Entity Sets 
        public IDbSet<User> UserSet { get; set; }
        public IDbSet<Role> RoleSet { get; set; }
        public IDbSet<UserRole> UserRoleSet { get; set; }
        public IDbSet<Ticket> TicketSet { get; set; }
        public IDbSet<Customer> CustomerSet { get; set; }
        public IDbSet<Product> ProductSet { get; set; }
        public IDbSet<Haulier> HaulierSet { get; set; }
        public IDbSet<Driver> DriverSet { get; set; }
        public IDbSet<Vehicle> VehicleSet { get; set; }
        public IDbSet<Destination> DestinationSet { get; set; }
        public IDbSet<Error> ErrorSet { get; set; }
        public IDbSet<StatusMessage> StatusMessageSet { get; set; }
        #endregion

        public virtual void Commit()
        {
            var result = base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new TicketConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new HaulierConfiguration());
            modelBuilder.Configurations.Add(new DriverConfiguration());
            modelBuilder.Configurations.Add(new VehicleConfiguration());
            modelBuilder.Configurations.Add(new DestinationConfiguration());
        }
    }
}
