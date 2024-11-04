﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyActivities.Core
{
    public class DayActivity
    {
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
        public DateTime Date { get; set; }
        public Guid Id { get; set; }
    }
}