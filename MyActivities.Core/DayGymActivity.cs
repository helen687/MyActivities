using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyActivities.Core
{
    public class DayGymActivity
    {
        public Guid GymActivityId { get; set; }
        public GymActivity GymActivity { get; set; }
        public DateTime Date { get; set; }
        public Guid Id { get; set; }
    }
}
