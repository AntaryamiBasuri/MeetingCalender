using System;

namespace MeetingCalender
{
    public class TimeSlot
    {
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        public DateTime StartTime => _startTime;
        public DateTime EndTime => _endTime;

        public int AvailableDuration => (int)EndTime.Subtract(StartTime).TotalMinutes;

        /// <summary>
        /// Initializes a new instance of <see cref="TimeSlot"/>
        /// </summary>
        /// <param name="startTime">The <see cref="DateTime"/> from where the time slot starts.</param>
        /// <param name="endTime">The <see cref="DateTime"/> where the time slot ends.</param>
        public TimeSlot(DateTime startTime, DateTime endTime)
        {
            _startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, 0);
            _endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, 0);

            if (_startTime > _endTime) throw new ArgumentException("The End time must be greater than the start time.", nameof(endTime));
        }
    }
}
