namespace TradeScales.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using TradeScales.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<TradeScalesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TradeScalesContext context)
        {
            // Create Customers
            context.CustomerSet.AddOrUpdate(GenerateCustomers());
           
            // Create Products
            context.ProductSet.AddOrUpdate(GenerateProducts());

            // Create Hauliers
            context.HaulierSet.AddOrUpdate(GenerateHauliers());

            // Create Drivers
            context.DriverSet.AddOrUpdate(GenerateDrivers());

            // Create Destinations
            context.DestinationSet.AddOrUpdate(GenerateDestinations());

            // Create StatusMessages
            context.StatusMessageSet.AddOrUpdate(GenerateStatusMessages());

            // Create Roles
            context.RoleSet.AddOrUpdate(r => r.Name, GenerateRoles());
  
            // username: admin, password: admin
            context.UserSet.AddOrUpdate(u => u.Email, new User[]{
                new User()
                {
                    Email="admin@tradescales.co.za",
                    Username="admin",
                    HashedPassword ="noCjMkhAGm87COu62R9sTeuQblHfn480A6/NVcH0Kk4=",
                    Salt = "xJHxcRv5DcquBQP5cCMHxA==",
                    IsLocked = false,
                    DateCreated = DateTime.Now
                }
            });

            // // create user-admin for whurter5
            context.UserRoleSet.AddOrUpdate(new UserRole[] {
                new UserRole() {
                    RoleId = 1, // admin
                    UserId = 1  // whurter5
                }
            });
        }

        #region Generate Functions

        private Customer[] GenerateCustomers()
        {
            Customer[] customers = new Customer[]
            {
                new Customer(){Code = "CUS-001", Name = "Customer 1"},
                new Customer(){Code = "CUS-002", Name = "Customer 2"},
                new Customer(){Code = "CUS-003", Name = "Customer 3"}
            };
            return customers;
        }
       
        private Product[] GenerateProducts()
        {
            Product[] products = new Product[]
            {
                new Product(){Code = "PRO-001", Name = "Product 1"},
                new Product(){Code = "PRO-002", Name = "Product 2"},
                new Product(){Code = "PRO-003", Name = "Product 3"}
            };

            return products;
        }

        private Haulier[] GenerateHauliers()
        {
            Haulier[] hauliers = new Haulier[]
            {
                new Haulier(){Code = "HAU-001", Name = "Haulier 1"},
                new Haulier(){Code = "HAU-002", Name = "Haulier 2"},
                new Haulier(){Code = "HAU-003", Name = "Haulier 3"}
            };

            return hauliers;
        }

        private Driver[] GenerateDrivers()
        {
            List<Driver> drivers = new List<Driver>();

            // Create 100 drivers
            for (int i = 0; i < 100; i++)
            {
                Driver customer = new Driver()
                {                 
                    Code = string.Format("Dri-{0}", (i + 1).ToString("D4")),
                    FirstName = MockData.Person.FirstName(),
                    LastName = MockData.Person.Surname(),
                    Email = MockData.Internet.Email(),
                    IdentityCard = Guid.NewGuid().ToString(),
                    Mobile = MockData.RandomNumber.Next(100000000, 999999999).ToString(),
                    VehicleRegistration = string.Format("CA-{0}", MockData.RandomNumber.Next(100000, 999999)),
                    UniqueKey = Guid.NewGuid(),
                    DateOfBirth = new DateTime(1985, 10, 20).AddMonths(i).AddDays(10),
                    RegistrationDate = DateTime.Now.AddDays(i)
                };

                drivers.Add(customer);
            }


            return drivers.ToArray();
        }

        private Destination[] GenerateDestinations()
        {
            Destination[] destinations = new Destination[]
            {
                new Destination(){Code = "DES-001", Name = "Destination 1"},
                new Destination(){Code = "DES-002", Name = "Destination 2"},
                new Destination(){Code = "DES-003", Name = "Destination 3"}
            };

            return destinations;
        }

        private Role[] GenerateRoles()
        {
            Role[] roles = new Role[]{
                new Role(){Name="Admin"}
            };

            return roles;
        }

        private StatusMessage[] GenerateStatusMessages()
        {
            StatusMessage[] statusMessages = new StatusMessage[]
           {
                new StatusMessage(){Type = "SUCCESS", Message = "Welcome to our Early Access Program for TradeScales."},
                new StatusMessage(){Type = "SUCCESS", Message = "We are glad to have you on board, because your feedback will help us make the production release of TradeScales fully functional with lots of new and useful features."}               
           };

            return statusMessages;
        }

        #endregion
    }
}
