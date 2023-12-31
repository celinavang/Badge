﻿using Badge.Data;
using Badge.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.GroupAdmin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; }
        public string TypeSort { get; set; }
        public string LeaderSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public string MembersSort { get; set; }
        public int? PageIndex { get; set; }
        public bool MyGroups { get; set; }


        public PaginatedList<Group> Groups { get; set; }

        public async Task OnGetAsync(string sortOrder, string CurrentFilter, string searchString, int? pageIndex, bool? myGroups)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Name_asc") ? "Name_desc" : "Name_asc";
            TypeSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Type_asc") ? "Type_desc" : "Type_asc";
            LeaderSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Leader_asc") ? "Leader_desc" : "Leader_asc";
            MembersSort = string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Member_asc") ? "Member_desc" : "Member_asc";
            if (myGroups == null && User.IsInRole("Leader") || myGroups == true)
            {
                MyGroups = true;
            }
            else
            {
                MyGroups=false;
            }

            if (searchString != null)
            {
                pageIndex = 1;

            }
            else
            {
                searchString = CurrentFilter;
            }
            PageIndex = pageIndex == null ? 1 : pageIndex;
            CurrentFilter = searchString;

            IQueryable<Group> groupsIQ = from g in _context.Groups
                                         select g;
            if (MyGroups)
            {
                groupsIQ = groupsIQ.Where(g => g.LeaderId == User.Identity.GetUserId());
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                groupsIQ = groupsIQ.Where(g => g.Name.Contains(searchString)
                || g.GroupType.Name.Contains(searchString)
                || (g.Leader.FName + " " + g.Leader.LName).Contains(searchString));


            }

            switch (sortOrder)
            {
                case "Name_desc":
                    groupsIQ = groupsIQ.OrderByDescending(g => g.Name);
                    break;
                case "Name_asc":
                    groupsIQ = groupsIQ.OrderBy(g => g.Name);
                    break;
                case "Type_desc":
                    groupsIQ = groupsIQ.OrderByDescending(g => g.GroupType.Name);
                    break;
                case "Type_asc":
                    groupsIQ = groupsIQ.OrderBy(g => g.GroupType.Name);
                    break;
                case "Leader_desc":
                    groupsIQ = groupsIQ.OrderByDescending(g => g.Leader.FName);
                    break;
                case "Leader_asc":
                    groupsIQ = groupsIQ.OrderBy(g => g.Leader.FName);
                    break;
                case "Member_desc":
                    groupsIQ = groupsIQ.OrderByDescending(g => g.Members.Count);
                    break;
                case "Member_asc":
                    groupsIQ = groupsIQ.OrderBy(g => g.Members.Count);
                    break;
                default:
                    groupsIQ = groupsIQ.OrderBy(g => g.Name);
                    break;

            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Groups = await PaginatedList<Group>.CreateAsync(groupsIQ.AsNoTracking()
                .Include(g => g.GroupType)
                .Include(g => g.Leader)
                .Include(g => g.Members)
                , pageIndex ?? 1, pageSize);





        }
    }
}
