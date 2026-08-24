using Microsoft.AspNetCore.Identity;
using OnlineShop.DB.Models;
using OnlineShop.DB.Models.Bids;
using OnlineShop.DB.Models.Comparisons;
using OnlineShop.DB.Models.Favorites;
using OnlineShop.DB.Models.Orders;
using OnlineShop.DB.Models.Products;
using OnlineShop.DB.Models.Users;
using OnlineShopWebApp.Areas.Admin;
using OnlineShopWebApp.Models;
using System.Data;

namespace OnlineShopWebApp.Helpers
{
    public static class Mapper
    {
        #region Product

        public static List<ProductViewModel> ToProductViewModels(this List<Product> productsDb)
        {
            var productsViewModel = new List<ProductViewModel>();

            foreach (var productDb in productsDb)
            {
                productsViewModel.Add(productDb.ToProductViewModel());
            }

            return productsViewModel;
        }

        public static ProductViewModel ToProductViewModel(this Product productDb)
        {
            var viewModel = new ProductViewModel
            {
                ID = productDb.ID,
                Name = productDb.Name,
                Cost = productDb.Cost,
                Area = productDb.Area,
                Description = productDb.Description,
                TotalFloors = productDb.TotalFloors,
                Category = productDb.Category,
                Developer = productDb.Developer,
                City = productDb.City,
                PreviewImagePath = productDb.PreviewImagePath,
                Class = productDb.Class
            };

            if (productDb is Apartment apartment)
            {
                viewModel.Floor = apartment.Floor;
                viewModel.HasBalcony = apartment.HasBalcony;
                viewModel.CeilingHeight = apartment.CeilingHeight;
            }

            if (productDb is House house)
            {
                viewModel.LandArea = house.LandArea;
                viewModel.HasGarage = house.HasGarage;
            }

            return viewModel;
        }

        public static Product ToProductDb(this ProductViewModel viewModel)
        {
            if (viewModel.Category == Category.Apartments)
            {
                return new Apartment
                {
                    ID = viewModel.ID,
                    Name = viewModel.Name,
                    Cost = viewModel.Cost,
                    Area = viewModel.Area,
                    Description = viewModel.Description,
                    TotalFloors = viewModel.TotalFloors,
                    Category = Category.Apartments,
                    Developer = viewModel.Developer,
                    City = viewModel.City,
                    PreviewImagePath = viewModel.PreviewImagePath,
                    Class = viewModel.Class,
                    Floor = viewModel.Floor,
                    HasBalcony = viewModel.HasBalcony,
                    CeilingHeight = viewModel.CeilingHeight
                };
            }

            if (viewModel.Category == Category.Houses)
            {
                return new House
                {
                    ID = viewModel.ID,
                    Name = viewModel.Name,
                    Cost = viewModel.Cost,
                    Area = viewModel.Area,
                    Description = viewModel.Description,
                    TotalFloors = viewModel.TotalFloors,
                    Category = Category.Houses,
                    Developer = viewModel.Developer,
                    City = viewModel.City,
                    PreviewImagePath = viewModel.PreviewImagePath,
                    LandArea = viewModel.LandArea,
                    HasGarage = viewModel.HasGarage
                };
            }

            return null;
        }

        #endregion

        #region Bid

        public static List<BidViewModel> ToBidsViewModels(this List<Bid> bidsDb)
        {
            var bidsViewModel = new List<BidViewModel>();

            foreach (var bidDb in bidsDb)
            {
                bidsViewModel.Add(bidDb.ToBidViewModel());
            }

            return bidsViewModel;
        }

        public static BidViewModel ToBidViewModel(this Bid? bidDb)
        {
            if (bidDb == null)
            {
                return null;
            }

            return new BidViewModel
            {
                ID = bidDb.ID,
                Items = bidDb.Items?.Select(item => new BidItemViewModel
                {
                    ID = item.ID,
                    Product = item.Product,
                    Services = item.Services?.Select(services => new ServiceViewModel
                    {
                        ID = services.ID,
                        IsRemoved = services.IsRemoved,
                        Price = services.Price,
                        Name = services.Name,
                    }).ToList()
                }).ToList()
            };
        }

        public static Bid ToBidDb(this BidViewModel viewModel)
        {
            return new Bid
            {
                ID = viewModel.ID,
                Items = viewModel.Items?.Select(item => new BidItem
                {
                    ID = item.ID,
                    Product = item.Product,
                    Services = item.Services?.Select(services => new Service
                    {
                        ID = services.ID,
                        IsRemoved = services.IsRemoved,
                        Price = services.Price,
                        Name = services.Name
                    }).ToList()
                }).ToList()
            };
        }

        #endregion

        #region Favorite

        public static List<FavoriteViewModel> ToFavoriteViewModels(this List<Favorite> favoritesDb)
        {
            var favoritesViewModel = new List<FavoriteViewModel>();

            foreach (var favoriteDb in favoritesDb)
            {
                favoritesViewModel.Add(favoriteDb.ToFavoriteViewModel());
            }

            return favoritesViewModel;
        }

        public static FavoriteViewModel ToFavoriteViewModel(this Favorite? favoriteDb)
        {
            if (favoriteDb == null)
            {
                return null;
            }

            return new FavoriteViewModel
            {
                ID = favoriteDb.ID,
                UserId = favoriteDb.UserID,
                Products = favoriteDb.Products
            };
        }

