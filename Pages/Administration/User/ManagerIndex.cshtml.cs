using Badge.Areas.Identity.Data;
using Badge.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Badge.Pages.Administration.User
{
    public class ManagerIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public ManagerIndexModel(Badge.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string CurrentFilter { get; set; }
        public IList<ApplicationUser> User { get; set; } = default!;
  

        public async Task OnGetAsync(string? searchString)
        {

            if (_context.Users != null)
            {
                IdentityRole role = await _context.Roles.FirstAsync(r => r.Name == "Manager");
                string roleid = role.Id;
                User = await (from u in _context.Users where (from r in _context.UserRoles where r.RoleId == roleid && r.UserId == u.Id select r).ToList().Count > 0 select u).ToListAsync();


                if (searchString != null)
                {
                    CurrentFilter = searchString;
                }

                if (User != null)
                {

                    if (CurrentFilter!= null)
                    {

                        IQueryable<ApplicationUser> UserIQ = from u in _context.Users where (from r in _context.UserRoles where r.RoleId == roleid && r.UserId == u.Id select r).ToList().Count > 0 select u;
                        UserIQ = UserIQ.Where(l => l.FName.Contains(searchString) || l.LName.Contains(searchString));


                        User =  UserIQ.ToList();
                    }
                }
            }
        }
    }
}

