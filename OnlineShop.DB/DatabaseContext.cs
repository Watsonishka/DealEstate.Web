using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Models.Bids;
using OnlineShop.DB.Models.Comparisons;
using OnlineShop.DB.Models.Favorites;
using OnlineShop.DB.Models.Orders;
using OnlineShop.DB.Models.Products;
using OnlineShop.DB.Models.Users;

namespace OnlineShop.DB
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Apartment> Apartments { get; set; } = null!;
        public DbSet<House> Houses { get; set; } = null!;
        public DbSet<Bid> Bids { get; set; } = null!;
        public DbSet<BidItem> BidItems { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<CatalogService> CatalogServices { get; set; } = null!;
        public DbSet<Favorite> Favorites { get; set; } = null!;
        public DbSet<Comparison> Comparisons { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<DeliveryUser> DeliveryUsers { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasDiscriminator<string>("ProductType")
                .HasValue<Apartment>("Apartment")
                .HasValue<House>("House");

            modelBuilder.Entity<CatalogService>().HasData(new List<CatalogService>
            {
                new CatalogService { ID = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Срочный показ", Price = 500m},
                new CatalogService { ID = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Трансфер клиента", Price = 2000m},
                new CatalogService { ID = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Консультация дизайнера", Price = 30000m},
                new CatalogService { ID = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Юридическое сопровождение", Price = 50000m}
            });

            modelBuilder.Entity<Apartment>().HasData(new List<Apartment>
            {
                new Apartment
                {
                    ID = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    Name = "2-комнатная квартира, ЖК «Новое Поколение»",
                    Cost = 35500000m,
                    Area = 45.5,
                    Description = "Светлая квартира с видом на парк, рядом метро",
                    City = "Воронеж",
                    Developer = "Start Life",
                    TotalFloors = 9,
                    Category = Category.Apartments,
                    PreviewImagePath = "/img/Apartment_white.png",
                    Class = ApartmentClass.Comfort,
                    Floor = 5,
                    HasBalcony = true,
                    CeilingHeight = 2.7
                },
                new Apartment
                {
                    ID = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    Name = "3-комнатная квартира, ЖК «STONE Art»",
                    Cost = 89000000m,
                    Area = 78.5,
                    Description = "Просторная квартира, свежая отделка, закрытый двор",
                    City = "Москва",
                    Developer = "Grand City Development",
                    TotalFloors = 16,
                    Category = Category.Apartments,
                    PreviewImagePath = "/img/Apartment_white.png",
                    Class = ApartmentClass.Business,
                    Floor = 7,
                    HasBalcony = true,
                    CeilingHeight = 2.8
                },
                new Apartment
                {
                    ID = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Name = "1-комнатная квартира, ЖК «Зелёные Аллеи»",
                    Cost = 28500000m,
                    Area = 32.5,
                    Description = "Уютная студия с современной отделкой, закрытая территория, видеонаблюдение",
                    City = "Санкт-Петербург",
                    Developer = "Лидер Групп",
                    TotalFloors = 14,
                    Category = Category.Apartments,
                    PreviewImagePath = "/img/Apartment_white.png",
                    Class = ApartmentClass.Comfort,
                    Floor = 8,
                    HasBalcony = true,
                    CeilingHeight = 2.75
                }
            });

            modelBuilder.Entity<House>().HasData(new List<House>
            {
                new House
                {
                    ID = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                    Name = "Коттедж КП «Лесное озеро»",
                    Cost = 12500000m,
                    Area = 120.0,
                    Description = "Красивый вид, тихое место",
                    City = "Московская область",
                    Developer = "TerraDom",
                    TotalFloors = 2,
                    Category = Category.Houses,
                    PreviewImagePath = "/img/House_white.png",
                    LandArea = 600.0,
                    HasGarage = true
                },
                new House
                {
                    ID = Guid.Parse("EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"),
                    Name = "Таунхаус, КП «Английский квартал»",
                    Cost = 18500000m,
                    Area = 145.0,
                    Description = "Современный таунхаус с террасой и участком, въезд готов",
                    City = "Краснодар",
                    Developer = "ЮгСтройИнвест",
                    TotalFloors = 3,
                    Category = Category.Houses,
                    PreviewImagePath = "/img/House_white.png",
                    LandArea = 350.0,
                    HasGarage = true
                }
            });
        }
    }
}

