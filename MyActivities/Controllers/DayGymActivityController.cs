using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyActivities.Core;
using MyActivities.DB;

namespace MyActivities.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DayGymActivityController : Controller
    {
        private readonly ILogger<DayGymActivityController> _logger;
        private readonly ActivitiesDBContext _context;

        public DayGymActivityController(ILogger<DayGymActivityController> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Route("{gymActivityId}/{day}")]
        public bool Post(Guid gymActivityId, DateTime day)
        {
            try
            {
                var existingDayActivity = _context.DayGymActivities.Where(da => da.GymActivityId == gymActivityId && da.Date.Year == day.Year && da.Date.Month == day.Month && da.Date.Day == day.Day).FirstOrDefault();
                if (existingDayActivity != null)
                {
                    _context.DayGymActivities.Remove(existingDayActivity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    var newDayGymActivity = new DayGymActivity();
                    newDayGymActivity.GymActivityId = gymActivityId;
                    newDayGymActivity.Date = day;
                    newDayGymActivity.Id = new Guid();

                    _context.DayGymActivities.Add(newDayGymActivity);
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
