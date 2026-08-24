using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using OnlineShop.DB.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class ProductController : Controller
    {
        private readonly IProductsStorage _productsStorage;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductsStorage productsStorage, IWebHostEnvironment webHostEnvironment)
        {
            _productsStorage = productsStorage;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _productsStorage.GetAll();

            return View(products.ToProductViewModels());
        }

        public IActionResult Add()
        {
            return View(new ProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel productViewModel)
        {
            if (productViewModel.UploadedImage != null && productViewModel.UploadedImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productViewModel.UploadedImage.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productViewModel.UploadedImage.CopyToAsync(stream);
                }

                productViewModel.PreviewImagePath = "/images/products/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _productsStorage.Add(productViewModel.ToProductDb());

                return RedirectToAction(nameof(Index));
            }

            return View("Add", productViewModel);
        }

        public IActionResult Update(Guid productID)
        {
            var product = _productsStorage.TryGetByID(productID);
            var productViewModel = product.ToProductViewModel();

            return View(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", productViewModel);
            }

            if (productViewModel.UploadedImage != null && productViewModel.UploadedImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productViewModel.UploadedImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productViewModel.UploadedImage.CopyToAsync(stream);
                }

                productViewModel.PreviewImagePath = "/images/products/" + fileName;
            }
            else
            {
                var existingProduct = _productsStorage.TryGetByID(productViewModel.ID);
                if (existingProduct != null)
                {
                    productViewModel.PreviewImagePath = existingProduct.PreviewImagePath;
                }
            }

            _productsStorage.Update(productViewModel.ToProductDb());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(Guid productID)
        {
            _productsStorage.Delete(productID);
            return RedirectToAction(nameof(Index));
        }
    }
}
