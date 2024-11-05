using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyActivities.Core;
using MyActivities.DB;

namespace MyActivities.Pages
{
    public class MyDayModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ActivitiesDBContext _context;

        public MyDayModel(ILogger<IndexModel> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IEnumerable<Activity> Activities { get; set; } = Enumerable.Empty<Activity>();
        public void OnGet()
        {
            Activities = _context.Activities.Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
        }

        public IEnumerable<DateTime> GetDays()
        {
            var today = DateTime.Today;
            var days = new List<DateTime>();
            for (var i = -7; i <= 0; i++)
            {
                days.Add(today.AddDays(i));
            }
            return days;
        }

        public string DayToString(DateTime day) {
            return day.ToString("MMM dd");
        }

        public string DayToUniversalString(DateTime day)
        {
            return day.ToString("yyyy-MM-dd");
        }

        public string GetCheckboxId(Activity activity, DateTime day)
        {
            return "chk_" + activity.Id + "_" + @day.ToString("yyyy-MM-dd");
        }

        public string ISChecked(Activity activity, DateTime day)
        {
            var checkd = _context.DayActivities.Any(da => da.ActivityId == activity.Id && da.Date.Year == day.Year && da.Date.Month == day.Month && da.Date.Day == day.Day);
            return checkd ? "checked" : "";
        }

        public string GetTdClass(DateTime day) {
            if (day == DateTime.Today) {
                return "today";
             }
            return "";
        }
    }
}
