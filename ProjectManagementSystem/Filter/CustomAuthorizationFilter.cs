using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Filter
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly List<string> _policies;

        public CustomAuthorizationFilter(IAuthorizationService authorizationService, List<string> policies)
        {
            _authorizationService = authorizationService;
            _policies = policies;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            foreach (var policy in _policies)
            {
                var authorized = await _authorizationService.AuthorizeAsync(context.HttpContext.User, policy);
                if (!authorized.Succeeded)
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                }
            }
        }
    }


}
