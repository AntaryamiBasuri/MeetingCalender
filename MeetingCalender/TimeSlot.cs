using System;
using MeetingCalender.Extensions;

namespace MeetingCalender
{
    public class TimeSlot
    {
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        /// <summary>
        /// Gets the start time of a <see cref="TimeSlot"/>.
        /// </summary>
        public DateTime StartTime => _startTime;
        /// <summary>
        /// Gets the end time of a <see cref="TimeSlot"/>.
        /// </summary>
        public DateTime EndTime => _endTime;

        public int AvailableDuration => (int)EndTime.Subtract(StartTime).TotalMinutes;

        /// <summary>
        /// Initializes a new instance of <see cref="TimeSlot"/>
        /// </summary>
        /// <param name="startTime">The <see cref="DateTime"/> from where the time slot starts.</param>
        /// <param name="endTime">The <see cref="DateTime"/> where the time slot ends.</param>
        public TimeSlot(DateTime startTime, DateTime endTime)
        {
            _startTime = startTime.CalibrateToMinutes();
            _endTime = endTime.CalibrateToMinutes();

            if (_startTime > _endTime) throw new ArgumentException("The End time must be greater than the start time.", nameof(endTime));
        }
    }
}
