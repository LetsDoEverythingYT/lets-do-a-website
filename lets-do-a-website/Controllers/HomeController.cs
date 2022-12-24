using lets_do_a_website.Data;
using lets_do_a_website.Models;
using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using System.Security.Claims;

namespace lets_do_a_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITrackerData TrackerData;

        public HomeController(ITrackerData trackerData)
        {
            TrackerData = trackerData;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View(TrackerData);
        }
        public IActionResult Changelog()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginModel { ReturnUrl = returnUrl});
        }


        public IActionResult LoginWithTwitch(string returnUrl = "/")
        {

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginWithTwitchCallback"),
                Items =
                {
                    {"returnUrl", returnUrl }
                }
            };
            return Challenge(props, TwitchAuthenticationDefaults.AuthenticationScheme);
        }


        public async Task<IActionResult> Login2(LoginModel model)
        {
            var user = "guest";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user),
                new Claim(ClaimTypes.Name, user),
                new Claim("displayname", "Guest"),
                new Claim("profileimageurl", "/img/Guest.png")
                
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            return LocalRedirect(model.ReturnUrl);
        }

        public async Task<IActionResult> LoginWithTwitchCallback()
        {
            
            var result = await HttpContext.AuthenticateAsync(TwitchAuthenticationDefaults.AuthenticationScheme);

            var externalClaims = result.Principal!.Claims.ToList();

            var nameId = externalClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value.ToString();
            var name = externalClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value.ToString();
            var dispName = externalClaims.FirstOrDefault(x => x.Type == "urn:twitch:displayname")!.Value.ToString();
            var dispImg = externalClaims.FirstOrDefault(x => x.Type == "urn:twitch:profileimageurl")!.Value.ToString();
            


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, nameId),
                new Claim(ClaimTypes.Name, name),
                new Claim("displayname", dispName),
                new Claim("profileimageurl", dispImg)

            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return  LocalRedirect(result.Properties!.Items["returnUrl"]!);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}