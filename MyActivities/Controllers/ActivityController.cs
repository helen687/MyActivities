using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyActivities.Core;
using MyActivities.DB;

namespace MyActivities.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ActivityController : Controller
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly ActivitiesDBContext _context;

        public ActivityController(ILogger<ActivityController> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public bool Post(Activity activity)
        {
            try
            {
                var existingActivity = _context.Activities.Where(a => a.Id == activity.Id).FirstOrDefault();
                if (existingActivity != null)
                {
                    existingActivity.Name = activity.Name;

                    _context.Activities.Update(existingActivity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    var newActivity = new Activity();
                    newActivity.Id = activity.Id;
                    newActivity.Name = activity.Name;

                    _context.Activities.Add(newActivity);
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
