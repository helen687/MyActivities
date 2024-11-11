using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyActivities.Core;
using MyActivities.DB;

namespace MyActivities.Pages
{
    public class GymHistoryModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ActivitiesDBContext _context;

        public GymHistoryModel(ILogger<IndexModel> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<GymActivity> GymActivities { get; set; } = Enumerable.Empty<GymActivity>();

        public void OnGet()
        {
            GymActivities = _context.GymActivities.Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
        }
    }
}
