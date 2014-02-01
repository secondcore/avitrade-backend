using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories
{
    public class AviTradeContext : DbContext
    {
        //static AviTradeContext()
        //{
        //    Console.WriteLine("Inside Static AviTradeContext Constructor!!!!");
        //    //Database.SetInitializer(new DropCreateDatabaseAlways<SecondCore.TradingHub.Repository.AviTradeContext>());
        //    Database.SetInitializer(new DropCreateDatabaseAlwaysWithSeedData());
        //}

        public DbSet<Region> Regions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Vimba.AviTrade.Models.TimeZone> TimeZones { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Instance> Instances { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<TraderRegistrationToken> TraderRegistrationTokens { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLineItem> OrderLineItems { get; set; }
        public DbSet<OrderView> OrderViews { get; set; }
        public DbSet<OrderArchive> OrderArchives { get; set; }
        public DbSet<OrderIgnore> OrderIgnores { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserConfigurationItem> UserConfigurationItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // For now, turn off all cascade delete on one to many relationships
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Configurations for base classes
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new AirportConfiguration());
            modelBuilder.Configurations.Add(new InstanceConfiguration());
            modelBuilder.Configurations.Add(new TraderConfiguration());
            modelBuilder.Configurations.Add(new CreditCardConfiguration());
            modelBuilder.Configurations.Add(new ContractConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new OrderLineItemConfiguration());
            modelBuilder.Configurations.Add(new OrderViewConfiguration());
            modelBuilder.Configurations.Add(new OrderArchiveConfiguration());
            modelBuilder.Configurations.Add(new OrderIgnoreConfiguration());
            modelBuilder.Configurations.Add(new InvoiceConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new UserConfigurationItemConfiguration());
            modelBuilder.Configurations.Add(new TraderRegistrationTokenConfiguration());

            // Configurations for extended classes
        }
    }

    internal class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            HasRequired(l => l.Region).WithMany().Map(c => c.MapKey("RegionId"));
            HasRequired(l => l.Currency).WithMany().Map(c => c.MapKey("CurrencyId"));
        }
    }

    internal class AirportConfiguration : EntityTypeConfiguration<Airport>
    {
        public AirportConfiguration()
        {
            HasRequired(l => l.Country).WithMany().Map(c => c.MapKey("CountryId"));
        }
    }

    internal class InstanceConfiguration : EntityTypeConfiguration<Instance>
    {
        public InstanceConfiguration()
        {
            HasRequired(l => l.PivotCurrency).WithMany().Map(c => c.MapKey("PivotCurrencyId"));
            HasRequired(l => l.GlobalPivotCurrency).WithMany().Map(c => c.MapKey("GlobalPivotCurrencyId"));
        }
    }

    internal class CreditCardConfiguration : EntityTypeConfiguration<CreditCard>
    {
        public CreditCardConfiguration()
        {
            HasRequired(l => l.Trader).WithMany(l=> l.CreditCards).Map(c => c.MapKey("TraderId"));
        }
    }

    internal class ContractConfiguration : EntityTypeConfiguration<Contract>
    {
        public ContractConfiguration()
        {
            HasRequired(l => l.TraderOne).WithMany().Map(c => c.MapKey("TraderOneId"));
            HasRequired(l => l.TraderTwo).WithMany().Map(c => c.MapKey("TraderTwoId"));
            HasRequired(l => l.BillingCurrency).WithMany().Map(c => c.MapKey("BillingCurrencyId"));
            HasRequired(l => l.Instance).WithMany().Map(c => c.MapKey("InstanceId"));
            HasRequired(l => l.TimeZone).WithMany().Map(c => c.MapKey("TimeZoneId"));
        }
    }

    internal class TraderConfiguration : EntityTypeConfiguration<Trader>
    {
        public TraderConfiguration()
        {
            HasRequired(l => l.Country).WithMany().Map(c => c.MapKey("CountryId"));
        }
    }

    internal class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            HasOptional(l => l.TakeoffAirport).WithMany().Map(c => c.MapKey("TakeoffAirportId"));
            HasOptional(l => l.LandingAirport).WithMany().Map(c => c.MapKey("LandingAirportId"));
            HasOptional(l => l.Aircraft).WithMany().Map(c => c.MapKey("AircraftId"));
            //HasOptional(l => l.Invoice).WithOptionalPrincipal().Map(c => c.MapKey("InvoiceId"));
            HasRequired(l => l.Contract).WithMany().Map(c => c.MapKey("ContractId"));
            HasRequired(l => l.Buyer).WithMany().Map(c => c.MapKey("BuyerId"));
            HasRequired(l => l.Seller).WithMany().Map(c => c.MapKey("SellerId"));
        }
    }

    internal class OrderLineItemConfiguration : EntityTypeConfiguration<OrderLineItem>
    {
        public OrderLineItemConfiguration()
        {
            HasRequired(l => l.Order).WithMany(m => m.LineItems).Map(c => c.MapKey("OrderId"));
            HasRequired(l => l.Item).WithMany().Map(c => c.MapKey("ItemId"));
            HasRequired(l => l.Currency).WithMany().Map(c => c.MapKey("CurrencyId"));
            HasRequired(l => l.Order).WithMany(l => l.LineItems).Map(c => c.MapKey("OrderId"));
        }
    }

    internal class InvoiceConfiguration : EntityTypeConfiguration<Invoice>
    {
        public InvoiceConfiguration()
        {
            HasRequired(l => l.BillingPeriod).WithMany().Map(c => c.MapKey("BillingPeriodId"));
            //HasRequired(l => l.Order).WithRequiredDependent(c => c.Invoice).Map(c => c.MapKey("OrderId"));
            HasRequired(l => l.Order).WithMany().Map(c => c.MapKey("OrderId"));
        }
    }

    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasRequired(l => l.Group).WithMany().Map(c => c.MapKey("GroupId"));
            // MKA - 30AUG2012 - Removed the complexity of having multiple roles per user
            HasRequired(l => l.Role).WithMany().Map(c => c.MapKey("RoleId"));
            //HasMany(t => t.Roles).WithMany(a => a.Users).Map(c =>
            //{
            //    c.ToTable("UserRoles");
            //    c.MapLeftKey("RoleId"); // Refers to the class being configured
            //    c.MapRightKey("UserId");
            //});
        }
    }

    internal class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
        }
    }

    internal class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
        }
    }

    internal class UserConfigurationItemConfiguration : EntityTypeConfiguration<UserConfigurationItem>
    {
        public UserConfigurationItemConfiguration()
        {
            HasRequired(l => l.User).WithMany(l => l.ConfigurationItems).Map(c => c.MapKey("UserId"));
        }
    }

    internal class TraderRegistrationTokenConfiguration : EntityTypeConfiguration<TraderRegistrationToken>
    {
        public TraderRegistrationTokenConfiguration()
        {
            HasRequired(l => l.Trader).WithMany().Map(c => c.MapKey("TraderId"));
            HasRequired(l => l.Role).WithMany().Map(c => c.MapKey("RoleId"));
        }
    }

    internal class OrderViewConfiguration : EntityTypeConfiguration<OrderView>
    {
        public OrderViewConfiguration()
        {
            // Indicate that this has a compound key consisting of two keys
            HasKey(k => new { k.OrderId, k.UserId, k.Status});
            //HasRequired(l => l.Order).WithMany().Map(c => c.MapKey("OrderId"));
            //HasRequired(l => l.User).WithMany().Map(c => c.MapKey("UserId"));
        }
    }

    internal class OrderArchiveConfiguration : EntityTypeConfiguration<OrderArchive>
    {
        public OrderArchiveConfiguration()
        {
            // Indicate that this has a compound key consisting of two keys
            HasKey(k => new { k.OrderId, k.UserId });
            //HasRequired(l => l.Order).WithMany().Map(c => c.MapKey("OrderId"));
            //HasRequired(l => l.User).WithMany().Map(c => c.MapKey("UserId"));
        }
    }

    internal class OrderIgnoreConfiguration : EntityTypeConfiguration<OrderIgnore>
    {
        public OrderIgnoreConfiguration()
        {
            // Indicate that this has a compound key consisting of two keys
            HasKey(k => new { k.OrderId, k.UserId });
            //HasRequired(l => l.Order).WithMany().Map(c => c.MapKey("OrderId"));
            //HasRequired(l => l.User).WithMany().Map(c => c.MapKey("UserId"));
        }
    }

    // Configurations for Extended Classes

    /*** Initializer ****/

    public class DropCreateDatabaseAlwaysWithSeedData : DropCreateDatabaseAlways<AviTradeContext>
    {
        protected override void Seed(AviTradeContext context)
        {
            try
            {
                context.Languages.Add(new Language() { Id = "EN", Name = "English" });
                context.Languages.Add(new Language() { Id = "AR", Name = "Arabic" });

                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -12, Name = "Pacific -12" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -11, Name = "Pacific -11" });
                Vimba.AviTrade.Models.TimeZone uaeTimeZone = new Vimba.AviTrade.Models.TimeZone();
                uaeTimeZone.Id = -10;
                uaeTimeZone.Name = "Pacific - 10";
                context.TimeZones.Add(uaeTimeZone);
                Vimba.AviTrade.Models.TimeZone egyTimeZone = new Vimba.AviTrade.Models.TimeZone();
                egyTimeZone.Id = -9;
                egyTimeZone.Name = "Pacific - 9";
                context.TimeZones.Add(egyTimeZone);
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -8, Name = "Pacific -8" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -7, Name = "Pacific -7" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -6, Name = "Pacific -6" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -5, Name = "Pacific -5" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -4, Name = "Pacific -4" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -3, Name = "Pacific -3" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -2, Name = "Pacific -2" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = -1, Name = "Pacific -1" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = 0, Name = "Pacific" });
                context.TimeZones.Add(new Vimba.AviTrade.Models.TimeZone() { Id = 3, Name = "Pacfic +3" });

                Currency uaeCurrency = new Currency() { Id = "AED", Symbol = "AE", Name = "Dirham Emarati", Rate = 3.67 };
                Currency usdCurrency = new Currency() { Id = "USD", Symbol = "$", Name = "United States Dollars", Rate = 1.00 };
                Currency lebCurrency = new Currency() { Id = "LEB", Symbol = "LB", Name = "Lebanese Pound", Rate = 1500 };
                Currency saCurrency = new Currency() { Id = "SAR", Symbol = "SR", Name = "Saudi Ryal", Rate = 3.67 };
                Currency egCurrency = new Currency() { Id = "EGY", Symbol = "EG", Name = "Egyptian Jeneh", Rate = 7 };

                context.Currencies.Add(uaeCurrency);
                context.Currencies.Add(usdCurrency);
                context.Currencies.Add(lebCurrency);
                context.Currencies.Add(saCurrency);
                context.Currencies.Add(egCurrency);

                Region region = new Region() { Id = "ME", Name = "Middle East" };
                context.Regions.Add(region);

                Country uae = new Country();
                uae.Id = "UAE";
                uae.Name = "United Arab Emirates";
                uae.Region = region;
                uae.Currency = uaeCurrency;
                context.Countries.Add(uae);

                Country leb = new Country();
                leb.Id = "LEB";
                leb.Name = "Lebanon";
                leb.Region = region;
                leb.Currency = lebCurrency;
                context.Countries.Add(leb);

                Country sar = new Country();
                sar.Id = "SAR";
                sar.Name = "Saudi Arabia";
                sar.Region = region;
                sar.Currency = saCurrency;
                context.Countries.Add(sar);

                Country egy = new Country();
                egy.Id = "EGY";
                egy.Name = "Egypt";
                egy.Region = region;
                egy.Currency = egCurrency;
                context.Countries.Add(egy);

                context.Groups.Add(new Group() { Id = Group.AviTrade, Name = Group.AviTrade, Description = "AviTrade User. It can be admins, employess or officers." });
                context.Groups.Add(new Group() { Id = Group.Mebaa, Name = Group.Mebaa, Description = "Mebaa Member Users" });
                context.Groups.Add(new Group() { Id = Group.Traders, Name = Group.Traders, Description = "Avirade Trader Users" });

                context.Roles.Add(new Role() { Id = Role.Admins, Name = Role.Admins, Description = Role.Admins });
                context.Roles.Add(new Role() { Id = Role.Executives, Name = Role.Executives, Description = Role.Executives });
                context.Roles.Add(new Role() { Id = Role.IT, Name = Role.IT, Description = Role.IT });
                context.Roles.Add(new Role() { Id = Role.Marketing, Name = Role.Marketing, Description = Role.Marketing });
                context.Roles.Add(new Role() { Id = Role.Sales, Name = Role.Sales, Description = Role.Sales });
                context.Roles.Add(new Role() { Id = Role.Finance, Name = Role.Finance, Description = Role.Finance });
                context.Roles.Add(new Role() { Id = Role.Support, Name = Role.Support, Description = Role.Support });
                context.Roles.Add(new Role() { Id = Role.Pilot, Name = Role.Pilot, Description = Role.Pilot });

                Instance instance = new Instance()
                {
                    Name = "MEBAA AviTrade",
                    Description = "Trading Hub targeting MEBAA members!!!",
                    Industry = "Business Aviation",
                    AdminFeePercentage = 1.5,
                    PivotCurrency = usdCurrency,
                    GlobalPivotCurrency = usdCurrency
                };
                context.Instances.Add(instance);

                context.Items.Add(new Item() { Category = "Flight", SubCategory = "Fuel", Name = "BP Octane 102", Unit = "Liter", Description = "British Pertrolium Octane Grade 102...permium....", Rating = 5, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg"});
                context.Items.Add(new Item() { Category = "Flight", SubCategory = "Fuel", Name = "BP Octane 98", Unit = "Liter", Description = "British Pertrolium Octane Grade 102...good....", Rating = 6, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Flight", SubCategory = "Food", Name = "Steak", Unit = "Meal", Description = "Lamb Steak...", Rating = 8, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Flight", SubCategory = "Food", Name = "Fish", Unit = "Meal", Description = "Salmon ....", Rating = 9, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Flight", SubCategory = "Permits", Name = "UAE Fly Zone", Unit = "Permit", Description = "UAE Fly  Zone Permit....", Rating = 4, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Flight", SubCategory = "Permits", Name = "Qatar Fly Zone", Unit = "Permit", Description = "Qatar Fly  Zone Permit....", Rating = 3, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });

                context.Items.Add(new Item() { Category = "Handling", SubCategory = "Baggage", Name = "Conveyor 10", Unit = "Liter", Description = "British Pertrolium Octane Grade 102...permium....", Rating = 5, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Handling", SubCategory = "Baggage", Name = "Conveyor 12", Unit = "Liter", Description = "British Pertrolium Octane Grade 102...good....", Rating = 6, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Handling", SubCategory = "Taxi", Name = "Towe", Unit = "Pounds", Description = "Lamb Steak...", Rating = 8, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Handling", SubCategory = "Taxi", Name = "Morse", Unit = "Pounds", Description = "Salmon ....", Rating = 9, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Handling", SubCategory = "Parking", Name = "Long Nose", Unit = "Stairs", Description = "UAE Fly  Zone Permit....", Rating = 4, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });
                context.Items.Add(new Item() { Category = "Handling", SubCategory = "Parking", Name = "Short Nose", Unit = "Stairs", Description = "Qatar Fly  Zone Permit....", Rating = 3, ImageUrl = "http://www.gesolutionssl.com/commodities/jet_fuel_for_sale.jpg" });

                context.Aircrafts.Add(new Aircraft()
                {
                    Manufacturer = "Unknown",
                    Model = "UNK"
                });
                context.Aircrafts.Add(new Aircraft()
                {
                    Manufacturer = "Boeing",
                    Model = "BBJ"
                });
                context.Aircrafts.Add(new Aircraft()
                {
                    Manufacturer = "Airbus",
                    Model = "A19"
                });
                context.Aircrafts.Add(new Aircraft()
                {
                    Manufacturer = "Bombardier",
                    Model = "B34"
                });
                context.Aircrafts.Add(new Aircraft()
                {
                    Manufacturer = "Learjet",
                    Model = "C12"
                });

                context.Airports.Add(new Airport()
                {
                    Name = "Unknown",
                    Description = "Unknown.....",
                    City = "Dubai",
                    Country = uae
                });
                context.Airports.Add(new Airport()
                {
                    Name = "World Central",
                    Description = "Lorum.....",
                    City = "Dubai",
                    Country = uae
                });
                context.Airports.Add(new Airport()
                {
                    Name = "Bateen",
                    Description = "Lorum.....",
                    City = "Abou Dhabi",
                    Country = uae
                });
                context.Airports.Add(new Airport()
                {
                    Name = "King Abdul Aziz",
                    Description = "Lorum.....",
                    City = "Jeddah",
                    Country = sar
                });
                context.Airports.Add(new Airport()
                {
                    Name = "Rafik Alhareeri International",
                    Description = "Lorum.....",
                    City = "Beirut",
                    Country = leb
                });
                context.Airports.Add(new Airport()
                {
                    Name = "Cairo International",
                    Description = "Lorum.....",
                    City = "Cairo",
                    Country = egy
                });

                Trader jetex = new Trader();
                jetex.Account = "V767166";
                jetex.Name = "Jetex";
                jetex.Address = "4 DFZA East Wing";
                jetex.Address2 = "";
                jetex.City = "Dubai";
                jetex.Phone = "+971.4.555.1212";
                jetex.Email = "sales@jetex.aero";
                jetex.Contact = "Mr. Wael";
                jetex.Fax = "+971.4.555.1313";
                jetex.CurrentInvoiceCounter = 0;
                jetex.Country = uae;
                context.Traders.Add(jetex);

                CreditCard jetexCc = new CreditCard();
                jetexCc.HolderName = "Jetex Credit Card";
                jetexCc.CardNumber = "4444555566667777";
                jetexCc.ExpDate = "12/20";
                jetexCc.SecurityCode = "920";
                jetexCc.Trader = jetex;
                context.CreditCards.Add(jetexCc);

                Trader hadeed = new Trader();
                hadeed.Account = "V767876";
                hadeed.Name = "Hadeed";
                hadeed.Address = "37 DFZA West Wing";
                hadeed.Address2 = "";
                hadeed.City = "Dubai";
                hadeed.Phone = "+971.4.555.1212";
                hadeed.Email = "sales@hadeed.aero";
                hadeed.Contact = "Miss. Ayeshah";
                hadeed.Fax = "+971.4.555.1313";
                hadeed.CurrentInvoiceCounter = 0;
                hadeed.Country = uae;
                context.Traders.Add(hadeed);

                CreditCard hadeedCc = new CreditCard();
                hadeedCc.HolderName = "Hadeed Credit Card";
                hadeedCc.CardNumber = "4444555566667777";
                hadeedCc.ExpDate = "12/20";
                hadeedCc.SecurityCode = "920";
                hadeedCc.Trader = hadeed;
                context.CreditCards.Add(hadeedCc);

                Trader aljaber = new Trader();
                aljaber.Account = "O9000876";
                aljaber.Name = "Al Jaber Aviation";
                aljaber.Address = "37 Bateen Airport";
                aljaber.Address2 = "";
                aljaber.City = "Abou Dhabi";
                aljaber.Phone = "+971.4.555.1212";
                aljaber.Email = "sales@aljaber.aero";
                aljaber.Contact = "Dr, Mark";
                aljaber.Fax = "+971.4.555.1313";
                aljaber.CurrentInvoiceCounter = 0;
                aljaber.Country = uae;
                context.Traders.Add(aljaber);

                CreditCard aljaberCc = new CreditCard();
                aljaberCc.HolderName = "Al Jaber Credit Card";
                aljaberCc.CardNumber = "4444555566667777";
                aljaberCc.ExpDate = "12/20";
                aljaberCc.SecurityCode = "920";
                aljaberCc.Trader = aljaber;
                context.CreditCards.Add(aljaberCc);

                Trader wings = new Trader();
                wings.Account = "O899876";
                wings.Name = "Gulf Wings";
                wings.Address = "37 DFZA Road";
                wings.Address2 = "";
                wings.City = "Sharjah";
                wings.Phone = "+971.4.555.1212";
                wings.Email = "sales@gulfwings.aero";
                wings.Contact = "Mr. Osman";
                wings.Fax = "+971.4.555.1313";
                wings.CurrentInvoiceCounter = 0;
                wings.Country = uae;
                context.Traders.Add(wings);

                CreditCard wingsCc = new CreditCard();
                wingsCc.HolderName = "Gulf Wings Credit Card";
                wingsCc.CardNumber = "4444555566667777";
                wingsCc.ExpDate = "12/20";
                wingsCc.SecurityCode = "920";
                wingsCc.Trader = wings;
                context.CreditCards.Add(wingsCc);

                Trader safar = new Trader();
                safar.Account = "V87878";
                safar.Name = "Safar";
                safar.Address = "4 DFZA East Wing";
                safar.Address2 = "";
                safar.City = "Beirut";
                safar.Phone = "+961.3.555.121";
                safar.Email = "sales@safar.aero";
                safar.Contact = "Mr. Ahmad";
                safar.Fax = "+961.3.555.131";
                safar.CurrentInvoiceCounter = 0;
                safar.Country = leb;
                context.Traders.Add(safar);

                CreditCard safarCc = new CreditCard();
                safarCc.HolderName = "Safar Credit Card";
                safarCc.CardNumber = "4444555566667777";
                safarCc.ExpDate = "12/20";
                safarCc.SecurityCode = "920";
                safarCc.Trader = safar;
                context.CreditCards.Add(safarCc);

                Trader royo = new Trader();
                royo.Account = "O899889";
                royo.Name = "Royo";
                royo.Address = "37 DFZA Road";
                royo.Address2 = "";
                royo.City = "Beirut";
                royo.Phone = "+961.3.555.121";
                royo.Email = "sales@royo.aero";
                royo.Contact = "Mr. Wakeel";
                royo.Fax = "+961.4.555.131";
                royo.CurrentInvoiceCounter = 0;
                royo.Country = leb;
                context.Traders.Add(royo);

                CreditCard royoCc = new CreditCard();
                royoCc.HolderName = "Royo Credit Card";
                royoCc.CardNumber = "4444555566667777";
                royoCc.ExpDate = "12/20";
                royoCc.SecurityCode = "920";
                royoCc.Trader = royo;
                context.CreditCards.Add(royoCc);

                Trader jetem = new Trader();
                jetem.Account = "V87878";
                jetem.Name = "Jetem";
                jetem.Address = "4 DFZA East Wing";
                jetem.Address2 = "";
                jetem.City = "Jeddah";
                jetem.Phone = "+966.50.555.1211";
                jetem.Email = "sales@Jetem.aero";
                jetem.Contact = "Mr. Ahmad";
                jetem.Fax = "+966.50.555.1311";
                jetem.CurrentInvoiceCounter = 0;
                jetem.Country = sar;
                context.Traders.Add(jetem);

                CreditCard jetemCc = new CreditCard();
                jetemCc.HolderName = "Jetem Credit Card";
                jetemCc.CardNumber = "4444555566667777";
                jetemCc.ExpDate = "12/20";
                jetemCc.SecurityCode = "920";
                jetemCc.Trader = jetem;
                context.CreditCards.Add(jetemCc);

                Trader soyo = new Trader();
                soyo.Account = "O89899889";
                soyo.Name = "Soyo";
                soyo.Address = "37 DFZA Road";
                soyo.Address2 = "";
                soyo.City = "Jeddah";
                soyo.Phone = "+961.50.555.1211";
                soyo.Email = "sales@soyo.aero";
                soyo.Contact = "Mr. Wakeel";
                soyo.Fax = "+966.50.555.1311";
                soyo.CurrentInvoiceCounter = 0;
                soyo.Country = sar;
                context.Traders.Add(soyo);

                CreditCard soyoCc = new CreditCard();
                soyoCc.HolderName = "Soyo Credit Card";
                soyoCc.CardNumber = "4444555566667777";
                soyoCc.ExpDate = "12/20";
                soyoCc.SecurityCode = "920";
                soyoCc.Trader = soyo;
                context.CreditCards.Add(soyoCc);

                Trader kellem = new Trader();
                kellem.Account = "V87878";
                kellem.Name = "Kellem";
                kellem.Address = "4 DFZA East Wing";
                kellem.Address2 = "";
                kellem.City = "Cairo";
                kellem.Phone = "+20.50.555.1211";
                kellem.Email = "sales@kellem.aero";
                kellem.Contact = "Mr. Ahmad";
                kellem.Fax = "+20.50.555.1311";
                kellem.CurrentInvoiceCounter = 0;
                kellem.Country = egy;
                context.Traders.Add(kellem);

                CreditCard kellemCc = new CreditCard();
                kellemCc.HolderName = "Kellem Credit Card";
                kellemCc.CardNumber = "4444555566667777";
                kellemCc.ExpDate = "12/20";
                kellemCc.SecurityCode = "920";
                kellemCc.Trader = kellem;
                context.CreditCards.Add(kellemCc);

                Trader masri = new Trader();
                masri.Account = "O89898565";
                masri.Name = "Masri";
                masri.Address = "37 DFZA Road";
                masri.Address2 = "";
                masri.City = "Cairo";
                masri.Phone = "+20.50.555.1211";
                masri.Email = "sales@masri.aero";
                masri.Contact = "Mr. Wakeel";
                masri.Fax = "+20.50.555.1311";
                masri.CurrentInvoiceCounter = 0;
                masri.Country = egy;
                context.Traders.Add(masri);

                CreditCard masriCc = new CreditCard();
                masriCc.HolderName = "Masri Credit Card";
                masriCc.CardNumber = "4444555566667777";
                masriCc.ExpDate = "12/20";
                masriCc.SecurityCode = "920";
                masriCc.Trader = masri;
                context.CreditCards.Add(masriCc);

                Contract jetexAljaber = new Contract();
                jetexAljaber.CreateDate = DateTime.Now.AddDays(-365);
                jetexAljaber.Name = "Jetex-Aljaber";
                jetexAljaber.Description = "Jetex-Aljaber More Details";
                jetexAljaber.StartDate = jetexAljaber.CreateDate;
                jetexAljaber.EndDate = DateTime.Now.AddDays(30);
                jetexAljaber.TraderOneApprovalDate = jetexAljaber.CreateDate;
                jetexAljaber.TraderTwoApprovalDate = jetexAljaber.CreateDate;
                jetexAljaber.IsTraderOneApproved = true;
                jetexAljaber.IsTraderTwoApproved = true;
                jetexAljaber.TraderOne = jetex;
                jetexAljaber.TraderTwo = aljaber;
                jetexAljaber.Instance = instance;
                jetexAljaber.BillingCurrency = uaeCurrency;
                jetexAljaber.TimeZone = uaeTimeZone;
                context.Contracts.Add(jetexAljaber);

                Contract jetexAljaber1 = new Contract();
                jetexAljaber1.CreateDate = DateTime.Now.AddDays(-30);
                jetexAljaber1.Name = "Jetex-Aljaber";
                jetexAljaber1.Description = "Jetex-Aljaber - Sample of Semi Approved by Trader One only";
                jetexAljaber1.StartDate = jetexAljaber.CreateDate;
                jetexAljaber1.EndDate = DateTime.Now.AddDays(365);
                jetexAljaber1.TraderOneApprovalDate = jetexAljaber.CreateDate;
                jetexAljaber1.TraderTwoApprovalDate = jetexAljaber.CreateDate;
                jetexAljaber1.IsTraderOneApproved = true;
                jetexAljaber1.IsTraderTwoApproved = false;
                jetexAljaber1.TraderOne = jetex;
                jetexAljaber1.TraderTwo = aljaber;
                jetexAljaber1.Instance = instance;
                jetexAljaber1.BillingCurrency = uaeCurrency;
                jetexAljaber1.TimeZone = uaeTimeZone;
                context.Contracts.Add(jetexAljaber1);

                Contract jetexAljaber2 = new Contract();
                jetexAljaber2.CreateDate = DateTime.Now.AddDays(-45);
                jetexAljaber2.Name = "Jetex-Aljaber";
                jetexAljaber2.Description = "Jetex-Aljaber - Sample of Semi Approved by Trader Two only";
                jetexAljaber2.StartDate = jetexAljaber.CreateDate;
                jetexAljaber2.EndDate = DateTime.Now.AddDays(365);
                jetexAljaber2.TraderOneApprovalDate = jetexAljaber.CreateDate;
                jetexAljaber2.TraderTwoApprovalDate = jetexAljaber.CreateDate;
                jetexAljaber2.IsTraderOneApproved = false;
                jetexAljaber2.IsTraderTwoApproved = true;
                jetexAljaber2.TraderOne = jetex;
                jetexAljaber2.TraderTwo = aljaber;
                jetexAljaber2.Instance = instance;
                jetexAljaber2.BillingCurrency = uaeCurrency;
                jetexAljaber2.TimeZone = uaeTimeZone;
                context.Contracts.Add(jetexAljaber2);

                Contract hadeedWings = new Contract();
                hadeedWings.CreateDate = DateTime.Now.AddDays(-365);
                hadeedWings.Name = "Hadeed-Wings";
                hadeedWings.Description = "Hadeed-Wings More Details";
                hadeedWings.StartDate = hadeedWings.CreateDate;
                hadeedWings.EndDate = DateTime.Now.AddDays(30);
                hadeedWings.TraderOneApprovalDate = hadeedWings.CreateDate;
                hadeedWings.TraderTwoApprovalDate = hadeedWings.CreateDate;
                hadeedWings.IsTraderOneApproved = true;
                hadeedWings.IsTraderTwoApproved = true;
                hadeedWings.TraderOne = hadeed;
                hadeedWings.TraderTwo = wings;
                hadeedWings.Instance = instance;
                hadeedWings.BillingCurrency = uaeCurrency;
                hadeedWings.TimeZone = uaeTimeZone;
                context.Contracts.Add(hadeedWings);

                Contract safarRoyo = new Contract();
                safarRoyo.CreateDate = DateTime.Now.AddDays(-365);
                safarRoyo.Name = "Safar-Royo";
                safarRoyo.Description = "Safar-Royo More Details";
                safarRoyo.StartDate = safarRoyo.CreateDate;
                safarRoyo.EndDate = DateTime.Now.AddDays(30);
                safarRoyo.TraderOneApprovalDate = safarRoyo.CreateDate;
                safarRoyo.TraderTwoApprovalDate = safarRoyo.CreateDate;
                safarRoyo.IsTraderOneApproved = true;
                safarRoyo.IsTraderTwoApproved = true;
                safarRoyo.TraderOne = safar;
                safarRoyo.TraderTwo = royo;
                safarRoyo.Instance = instance;
                safarRoyo.BillingCurrency = usdCurrency;
                safarRoyo.TimeZone = egyTimeZone;
                context.Contracts.Add(safarRoyo);

                Contract jetemSoyo = new Contract();
                jetemSoyo.CreateDate = DateTime.Now.AddDays(-365);
                jetemSoyo.Name = "Jetem-Soyo";
                jetemSoyo.Description = "Jetem-Soyo More Details";
                jetemSoyo.StartDate = jetemSoyo.CreateDate;
                jetemSoyo.EndDate = DateTime.Now.AddDays(30);
                jetemSoyo.TraderOneApprovalDate = jetemSoyo.CreateDate;
                jetemSoyo.TraderTwoApprovalDate = jetemSoyo.CreateDate;
                jetemSoyo.IsTraderOneApproved = true;
                jetemSoyo.IsTraderTwoApproved = true;
                jetemSoyo.TraderOne = jetem;
                jetemSoyo.TraderTwo = soyo;
                jetemSoyo.Instance = instance;
                jetemSoyo.BillingCurrency = saCurrency;
                jetemSoyo.TimeZone = uaeTimeZone;
                context.Contracts.Add(jetemSoyo);

                Contract kellemMasri= new Contract();
                kellemMasri.CreateDate = DateTime.Now.AddDays(-365);
                kellemMasri.Name = "Kellem-Masri";
                kellemMasri.Description = "Kellem-Masri More Details";
                kellemMasri.StartDate = kellemMasri.CreateDate;
                kellemMasri.EndDate = DateTime.Now.AddDays(30);
                kellemMasri.TraderOneApprovalDate = kellemMasri.CreateDate;
                kellemMasri.TraderTwoApprovalDate = kellemMasri.CreateDate;
                kellemMasri.IsTraderOneApproved = true;
                kellemMasri.IsTraderTwoApproved = true;
                kellemMasri.TraderOne = kellem;
                kellemMasri.TraderTwo = masri;
                kellemMasri.Instance = instance;
                kellemMasri.BillingCurrency = egCurrency;
                kellemMasri.TimeZone = uaeTimeZone;
                context.Contracts.Add(kellemMasri);

                base.Seed(context);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var EVE in ex.EntityValidationErrors)
                {
                    Console.WriteLine(EVE);
                }
            }
        }
    }
}
