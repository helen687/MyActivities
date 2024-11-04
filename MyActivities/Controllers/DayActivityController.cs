using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyActivities.Core;
using MyActivities.DB;

namespace MyActivities.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DayActivityController : Controller
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly ActivitiesDBContext _context;

        public DayActivityController(ILogger<ActivityController> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Route("{activityId}/{day}")]
        public bool Post(Guid activityId, DateTime day)
        {
            try
            {
                var existingDayActivity = _context.DayActivities.Where(da => da.ActivityId == activityId && da.Date.Year == day.Year && da.Date.Month == day.Month && da.Date.Day == day.Day).FirstOrDefault();
                if (existingDayActivity != null)
                {
                    _context.DayActivities.Remove(existingDayActivity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    var newDayActivity = new DayActivity();
                    newDayActivity.ActivityId = activityId;
                    newDayActivity.Date = day;
                    newDayActivity.Id = new Guid();

                    _context.DayActivities.Add(newDayActivity);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving an activity", ex);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public bool Delete(Guid id)
        {
            try
            {
                var existingActivity = _context.Activities.Where(a => a.Id == id).FirstOrDefault();
                if (existingActivity != null)
                {
                    existingActivity.IsDeleted = true;
                    _context.Activities.Update(existingActivity);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting an activity", ex);
            }
        }
    }
}
