using Microsoft.AspNetCore.Mvc;
using Ecommerce.UI.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Ecommerce.BLL.Entities;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Ecommerce.BLL.Interfaces;
using Ecommerce.UI.Authorization;

namespace Ecommerce.UI.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ProductsController(UserManager<IdentityUser> userManager
                                  ,IProductService productService
                                  ,IProductRepository repository
                                  ,ISupplierRepository supplierRepository
                                  ,IMapper mapper
                                  ,ILogger<ProductsController> logger
                                  ,INotificator notificator
                                 )
        : base(userManager, mapper, logger,notificator)
        {
            _productService = productService;
            _productRepository = repository;
            _supplierRepository = supplierRepository;
        }

        [Route("product-list")]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetProductsSuppliers();
            var ProductsViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            _logger.LogInformation($"{_user} {Resources.ProductsLogs.AcessPageIndex}");

            if (TempData.TryGetValue("ItemDeleted", out var itemDeleted))
            {
                ViewBag.ItemDeleted = itemDeleted;
                TempData.Remove("ItemDeleted");
            }
            else
            {
                ViewBag.ItemDeleted = false;
            }

            return View(ProductsViewModel);
        }

        [ClaimsAuthorize("Product", "R")]
        [Route("product-details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);
            if (productViewModel == null) return NotFound();
            _logger.LogInformation($"{_user} {Resources.ProductsLogs.AcessPageDetails}");
            return View(productViewModel);
        }

        [ClaimsAuthorize("Product","C")]
        [Route("new-product")]
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularSuppliers(new ProductViewModel());
            _logger.LogInformation($"{_user} {Resources.ProductsLogs.AcessPageCreate}");
            return View(produtoViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Product", "C")]
        [Route("new-product")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                productViewModel = await PopularSuppliers(new ProductViewModel());

                return View(productViewModel);
            }

            try
            {
                var imgPrefix = Guid.NewGuid() + "_";

                if (!await FileUpload(productViewModel.ImageUpload, imgPrefix))
                {
                    return View(productViewModel);
                }

                productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;
                var product = _mapper.Map<Product>(productViewModel);

                await _productService.Add(product);

                if(!ValidOperation()) return View(productViewModel);

                _logger.LogInformation($"{_user} {Resources.ProductsLogs.ProductCreated} {product.Id}");
            }
            catch (Exception ex)
            {
                throw new Exception(Resources.ProductExceptions.CreateProductException);
            }

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "U")]
        [Route("edit-product/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            if (productViewModel == null) return NotFound();
            _logger.LogInformation($"{_user} {Resources.ProductsLogs.AcessPageEdit}");
            return View(productViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Product", "U")]
        [Route("edit-product/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            var currentProduct = await GetProduct(id);
            productViewModel.Supplier = currentProduct.Supplier;
            productViewModel.Image = currentProduct.Image;

            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";

                if (!await FileUpload(productViewModel.ImageUpload, imgPrefix))
                {
                    return View(productViewModel);
                }

                currentProduct.Image = imgPrefix + productViewModel.ImageUpload.FileName;
            }

            currentProduct.Name = productViewModel.Name;
            currentProduct.Description = productViewModel.Description;
            currentProduct.Value = productViewModel.Value;
            currentProduct.IsActive = productViewModel.IsActive;

            var product = _mapper.Map<Product>(currentProduct);

            try
            {
                await _productService.Update(product);

                if (!ValidOperation()) return View(productViewModel);

                _logger.LogInformation($"{_user} {Resources.ProductsLogs.ProductEdited} {product.Id}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction(nameof(Index));

        }

        [ClaimsAuthorize("Product", "D")]
        [Route("delete-product/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) { return NotFound(); }
            _logger.LogInformation($"{_user} {Resources.ProductsLogs.AcessPageRemove}");
            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize("Product", "D")]
        [Route("delete-product/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            try
            {
                await _productService.Remove(id);
                if (!ValidOperation()) return View(productViewModel);

                TempData["Sucesso"] = "Produto excluido com sucesso";
                TempData["ItemDeleted"] = true;
                _logger.LogInformation($"{_user} {Resources.ProductsLogs.ProductRemoved} {productViewModel.Id}");
            }
            catch (Exception ex)
            {
                throw new Exception(Resources.ProductExceptions.RemoveProductException);
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product = await _productRepository.GetProductSupplier(id);
            var productViewModel = _mapper.Map<ProductViewModel>(product);
            productViewModel = await PopularSuppliers(productViewModel);
            return productViewModel;
        }

        private async Task<ProductViewModel> PopularSuppliers(ProductViewModel productViewModel)
        {
            var suppliers = await _supplierRepository.GetAll();
            productViewModel.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
            return productViewModel;
        }

        private async Task<bool> FileUpload(IFormFile file, string imgPrefix)
        {
            if (file.Length <= 0)
            {
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }


    }
}
