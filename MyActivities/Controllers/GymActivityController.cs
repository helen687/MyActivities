using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyActivities.Core;
using MyActivities.DB;
using System.Diagnostics;

namespace MyActivities.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GymActivityController : Controller
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly ActivitiesDBContext _context;

        public GymActivityController(ILogger<ActivityController> logger, ActivitiesDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{gymActivityId:Guid}")]
        public List<GymActivityHistory> GetHistory(Guid gymActivityId)
        {
            if (gymActivityId != Guid.Empty)
                return _context.GymActivityHistories
                    .Where(gah => gah.GymActivityId == gymActivityId)
                    .OrderByDescending(gah => gah.DateTime)
                    .ToList();
            else
                return [];
        }

        [HttpPost]
        public bool Post(GymActivity gymActivity)
        {
            try
            {
                using var transaction = _context.Database.BeginTransaction();
                var existingActivity = _context.GymActivities.Where(a => a.Id == gymActivity.Id).FirstOrDefault();
                if (existingActivity != null)
                {
                    existingActivity.Name = gymActivity.Name;
                    if (existingActivity.Setting != gymActivity.Setting) {
                        var gymActivityHistory = new GymActivityHistory();
                        gymActivityHistory.Id = new Guid();
                        gymActivityHistory.DateTime = DateTime.Now;
                        gymActivityHistory.NewSetting = gymActivity.Setting;
                        gymActivityHistory.GymActivityId = gymActivity.Id;
                        _context.GymActivityHistories.Add(gymActivityHistory);
                    }
                    existingActivity.Setting = gymActivity.Setting;
                    _context.GymActivities.Update(existingActivity);               
                    _context.SaveChanges();
                }
                else
                {
                    var newGymActivity = new GymActivity();
                    newGymActivity.Id = gymActivity.Id;
                    newGymActivity.Name = gymActivity.Name;
                    newGymActivity.Setting = gymActivity.Setting;
                    _context.GymActivities.Add(newGymActivity);

                    var gymActivityHistory = new GymActivityHistory();
                    gymActivityHistory.Id = new Guid();
                    gymActivityHistory.DateTime = DateTime.Now;
                    gymActivityHistory.NewSetting = gymActivity.Setting;
                    gymActivityHistory.GymActivityId = gymActivity.Id;
                    _context.GymActivityHistories.Add(gymActivityHistory);

                    _context.SaveChanges();
                }
                transaction.Commit();
                return true;
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
                var existingActivity = _context.GymActivities.Where(a => a.Id == id).FirstOrDefault();
                if (existingActivity != null)
                {
                    existingActivity.IsDeleted = true;
                    _context.GymActivities.Update(existingActivity);
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