        public static Favorite ToFavoriteDb(this FavoriteViewModel viewModel)
        {
            return new Favorite
            {
                ID = viewModel.ID,
                UserID = viewModel.UserId,
                Products = viewModel.Products
            };
        }

        #endregion

        #region Comparsion

        public static List<ComparisonViewModel> ToComparsionViewModels(this List<Comparison> comparsionsDb)
        {
            var comparsionViewModel = new List<ComparisonViewModel>();

            foreach (var comparsionDb in comparsionsDb)
            {
                comparsionViewModel.Add(comparsionDb.ToComparsionViewModel());
            }

            return comparsionViewModel;
        }

        public static ComparisonViewModel ToComparsionViewModel(this Comparison? comparsionDb)
        {
            if (comparsionDb == null)
            {
                return null;
            }

            return new ComparisonViewModel
            {
                ID = comparsionDb.ID,
                UserID = comparsionDb.UserID,
                Products = comparsionDb.Products
            };
        }

        public static Comparison ToComparisonDb(this ComparisonViewModel viewModel)
        {
            return new Comparison
            {
                ID = viewModel.ID,
                UserID = viewModel.UserID,
                Products = viewModel.Products
            };
        }
        #endregion

        #region Order

        public static List<OrderViewModel> ToOrdersViewModels(this List<Order> ordersDb)
        {
            var ordersViewModel = new List<OrderViewModel>();

            foreach (var orderDb in ordersDb)
            {
                ordersViewModel.Add(orderDb.ToOrderViewModel());
            }

            return ordersViewModel;
        }

        public static OrderViewModel ToOrderViewModel(this Order orderDb)
        {
            return new OrderViewModel
            {
                ID = orderDb.ID,
                UserID = orderDb.UserID,
                CreatedAt = orderDb.CreatedAt,
                Status = orderDb.Status,
                DeliveryUser = orderDb.DeliveryUser?.ToDeliveryUserViewModel() ?? new DeliveryUserViewModel(),

                Items = orderDb.Items?.Select(item => new OrderItemViewModel
                {
                    ID = item.ID,
                    ProductName = item.ProductName,
                    ProductCity = item.ProductCity,
                    ProductArea = item.ProductArea,
                    CurrentPrice = item.CurrentPrice,

                    Services = item.Services?.Select(service => new ServiceViewModel
                    {
                        Name = service.Name,
                        Price = service.Price,
                        IsRemoved = service.IsRemoved,

                    }).ToList()
                }).ToList()
            };
        }

        #endregion

        #region DeliveryUser

        public static List<DeliveryUserViewModel> ToDeliveryUserViewModels(this List<DeliveryUser> deliveryUsersDb)
        {
            var deliveryUserViewModel = new List<DeliveryUserViewModel>();

            foreach (var deliveryUserDb in deliveryUsersDb)
            {
                deliveryUserViewModel.Add(deliveryUserDb.ToDeliveryUserViewModel());
            }

            return deliveryUserViewModel;
        }

        public static DeliveryUserViewModel ToDeliveryUserViewModel(this DeliveryUser deliveryUser)
        {
            return new DeliveryUserViewModel
            {
                LastName = deliveryUser.LastName,
                FirstName = deliveryUser.FirstName,
                Patronymic = deliveryUser.Patronymic,
                PhoneNumber = deliveryUser.PhoneNumber,
                Comment = deliveryUser.Comment
            };
        }

        public static DeliveryUser ToDeliveryUserDb(this DeliveryUserViewModel viewModel)
        {
            return new DeliveryUser
            {
                LastName = viewModel.LastName,
                FirstName = viewModel.FirstName,
                Patronymic = viewModel.Patronymic,
                PhoneNumber = viewModel.PhoneNumber,
                Comment = viewModel.Comment
            };
        }

        #endregion

        #region User

        public static List<UserViewModel> ToUserViewModels(this List<User> usersDb)
        {
            var usersViewModel = new List<UserViewModel>();

            foreach (var userDb in usersDb)
            {
                usersViewModel.Add(userDb.ToUserViewModel());
            }

            return usersViewModel;
        }

        public static UserViewModel ToUserViewModel(this User userDb, string role = "")
        {
            return new UserViewModel
            {
                ID = userDb.Id,
                FirstName = userDb.FirstName,
                Login = userDb.Email,
                LastName = userDb.LastName,
                Patronymic = userDb.Patronymic,
                PhoneNumber = userDb.PhoneNumber,
                Role = role,
                IsCancelled = userDb.IsCancelled
            };
        }

        #endregion

        #region Role

        public static List<RoleViewModel> ToRoleViewModels(this List<IdentityRole> rolesDb)
        {
            var rolesViewModel = new List<RoleViewModel>();

            foreach (var roleDb in rolesDb)
            {
                rolesViewModel.Add(roleDb.ToRoleViewModel());
            }

            return rolesViewModel;
        }

        public static RoleViewModel ToRoleViewModel(this IdentityRole roleDb)
        {
            return new RoleViewModel
            {
                ID = roleDb.Id,
                Name = roleDb.Name,
                IsCustom = roleDb.Name != Constants.AdminRoleName && roleDb.Name != Constants.UserRoleName
            };
        }

        public static IdentityRole ToRoleDb(this RoleViewModel viewModel)
        {
            return new IdentityRole
            {
                Name = viewModel.Name
            };
        }

        #endregion
    }
}