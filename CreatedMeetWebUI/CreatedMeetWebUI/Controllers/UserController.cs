using CreatedMeetWebUI.ApiJobs;
using CreatedMeetWebUI.Extensions;
using CreatedMeetWebUI.Models;
using CreatedMeetWebUI.Tools;
using CreatedMeetWebUI.Tools.User;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace CreatedMeetWebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService<USER> _userService;
        public UserController(IUserService<USER> userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // Kullanıcı doğrulama işlemi
                var user = await _userService.ValidateUserAsync(model.UserName, model.Password);
                if (user != null)
                {
                    var data = new ApplicationUser() { Password = user.PASSWORD, UserName = user.USERNAME };
                    await SignInUserAsync(data, true);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // Model geçersizse veya giriş başarısızsa, tekrar giriş formunu göster
            return View(model);
        }
        private async Task SignInUserAsync(ApplicationUser user, bool rememberMe)
        {
            var identity = new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.Name, user.UserName)
        // Diğer iddialar (claims) ekleyebilirsiniz
    }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Oturum süresi
                });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Oturum çerezlerini temizle
            HttpContext.Response.Cookies.Delete("YourCookieName");

            // Kullanıcıyı giriş sayfasına yönlendir
            return RedirectToAction("Login", "User");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // Kullanıcıyı oluştur
                var result = await _userService.CreateUserAsync(model.Email, model.Password);
                if (result)
                {
                    // Kullanıcıyı oturum açma işlemi
                    var signInResult = await _userService.SignInUserAsync(model.Email, model.Password);
                    if (signInResult)
                    {
                        // Kayıt başarılı, yönlendirme yap
                        return RedirectToLocal(returnUrl);
                    }
                }
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }

            return View(model);
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private IActionResult RedirecToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public object Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44359/User/Load";
            var apiLoads = ApiLoads<USER>.GetInstance(apiUrl);
            var loadTask = apiLoads.LoadtListAsync();
            loadTask.Wait();
            var listdata = apiLoads.List.InitializeIfNull();
            var extendedlist = HalfandStaticObject.GetInstance();
            extendedlist.SelectedRegistry.Users = listdata;
            return DataSourceLoader.Load(listdata, loadOptions);
        }
        [HttpGet]
        [Route("/User/Create")]
        public IActionResult Create()
        {
            var model = new USER();
            return View(model);
        }
        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert([FromBody] USER user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = true, message = "Başarılı" });

            }
            try
            {
                var apiUrl = "https://localhost:44359/User/PostItem?";
                var apiLoads = ApiLoads<USER>.GetInstance(apiUrl);
                var result = apiLoads.PostAsync(apiUrl, user);
                return BadRequest(new { success = false, message = "Hata" });

            catch (Exception)
            {

                throw;
            }
        }

    }
}
