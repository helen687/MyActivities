using Microsoft.AspNetCore.Mvc.RazorPages;
using MyActivities.Core;
using MyActivities.DB;

namespace MyActivities.Pages
{
    public class ActivitiesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ActivitiesDBContext _context;

        public ActivitiesModel(ILogger<IndexModel> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<Activity> Activities { get; set; } = Enumerable.Empty<Activity>();

        public void OnGet()
        {
            Activities = _context.Activities.Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
        }
    }
}
