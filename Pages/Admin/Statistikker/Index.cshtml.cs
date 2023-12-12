using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.Statistikker
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
              _context = context;
        }
        public IList<Group> Groups { get; set; }
        public IList<string> Names { get; set; }   
        public IList<int> Sales {  get; set; }

        public int G1Salg { get; set; }
        public int G2Salg { get; set; }

        public async Task OnGetAsync()
        {
          Names = new List<string>();

          Sales = new List<int>();

            var groups = await _context.Groups.Include(g => g.Members).ThenInclude(m => m.Sales).ToListAsync();

            foreach (Group g in groups)
            {
                Names.Add(g.Name);
                int counter = 0;

                foreach (Member m in g.Members) 
                {
                    int memberSales = m.Sales.Count();
                    if (memberSales != 0)
                    {
                        counter = counter + memberSales;
                    }

                }
                Sales.Add(counter);

            }

           
        }
    }
}
