﻿namespace MyActivities.Core
{
    public class GymActivityHistory
    {
        public Guid GymActivityId { get; set; }
        public GymActivity GymActivity { get; set; }
        public DateTime DateTime { get; set; }
        public Guid Id { get; set; }
        public string NewSetting { get; set; }
    }
}
