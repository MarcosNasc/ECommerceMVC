using AutoMapper;
using Ecommerce.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected IdentityUser _user;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly INotificator _notificator;

        protected  BaseController(UserManager<IdentityUser> userManager
                                  ,IMapper mapper
                                  ,ILogger logger
                                  ,INotificator notificator)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _notificator = notificator;
        }

        public bool ValidOperation()
        {
            return !_notificator.HasNotification();
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _user = await GetUserLogged();
            await next();
        }

        protected async Task<IdentityUser> GetUserLogged()
        {
            IdentityUser usuario = new IdentityUser();
            if (User.Identity.IsAuthenticated)
            {
                usuario = await _userManager.GetUserAsync(User);
                _logger.LogInformation($"Usuário autenticado {User.Identity.Name}");
            }
            else
            {
                _logger.LogInformation("Usuário não autenticado 'Visitante'");
                usuario = new IdentityUser
                {
                    UserName = "Visitante"
                };
            }
            return usuario;

        }
    }
}
