using Badge.Areas.Identity.Data;
using Badge.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Badge.Areas.Identity.Pages.Account.Manage
{
    public class ChangeProfileImageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ApplicationDbContext _context;

        public ChangeProfileImageModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            public IFormFile? Image { get; set; }
            public string? ImageString { get; set; }

        }

        private void Load(ApplicationUser user)
        {
            var imageString = user.AppUImageData;
            Input = new InputModel
            {
                ImageString = imageString,
                Image = null
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            if (Input.Image != null)
            {
                user.ImageFile = Input.Image;
                byte[] bytes = null;
                using (Stream fs = user.ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                }
                user.AppUImageData = Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteImageAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            user.ImageFile = null;
            user.AppUImageData = null;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToPage();
        }
    }
}
