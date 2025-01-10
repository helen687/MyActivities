using Microsoft.AspNetCore.Mvc.RazorPages;
using MyActivities.Core;
using MyActivities.DB;

namespace MyActivities.Pages
{
    public class MyDayGymModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ActivitiesDBContext _context;

        public MyDayGymModel(ILogger<IndexModel> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IEnumerable<GymActivity> GymActivities { get; set; } = Enumerable.Empty<GymActivity>();
        public void OnGet()
        {
            GymActivities = _context.GymActivities.Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
        }

        public IEnumerable<DateTime> GetDays()
        {
            var today = DateTime.Today;
            var days = new List<DateTime>();
            for (var i = -6; i <= 0; i++)
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

        public string GetCheckboxId(GymActivity gymActivity, DateTime day)
        {
            return "chk_" + gymActivity.Id + "_" + @day.ToString("yyyy-MM-dd");
        }

        public string ISChecked(GymActivity gymActivity, DateTime day)
        {
            var checkd = _context.DayGymActivities.Any(dga => dga.GymActivityId == gymActivity.Id && dga.Date.Year == day.Year && dga.Date.Month == day.Month && dga.Date.Day == day.Day);
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
