using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProjectUI.Controllers
{
    public class UsersController : Controller
    {
        // GET: UsersController
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                //TempData["Toast"] = "Bạn đã đăng nhập";
                //TempData["ToastType"] = "warning";
            }
            else
            {
                TempData["Toast"] = "Vui lòng đăng nhập hoặc đăng ký!";
                TempData["ToastType"] = "info";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync("https://localhost:7227/api/auth/login", new { Email = email, Password = password });
            if (!response.IsSuccessStatusCode)
            {
                TempData["Toast"] = "Sai tài khoản hoặc mật khẩu!";
                TempData["ToastType"] = "error";
                ViewBag.LoginSuccess = false;
                return View();
            }
            var json = await response.Content.ReadFromJsonAsync<LoginResult>();
            // Lấy tên user từ API trả về
            var userName = json?.user?.FullName ?? email;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = false
                });
            TempData["Toast"] = "Đăng nhập thành công!";
            TempData["ToastType"] = "success";
            ViewBag.LoginSuccess = true;
            return View(); // Không redirect ngay, để View xử lý chuyển trang sau 3s
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            // Lấy thông tin user từ claim hoặc tạm hardcode demo
            var fullName = User.Identity.Name ?? "User";
            var avatarUrl = "/images/default-avatar.png"; // Đổi thành ảnh thật nếu có
            var joinDate = DateTime.Now.AddYears(-1); // Tạm hardcode, sau này lấy từ DB/API
            var model = new UserProfileViewModel
            {
                FullName = fullName,
                AvatarUrl = avatarUrl,
                JoinDate = joinDate
            };
            return View(model);
        }

        public class LoginResult
        {
            public string message { get; set; }
            public UserInfo user { get; set; }
            public class UserInfo
            {
                public string FullName { get; set; }
                public string Email { get; set; }
            }
        }

        public class UserProfileViewModel
        {
            public string FullName { get; set; }
            public string AvatarUrl { get; set; }
            public DateTime JoinDate { get; set; }
        }
    }
}
