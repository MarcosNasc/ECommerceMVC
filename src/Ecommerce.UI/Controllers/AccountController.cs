using Ecommerce.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Resgister

        [HttpGet]
        public IActionResult Register()
        {
            var viewData = new RegisterViewModel()
            {
                ReturnUrl = Url.Content("/Home")
            };
            return View(viewData);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ModelState.Remove("ExternalLogins");
            if (ModelState.IsValid)
            {
                // Copia os dados do RegisterViewModel para o IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                // Armazena os dados do usuário na tabela AspNetUsers
                var result = await _userManager.CreateAsync(user, model.Password);

                // Se o usuário for criado com sucesso, faz login do usuário
                // usando o serviço SignInManager e redireciona para o Método Action Index da HomeController
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Se houver erros então inclui no ModelState
                // que será exibido pela tag helper Summary na validação

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View(model);
        }

        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewData = new LoginViewModel
            {
                ReturnUrl = Url.Content("/Home")
            };
            return View(viewData);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ModelState.Remove("ExternalLogins");
            ModelState.Remove("ReturnUrl");

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }


                ModelState.AddModelError(string.Empty, "Login Inválido");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
