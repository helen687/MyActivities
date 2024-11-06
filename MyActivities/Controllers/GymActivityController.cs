﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public bool Post(GymActivity gymActivity)
        {
            try
            {
                var existingActivity = _context.GymActivities.Where(a => a.Id == gymActivity.Id).FirstOrDefault();
                if (existingActivity != null)
                {
                    existingActivity.Name = gymActivity.Name;
                    existingActivity.Setting = gymActivity.Setting;

                    _context.GymActivities.Update(existingActivity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    var newGymActivity = new GymActivity();
                    newGymActivity.Id = gymActivity.Id;
                    newGymActivity.Name = gymActivity.Name;
                    newGymActivity.Setting = gymActivity.Setting;

                    _context.GymActivities.Add(newGymActivity);
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