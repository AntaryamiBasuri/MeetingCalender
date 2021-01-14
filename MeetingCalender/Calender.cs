using MeetingCalender.Extensions;
using MeetingCalender.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalender
{
    public class Calender : ICalender
    {
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        private IEnumerable<Attendee> _attendees;
        private readonly IList<TimeSlot> _availableMeetingSlots;

        public IEnumerable<Attendee> Attendees => _attendees;

        /// <summary>
        /// Initializes a new instance of <see cref="Calender"/>
        /// </summary>
        /// <param name="startTime">The lower bound of allowed meeting hours.</param>
        /// <param name="endTime">The upper bound of allowed meeting hours.</param>
        public Calender(DateTime startTime, DateTime endTime)
        {
            _startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, 0);
            _endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, 0);

            if (_startTime >= _endTime)
                throw new ArgumentException("The allowed meeting hours end time must be greater than the start time.", nameof(endTime));

            _availableMeetingSlots = new List<TimeSlot>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Calender"/>
        /// </summary>
        /// <param name="startTime">The lower bound of allowed meeting hours.</param>
        /// <param name="endTime">The upper bound of allowed meeting hours.</param>
        /// <param name="attendees">Attendees along with their <see cref="MeetingInfo"/></param>
        public Calender(DateTime startTime, DateTime endTime, IEnumerable<Attendee> attendees)
            : this(startTime, endTime)
        {
            _attendees = attendees;
        }

        /// <summary>
        /// Add attendees to the calender.
        /// </summary>
        public void AddAttendees(IEnumerable<Attendee> attendees)
        {
            _attendees = attendees;
        }

        /// <summary>
        /// Appends additional attendees to existing attendees list.
        /// </summary>
        /// <param name="additionalAttendees"></param>
        public void AppendAttendees(IEnumerable<Attendee> additionalAttendees)
        {
            _attendees = _attendees.Concat(additionalAttendees);
        }

        /// <summary>
        /// Returns the first time slot available for the requested meeting duration.
        /// </summary>
        /// <param name="meetingDuration">The meeting duration in minutes.</param>
        /// <returns>A time slot</returns>
        public TimeSlot GetFirstAvailableSlot(int meetingDuration)
        {
            var availableMeetingSlots = GetAllAvailableTimeSlots();
            return availableMeetingSlots.OrderBy(o => o.AvailableDuration).FirstOrDefault(t => t.AvailableDuration >= meetingDuration);
        }

        /// <summary>
        /// Find all available meeting slots.
        /// </summary>
        /// <returns>A list of <see cref="TimeSlot"/></returns>
        public IEnumerable<TimeSlot> GetAllAvailableTimeSlots()
        {
            var meetingHoursByMinutes = GetTimeSeriesByMinutes(_startTime, _endTime);

            //Map the meeting timings for each attendees
            if (_attendees != null && _attendees.Any())
            {
                _attendees.AsParallel().ForEach(attendee =>
                {
                    attendee.MeetingInfo.AsParallel().ForEach(scheduledMeeting =>
                    {
                        var timeSeries = GetTimeSeriesByMinutes(scheduledMeeting.StartTime, scheduledMeeting.EndTime, true);
                        timeSeries.AsParallel().ForEach(item =>
                        {
                            if (meetingHoursByMinutes.TryGetValue(item.Key, out bool prevValue))
                            {
                                //Use OR operator to merge all meeting duration of attendees
                                meetingHoursByMinutes.TryUpdate(item.Key, prevValue || item.Value, prevValue);
                            }
                        });
                    });
                });

                if (meetingHoursByMinutes.Any(i => i.Value == false)) //if any slot available then only calculate 
                {
                    CalculateAvailableSlots(meetingHoursByMinutes.OrderBy(i => i.Key));
                }
            }
            else
            {
                _availableMeetingSlots.Add(new TimeSlot(meetingHoursByMinutes.First().Key, meetingHoursByMinutes.Last().Key));
            }

            return _availableMeetingSlots;
        }

        private void CalculateAvailableSlots(IEnumerable<KeyValuePair<DateTime, bool>> scheduledHoursByMinutes, TimeSlot availableTimeSlot = null, bool searchVal = false)
        {
            var hoursByMinutes = scheduledHoursByMinutes.ToList();
            if (!hoursByMinutes.Any()) return;

            var foundItem = hoursByMinutes.FirstOrDefault(item => item.Value == searchVal);
            if (foundItem.Key != DateTime.MinValue)
            {
                var subSet = hoursByMinutes.Where(dict => dict.Key > foundItem.Key);
                if (searchVal && availableTimeSlot != null)
                {
                    availableTimeSlot = new TimeSlot(availableTimeSlot.StartTime, foundItem.Key.AddMinutes(-1));
                    _availableMeetingSlots.Add(availableTimeSlot);
                    CalculateAvailableSlots(subSet);
                }
                else
                {
                    availableTimeSlot = new TimeSlot(foundItem.Key, DateTime.MaxValue);
                    CalculateAvailableSlots(subSet, availableTimeSlot, true);
                }
            }
            else if (availableTimeSlot != null)
            {
                foundItem = hoursByMinutes.Last();
                availableTimeSlot = new TimeSlot(availableTimeSlot.StartTime, foundItem.Key);
                _availableMeetingSlots.Add(availableTimeSlot);
            }
        }

        private ConcurrentDictionary<DateTime, bool> GetTimeSeriesByMinutes(DateTime seriesStartTime, DateTime seriesEndTime, bool isScheduled = false)
        {
            var timeRange = new ConcurrentDictionary<DateTime, bool>();
            var temp = seriesStartTime;
            while (temp < seriesEndTime)
            {
                timeRange.TryAdd(temp, isScheduled);
                temp = temp.AddMinutes(1);
            }

            return timeRange;
        }
    }
}