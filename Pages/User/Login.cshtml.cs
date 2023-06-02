using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using BlazorServerSignalApp.IService;
using BlazorServerSignalApp.Data;
using BlazorServerSignalApp.Service;

namespace BlazorServerSignalApp.Pages.User
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGetAsync(string returnUrl = null)
        {
            string provider = "Google";
            // Request a redirect to the external login provider.
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("./Login",
                pageHandler: "Callback",
                values: new { returnUrl }),
            };
            return new ChallengeResult(provider, authenticationProperties);
        }
        public async Task<IActionResult> OnGetCallbackAsync(
            string returnUrl = null, string remoteError = null)
        {
            // Get the information about the user from the external login provider
            var GoogleUser = this.User.Identities.FirstOrDefault();
            if (GoogleUser.IsAuthenticated)
            {
                var userEmail = GoogleUser.FindFirst(ClaimTypes.Email).Value;
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = this.Request.Host.Value
                };
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(GoogleUser),
                authProperties);
                _userService.ExternalLoginProcess(userEmail);
            }
            return LocalRedirect("/");
        }
    }
}