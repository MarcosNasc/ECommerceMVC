using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ecommerce.BLL.Entities;
using Ecommerce.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.BLL.Interfaces.Repositories;
using Ecommerce.BLL.Interfaces.Services;
using Ecommerce.BLL.Interfaces;
using Ecommerce.UI.Authorization;

namespace Ecommerce.UI.Controllers
{
    [Authorize]
    public class SuppliersController : BaseController
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(UserManager<IdentityUser> userManager
                                   ,ISupplierService supplierService  
                                   ,ISupplierRepository repository
                                   ,IMapper mapper
                                   ,ILogger<SuppliersController> logger
                                   ,INotificator notificator
                                  )
        :base(userManager,mapper,logger, notificator)
        {
            _supplierService = supplierService;
            _supplierRepository = repository;
        }

        [Route("suppliers-list")]
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierRepository.GetAll();
            var listSuppliersViewModel = _mapper.Map<List<SupplierViewModel>>(suppliers);
            _logger.LogInformation($"{_user} {Resources.SuppliersLogs.AcessPageIndex}");
            return View(listSuppliersViewModel) ;
        }


        [ClaimsAuthorize("Supplier", "R")]
        [Route("supplier-details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var supplierViewModel = await GetSupplierAdress(id);
            if (supplierViewModel == null) return NotFound();
            _logger.LogInformation($"{_user} {Resources.SuppliersLogs.AcessPageDetails} {supplierViewModel.Id}");
            return View(supplierViewModel);
        }


        [ClaimsAuthorize("Supplier", "C")]
        [Route("new-supplier")]
        public IActionResult Create()
        {
            _logger.LogInformation($"{_user} {Resources.SuppliersLogs.AcessPageCreate}");
            return View();
        }


        [HttpPost]
        [ClaimsAuthorize("Supplier", "C")]
        [Route("new-supplier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return View(supplierViewModel);
            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            try
            {
                await _supplierService.Add(supplier);
                _logger.LogInformation($"{_user} {Resources.SuppliersLogs.SupplierCreated} {supplier.Id}");
                if (!ValidOperation()) return View(supplierViewModel);
            }
            catch (Exception)
            {
                throw new Exception(Resources.SuppliersExceptions.CreateSupplierException);
            }

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Supplier", "U")]
        [Route("edit-supplier/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplierViewModel = await GetSupplierProductsAdress(id);
            if (supplierViewModel == null) return NotFound();
            _logger.LogInformation($"{_user} {Resources.SuppliersLogs.AcessPageEdit}");
            return View(supplierViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Supplier", "U")]
        [Route("edit-supplier/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(supplierViewModel);
            
            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            try
            {
                await _supplierService.Update(supplier);
                _logger.LogInformation($"{_user} {Resources.SuppliersLogs.SupplierEdited} {supplier.Id}");
                if (!ValidOperation()) return View(supplierViewModel);
            }
            catch (Exception)
            {
                throw new Exception($"{Resources.SuppliersExceptions.EditSupplierException} {supplierViewModel.Id}");
            }

            return View("Details", supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "D")]
        [Route("delete-supplier/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAdress(id);

            if (supplierViewModel == null) return NotFound();
            _logger.LogInformation($"{_user} {Resources.SuppliersLogs.AcessPageRemove}");
            return View(supplierViewModel);
        }

        [HttpPost]
        [ClaimsAuthorize("Supplier", "D")]
        [Route("delete-supplier/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
         {
            var supplierViewModel = await GetSupplierAdress(id);

            if (supplierViewModel == null) return NotFound();

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            try
            {
                await _supplierService.Remove(id);
                _logger.LogInformation($"{_user} {Resources.SuppliersLogs.SupplierRemoved} {supplier.Id}");
                if (!ValidOperation()) return View(supplierViewModel);
            }
            catch(DbUpdateException ex)
            {
                throw;
            }
            catch(Exception)
            {
                throw new Exception($"{Resources.SuppliersExceptions.RemoveSupplierException} {supplierViewModel.Id}");
            }

           return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAddress(Guid id)
        {
            var supplier = await GetSupplierAdress(id);

            if (supplier == null) return NotFound();

            return PartialView("_AddressDetails",supplier);
        }

        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var supplier = await GetSupplierAdress(id);

            if (supplier == null) return NotFound();

            return PartialView("_UpdateAddress", supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(SupplierViewModel supplierViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");
            if (!ModelState.IsValid) return  PartialView("_UpdateAddress", supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _supplierService.UpdateAddress(supplier.Address);

            if (!ValidOperation()) return View(supplierViewModel);

            var url = Url.Action("GetAddress", "Suppliers", new { id = supplierViewModel.Address.SupplierId });

            return Json(new { success = true , url });
        }

        private async Task<SupplierViewModel> GetSupplierAdress(Guid id)
        {
            var supplier = await _supplierRepository.GetSupplierAdress(id);
            var viewModel = _mapper.Map<SupplierViewModel>(supplier);
            return viewModel;
        }

        private async Task<SupplierViewModel> GetSupplierProductsAdress(Guid id)
        {
            var supplier = await _supplierRepository.GetSupplierProductsAddress(id);
            var viewModel = _mapper.Map<SupplierViewModel>(supplier);
            return viewModel;
        }
    }
}
